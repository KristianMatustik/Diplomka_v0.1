using System;
using System.Windows.Forms;

namespace Diplomka
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWindow mw = new MainWindow();





            Cyclist c1 = new Cyclist(400, 1200, 80, 0.004, 0.97, 35, 200, 0.2, x => Math.Pow(x, 1.05), x => Math.Pow(x, 1 / 1.05));
            Cyclist c2 = new Cyclist(370, 1200, 76, 0.003, 0.97, 30, 200, 0.18, x => Math.Pow(x, 4), x => Math.Pow(x, 0.25));

            Cyclist cme = new Cyclist(277, 900, 92, 0.004, 0.97, 30, 200, 0.28, x => Math.Pow(x, 1), x => Math.Pow(x, 1));

            Track t = new Track();


            //t.testSine(c1, 1000, 0.1, 10);
            //t.initialSolution(c1);
            //t.solveWithCorners(c1, 100, 0.95, 100);






            t = new Track("TestI300.gpx");
            t.updateTime(cme);

            //t.initialSolution(cme);
            //t.setCorners(cme);
            //t.solveWithCorners(cme,100,0.95,100);
            //t.setCorners(c2);
            //t.solveWithCorners(c2,100,0.95,100);          

            //t.testFlat(c1, 1000);;
            //t.printFormattedTable(c1);

            //t.printFormattedTable(c1);
            mw.DisplayPlot(t.track);
            //var g = t.estimateGradient(c1);
            Application.Run(mw);
            



            Console.Read();
        }
    }
}
