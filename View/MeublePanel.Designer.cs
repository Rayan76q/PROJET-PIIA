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
            buttonMeuble.Location = new Point(26, 19);
            buttonMeuble.Margin = new Padding(3, 2, 3, 2);
            buttonMeuble.Name = "buttonMeuble";
            buttonMeuble.Size = new Size(79, 55);
            buttonMeuble.TabIndex = 0;
            buttonMeuble.Text = "\U0001fa91";
            buttonMeuble.UseVisualStyleBackColor = true;
            buttonMeuble.MouseDown += buttonMeuble_MouseDown;
            // 
            // meubleLabel
            // 
            meubleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            meubleLabel.AutoSize = true;
            meubleLabel.Location = new Point(166, 19);
            meubleLabel.Name = "meubleLabel";
            meubleLabel.Size = new Size(91, 15);
            meubleLabel.TabIndex = 1;
            meubleLabel.Text = "Texte par defaut";
            // 
            // buttonFav
            // 
            buttonFav.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonFav.FlatStyle = FlatStyle.Flat;
            buttonFav.Location = new Point(237, 52);
            buttonFav.Margin = new Padding(3, 2, 3, 2);
            buttonFav.Name = "buttonFav";
            buttonFav.Size = new Size(32, 22);
            buttonFav.TabIndex = 2;
            buttonFav.Text = "☆";
            buttonFav.UseVisualStyleBackColor = true;
            buttonFav.Click += buttonFav_Click;
            // 
            // MeublePanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.Gainsboro;
            Controls.Add(buttonFav);
            Controls.Add(meubleLabel);
            Controls.Add(buttonMeuble);
            Margin = new Padding(3, 2, 3, 2);
            Name = "MeublePanel";
            Padding = new Padding(2, 4, 9, 0);
            Size = new Size(280, 93);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonMeuble;
        private Label meubleLabel;
        private Button buttonFav;
    }
}
