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
            SearchMeubleSplit = new SplitContainer();
            FilterButton = new Button();
            ((System.ComponentModel.ISupportInitialize)SearchMeubleSplit).BeginInit();
            SearchMeubleSplit.Panel1.SuspendLayout();
            SearchMeubleSplit.SuspendLayout();
            SuspendLayout();
            // 
            // SearchMeubleSplit
            // 
            SearchMeubleSplit.Dock = DockStyle.Fill;
            SearchMeubleSplit.Location = new Point(0, 0);
            SearchMeubleSplit.Name = "SearchMeubleSplit";
            SearchMeubleSplit.Orientation = Orientation.Horizontal;
            // 
            // SearchMeubleSplit.Panel1
            // 
            SearchMeubleSplit.Panel1.BackColor = SystemColors.MenuHighlight;
            SearchMeubleSplit.Panel1.Controls.Add(FilterButton);
            // 
            // SearchMeubleSplit.Panel2
            // 
            SearchMeubleSplit.Panel2.BackColor = SystemColors.ActiveCaption;
            SearchMeubleSplit.Size = new Size(364, 466);
            SearchMeubleSplit.SplitterDistance = 180;
            SearchMeubleSplit.TabIndex = 0;
            // 
            // FilterButton
            // 
            FilterButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            FilterButton.Location = new Point(298, 3);
            FilterButton.Name = "FilterButton";
            FilterButton.Size = new Size(63, 50);
            FilterButton.TabIndex = 0;
            FilterButton.Text = "▩";
            FilterButton.UseVisualStyleBackColor = true;
            // 
            // SidePanelMeuble
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(SearchMeubleSplit);
            Name = "SidePanelMeuble";
            Size = new Size(364, 466);
            SearchMeubleSplit.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SearchMeubleSplit).EndInit();
            SearchMeubleSplit.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer SearchMeubleSplit;
        private Button FilterButton;
    }
}
