using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Model;


namespace PROJET_PIIA.View {
    public enum PlanMode { // jsp où mettre
        Deplacement,
        DessinPolygone
    }

    public partial class MainView : Form {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private PlanView planView;
        private Button switchmodebutton;
        private ControleurMainView ctrg;


        public MainView(Modele m) {
            InitializeComponent();
            ctrg = new ControleurMainView(m);
            planView = new PlanView(ctrg);
            splitContainer1.Panel2.Controls.Add(planView);
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
            switchmodebutton = new Button();
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
            splitContainer1.Panel2.Controls.Add(switchmodebutton);
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
            splitContainer1.Size = new Size(1034, 530);
            splitContainer1.SplitterDistance = 251;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 1;
            splitContainer1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 137);
            label2.Name = "label2";
            label2.Size = new Size(159, 20);
            label2.TabIndex = 1;
            label2.Text = "Rectangulaire 600x400";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.Click += label2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(3, 43);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(245, 91);
            button1.TabIndex = 0;
            button1.Text = "Plan Rectangulaire";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(566, 37);
            label1.Name = "label1";
            label1.Size = new Size(45, 20);
            label1.TabIndex = 8;
            label1.Text = "utile :";
            // 
            // labelsurface
            // 
            labelsurface.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelsurface.AutoSize = true;
            labelsurface.Location = new Point(566, 19);
            labelsurface.Name = "labelsurface";
            labelsurface.Size = new Size(82, 20);
            labelsurface.TabIndex = 7;
            labelsurface.Text = "Superficie :";
            labelsurface.Click += labelsurface_Click;
            // 
            // rotate
            // 
            rotate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            rotate.Location = new Point(709, 393);
            rotate.Name = "rotate";
            rotate.Size = new Size(50, 51);
            rotate.TabIndex = 6;
            rotate.Text = "button1";
            rotate.UseVisualStyleBackColor = true;
            // 
            // zoombar
            // 
            zoombar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            zoombar.Location = new Point(3, 453);
            zoombar.Name = "zoombar";
            zoombar.Size = new Size(150, 56);
            zoombar.TabIndex = 5;
            // 
            // showgrid
            // 
            showgrid.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            showgrid.Location = new Point(709, 462);
            showgrid.Name = "showgrid";
            showgrid.Size = new Size(50, 51);
            showgrid.TabIndex = 4;
            showgrid.Text = "button4";
            showgrid.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(3, 63);
            button3.Name = "button3";
            button3.Size = new Size(125, 51);
            button3.TabIndex = 3;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            // 
            // Redo
            // 
            Redo.Location = new Point(78, 7);
            Redo.Name = "Redo";
            Redo.Size = new Size(50, 51);
            Redo.TabIndex = 2;
            Redo.Text = "button2";
            Redo.UseVisualStyleBackColor = true;
            // 
            // Undo
            // 
            Undo.Location = new Point(3, 7);
            Undo.Name = "Undo";
            Undo.Size = new Size(50, 51);
            Undo.TabIndex = 1;
            Undo.Text = "button1";
            Undo.UseVisualStyleBackColor = true;
            Undo.Click += Undo_Click;
            // 
            // toggleButton
            // 
            toggleButton.Anchor = AnchorStyles.Left;
            toggleButton.CausesValidation = false;
            toggleButton.Location = new Point(3, 252);
            toggleButton.Name = "toggleButton";
            toggleButton.Size = new Size(32, 101);
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
            toolStrip1.Size = new Size(1034, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // switchmodebutton
            // 
            switchmodebutton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            switchmodebutton.Location = new Point(364, 240);
            switchmodebutton.Name = "switchmodebutton";
            switchmodebutton.Size = new Size(50, 51);
            switchmodebutton.TabIndex = 9;
            switchmodebutton.Text = "changer mode";
            switchmodebutton.UseVisualStyleBackColor = true;
            switchmodebutton.Click += switchmodebutton_Click;
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1034, 555);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
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
            List<Point> p = [ new Point(0, 0), new Point(600, 0), new Point(600, 400), new Point(0, 400) ];
            ctrg.setMurs(p);
        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void switchmodebutton_Click(object sender, EventArgs e) {
            this.ctrg.ChangerMode();
        }
    }

}
