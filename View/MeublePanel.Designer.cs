namespace PROJET_PIIA.View {
    partial class MeublePanel {
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
            buttonMeuble = new Button();
            meubleLabel = new Label();
            buttonFav = new Button();
            SuspendLayout();
            // 
            // buttonMeuble
            // 
            buttonMeuble.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            buttonMeuble.FlatStyle = FlatStyle.Flat;
            buttonMeuble.Location = new Point(30, 25);
            buttonMeuble.Name = "buttonMeuble";
            buttonMeuble.Size = new Size(90, 73);
            buttonMeuble.TabIndex = 0;
            buttonMeuble.Text = "\U0001fa91";
            buttonMeuble.UseVisualStyleBackColor = true;
            buttonMeuble.MouseDown += buttonMeuble_MouseDown;
            // 
            // meubleLabel
            // 
            meubleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            meubleLabel.AutoSize = true;
            meubleLabel.Location = new Point(158, 25);
            meubleLabel.Name = "meubleLabel";
            meubleLabel.Size = new Size(117, 20);
            meubleLabel.TabIndex = 1;
            meubleLabel.Text = "Texte par defaut";
            // 
            // buttonFav
            // 
            buttonFav.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonFav.FlatStyle = FlatStyle.Flat;
            buttonFav.Location = new Point(239, 69);
            buttonFav.Name = "buttonFav";
            buttonFav.Size = new Size(36, 29);
            buttonFav.TabIndex = 2;
            buttonFav.Text = "☆";
            buttonFav.UseVisualStyleBackColor = true;
            // 
            // MeublePanel
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            Controls.Add(buttonFav);
            Controls.Add(meubleLabel);
            Controls.Add(buttonMeuble);
            Name = "MeublePanel";
            Padding = new Padding(2, 5, 10, 0);
            Size = new Size(288, 124);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonMeuble;
        private Label meubleLabel;
        private Button buttonFav;
    }
}
