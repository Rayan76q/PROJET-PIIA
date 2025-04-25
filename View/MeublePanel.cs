using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.View {
    public partial class MeublePanel : UserControl {

        Meuble m;
        MeublePanelControler ctr;

        public MeublePanel(Meuble m, MeublePanelControler ctr) {
            InitializeComponent();
            this.ctr = ctr;
            meubleLabel.Text = m.Nom;
            this.m = m;
            this.Tag = m;
            this.buttonMeuble.Tag = m;
            this.buttonFav.Tag = m;
            this.meubleLabel.Tag = m;

            buttonFav.Text = ctr.IsFavorite(m) ? "★" : "☆";
        }

        private void buttonMeuble_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                buttonMeuble.DoDragDrop(m, DragDropEffects.Copy);
            }
        }


        private void buttonFav_Click(object sender, EventArgs e) {
            ctr.SwitchFavorite(m);
            buttonFav.Text = ctr.IsFavorite(m) ? "★" : "☆";
        }
    }
}
