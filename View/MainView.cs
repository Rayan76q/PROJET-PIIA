using System;
using System.Windows.Forms;


namespace PROJET_PIIA {
    partial class MainView : Form {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public MainView() {
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
            button3 = new Button();
            Redo = new Button();
            Undo = new Button();
            toggleButton = new Button();
            fontDialog1 = new FontDialog();
            showgrid = new Button();
            zoombar = new TrackBar();
            rotate = new Button();
            labelsurface = new Label();
            label1 = new Label();
            toolStrip1 = new ToolStrip();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)zoombar).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Bottom;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 25);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = Color.Silver;
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
            splitContainer1.Size = new Size(984, 381);
            splitContainer1.SplitterDistance = 245;
            splitContainer1.TabIndex = 1;
            splitContainer1.TabStop = false;
            // 
            // button3
            // 
            button3.Location = new Point(3, 59);
            button3.Name = "button3";
            button3.Size = new Size(125, 50);
            button3.TabIndex = 3;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            // 
            // Redo
            // 
            Redo.Location = new Point(78, 3);
            Redo.Name = "Redo";
            Redo.Size = new Size(50, 50);
            Redo.TabIndex = 2;
            Redo.Text = "button2";
            Redo.UseVisualStyleBackColor = true;
            // 
            // Undo
            // 
            Undo.Location = new Point(3, 3);
            Undo.Name = "Undo";
            Undo.Size = new Size(50, 50);
            Undo.TabIndex = 1;
            Undo.Text = "button1";
            Undo.UseVisualStyleBackColor = true;
            // 
            // toggleButton
            // 
            toggleButton.Anchor = AnchorStyles.Left;
            toggleButton.CausesValidation = false;
            toggleButton.Location = new Point(3, 144);
            toggleButton.Name = "toggleButton";
            toggleButton.Size = new Size(32, 102);
            toggleButton.TabIndex = 0;
            toggleButton.Text = ">";
            toggleButton.UseVisualStyleBackColor = true;
            toggleButton.Click += button1_Click;
            // 
            // showgrid
            // 
            showgrid.Location = new Point(673, 319);
            showgrid.Name = "showgrid";
            showgrid.Size = new Size(50, 50);
            showgrid.TabIndex = 4;
            showgrid.Text = "button4";
            showgrid.UseVisualStyleBackColor = true;
            // 
            // zoombar
            // 
            zoombar.Location = new Point(3, 325);
            zoombar.Name = "zoombar";
            zoombar.Size = new Size(150, 56);
            zoombar.TabIndex = 5;
            // 
            // rotate
            // 
            rotate.Location = new Point(673, 263);
            rotate.Name = "rotate";
            rotate.Size = new Size(50, 50);
            rotate.TabIndex = 6;
            rotate.Text = "button1";
            rotate.UseVisualStyleBackColor = true;
            // 
            // labelsurface
            // 
            labelsurface.AutoSize = true;
            labelsurface.Location = new Point(606, 18);
            labelsurface.Name = "labelsurface";
            labelsurface.Size = new Size(82, 20);
            labelsurface.TabIndex = 7;
            labelsurface.Text = "Superficie :";
            labelsurface.Click += labelsurface_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(606, 38);
            label1.Name = "label1";
            label1.Size = new Size(45, 20);
            label1.TabIndex = 8;
            label1.Text = "utile :";
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(984, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 406);
            Controls.Add(toolStrip1);
            Controls.Add(splitContainer1);
            Name = "MainView";
            Text = "Form1";
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
        private Button toggleButton;

        private void labelsurface_Click(object sender, EventArgs e) {

        }
    }
}
