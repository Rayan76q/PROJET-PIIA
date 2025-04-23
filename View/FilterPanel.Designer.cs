namespace PROJET_PIIA.View {
    partial class FilterPanel {
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
            layout = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // layout
            // 
            layout.AutoScroll = true;
            layout.Dock = DockStyle.Fill;
            layout.Location = new Point(0, 0);
            layout.Name = "layout";
            layout.Size = new Size(263, 131);
            layout.TabIndex = 0;
            // 
            // FilterPanel
            // 
            AutoScroll = true;
            Controls.Add(layout);
            Name = "FilterPanel";
            Size = new Size(263, 131);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel layout;
    }
}
