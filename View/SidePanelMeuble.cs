using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.View {
    public partial class SidePanelMeuble : UserControl {

        ControleurSidePanelMeuble ctr;
        
        public SidePanelMeuble(Modele m) {
            this.ctr = new ControleurSidePanelMeuble(m);
            InitializeComponent();
            filterPanel1 = new FilterPanel(new FilterControler(m));
            layout.Controls.Add(filterPanel1, 0, 1);
            update_meubles();
            flowLayoutPanel1.VerticalScroll.Visible = true;
        }

        private void buttonFiltre_Click(object sender, EventArgs e) {
            ctr.CollapseFilterSelection();
            if (!ctr.filterSelectionColapsed) {
                layout.RowStyles[1].SizeType = SizeType.AutoSize;
                layout.RowStyles[1].Height = 40; // hauteur par defaut au cas ou autosize ne fais rien
            } else {
                layout.RowStyles[1].SizeType = SizeType.Absolute;
                layout.RowStyles[1].Height = 0;
            }
        }

        private void update_meubles() {
            foreach (var m in filterPanel1.controller.getMeubleToDisplay()) {
                AddMeubleToPanel(m);
            }
            Invalidate();
        }

        private void AddMeubleToPanel(Meuble m) {
            MeublePanel meublePanel = new MeublePanel(m, new(ctr.compte));
            int verticalScrollBarWidth = flowLayoutPanel1.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0;

            // Calcul de la largeur en tenant compte de la barre de défilement et des marges
            int availableWidth = flowLayoutPanel1.ClientSize.Width - verticalScrollBarWidth - flowLayoutPanel1.Margin.Left - flowLayoutPanel1.Margin.Right;

            meublePanel.Width = availableWidth;
            flowLayoutPanel1.Controls.Add(meublePanel);
        }



    }
}
