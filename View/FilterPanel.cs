using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.View {
    public partial class FilterPanel : UserControl {
        public FilterControler controller;

        public FilterPanel(FilterControler c) {
            InitializeComponent();
            controller = c;
            controller.TagsModifies += MetAJourAffichage;
            MetAJourAffichage();
        }

        private void MetAJourAffichage() {
            layout.Controls.Clear();

            var tousLesTags = controller.tagsSelection.Concat(controller.tagsDisponible).Distinct();

            foreach (var tag in tousLesTags) {
                var bouton = new Button {
                    Text = tag.ToString(),
                    AutoSize = true,
                    BackColor = controller.EstActif(tag) ? Color.SteelBlue : SystemColors.Control,
                    ForeColor = controller.EstActif(tag) ? Color.White : Color.Black,
                    Margin = new Padding(5),
                };

                bouton.Click += (s, e) => controller.toggleTag(tag);

                layout.Controls.Add(bouton);
            }
        }
    }
}
