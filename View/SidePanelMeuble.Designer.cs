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
            buttonSearch = new Button();
            textBox1 = new TextBox();
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
            layout.Dock = DockStyle.Fill;
            layout.Location = new Point(0, 0);
            layout.Name = "layout";
            layout.RowCount = 3;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 267F));
            layout.Size = new Size(315, 464);
            layout.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = SystemColors.ControlDarkDark;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(3, 173);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(309, 288);
            flowLayoutPanel1.TabIndex = 1;
            flowLayoutPanel1.WrapContents = false;
            // 
            // seachBarPanel
            // 
            seachBarPanel.Controls.Add(buttonFiltre);
            seachBarPanel.Controls.Add(buttonSearch);
            seachBarPanel.Controls.Add(textBox1);
            seachBarPanel.Dock = DockStyle.Fill;
            seachBarPanel.Location = new Point(3, 3);
            seachBarPanel.Name = "seachBarPanel";
            seachBarPanel.Size = new Size(309, 34);
            seachBarPanel.TabIndex = 2;
            // 
            // buttonFiltre
            // 
            buttonFiltre.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonFiltre.Location = new Point(253, 3);
            buttonFiltre.Name = "buttonFiltre";
            buttonFiltre.Size = new Size(43, 27);
            buttonFiltre.TabIndex = 2;
            buttonFiltre.Text = "F";
            buttonFiltre.UseVisualStyleBackColor = true;
            buttonFiltre.Click += buttonFiltre_Click;
            // 
            // buttonSearch
            // 
            buttonSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSearch.Location = new Point(204, 4);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(43, 27);
            buttonSearch.TabIndex = 1;
            buttonSearch.Text = "S";
            buttonSearch.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(3, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(195, 27);
            textBox1.TabIndex = 0;
            // 
            // SidePanelMeuble
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(layout);
            Name = "SidePanelMeuble";
            Size = new Size(315, 464);
            layout.ResumeLayout(false);
            seachBarPanel.ResumeLayout(false);
            seachBarPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel layout;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel seachBarPanel;
        private Button buttonSearch;
        private TextBox textBox1;
        private Button buttonFiltre;
        private FilterPanel filterPanel1;
    }
}
