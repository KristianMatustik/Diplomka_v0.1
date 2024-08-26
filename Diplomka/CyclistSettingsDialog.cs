using System;
using System.Windows.Forms;

namespace Diplomka
{
    public class CyclistSettingsDialog : Form
    {
        public NumericUpDown numericUpDownPower;
        public ComboBox comboBoxFunction;
        public NumericUpDown numericUpDownCrr;
        public NumericUpDown numericUpDownWeight;
        public NumericUpDown numericUpDownEfficiency;
        public ComboBox comboBoxCdAOption;
        public NumericUpDown numericUpDownThreshold;
        public NumericUpDown numericUpDownCdA1;
        public NumericUpDown numericUpDownCdA2;
        public NumericUpDown numericBankAngle;
        public NumericUpDown numericBrakeForce;
        public Button buttonOk;
        public Button buttonCancel;
        private Label labelPower;
        private Label labelFunction;
        private Label labelCrr;
        private Label labelWeight;
        private Label labelEfficiency;
        private Label labelCdAOption;
        private Label labelThreshold;
        private Label labelCdA1;
        private Label labelCdA2;
        private Label labelBankAngle;
        private Label labelBrakeForce;

        public CyclistSettingsDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.numericUpDownPower = new System.Windows.Forms.NumericUpDown();
            this.comboBoxFunction = new System.Windows.Forms.ComboBox();
            this.numericUpDownCrr = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWeight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownEfficiency = new System.Windows.Forms.NumericUpDown();
            this.comboBoxCdAOption = new System.Windows.Forms.ComboBox();
            this.numericUpDownThreshold = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCdA1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCdA2 = new System.Windows.Forms.NumericUpDown();
            this.numericBankAngle = new System.Windows.Forms.NumericUpDown();
            this.numericBrakeForce = new System.Windows.Forms.NumericUpDown();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelPower = new System.Windows.Forms.Label();
            this.labelFunction = new System.Windows.Forms.Label();
            this.labelCrr = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelEfficiency = new System.Windows.Forms.Label();
            this.labelCdAOption = new System.Windows.Forms.Label();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.labelCdA1 = new System.Windows.Forms.Label();
            this.labelCdA2 = new System.Windows.Forms.Label();
            this.labelBankAngle = new System.Windows.Forms.Label();
            this.labelBrakeForce = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCrr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEfficiency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCdA1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCdA2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericBankAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericBrakeForce)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownPower
            // 
            this.numericUpDownPower.Location = new System.Drawing.Point(180, 10);
            this.numericUpDownPower.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownPower.Name = "numericUpDownPower";
            this.numericUpDownPower.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownPower.TabIndex = 1;
            this.numericUpDownPower.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // comboBoxFunction
            // 
            this.comboBoxFunction.Items.AddRange(new object[] {
            "Average",
            "Normalized"});
            this.comboBoxFunction.Location = new System.Drawing.Point(180, 40);
            this.comboBoxFunction.Name = "comboBoxFunction";
            this.comboBoxFunction.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFunction.TabIndex = 3;
            // 
            // numericUpDownCrr
            // 
            this.numericUpDownCrr.DecimalPlaces = 4;
            this.numericUpDownCrr.Increment = new decimal(new int[] {
            5,
            0,
            0,
            262144});
            this.numericUpDownCrr.Location = new System.Drawing.Point(180, 70);
            this.numericUpDownCrr.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownCrr.Name = "numericUpDownCrr";
            this.numericUpDownCrr.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownCrr.TabIndex = 5;
            this.numericUpDownCrr.Value = new decimal(new int[] {
            4,
            0,
            0,
            196608});
            // 
            // numericUpDownWeight
            // 
            this.numericUpDownWeight.Location = new System.Drawing.Point(180, 100);
            this.numericUpDownWeight.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.numericUpDownWeight.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownWeight.Name = "numericUpDownWeight";
            this.numericUpDownWeight.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWeight.TabIndex = 7;
            this.numericUpDownWeight.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // numericUpDownEfficiency
            // 
            this.numericUpDownEfficiency.DecimalPlaces = 2;
            this.numericUpDownEfficiency.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDownEfficiency.Location = new System.Drawing.Point(180, 130);
            this.numericUpDownEfficiency.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownEfficiency.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            this.numericUpDownEfficiency.Name = "numericUpDownEfficiency";
            this.numericUpDownEfficiency.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownEfficiency.TabIndex = 9;
            this.numericUpDownEfficiency.Value = new decimal(new int[] {
            98,
            0,
            0,
            131072});
            // 
            // comboBoxCdAOption
            // 
            this.comboBoxCdAOption.Items.AddRange(new object[] {
            "Power",
            "Slope"});
            this.comboBoxCdAOption.Location = new System.Drawing.Point(180, 160);
            this.comboBoxCdAOption.Name = "comboBoxCdAOption";
            this.comboBoxCdAOption.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCdAOption.TabIndex = 11;
            // 
            // numericUpDownThreshold
            // 
            this.numericUpDownThreshold.Location = new System.Drawing.Point(180, 190);
            this.numericUpDownThreshold.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownThreshold.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDownThreshold.Name = "numericUpDownThreshold";
            this.numericUpDownThreshold.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownThreshold.TabIndex = 13;
            // 
            // numericUpDownCdA1
            // 
            this.numericUpDownCdA1.DecimalPlaces = 4;
            this.numericUpDownCdA1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.numericUpDownCdA1.Location = new System.Drawing.Point(180, 220);
            this.numericUpDownCdA1.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCdA1.Name = "numericUpDownCdA1";
            this.numericUpDownCdA1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownCdA1.TabIndex = 15;
            this.numericUpDownCdA1.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            // 
            // numericUpDownCdA2
            // 
            this.numericUpDownCdA2.DecimalPlaces = 4;
            this.numericUpDownCdA2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.numericUpDownCdA2.Location = new System.Drawing.Point(180, 250);
            this.numericUpDownCdA2.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCdA2.Name = "numericUpDownCdA2";
            this.numericUpDownCdA2.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownCdA2.TabIndex = 17;
            this.numericUpDownCdA2.Value = new decimal(new int[] {
            3,
            0,
            0,
            65536});
            // 
            // numericBankAngle
            // 
            this.numericBankAngle.Location = new System.Drawing.Point(180, 280);
            this.numericBankAngle.Maximum = new decimal(new int[] {
            89,
            0,
            0,
            0});
            this.numericBankAngle.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericBankAngle.Name = "numericBankAngle";
            this.numericBankAngle.Size = new System.Drawing.Size(120, 20);
            this.numericBankAngle.TabIndex = 19;
            this.numericBankAngle.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // numericBrakeForce
            // 
            this.numericBrakeForce.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericBrakeForce.Location = new System.Drawing.Point(180, 310);
            this.numericBrakeForce.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericBrakeForce.Name = "numericBrakeForce";
            this.numericBrakeForce.Size = new System.Drawing.Size(120, 20);
            this.numericBrakeForce.TabIndex = 21;
            this.numericBrakeForce.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(60, 350);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 23;
            this.buttonOk.Text = "OK";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(180, 350);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 25;
            this.buttonCancel.Text = "Zrušit";
            // 
            // labelPower
            // 
            this.labelPower.Location = new System.Drawing.Point(10, 10);
            this.labelPower.Name = "labelPower";
            this.labelPower.Size = new System.Drawing.Size(150, 20);
            this.labelPower.TabIndex = 0;
            this.labelPower.Text = "Omezení výkonu (W):";
            // 
            // labelFunction
            // 
            this.labelFunction.Location = new System.Drawing.Point(10, 40);
            this.labelFunction.Name = "labelFunction";
            this.labelFunction.Size = new System.Drawing.Size(150, 20);
            this.labelFunction.TabIndex = 1;
            this.labelFunction.Text = "Model optimalizace:";
            // 
            // labelCrr
            // 
            this.labelCrr.Location = new System.Drawing.Point(10, 70);
            this.labelCrr.Name = "labelCrr";
            this.labelCrr.Size = new System.Drawing.Size(150, 20);
            this.labelCrr.TabIndex = 2;
            this.labelCrr.Text = "Crr:";
            // 
            // labelWeight
            // 
            this.labelWeight.Location = new System.Drawing.Point(10, 100);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(150, 20);
            this.labelWeight.TabIndex = 3;
            this.labelWeight.Text = "Váha (kg):";
            // 
            // labelEfficiency
            // 
            this.labelEfficiency.Location = new System.Drawing.Point(10, 130);
            this.labelEfficiency.Name = "labelEfficiency";
            this.labelEfficiency.Size = new System.Drawing.Size(150, 20);
            this.labelEfficiency.TabIndex = 4;
            this.labelEfficiency.Text = "Účinnost:";
            // 
            // labelCdAOption
            // 
            this.labelCdAOption.Location = new System.Drawing.Point(10, 160);
            this.labelCdAOption.Name = "labelCdAOption";
            this.labelCdAOption.Size = new System.Drawing.Size(150, 20);
            this.labelCdAOption.TabIndex = 5;
            this.labelCdAOption.Text = "Hranice změny CdA podle:";
            // 
            // labelThreshold
            // 
            this.labelThreshold.Location = new System.Drawing.Point(10, 190);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(150, 20);
            this.labelThreshold.TabIndex = 6;
            this.labelThreshold.Text = "Hodnota hranice (W or %):";
            // 
            // labelCdA1
            // 
            this.labelCdA1.Location = new System.Drawing.Point(10, 220);
            this.labelCdA1.Name = "labelCdA1";
            this.labelCdA1.Size = new System.Drawing.Size(150, 20);
            this.labelCdA1.TabIndex = 7;
            this.labelCdA1.Text = "CdA TT:";
            // 
            // labelCdA2
            // 
            this.labelCdA2.Location = new System.Drawing.Point(10, 250);
            this.labelCdA2.Name = "labelCdA2";
            this.labelCdA2.Size = new System.Drawing.Size(150, 20);
            this.labelCdA2.TabIndex = 8;
            this.labelCdA2.Text = "CdA ze sedla:";
            // 
            // labelBankAngle
            // 
            this.labelBankAngle.Location = new System.Drawing.Point(10, 280);
            this.labelBankAngle.Name = "labelBankAngle";
            this.labelBankAngle.Size = new System.Drawing.Size(150, 20);
            this.labelBankAngle.TabIndex = 9;
            this.labelBankAngle.Text = "Úhel náklonu:";
            // 
            // labelBrakeForce
            // 
            this.labelBrakeForce.Location = new System.Drawing.Point(10, 310);
            this.labelBrakeForce.Name = "labelBrakeForce";
            this.labelBrakeForce.Size = new System.Drawing.Size(150, 20);
            this.labelBrakeForce.TabIndex = 10;
            this.labelBrakeForce.Text = "Síla brzdění (N):";
            // 
            // CyclistSettingsDialog
            // 
            this.ClientSize = new System.Drawing.Size(330, 390);
            this.Controls.Add(this.labelPower);
            this.Controls.Add(this.labelFunction);
            this.Controls.Add(this.labelCrr);
            this.Controls.Add(this.labelWeight);
            this.Controls.Add(this.labelEfficiency);
            this.Controls.Add(this.labelCdAOption);
            this.Controls.Add(this.labelThreshold);
            this.Controls.Add(this.labelCdA1);
            this.Controls.Add(this.labelCdA2);
            this.Controls.Add(this.labelBankAngle);
            this.Controls.Add(this.labelBrakeForce);
            this.Controls.Add(this.numericUpDownPower);
            this.Controls.Add(this.comboBoxFunction);
            this.Controls.Add(this.numericUpDownCrr);
            this.Controls.Add(this.numericUpDownWeight);
            this.Controls.Add(this.numericUpDownEfficiency);
            this.Controls.Add(this.comboBoxCdAOption);
            this.Controls.Add(this.numericUpDownThreshold);
            this.Controls.Add(this.numericUpDownCdA1);
            this.Controls.Add(this.numericUpDownCdA2);
            this.Controls.Add(this.numericBankAngle);
            this.Controls.Add(this.numericBrakeForce);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Name = "Nastavení dialog";
            this.Text = "Nastavení";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCrr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEfficiency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCdA1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCdA2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericBankAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericBrakeForce)).EndInit();
            this.ResumeLayout(false);

        }
    }
}