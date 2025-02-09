using System.Drawing;
using System.Windows.Forms;

namespace BomberUI
{
    partial class MapSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSelect));
            mapSelectText = new Label();
            smallMapPictureBox = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            pictureBox5 = new PictureBox();
            pictureBox6 = new PictureBox();
            pictureBox7 = new PictureBox();
            pictureBox8 = new PictureBox();
            pictureBox9 = new PictureBox();
            pictureBox10 = new PictureBox();
            pictureBox11 = new PictureBox();
            mediumMapPictureBox = new PictureBox();
            largeMapPictureBox = new PictureBox();
            customMapPictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)smallMapPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mediumMapPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)largeMapPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)customMapPictureBox).BeginInit();
            SuspendLayout();
            // 
            // mapSelectText
            // 
            mapSelectText.AutoSize = true;
            mapSelectText.Font = new Font("Palatino Linotype", 48F, FontStyle.Bold, GraphicsUnit.Point, 238);
            mapSelectText.ForeColor = SystemColors.ControlText;
            mapSelectText.Location = new Point(95, 23);
            mapSelectText.Name = "mapSelectText";
            mapSelectText.Size = new Size(607, 87);
            mapSelectText.TabIndex = 0;
            mapSelectText.Text = "Please select a map!";
            // 
            // smallMapPictureBox
            // 
            smallMapPictureBox.Image = Properties.Resources._10x10;
            smallMapPictureBox.Location = new Point(27, 130);
            smallMapPictureBox.Name = "smallMapPictureBox";
            smallMapPictureBox.Size = new Size(160, 160);
            smallMapPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            smallMapPictureBox.TabIndex = 1;
            smallMapPictureBox.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Palatino Linotype", 24F, FontStyle.Bold);
            label1.Location = new Point(55, 368);
            label1.Name = "label1";
            label1.Size = new Size(99, 44);
            label1.TabIndex = 5;
            label1.Text = "10x10";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Palatino Linotype", 24F, FontStyle.Bold);
            label2.Location = new Point(251, 368);
            label2.Name = "label2";
            label2.Size = new Size(99, 44);
            label2.TabIndex = 6;
            label2.Text = "15x15";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Palatino Linotype", 24F, FontStyle.Bold);
            label3.Location = new Point(441, 368);
            label3.Name = "label3";
            label3.Size = new Size(99, 44);
            label3.TabIndex = 7;
            label3.Text = "20x20";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Palatino Linotype", 24F, FontStyle.Bold);
            label4.Location = new Point(624, 368);
            label4.Name = "label4";
            label4.Size = new Size(133, 44);
            label4.TabIndex = 8;
            label4.Text = "Custom";
            // 
            // pictureBox5
            // 
            pictureBox5.Image = Properties.Resources.skull;
            pictureBox5.Location = new Point(78, 315);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(50, 50);
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.TabIndex = 9;
            pictureBox5.TabStop = false;
            // 
            // pictureBox6
            // 
            pictureBox6.Image = Properties.Resources.skull;
            pictureBox6.Location = new Point(246, 315);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(50, 50);
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.TabIndex = 10;
            pictureBox6.TabStop = false;
            // 
            // pictureBox7
            // 
            pictureBox7.Image = Properties.Resources.skull;
            pictureBox7.Location = new Point(304, 315);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(50, 50);
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.TabIndex = 11;
            pictureBox7.TabStop = false;
            // 
            // pictureBox8
            // 
            pictureBox8.Image = Properties.Resources.skull;
            pictureBox8.Location = new Point(407, 315);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(50, 50);
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 12;
            pictureBox8.TabStop = false;
            // 
            // pictureBox9
            // 
            pictureBox9.Image = Properties.Resources.skull;
            pictureBox9.Location = new Point(467, 315);
            pictureBox9.Name = "pictureBox9";
            pictureBox9.Size = new Size(50, 50);
            pictureBox9.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox9.TabIndex = 13;
            pictureBox9.TabStop = false;
            // 
            // pictureBox10
            // 
            pictureBox10.Image = Properties.Resources.skull;
            pictureBox10.Location = new Point(526, 315);
            pictureBox10.Name = "pictureBox10";
            pictureBox10.Size = new Size(50, 50);
            pictureBox10.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox10.TabIndex = 14;
            pictureBox10.TabStop = false;
            // 
            // pictureBox11
            // 
            pictureBox11.Image = Properties.Resources.skull;
            pictureBox11.Location = new Point(669, 315);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new Size(50, 50);
            pictureBox11.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox11.TabIndex = 15;
            pictureBox11.TabStop = false;
            // 
            // mediumMapPictureBox
            // 
            mediumMapPictureBox.Image = Properties.Resources._15x15;
            mediumMapPictureBox.Location = new Point(222, 130);
            mediumMapPictureBox.Name = "mediumMapPictureBox";
            mediumMapPictureBox.Size = new Size(160, 160);
            mediumMapPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            mediumMapPictureBox.TabIndex = 19;
            mediumMapPictureBox.TabStop = false;
            // 
            // largeMapPictureBox
            // 
            largeMapPictureBox.Image = Properties.Resources._20x20;
            largeMapPictureBox.Location = new Point(416, 130);
            largeMapPictureBox.Name = "largeMapPictureBox";
            largeMapPictureBox.Size = new Size(160, 160);
            largeMapPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            largeMapPictureBox.TabIndex = 20;
            largeMapPictureBox.TabStop = false;
            // 
            // customMapPictureBox
            // 
            customMapPictureBox.Image = Properties.Resources.customMap;
            customMapPictureBox.Location = new Point(613, 130);
            customMapPictureBox.Name = "customMapPictureBox";
            customMapPictureBox.Size = new Size(160, 160);
            customMapPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            customMapPictureBox.TabIndex = 21;
            customMapPictureBox.TabStop = false;
            // 
            // MapSelect
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(800, 450);
            Controls.Add(customMapPictureBox);
            Controls.Add(largeMapPictureBox);
            Controls.Add(mediumMapPictureBox);
            Controls.Add(pictureBox11);
            Controls.Add(pictureBox10);
            Controls.Add(pictureBox9);
            Controls.Add(pictureBox8);
            Controls.Add(pictureBox7);
            Controls.Add(pictureBox6);
            Controls.Add(pictureBox5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(smallMapPictureBox);
            Controls.Add(mapSelectText);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MapSelect";
            Text = "MapSelect";
            ((System.ComponentModel.ISupportInitialize)smallMapPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox10).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            ((System.ComponentModel.ISupportInitialize)mediumMapPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)largeMapPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)customMapPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label mapSelectText;
        private PictureBox smallMapPictureBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private PictureBox pictureBox5;
        private PictureBox pictureBox6;
        private PictureBox pictureBox7;
        private PictureBox pictureBox8;
        private PictureBox pictureBox9;
        private PictureBox pictureBox10;
        private PictureBox pictureBox11;
        private PictureBox mediumMapPictureBox;
        private PictureBox largeMapPictureBox;
        private PictureBox customMapPictureBox;
    }
}