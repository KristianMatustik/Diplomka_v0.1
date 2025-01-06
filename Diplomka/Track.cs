using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;


namespace Diplomka
{
    class Track
    {
        public class Point
        {
            //constants, set at creation for specific point
            public double lat { get; set; }                             //latitude
            public double lon { get; set; }                             //longtitude
            public double alt { get; set; }                             //altitude above sea level
            public double rho { get; set; }                             //air density
            public double crr { get; set; }                             //rolling resistance coefficient
            public double wind { get; set; }                            //relative wind velocity (+ is headwind, - is tailwind)
            public double x { get; set; }                               //coordinates from start, 1unit=1meter, +x=east, used only to evaluate corners   
            public double y { get; set; }                               //  -||-  1unit=1meter, +y=north    
            public double dist_to_next { get; set; }
            public double max_v { get; set; }                           //max speed - set for the corners

            //variables, can change with the found solution
            public double power { get; set; }                           //Power
            public double kineticEnergy { get; set; }                   //Kinetic energy, 0.5*m*v*v
            public double velocity { get; set; }                        //velocity
            public double time { get; set; }                            //time from start

            public Point(double altitude, double crr)
            {
                Initialize(0, 0, altitude, crr, 0, 0);
            }

            public Point(double latitude, double longitude, double altitude, double crr, double power = 0, double time = 0)
            {
                Initialize(latitude, longitude, altitude, crr, power, time);
            }

            public void Initialize(double latitude, double longitude, double altitude, double crr, double power, double time)
            {
                this.lat = latitude;
                this.lon = longitude;
                this.alt = altitude;
                this.rho = calculateRho(altitude);
                this.crr = crr;
                this.wind = 0;
                this.x = 0;
                this.y = 0;
                this.dist_to_next = 1;
                this.max_v = 100;
                this.power = power;
                this.kineticEnergy = 0;
                this.velocity = 0;
                this.time = time;
            }

            public void Copy(Point other)
            {
                this.lat = other.lat;
                this.lon = other.lon;
                this.alt = other.alt;
                this.rho = other.rho;
                this.crr = other.crr;
                this.wind = other.wind;
                this.x = other.x;
                this.y = other.y;
                this.dist_to_next = other.dist_to_next;
                this.max_v = other.max_v;
                this.power = other.power;
                this.kineticEnergy = other.kineticEnergy;
                this.velocity = other.velocity;
                this.time = other.time;
            }


            public static double calculateRho(double altitude)
            {
                return 1.225 * (9000 - altitude) / 9000;            //linear approximation for low altitudes (0-1500 m, but holds up to 3000 m)
            }

            public double timeToNextPoint()
            {
                return dist_to_next / velocity;                            //based on current speed, time to next point
            }

            public double slope(Point next)                         //calculates the slope on the segment betwwen 2 points
            {
                return (next.alt - alt) / dist_to_next;
            }

            public void update(Point next, Cyclist cyclist)         //calculates the variables of next point
            {
                double m = cyclist.mass;
                double cda = cyclist.CdA(power, slope(next));

                next.kineticEnergy = kineticEnergy + power*timeToNextPoint() + Functions.potentialEnergy(m,alt) - Functions.potentialEnergy(m, next.alt) - dist_to_next*(Functions.aerodynamicResistance(velocity+wind,cda,rho) + Functions.rollingResistance(m,crr));

                double minV = 1;
                if (next.kineticEnergy < Functions.kineticEnergy(m,minV))                              //to prevent negative velocity
                {
                    next.kineticEnergy = Functions.kineticEnergy(m, minV);
                    //P = (next.KE - KE - Functions.potentialEnergy(m, alt) + Functions.potentialEnergy(m, next.alt) + dist_to_next * (Functions.aerodynamicResistance(v + wind, cda, rho) + Functions.rollingResistance(m, crr))) / timeToNextPoint();
                }
               
                next.velocity = Functions.speedFromKE(m, next.kineticEnergy);
                next.time = time + timeToNextPoint();
            }
        }


        //////// atributes

        public List<Point> track { get; set; }
        public double progress { get; set; }

        private double _crr;
        public double crr
        {
            get {return _crr;}
            set
            {
                _crr = value;
                if (track != null)
                {
                    for (int i = 0; i < track.Count; i++)
                        track[i].crr = value;
                }
            }
        }

        //////// methods

        public Track(int length = 0, double altitude = 0, double crr = 0.004)
        {
            if (length < 0)
                length = 0;
            this.crr = crr;
            track = new List<Point>(length);
            for (int i = 0; i < length; i++)
            {
                track.Add(new Point(altitude,crr));
            }
        }

        public Track(string gpxFile, double crr = 0.004)
        {
            this.crr = crr;
            track = loadGPX(gpxFile);
        }

        private List<Point> loadGPX(string gpxFile)                                  //loads gpx file as the track
        {
            XDocument gpxDoc = XDocument.Load(gpxFile);
            XNamespace gpxNs = "http://www.topografix.com/GPX/1/1";

            var trkpts = gpxDoc.Descendants(gpxNs + "trkpt").ToList();
            DateTime startTime = DateTime.MinValue;

            var firstTrkpt = trkpts.FirstOrDefault();
            if (firstTrkpt != null)
            {
                var timeElement = firstTrkpt.Element(gpxNs + "time");
                if (timeElement != null)
                {
                    startTime = DateTime.Parse(timeElement.Value, CultureInfo.InvariantCulture);
                }
            }


            List<Point> points = trkpts.Select(trkpt =>
            {
                double lat = double.Parse(trkpt.Attribute("lat").Value, CultureInfo.InvariantCulture);
                double lon = double.Parse(trkpt.Attribute("lon").Value, CultureInfo.InvariantCulture);
                double ele = double.Parse(trkpt.Element(gpxNs + "ele").Value, CultureInfo.InvariantCulture);
                //double wbal = double.Parse(trkpt.Element(gpxNs + "wbal").Value, CultureInfo.InvariantCulture); /////// for CP model
                double time = 0;

                var timeElement = trkpt.Element(gpxNs + "time");
                if (timeElement!=null)
                    time=(DateTime.Parse(timeElement.Value, CultureInfo.InvariantCulture) - startTime).TotalSeconds;

                double power = 0;
                var powerElement = trkpt.Element(gpxNs + "extensions")?.Element(gpxNs + "power");
                if (powerElement != null && double.TryParse(powerElement.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedPower))
                {
                    power = parsedPower;
                }
                Point p = new Point(lat, lon, ele, crr, power, time);
                //p.velocity = wbal/3.6;                                                              //////////// for CP model
                return p;
            }).ToList();

            GeoCoordinate start = new GeoCoordinate(points[0].lat, points[0].lon);

            for (int i = 0; i < points.Count(); i++)
            {
                if (i < points.Count() - 1)
                {
                    points[i].dist_to_next = (new GeoCoordinate(points[i].lat, points[i].lon)).GetDistanceTo(new GeoCoordinate(points[i + 1].lat, points[i + 1].lon));
                    if (points[i + 1].time != 0)
                    {
                        points[i].velocity = points[i].dist_to_next / (points[i + 1].time - points[i].time);     //////////// comment to view W' with CP model
                    }
                }
                points[i].x = start.GetDistanceTo(new GeoCoordinate(points[0].lat, points[i].lon)) * Math.Sign(points[i].lon - points[0].lon);
                points[i].y = start.GetDistanceTo(new GeoCoordinate(points[i].lat, points[0].lon)) * Math.Sign(points[i].lat - points[0].lat);
            }
            points.Last().power = 0;
            return points;
        }

        public void saveGPX(string gpxFile)                                         //saves the track as a gpx file
        {
            XNamespace gpxNs = "http://www.topografix.com/GPX/1/1";
            XNamespace xsiNs = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace schemaLocationNs = "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd";

            XElement gpx = new XElement(gpxNs + "gpx",
                new XAttribute(XNamespace.Xmlns + "xsi", xsiNs),
                new XAttribute(xsiNs + "schemaLocation", schemaLocationNs),
                new XAttribute("version", "1.1"),
                new XAttribute("creator", "RaceOptimiser"),
                new XElement(gpxNs + "trk",
                    new XElement(gpxNs + "name", "Optimised track"),
                    new XElement(gpxNs + "trkseg",
                        from point in track
                        select new XElement(gpxNs + "trkpt",
                            new XAttribute("lat", point.lat.ToString(CultureInfo.InvariantCulture)),
                            new XAttribute("lon", point.lon.ToString(CultureInfo.InvariantCulture)),
                            new XElement(gpxNs + "ele", point.alt.ToString(CultureInfo.InvariantCulture)),
                            new XElement(gpxNs + "time", DateTimeOffset.FromUnixTimeMilliseconds((long)(point.time * 1000)).ToString("yyyy-MM-ddTHH:mm:ss.fffZ")),
                            new XElement(gpxNs + "extensions",
                                new XElement(gpxNs + "power", point.power.ToString(CultureInfo.InvariantCulture))
                            )
                        )
                    )
                )
            );
            gpx.Save(gpxFile);
        }

        public void setCorners(Cyclist cyclist)                                  //sets the maximum speed for corners along the course
        {
            double bankAngle = cyclist.bankAngle;
            if (bankAngle < 1 || bankAngle > 89)
                bankAngle = 89;
            for (int i = 1; i < track.Count() - 1; i++)
            {
                double r = Functions.calculateCircleRadius(track[i - 1].x, track[i - 1].y, track[i].x, track[i].y, track[i + 1].x, track[i + 1].y);

                double maxV = 100;
                if (r < 100)
                    maxV = Math.Sqrt(Math.Tan(bankAngle * Math.PI / 180) * 9.8 * r);

                track[i].max_v = maxV;
            }
        }

        public void setWind(double speed, double direction)                      //sets the relative windspeed along the course
        {
            for (int i = 0; i < track.Count() - 1; i++)
            {
                double bearing = Functions.calculateBearing(track[i].lat, track[i].lon, track[i + 1].lat, track[i + 1].lon);
                double relativeWindSpeed = Math.Cos((direction - bearing) * Math.PI / 180) * speed;
                track[i].wind = relativeWindSpeed;
            }
        }

        /////////

        public double weightedAveragePower(Cyclist c, int start = 0, int end=-1)        //calculates power metric based on cyclist (AP, NP, ...)
        {
            if(start < 0 || start > track.Count() - 1)
                start = 0;
            if (end < 0 || end > track.Count() - 1)
            {
                end = track.Count() - 1;
            }

            double total = 0;
            for (int i = start; i < end; i++)
            {
                if(track[i].power>0)
                    total += c.f(track[i].power) * track[i].timeToNextPoint();
            }

            if ((track[end].time - track[start].time)==0)
            {
                c.f_inv(total);
            }
            return c.f_inv(total / (track[end].time - track[start].time));
        }

        public double updateTime(Cyclist cyclist, int start = 0, int end = -1)      //calculates how the ride progresses
        {
            if (start < 0 || start > track.Count() - 1)
                start = 0;
            if (end < 0 || end > track.Count() - 1)
            {
                end = track.Count() - 1;
            }

            track[start].kineticEnergy = Functions.kineticEnergy(cyclist.mass, track[start].velocity);
            for (int i = start; i < end; i++)
            {
                track[i].update(track[i + 1], cyclist);
            }

            return track[end].time;
        }

        public void initialSolution(Cyclist c, double v0 = 5)               //finds initial solution with equal power everywhere
        {
            track[0].time = 0;
            track[0].velocity = v0;
            track[0].kineticEnergy = Functions.kineticEnergy(c.mass, v0);
            for (int i = 0; i < track.Count() - 1; i++)
            {
                track[i].power = c.PowerLIMIT;
            }
            updateTime(c);
        }

        public void adjustPower(Cyclist c)                                  //adjusts power so that it is feasible, to fix errors during calculation
        {
            double P_avg = weightedAveragePower(c);
            double P_lim = c.PowerLIMIT;
            if (P_lim==0 || P_avg==0)
            {
                P_avg = 1; P_lim = 0;
            }

            for (int i = 0; i < track.Count() - 1; i++)
            {
                track[i].power = track[i].power * P_lim / P_avg;
                if (track[i].power > c.PowerMAX)
                    track[i].power = c.PowerMAX;
            }
        }

        public List<double> estimateGradient(Cyclist c, double dE = 100, int estimationDistance=-1)        //estimates the gradient on fixed amount of steps (-1=whole course)
        {
            if (estimationDistance < 0)
                estimationDistance = track.Count();
            List<double> gradient = new List<double>(track.Count()-1);
            updateTime(c);
            for (int i = 0; i < track.Count()-1; i++)
            {
                double initialPower = track[i].power;

                double fp = c.f(track[i].power) + dE / track[i].timeToNextPoint();
                track[i].power = c.f_inv(fp);

                gradient.Add(track[track.Count() - 1].time);
                if (i+estimationDistance < track.Count())
                    gradient[i] = track[i + estimationDistance].time;
                gradient[i] -= updateTime(c,i, i + estimationDistance);

                track[i].power = initialPower;
                updateTime(c, i, i + estimationDistance);
            }
            return gradient;
        }
       
        public List<int> sortedGradient(List<double> gradient)                                      //gives indexes of of sorted gradient elements
        {
            return gradient
            .Select((value, index) => new { Value = value, Index = index })
            .OrderBy(pair => pair.Value)
            .Select(pair => pair.Index)
            .ToList();
        }

        public List<int> gradientToChange(List<double> gradient, double Pmax, int maxChanges=-1)   //returns list with -1 or +1 if power should be decreased or increased to improve solution
        {
            if(maxChanges<0)
                maxChanges = track.Count()/2;
            List<int> sg = sortedGradient(gradient);
            List<int> change = Enumerable.Repeat(0, gradient.Count()).ToList();

            int decrease = 0;
            int increase = gradient.Count() - 1;

            while (decrease < increase && maxChanges > 0)
            {
                while (track[sg[decrease]].power <= 0 && decrease < increase)
                {
                    decrease++;
                }
                while (track[sg[increase]].power >= Pmax && decrease < increase)
                {
                    increase--;
                }
                if (decrease < increase && maxChanges > 0)
                {
                    change[sg[decrease++]] = -1;
                    change[sg[increase--]] = 1;
                    maxChanges--;
                }
            }
            return change;
        }

        double updateTimeAndPowerDP(Cyclist cyclist, List<int> adjustments, double dP, int start = 0, int end = -1) //calculates how the ride progresses while updating power
        {
            if (start < 0 || start > track.Count() - 1)
                start = 0;
            if (end < 0 || end > track.Count() - 1)
            {
                end = track.Count() - 1;
            }

            track[start].kineticEnergy = Functions.kineticEnergy(cyclist.mass, track[start].velocity);
            for (int i = start; i < end; i++)
            {
                if (adjustments[i] == 1)
                {
                    track[i].power += dP;
                    if (track[i].power > cyclist.PowerMAX)
                        track[i].power = cyclist.PowerMAX;
                }
                else if (adjustments[i] == -1)
                {
                    track[i].power -= dP;
                    if (track[i].power < 0)
                        track[i].power = 0;
                }
                track[i].update(track[i + 1], cyclist);
            }
            return track[end].time;
        }

        public void solve(Cyclist c, int iters = 100, double decay = 0.95, int gradientEstimation = -1)     //finds solution without considering corners
        {
            double avgT = (track.Last().time- track.First().time) /track.Count();

            double dE = c.f(c.PowerLIMIT)*avgT/4;
            //double dP = weightedAveragePower(c)/2;
            //double dE = (c.f(c.PowerLIMIT+dP)-c.f(c.PowerLIMIT))*avgT;
            int n = track.Count() / 2;
            for (int i = 0; i< iters; i++)
            {
                progress = (double)i / iters/2;

                List<double> gradient = estimateGradient(c,dE,gradientEstimation);
                List<int> change = gradientToChange(gradient, c.PowerMAX, n);

                //updateTimeAndPowerDP(c,change,dP);
                updateTimeAndPowerDE(c, change, dE);

                if (i % (iters / 10) == 0 || i == iters - 1)
                {
                    adjustPower(c);
                }

                //dP *= decay;  //totalMaxChange = dP(1-decay^iters)/(1-decay)
                dE *= decay;
                //printFormattedTable(c,dE);
                Console.WriteLine($"{i,5}\t{track[track.Count-1].time,10:F5}\t{weightedAveragePower(c),5:F2}");
            }
        }

        public void solveWithCorners(Cyclist c, int iters = 100, double decay = 0.95, int gradientEstimation = -1)  //finds solution while considering corners
        {
            solve(c,iters,decay,gradientEstimation);
            List<bool> corner = new List<bool>(track.Count());
            for(int i = 0;i<track.Count();i++)
            {
                if (track[i].velocity < track[i].max_v)
                    corner.Add(false);
                else
                    corner.Add(true);
            }

            List<int> segmentEnd = new List<int>();
            List<double> cornerSpeed = new List<double>();
            int end = 1;
            segmentEnd.Add(-1);
            while (end < track.Count())
            {
                if (corner[end] == false)
                {
                    end++;                   
                }
                else
                {
                    double minSpeed = double.PositiveInfinity;
                    while (end<track.Count && corner[end] == true)
                    {
                        if(track[end].max_v<minSpeed)
                            minSpeed = track[end].max_v;
                        end++;
                    }
                    segmentEnd.Add(end-1);
                    cornerSpeed.Add(minSpeed);
                }
            }
            if (segmentEnd.Last() != end - 1)
                segmentEnd.Add(end - 1);
            else
                cornerSpeed.Add(track.Last().max_v);

            List<Track> segment = new List<Track>(segmentEnd.Count()-1);
            for(int i=0;i<segmentEnd.Count()-1;i++)
            {
                int segmentLength = segmentEnd[i + 1] - segmentEnd[i];
                segment.Add(new Track(segmentLength));
                for (int j=0; j<segmentLength; j++)
                {
                    segment[i].track[j].Copy(track[segmentEnd[i]+1+j]);
                    if (corner[segmentEnd[i] + 1 + j]==true)
                    {
                        segment[i].track[j].max_v=cornerSpeed[i];
                    }
                }
            }

            double originalPowerLimit = c.PowerLIMIT;
            for (int i=0;i<segment.Count();i++) 
            {
                Track t = segment[i];
                c.PowerLIMIT = t.weightedAveragePower(c);
                if (i != 0)
                {
                    t.track[0].velocity = segment[i-1].track.Last().velocity;
                    t.track[0].time = segment[i - 1].track.Last().time + segment[i - 1].track.Last().timeToNextPoint();
                    t.updateTime(c);
                }
                t.solve(c, iters , decay, gradientEstimation);
                progress += (double)t.track.Count/track.Count/2;

                int idx = t.track.Count() - 1;
                while (t.track[idx].velocity > t.track[idx].max_v && idx>=0)
                {
                    t.track[idx].velocity = cornerSpeed[i];
                    t.track[idx].power = 0;
                    t.track[idx].kineticEnergy = Functions.kineticEnergy(c.mass, cornerSpeed[i]);
                    idx--;
                }
                
                if (idx== t.track.Count() - 1)
                    continue;
                idx++;
                while (idx>0)
                {
                    double prevKE = t.track[idx].kineticEnergy + t.track[idx-1].dist_to_next*c.brakingForce - Functions.potentialEnergy(c.mass, t.track[idx-1].alt) + Functions.potentialEnergy(c.mass, t.track[idx].alt) + t.track[idx-1].dist_to_next * (Functions.aerodynamicResistance(t.track[idx].velocity + t.track[idx].wind, c.CdA(0,0), t.track[idx].rho) + Functions.rollingResistance(c.mass, t.track[idx].crr));
                    double prevV = Functions.speedFromKE(c.mass, prevKE);
                    t.track[idx-1].power = 0;
                    if (t.track[idx - 1].velocity > prevV)
                    {
                        t.track[idx - 1].velocity = prevV;
                        t.track[idx - 1].kineticEnergy = prevKE;
                        idx--;
                    }
                    else
                    {
                        break;
                    }    
                }
                idx--;
                if (idx < 0)
                    idx = 0;
                while (idx < t.track.Count()-1)
                {
                    t.track[idx + 1].time = t.track[idx].time + t.track[idx].timeToNextPoint();
                    idx++;
                }
            }
            c.PowerLIMIT=originalPowerLimit;

            int index = 0;
            for (int i = 0; i < segment.Count();i++)
            {
                for (int j = 0; j < segment[i].track.Count(); j++)
                {
                    track[index].Copy(segment[i].track[j]);
                    index++;
                }
            }
            progress = 1;
        }
        
        //
        // FUNCTIONS TO TEST SOME EXAMPLES
        //

        public void testAlgorithm(Cyclist c, int iters = 100, double decay = 0.95)
        {
            printFormattedTable(c,c.f(100));
            Console.WriteLine("-------------------------------------------------------");
            solve(c, iters, decay);
            Console.WriteLine("-------------------------------------------------------");
            printFormattedTable(c,c.f(100));
        }

        public void testFlat(Cyclist c, int length)
        {
            track = new List<Point>(length);
            for (int i = 0; i < length; i++)
            {
                track.Add(new Point(0, crr));
            }
            initialSolution(c);
            //solve(c);
        }

        public void testWind(Cyclist c, int length, double windVelocity)
        {
            track = new List<Point>(length);
            for (int i = 0; i < length; i++)
            {
                track.Add(new Point(0, crr));
            }
            for (int i = 0; i < length / 2; i++)
            {
                track[i].wind = -windVelocity;
            }
            for (int i = length / 2; i < length; i++)
            {
                track[i].wind = windVelocity;
            }
            initialSolution(c);
            solve(c);
        }

        public void testHillFinish(Cyclist c, int length, double slope)
        {
            track = new List<Point>(length);
            for(int i=0;i<length/2;i++)
            {
                track.Add(new Point(0, crr));
            }
            for (int i = length/2; i < length; i++)
            {
                track.Add(new Point((1+i - length / 2) * slope, crr));
            }
            initialSolution(c);
            solve(c);
        }

        public void testA(Cyclist c, int length, double slope)
        {
            track = new List<Point>(length);
            for (int i = 0; i < length / 2; i++)
            {
                track.Add(new Point((i)*slope, crr));
            }
            for (int i = length / 2; i < length; i++)
            {
                track.Add(new Point((length - i - 1) * slope, crr));
            }
            initialSolution(c);
            solve(c);
        }

        public void testV(Cyclist c, int length, double slope)
        {
            track = new List<Point>(length);
            for (int i = 0; i < length / 2; i++)
            {
                track.Add(new Point((length / 2 - i) * slope, crr));
            }
            for (int i = length / 2; i < length; i++)
            {
                track.Add(new Point(slope*(i - length / 2), crr));
            }
            initialSolution(c);
            solve(c);
        }

        public void testSine(Cyclist c, int length, double maxSlope, double altDiff)
        {
            track = new List<Point>(length);
            double amplitude = altDiff / 2;
            double frequency = 2 * maxSlope / altDiff;

            for (int i = 0; i < length; i++)
            {
                double altitude = amplitude * Math.Sin(frequency * i);
                track.Add(new Point(altitude, crr));
            }
            initialSolution(c);
            solve(c);
        }

        public void testCorner(Cyclist c, int length)
        {
            track = new List<Point>(length);
            for (int i = 0; i < length; i++)
            {
                track.Add(new Point(0, crr));
            }
            track[length / 2].max_v = 5;
            initialSolution(c);
            solveWithCorners(c);
        }

        public void printResult(Cyclist c)
        {
            Console.WriteLine($"{track[track.Count()-1].time,5}\t{weightedAveragePower(c),5:F2}");
        }

        public void printFormattedTable(Cyclist c, double dE=-1)
        {
            if(dE==-1)
            {
                dE = c.f(c.PowerLIMIT/10);
            }
            double avg = weightedAveragePower(c);
            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.WriteLine($"{"i",5}\t{"dist",10}\t{"time",10}\t{"speed",10}\t{"power",10}\t{"gradient"}");
            Console.WriteLine("----------------------------------------------------------------------------------");
            List<double> gradient = estimateGradient(c, dE);
            gradient.Add(0);
            double dist = 0;
            for (int i = 0; i < track.Count(); i++)
            {
                //Console.WriteLine($"{dist,10:F2}\t{track[i].time,10:F2}\t{track[i].v * 3.6,10:F2}\t{track[i].P,10:F2}");
                Console.WriteLine($"{i,5}\t{dist,10:F2}\t{track[i].time,10:F2}\t{track[i].velocity * 3.6,10:F2}\t{track[i].power,10:F2}\t{gradient[i]:F8}");
                dist += track[i].dist_to_next;
            }
        }







        //deprecated methods, found out inferior in testing toother solutions, not in use, to be deleted later (some are already gone)

        public void adjustElevation(int n)
        {
            if (n < 0 || n > track.Count() / 2 - 1)
                return;
            double eleSum = 0;
            List<double> newEle = new List<double>();
            for (int i = 0; i < n + 1; i++)
            {
                eleSum += track[i].alt;
            }
            for (int i = 0; i < n; i++)
            {
                newEle.Add(eleSum / (n + 1 + i));
                eleSum += track[i + n + 1].alt;
            }
            for (int i = n; i < track.Count() - n - 1; i++)
            {
                newEle.Add(eleSum / (2 * n + 1));
                eleSum += track[i + n + 1].alt;
                eleSum -= track[i - n].alt;
            }
            for (int i = track.Count() - n - 1; i < track.Count(); i++)
            {
                newEle.Add(eleSum / (n + track.Count() - i));
                eleSum -= track[i - n].alt;
            }
            for (int i = 0; i < track.Count(); i++)
            {
                track[i].alt = newEle[i];
                track[i].rho = Point.calculateRho(newEle[i]);
            }
        }

        public void averageOutPower(Cyclist cyclist, int n)
        {
            Console.WriteLine(weightedAveragePower(cyclist));
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += track[i].power;
            }
            for (int i = 0; i < track.Count() - n - 1; i++)
            {
                double p = track[i].power;
                track[i].power = sum / n;
                if (sum < 0)
                {
                    track[i].power = 0;
                }
                sum -= p;
                updateTime(cyclist, i, i + n + 1);
                sum += track[i + n].power;
            }
            for (int i = track.Count() - n - 1; i < track.Count() - 1; i++)
            {
                if (sum < 0)
                {
                    track[i].power = 0;
                }
                else
                {
                    track[i].power = sum / n;
                }
            }
            updateTime(cyclist);
        }
        

        double updateTimeAndPowerDE(Cyclist cyclist, List<int> adjustments, double dE, int start = 0, int end = -1) //calculates how the ride progresses while updating power
        {
            if (start < 0 || start > track.Count() - 1)
                start = 0;
            if (end < 0 || end > track.Count() - 1)
            {
                end = track.Count() - 1;
            }

            track[start].kineticEnergy = Functions.kineticEnergy(cyclist.mass, track[start].velocity);
            for (int i = start; i < end; i++)
            {
                if (adjustments[i] == 1)
                {
                    double fp = cyclist.f(track[i].power) + dE / track[i].timeToNextPoint();
                    if (fp < cyclist.f(cyclist.PowerMAX))
                        track[i].power = cyclist.f_inv(fp);
                    else
                        track[i].power = cyclist.PowerMAX;
                }
                else if (adjustments[i] == -1)
                {
                    double fp = cyclist.f(track[i].power) - dE / track[i].timeToNextPoint();
                    if (fp > 0)
                        track[i].power = cyclist.f_inv(fp);
                    else
                        track[i].power = 0;
                }
                track[i].update(track[i + 1], cyclist);
            }

            return track[end].time;
        }
    }
}
