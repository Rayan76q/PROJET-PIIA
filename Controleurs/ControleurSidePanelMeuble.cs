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
        Compte compte;
        public bool filterSelectionColapsed = true;
        
        public ControleurSidePanelMeuble(Modele m) {
            cata = m.Catalogue;
            compte = m.compteActuel;
        }

        public bool IsFavorite(Meuble m) {
            return compte.Favorites.Contains(m);
        }

        public void SwitchFavorite(Meuble m) {
            if (IsFavorite(m)) { 
                compte.Favorites.Remove(m);
            } else {
                compte.Favorites.Add(m);
            }

        }

        public List<Meuble> GetFilteredMeubles(string searchQuery = "") {
            return cata.Meubles
                       .Where(m =>
                           IsMeubleVisibleWithCurrentTags(m) &&
                           (string.IsNullOrEmpty(searchQuery) || m.Nom.ToLower().Contains(searchQuery.ToLower()))
                       )
                       .ToList();
        }

        private bool IsMeubleVisibleWithCurrentTags(Meuble m) {
            // Ta logique de filtre de tags ici
            return true;
        }


        public void CollapseFilterSelection() {
            filterSelectionColapsed = !filterSelectionColapsed;
        }

        public List<Meuble> getMeubles() {
            // TODO appliquer les filtres
            return cata.Meubles; 
        }

    }
}
