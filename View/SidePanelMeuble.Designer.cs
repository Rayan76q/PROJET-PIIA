namespace PROJET_PIIA.View {
    partial class SidePanelMeuble {
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
            layout = new TableLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            seachBarPanel = new Panel();
            buttonFiltre = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            layout.SuspendLayout();
            seachBarPanel.SuspendLayout();
            SuspendLayout();
            // 
            // layout
            // 
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.Controls.Add(flowLayoutPanel1, 0, 2);
            layout.Controls.Add(seachBarPanel, 0, 0);
            layout.Controls.Add(label1, 0, 1);
            layout.Dock = DockStyle.Fill;
            layout.Location = new Point(0, 0);
            layout.Margin = new Padding(3, 2, 3, 2);
            layout.Name = "layout";
            layout.RowCount = 3;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 98F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F));
            layout.Size = new Size(276, 348);
            layout.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = SystemColors.ControlDarkDark;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(3, 130);
            flowLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(270, 216);
            flowLayoutPanel1.TabIndex = 1;
            flowLayoutPanel1.WrapContents = false;
            // 
            // seachBarPanel
            // 
            seachBarPanel.Controls.Add(buttonFiltre);
            seachBarPanel.Controls.Add(textBox1);
            seachBarPanel.Dock = DockStyle.Fill;
            seachBarPanel.Location = new Point(3, 2);
            seachBarPanel.Margin = new Padding(3, 2, 3, 2);
            seachBarPanel.Name = "seachBarPanel";
            seachBarPanel.Size = new Size(270, 26);
            seachBarPanel.TabIndex = 2;
            // 
            // buttonFiltre
            // 
            buttonFiltre.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonFiltre.Location = new Point(221, 2);
            buttonFiltre.Margin = new Padding(3, 2, 3, 2);
            buttonFiltre.Name = "buttonFiltre";
            buttonFiltre.Size = new Size(38, 20);
            buttonFiltre.TabIndex = 2;
            buttonFiltre.Text = "🝖";
            buttonFiltre.UseVisualStyleBackColor = true;
            buttonFiltre.Click += buttonFiltre_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(3, 3);
            textBox1.Margin = new Padding(3, 2, 3, 2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(171, 23);
            textBox1.TabIndex = 0;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 30);
            label1.Name = "label1";
            label1.Size = new Size(31, 15);
            label1.TabIndex = 3;
            label1.Text = "Tags";
            // 
            // SidePanelMeuble
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(layout);
            Margin = new Padding(3, 2, 3, 2);
            Name = "SidePanelMeuble";
            Size = new Size(276, 348);
            layout.ResumeLayout(false);
            layout.PerformLayout();
            seachBarPanel.ResumeLayout(false);
            seachBarPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel layout;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel seachBarPanel;
        private TextBox textBox1;
        private Button buttonFiltre;
        private FilterPanel filterPanel1;
        private Label label1;
    }
}
