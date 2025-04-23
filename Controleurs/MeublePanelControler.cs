using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.Controleurs {
    public class MeublePanelControler(Compte compte) {

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
    }
}
