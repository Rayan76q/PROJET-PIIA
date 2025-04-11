using System;
using System.Windows.Forms;


namespace PROJET_PIIA.Modele {
    partial public class MainView : Form {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private PlanView planView;

        private Controleur ctr ;



        public MainView() {
            ctr = new Controleur(new Modele(), this);
            InitializeComponent();

        }

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            splitContainer1 = new SplitContainer();
            label2 = new Label();
            button1 = new Button();
            label1 = new Label();
            labelsurface = new Label();
            rotate = new Button();
            zoombar = new TrackBar();
            showgrid = new Button();
            button3 = new Button();
            Redo = new Button();
            Undo = new Button();
            toggleButton = new Button();
            fontDialog1 = new FontDialog();
            toolStrip1 = new ToolStrip();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)zoombar).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 25);
            splitContainer1.Margin = new Padding(3, 2, 3, 2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = Color.Silver;
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(button1);
            splitContainer1.Panel1.Paint += splitContainer1_Panel1_Paint;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(labelsurface);
            splitContainer1.Panel2.Controls.Add(rotate);
            splitContainer1.Panel2.Controls.Add(zoombar);
            splitContainer1.Panel2.Controls.Add(showgrid);
            splitContainer1.Panel2.Controls.Add(button3);
            splitContainer1.Panel2.Controls.Add(Redo);
            splitContainer1.Panel2.Controls.Add(Undo);
            splitContainer1.Panel2.Controls.Add(toggleButton);
            splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
            splitContainer1.Size = new Size(905, 391);
            splitContainer1.SplitterDistance = 220;
            splitContainer1.TabIndex = 1;
            splitContainer1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 103);
            label2.Name = "label2";
            label2.Size = new Size(123, 15);
            label2.TabIndex = 1;
            label2.Text = "Rectangulaire 600x400";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.Click += label2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(3, 32);
            button1.Name = "button1";
            button1.Size = new Size(214, 68);
            button1.TabIndex = 0;
            button1.Text = "Plan Rectangulaire";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(495, 28);
            label1.Name = "label1";
            label1.Size = new Size(36, 15);
            label1.TabIndex = 8;
            label1.Text = "utile :";
            // 
            // labelsurface
            // 
            labelsurface.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelsurface.AutoSize = true;
            labelsurface.Location = new Point(495, 14);
            labelsurface.Name = "labelsurface";
            labelsurface.Size = new Size(65, 15);
            labelsurface.TabIndex = 7;
            labelsurface.Text = "Superficie :";
            labelsurface.Click += labelsurface_Click;
            // 
            // rotate
            // 
            rotate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            rotate.Location = new Point(620, 288);
            rotate.Margin = new Padding(3, 2, 3, 2);
            rotate.Name = "rotate";
            rotate.Size = new Size(44, 38);
            rotate.TabIndex = 6;
            rotate.Text = "button1";
            rotate.UseVisualStyleBackColor = true;
            // 
            // zoombar
            // 
            zoombar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            zoombar.Location = new Point(3, 333);
            zoombar.Margin = new Padding(3, 2, 3, 2);
            zoombar.Name = "zoombar";
            zoombar.Size = new Size(131, 45);
            zoombar.TabIndex = 5;
            // 
            // showgrid
            // 
            showgrid.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            showgrid.Location = new Point(620, 340);
            showgrid.Margin = new Padding(3, 2, 3, 2);
            showgrid.Name = "showgrid";
            showgrid.Size = new Size(44, 38);
            showgrid.TabIndex = 4;
            showgrid.Text = "button4";
            showgrid.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(3, 47);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(109, 38);
            button3.TabIndex = 3;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            // 
            // Redo
            // 
            Redo.Location = new Point(68, 5);
            Redo.Margin = new Padding(3, 2, 3, 2);
            Redo.Name = "Redo";
            Redo.Size = new Size(44, 38);
            Redo.TabIndex = 2;
            Redo.Text = "button2";
            Redo.UseVisualStyleBackColor = true;
            // 
            // Undo
            // 
            Undo.Location = new Point(3, 5);
            Undo.Margin = new Padding(3, 2, 3, 2);
            Undo.Name = "Undo";
            Undo.Size = new Size(44, 38);
            Undo.TabIndex = 1;
            Undo.Text = "button1";
            Undo.UseVisualStyleBackColor = true;
            Undo.Click += Undo_Click;
            // 
            // toggleButton
            // 
            toggleButton.Anchor = AnchorStyles.Left;
            toggleButton.CausesValidation = false;
            toggleButton.Location = new Point(3, 185);
            toggleButton.Margin = new Padding(3, 2, 3, 2);
            toggleButton.Name = "toggleButton";
            toggleButton.Size = new Size(28, 76);
            toggleButton.TabIndex = 0;
            toggleButton.Text = ">";
            toggleButton.UseVisualStyleBackColor = true;
            toggleButton.Click += button1_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(905, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(905, 416);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainView";
            Text = "Form1";
            Load += MainView_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)zoombar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e) {
            if (splitContainer1.Panel1Collapsed) {
                splitContainer1.Panel1Collapsed = false;
                toggleButton.Text = "<";  // Barre ouverte, bouton pointant à droite
            } else {
                splitContainer1.Panel1Collapsed = true;
                toggleButton.Text = ">";  // Barre fermée, bouton pointant à gauche
            }
        }
        private SplitContainer splitContainer1;
        private FontDialog fontDialog1;
        private Button button3;
        private Button Redo;
        private Button Undo;
        private Button showgrid;
        private TrackBar zoombar;
        private Label labelsurface;
        private Button rotate;
        private Label label1;
        private ToolStrip toolStrip1;
        private Button button1;
        private Label label2;
        private Button toggleButton;

        private void labelsurface_Click(object sender, EventArgs e) {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e) {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e) {

        }

        private void MainView_Load(object sender, EventArgs e) {

        }

        private void Undo_Click(object sender, EventArgs e) {

        }

        private void button1_Click_1(object sender, EventArgs e) {
            Murs m = new Murs(new List<Position> { new Position(0, 0), new Position(600, 0), new Position(600, 400), new Position(0, 400) });
            ctr.setMurs(m);
            
        }

        private void label2_Click(object sender, EventArgs e) {

        }
    }
}
