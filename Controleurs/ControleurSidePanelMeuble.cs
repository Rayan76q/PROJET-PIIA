using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.Controleurs {
    public class ControleurSidePanelMeuble {

        Catalogue cata;
        public bool filterSelectionColapsed = false;
        public Compte compte;
        
        public ControleurSidePanelMeuble(Modele m) {
            cata = m.Catalogue;
            compte = m.compteActuel;
        }

        public void CollapseFilterSelection() {
            filterSelectionColapsed = !filterSelectionColapsed;
        }

        


    }
}
