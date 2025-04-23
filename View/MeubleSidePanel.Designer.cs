namespace PROJET_PIIA.View {
    public partial class MeubleSidePanel {
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
            splitfilterpanel = new SplitContainer();
            searchBox = new Button();
            filterButton = new Button();
            textBox1 = new TextBox();
            filterSplitContainer = new SplitContainer();
            flowpaneldispo = new FlowLayoutPanel();
            label1 = new Label();
            labelselection = new Label();
            selectionFlowPanel = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitfilterpanel).BeginInit();
            splitfilterpanel.Panel1.SuspendLayout();
            splitfilterpanel.Panel2.SuspendLayout();
            splitfilterpanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)filterSplitContainer).BeginInit();
            filterSplitContainer.Panel1.SuspendLayout();
            filterSplitContainer.Panel2.SuspendLayout();
            filterSplitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = SystemColors.ActiveCaption;
            flowLayoutPanel1.Location = new Point(16, 170);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(407, 293);
            flowLayoutPanel1.TabIndex = 0;
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
            splitContainer1.Panel1.Controls.Add(splitfilterpanel);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel1);
            splitContainer1.Size = new Size(407, 504);
            splitContainer1.SplitterDistance = 252;
            splitContainer1.TabIndex = 1;
            // 
            // splitfilterpanel
            // 
            splitfilterpanel.Dock = DockStyle.Fill;
            splitfilterpanel.Location = new Point(0, 0);
            splitfilterpanel.Name = "splitfilterpanel";
            splitfilterpanel.Orientation = Orientation.Horizontal;
            // 
            // splitfilterpanel.Panel1
            // 
            splitfilterpanel.Panel1.Controls.Add(searchBox);
            splitfilterpanel.Panel1.Controls.Add(filterButton);
            splitfilterpanel.Panel1.Controls.Add(textBox1);
            // 
            // splitfilterpanel.Panel2
            // 
            splitfilterpanel.Panel2.Controls.Add(filterSplitContainer);
            splitfilterpanel.Size = new Size(407, 252);
            splitfilterpanel.SplitterDistance = 116;
            splitfilterpanel.TabIndex = 4;
            // 
            // searchBox
            // 
            searchBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            searchBox.FlatStyle = FlatStyle.Flat;
            searchBox.Location = new Point(318, 6);
            searchBox.Name = "searchBox";
            searchBox.Size = new Size(40, 30);
            searchBox.TabIndex = 1;
            searchBox.Text = "🔍︎";
            searchBox.UseVisualStyleBackColor = true;
            // 
            // filterButton
            // 
            filterButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            filterButton.FlatStyle = FlatStyle.Flat;
            filterButton.Location = new Point(364, 7);
            filterButton.Name = "filterButton";
            filterButton.Size = new Size(40, 29);
            filterButton.TabIndex = 2;
            filterButton.Text = "▩";
            filterButton.UseVisualStyleBackColor = true;
            filterButton.Click += filterButton_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(3, 9);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(309, 27);
            textBox1.TabIndex = 0;
            // 
            // filterSplitContainer
            // 
            filterSplitContainer.Dock = DockStyle.Fill;
            filterSplitContainer.Location = new Point(0, 0);
            filterSplitContainer.Name = "filterSplitContainer";
            // 
            // filterSplitContainer.Panel1
            // 
            filterSplitContainer.Panel1.AutoScroll = true;
            filterSplitContainer.Panel1.Controls.Add(flowpaneldispo);
            filterSplitContainer.Panel1.Controls.Add(label1);
            // 
            // filterSplitContainer.Panel2
            // 
            filterSplitContainer.Panel2.AutoScroll = true;
            filterSplitContainer.Panel2.Controls.Add(labelselection);
            filterSplitContainer.Panel2.Controls.Add(selectionFlowPanel);
            filterSplitContainer.Size = new Size(407, 132);
            filterSplitContainer.SplitterDistance = 200;
            filterSplitContainer.TabIndex = 3;
            filterSplitContainer.Visible = false;
            // 
            // flowpaneldispo
            // 
            flowpaneldispo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowpaneldispo.Location = new Point(3, 23);
            flowpaneldispo.Name = "flowpaneldispo";
            flowpaneldispo.Size = new Size(194, 109);
            flowpaneldispo.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(36, 0);
            label1.Name = "label1";
            label1.Size = new Size(122, 20);
            label1.TabIndex = 2;
            label1.Text = "Filtres disponible";
            // 
            // labelselection
            // 
            labelselection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelselection.AutoSize = true;
            labelselection.Location = new Point(43, 0);
            labelselection.Name = "labelselection";
            labelselection.Size = new Size(127, 20);
            labelselection.TabIndex = 1;
            labelselection.Text = "Filtres séléctionné";
            // 
            // selectionFlowPanel
            // 
            selectionFlowPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            selectionFlowPanel.Location = new Point(0, 23);
            selectionFlowPanel.Name = "selectionFlowPanel";
            selectionFlowPanel.Size = new Size(203, 109);
            selectionFlowPanel.TabIndex = 0;
            // 
            // MeubleSidePanel
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "MeubleSidePanel";
            Size = new Size(407, 504);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitfilterpanel.Panel1.ResumeLayout(false);
            splitfilterpanel.Panel1.PerformLayout();
            splitfilterpanel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitfilterpanel).EndInit();
            splitfilterpanel.ResumeLayout(false);
            filterSplitContainer.Panel1.ResumeLayout(false);
            filterSplitContainer.Panel1.PerformLayout();
            filterSplitContainer.Panel2.ResumeLayout(false);
            filterSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)filterSplitContainer).EndInit();
            filterSplitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private SplitContainer splitContainer1;
        private TextBox textBox1;
        private Button searchBox;
        private Button filterButton;
        private SplitContainer filterSplitContainer;
        private FlowLayoutPanel selectionFlowPanel;
        private SplitContainer splitfilterpanel;
        private Label labelselection;
        private Label label1;
        private FlowLayoutPanel flowpaneldispo;
    }
}
