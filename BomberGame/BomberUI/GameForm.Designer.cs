using System.Drawing;
using System.Windows.Forms;

namespace BomberUI
{
    partial class GameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            gridPanel = new Panel();
            newGameBtn = new Button();
            elapsedTimeLabel = new Label();
            killedEnemiesLabel = new Label();
            displayTimeLabel = new Label();
            killedEnemiesCountLabel = new Label();
            pauseGameBtn = new Button();
            startGameBtn = new Button();
            SuspendLayout();
            // 
            // gridPanel
            // 
            gridPanel.Location = new Point(31, 55);
            gridPanel.Name = "gridPanel";
            gridPanel.Size = new Size(800, 800);
            gridPanel.TabIndex = 0;
            // 
            // newGameBtn
            // 
            newGameBtn.BackColor = Color.Transparent;
            newGameBtn.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            newGameBtn.Location = new Point(447, 16);
            newGameBtn.Name = "newGameBtn";
            newGameBtn.Size = new Size(107, 28);
            newGameBtn.TabIndex = 1;
            newGameBtn.Text = "New Game";
            newGameBtn.UseVisualStyleBackColor = false;
            // 
            // elapsedTimeLabel
            // 
            elapsedTimeLabel.AutoSize = true;
            elapsedTimeLabel.Font = new Font("Microsoft Sans Serif", 12F);
            elapsedTimeLabel.Location = new Point(31, 20);
            elapsedTimeLabel.Name = "elapsedTimeLabel";
            elapsedTimeLabel.Size = new Size(109, 20);
            elapsedTimeLabel.TabIndex = 4;
            elapsedTimeLabel.Text = "Elapsed Time:";
            // 
            // killedEnemiesLabel
            // 
            killedEnemiesLabel.AutoSize = true;
            killedEnemiesLabel.Font = new Font("Microsoft Sans Serif", 12F);
            killedEnemiesLabel.Location = new Point(271, 20);
            killedEnemiesLabel.Name = "killedEnemiesLabel";
            killedEnemiesLabel.Size = new Size(120, 20);
            killedEnemiesLabel.TabIndex = 5;
            killedEnemiesLabel.Text = "Killed Enemies: ";
            // 
            // displayTimeLabel
            // 
            displayTimeLabel.AutoSize = true;
            displayTimeLabel.Font = new Font("Microsoft Sans Serif", 12F);
            displayTimeLabel.Location = new Point(159, 20);
            displayTimeLabel.Name = "displayTimeLabel";
            displayTimeLabel.Size = new Size(71, 20);
            displayTimeLabel.TabIndex = 6;
            displayTimeLabel.Text = "00:00:00";
            // 
            // killedEnemiesCountLabel
            // 
            killedEnemiesCountLabel.AutoSize = true;
            killedEnemiesCountLabel.Font = new Font("Microsoft Sans Serif", 12F);
            killedEnemiesCountLabel.Location = new Point(397, 20);
            killedEnemiesCountLabel.Name = "killedEnemiesCountLabel";
            killedEnemiesCountLabel.Size = new Size(18, 20);
            killedEnemiesCountLabel.TabIndex = 7;
            killedEnemiesCountLabel.Text = "0";
            // 
            // pauseGameBtn
            // 
            pauseGameBtn.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pauseGameBtn.Location = new Point(693, 16);
            pauseGameBtn.Name = "pauseGameBtn";
            pauseGameBtn.Size = new Size(78, 28);
            pauseGameBtn.TabIndex = 8;
            pauseGameBtn.Text = "Pause";
            pauseGameBtn.UseVisualStyleBackColor = true;
            // 
            // startGameBtn
            // 
            startGameBtn.BackColor = Color.Transparent;
            startGameBtn.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            startGameBtn.Location = new Point(584, 16);
            startGameBtn.Name = "startGameBtn";
            startGameBtn.Size = new Size(78, 28);
            startGameBtn.TabIndex = 10;
            startGameBtn.Text = "Start";
            startGameBtn.UseVisualStyleBackColor = false;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(852, 876);
            Controls.Add(startGameBtn);
            Controls.Add(pauseGameBtn);
            Controls.Add(killedEnemiesCountLabel);
            Controls.Add(displayTimeLabel);
            Controls.Add(killedEnemiesLabel);
            Controls.Add(elapsedTimeLabel);
            Controls.Add(newGameBtn);
            Controls.Add(gridPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "GameForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bomber Game - YCFCIY";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel gridPanel;
        private Button newGameBtn;
        private Label elapsedTimeLabel;
        private Label killedEnemiesLabel;
        private Label displayTimeLabel;
        private Label killedEnemiesCountLabel;
        private Button pauseGameBtn;
        private Button startGameBtn;
    }
}
