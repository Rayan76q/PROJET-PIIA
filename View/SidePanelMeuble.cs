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
            var fcontroleur = new FilterControler(m);
            fcontroleur.TagsModifies += update_meubles;
            filterPanel1 = new FilterPanel(fcontroleur);
            layout.Controls.Add(filterPanel1, 0, 1);
            filterPanel1.Dock = DockStyle.Fill;
            update_meubles();
            flowLayoutPanel1.VerticalScroll.Visible = true; // metre via designer ?

            //pas du tout une entourloupe
            buttonFiltre_Click(null, null);
            buttonFiltre_Click(null, null);
        }

        private void buttonFiltre_Click(object sender, EventArgs e) {
            ctr.CollapseFilterSelection();
            if (!ctr.filterSelectionColapsed) {
                layout.RowStyles[1].SizeType = SizeType.AutoSize;
                layout.RowStyles[1].Height = 20; // hauteur par defaut au cas ou autosize ne fais rien
                layout.RowStyles[2].Height = 80;
            } else {
                layout.RowStyles[1].SizeType = SizeType.Absolute;
                layout.RowStyles[1].Height = 0;
                layout.RowStyles[2].Height = 0;
            }
        }

        private void update_meubles() {
            bool scrollbarshown = flowLayoutPanel1.VerticalScroll.Visible;
            flowLayoutPanel1.Controls.Clear();
            foreach (var m in filterPanel1.controller.getMeubleToDisplay(textBox1.Text)) {
                AddMeubleToPanel(m, scrollbarshown);
            }
            Invalidate();
        }

        private void AddMeubleToPanel(Meuble m, bool scroll) {
            MeublePanel meublePanel = new MeublePanel(m, new(ctr.compte));          
            int availableWidth = flowLayoutPanel1.Width - SystemInformation.VerticalScrollBarWidth*(scroll?1:0) - flowLayoutPanel1.Margin.Left - flowLayoutPanel1.Margin.Right;

            meublePanel.Width = availableWidth-50;
            flowLayoutPanel1.Controls.Add(meublePanel);
        }

        private void buttonSearch_Click(object sender, EventArgs e) {
            update_meubles();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            update_meubles();
        }

    }
}
