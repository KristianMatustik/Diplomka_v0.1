using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplomka
{
    internal class MainWindow : Form
    {
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private PlotView plotView;
        private ToolStripProgressBar toolStripProgressBar1;

        private Track track;
        private ToolStripButton toolStripButton4;
        private Cyclist cyclist;

        private Cyclist cAvg = new Cyclist(0, 0, 0, 0, 0, 0, 0, 0, x => x, x => x);
        private Cyclist cNorm = new Cyclist(0, 0, 0, 0, 0, 0, 0, 0, x => Math.Pow(x, 4), x => Math.Pow(x, 0.25));

        public MainWindow()
        {
            InitializeComponent();
            InitializePlotView();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "GPX files (*.gpx)|*.gpx|All files (*.*)|*.*";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripProgressBar1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 28);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(101, 25);
            this.toolStripButton1.Text = "Nahrát trasu";
            this.toolStripButton1.ToolTipText = "Nahrajte gpx soubor s trasou";
            this.toolStripButton1.Click += new System.EventHandler(this.ToolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(83, 25);
            this.toolStripButton2.Text = "Nastavení";
            this.toolStripButton2.ToolTipText = "Nastavení cyklisty jako CdA, Crr atd.";
            this.toolStripButton2.Click += new System.EventHandler(this.ToolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(62, 25);
            this.toolStripButton3.Text = "Vyřešit";
            this.toolStripButton3.ToolTipText = "Nalezne ideální řešení aktuální trasy a cyklisty";
            this.toolStripButton3.Click += new System.EventHandler(this.ToolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(100, 25);
            this.toolStripButton4.Text = "Uložit řešení";
            this.toolStripButton4.ToolTipText = "Uloží nalezené řešení jako gpx soubor";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 25);
            this.toolStripProgressBar1.ToolTipText = "Ukazuje progress hledání řešení";
            // 
            // MainWindow
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainWindow";
            this.Text = "PowerOptimiser";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitializePlotView()
        {
            this.plotView = new PlotView
            {
                Location = new Point(0, this.toolStrip1.Height), // Start the PlotView below the ToolStrip
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height - this.toolStrip1.Height), // Adjust the size to fit the remaining space
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right // Ensure it resizes with the form
            };
            this.Controls.Add(this.plotView);
        }

        public void DisplayPlot(List<Track.Point> values)
        {
            var plotModel = new PlotModel { Title = "" };
            var legend = new Legend
            {
                LegendTitle = "",
                LegendTitleFontSize = 14,
                LegendFontSize = 12,
                LegendPosition = LegendPosition.BottomLeft,
                LegendPlacement = LegendPlacement.Inside,
                LegendOrientation = LegendOrientation.Vertical,
                LegendBorderThickness = 1
            };
            plotModel.Legends.Add(legend);

            var primaryYAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Key = "Primary",
                Title = "Výška (m.n.m.)",
                TitleFontSize = 18,
                FontSize = 14
            };
            var secondaryYAxis = new LinearAxis
            {
                Position = AxisPosition.Right,
                Key = "Secondary",
                Title = "Výkon (W)",
                StartPosition = 0,
                EndPosition = 0.7,
                TitleFontSize = 18,
                FontSize = 14,
                Minimum = 0
            };
            var tertiaryYAxis = new LinearAxis
            {
                Position = AxisPosition.Right,
                Key = "Tertiary",
                Title = "Rychlost (km/h)",
                //Title = "W' (J)",              ////// change back
                StartPosition = 0.7,
                EndPosition = 1,
                TitleFontSize = 18,
                FontSize = 14,
                Minimum = 0
            };
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Vzdálenost (m)",
                TitleFontSize = 18,
                FontSize = 14
            };

            plotModel.Axes.Add(primaryYAxis);
            plotModel.Axes.Add(secondaryYAxis);
            plotModel.Axes.Add(tertiaryYAxis);
            plotModel.Axes.Add(xAxis);

            List<double> distance = new List<double>(values.Count());
            distance.Add(0);
            for (int i = 0; i < values.Count() - 1; i++)
            {
                distance.Add(distance[i] + values[i].dist_to_next);
            }


            var lineSeries1 = new LineSeries { Title = "Výška (h)", YAxisKey = "Primary" };
            for (int i = 0; i < values.Count; i++)
            {
                lineSeries1.Points.Add(new DataPoint(distance[i], values[i].alt));
            }

            var lineSeries2 = new LineSeries { Title = "Výkon (P)", YAxisKey = "Secondary" };
            for (int i = 0; i < values.Count; i++)
            {
                lineSeries2.Points.Add(new DataPoint(distance[i], values[i].power));
            }

            var lineSeries3 = new LineSeries { Title = "Rychlost (v)", YAxisKey = "Tertiary" };
            //var lineSeries3 = new LineSeries { Title = "W' zbývající", YAxisKey = "Tertiary" }; // for W' with CP model
            for (int i = 0; i < values.Count; i++)
            {
                lineSeries3.Points.Add(new DataPoint(distance[i], values[i].velocity * 3.6));
            }


            plotModel.Series.Add(lineSeries1);
            plotModel.Series.Add(lineSeries2);
            plotModel.Series.Add(lineSeries3);

            this.plotView.Model = plotModel;
        }       

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string filePath = openFileDialog1.FileName;
            try
            {
                track = new Track(filePath);
                DisplayPlot(track.track);
                MessageBox.Show("Soubor úspěšně nahrán");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Soubor se nepodařilo nahrát");
            }
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            using (CyclistSettingsDialog dialog = new CyclistSettingsDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    double power = (double)dialog.numericUpDownPower.Value;
                    double maxPower = 3 * power;
                    int model = dialog.comboBoxFunction.SelectedIndex;
                    double crr = (double)dialog.numericUpDownCrr.Value;
                    double weight = (double)dialog.numericUpDownWeight.Value;
                    double efficiency = (double)dialog.numericUpDownEfficiency.Value;
                    int tresholdType = dialog.comboBoxCdAOption.SelectedIndex;
                    double tresholdValue = (double)dialog.numericUpDownThreshold.Value;
                    double cdaNormal = (double)dialog.numericUpDownCdA1.Value;
                    double cdaStanding = (double)dialog.numericUpDownCdA2.Value;
                    int bankAngle = (int)dialog.numericBankAngle.Value;
                    int brakeForce = (int)dialog.numericBrakeForce.Value;

                    Func<double, double> f, f_inv;
                    if (model==0)
                    {
                        f = x => Math.Pow(x, 1.05);
                        f_inv = x => Math.Pow(x, 1/1.05);
                    }
                    else
                    {
                        f = x => Math.Pow(x, 4);
                        f_inv = x => Math.Pow(x, 0.25);
                    }
                    double[,] CdA;
                    double[] CdA_power;
                    double[] CdA_slope;
                    if (tresholdType==0)
                    {
                        CdA_power = new double[1] {tresholdValue};
                        CdA = new double[2, 2] { { cdaNormal, cdaNormal },{ cdaStanding, cdaStanding } };
                        CdA_slope = new double[0];
                    }
                    else 
                    {
                        CdA_slope = new double[1] { tresholdValue / 100 };
                        CdA = new double[2, 2] { { cdaNormal, cdaStanding },{ cdaNormal, cdaStanding } };
                        CdA_power = new double[0];
                    }

                    cyclist = new Cyclist(power, maxPower, weight, crr, efficiency, bankAngle, brakeForce, CdA, CdA_power, CdA_slope, f, f_inv);
                    if (track != null)
                    {
                        track.crr = crr;
                        track.setCorners(cyclist);
                    }
                    MessageBox.Show("Nastavení cyklisty aktualizovány");
                }
            }
        }

        private async void ToolStripButton3_Click(object sender, EventArgs e)
        {
            if (track != null && cyclist!=null)
            {
                track.initialSolution(cyclist);
                var trackTask = Task.Run(() => track.solveWithCorners(cyclist, 100, 0.95, 100));
                var progressTask = UpdateProgressAsync();

                await Task.WhenAll(trackTask, progressTask);
                DisplayPlot(track.track);
                MessageBox.Show("Řešení nalezeno. \n\nAP: "+ (int)track.weightedAveragePower(cAvg)+" W\nNP: "+(int)track.weightedAveragePower(cNorm)+" W\nDoba jízdy: "+ (int)track.track.Last().time+" s");
            }
            else
            {
                MessageBox.Show("Není nahraná trať nebo nastaveny parametry cyklisty");
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (track != null)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;
                    track.saveGPX(filePath);
                    MessageBox.Show("Soubor uložen");
                }
            }
            else
            {
                MessageBox.Show("Není nahrána žádná trasa pro uložení");
            }
        }

        private async Task UpdateProgressAsync()
        {
            while (track.progress < 1)
            {
                toolStripProgressBar1.Value = (int)(track.progress * 100);
                await Task.Delay(1);
            }
            toolStripProgressBar1.Value = 0;
            track.progress = 0;
        }


        private void MainWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
