namespace PROJET_PIIA.View {
    partial class MurSidePanel {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent() {
            flowLayoutPanel1 = new FlowLayoutPanel();
            splitContainer1 = new SplitContainer();
            buttonScaling = new Button();
            searchBox = new TextBox();
            label1 = new Label();
            splittousPorteFenetre = new SplitContainer();
            buttonFenetre = new Button();
            buttonPorte = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splittousPorteFenetre).BeginInit();
            splittousPorteFenetre.Panel1.SuspendLayout();
            splittousPorteFenetre.Panel2.SuspendLayout();
            splittousPorteFenetre.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = SystemColors.AppWorkspace;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(357, 366);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(buttonScaling);
            splitContainer1.Panel1.Controls.Add(searchBox);
            splitContainer1.Panel1.Controls.Add(label1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel1);
            splitContainer1.Size = new Size(357, 459);
            splitContainer1.SplitterDistance = 89;
            splitContainer1.TabIndex = 2;
            // 
            // buttonScaling
            // 
            buttonScaling.Location = new Point(249, 24);
            buttonScaling.Name = "buttonScaling";
            buttonScaling.Size = new Size(55, 29);
            buttonScaling.TabIndex = 2;
            buttonScaling.Text = "✓";
            buttonScaling.UseVisualStyleBackColor = true;
            buttonScaling.Click += buttonScaling_Click;
            // 
            // searchBox
            // 
            searchBox.Location = new Point(112, 25);
            searchBox.Name = "searchBox";
            searchBox.Size = new Size(118, 27);
            searchBox.TabIndex = 1;
            searchBox.KeyDown += searchBox_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 28);
            label1.Name = "label1";
            label1.Size = new Size(82, 20);
            label1.TabIndex = 0;
            label1.Text = "Superficie :";
            // 
            // splittousPorteFenetre
            // 
            splittousPorteFenetre.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splittousPorteFenetre.FixedPanel = FixedPanel.Panel2;
            splittousPorteFenetre.Location = new Point(0, 0);
            splittousPorteFenetre.Name = "splittousPorteFenetre";
            splittousPorteFenetre.Orientation = Orientation.Horizontal;
            // 
            // splittousPorteFenetre.Panel1
            // 
            splittousPorteFenetre.Panel1.Controls.Add(splitContainer1);
            // 
            // splittousPorteFenetre.Panel2
            // 
            splittousPorteFenetre.Panel2.Controls.Add(buttonFenetre);
            splittousPorteFenetre.Panel2.Controls.Add(buttonPorte);
            splittousPorteFenetre.Size = new Size(357, 522);
            splittousPorteFenetre.SplitterDistance = 459;
            splittousPorteFenetre.TabIndex = 0;
            splittousPorteFenetre.SplitterMoved += splittousPorteFenetre_SplitterMoved;
            // 
            // buttonFenetre
            // 
            buttonFenetre.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonFenetre.BackColor = Color.LightBlue;
            buttonFenetre.Cursor = Cursors.Hand;
            buttonFenetre.Location = new Point(164, 21);
            buttonFenetre.Name = "buttonFenetre";
            buttonFenetre.Size = new Size(92, 35);
            buttonFenetre.TabIndex = 1;
            buttonFenetre.Text = "Fenetre";
            buttonFenetre.UseVisualStyleBackColor = false;
            buttonFenetre.MouseDown += buttonFenetre_MouseDown;
            // 
            // buttonPorte
            // 
            buttonPorte.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonPorte.BackColor = Color.LightBlue;
            buttonPorte.Cursor = Cursors.Hand;
            buttonPorte.Location = new Point(24, 21);
            buttonPorte.Name = "buttonPorte";
            buttonPorte.Size = new Size(92, 35);
            buttonPorte.TabIndex = 0;
            buttonPorte.Text = "🚪 Porte";
            buttonPorte.UseVisualStyleBackColor = false;
            buttonPorte.MouseDown += buttonPorte_MouseDown;
            // 
            // MurSidePanel
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splittousPorteFenetre);
            Name = "MurSidePanel";
            Size = new Size(357, 522);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splittousPorteFenetre.Panel1.ResumeLayout(false);
            splittousPorteFenetre.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splittousPorteFenetre).EndInit();
            splittousPorteFenetre.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private SplitContainer splitContainer1;
        private TextBox searchBox;
        private Label label1;
        private SplitContainer splittousPorteFenetre;
        private Button buttonPorte;
        private Button buttonFenetre;
        private Button buttonScaling;
    }
}
