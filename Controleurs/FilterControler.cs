using System.Diagnostics;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.Controleurs {
    public class FilterControler {
        Catalogue catalogue;
        public List<Tag> tagsSelection;
        public List<Tag> tagsDisponible;
        public Compte compteAct;

        public event Action TagsModifies;

        public FilterControler(Modele m) { 
            catalogue = m.Catalogue;
            tagsSelection = new List<Tag>();
            tagsDisponible = Enum.GetValues(typeof(Tag)).Cast<Tag>().ToList();
            compteAct = m.compteActuel;
        }

        public void toggleTag(Tag tag) {
            if (EstActif(tag)) {
                tagsDisponible.Add(tag);
                tagsSelection.Remove(tag);
            } else {
                tagsDisponible.Remove(tag);
                tagsSelection.Add(tag);
            }
            notify();
        }

        private void notify() {
            TagsModifies?.Invoke();
        }

        public bool EstActif(Tag tag) => tagsSelection.Contains(tag);


        public List<Meuble> getMeubleToDisplay(string searchQuery) {
            searchQuery = searchQuery.Trim().ToLower();
            return this.catalogue.Meubles
         .Where(m =>
             IsMeubleVisibleWithCurrentTags(m) &&
             (string.IsNullOrEmpty(searchQuery) || m.Nom.ToLower().Contains(searchQuery))
         )
         
         .OrderByDescending(m => compteAct.Favorites.Contains(m.catRef))
         .ThenBy(m => m.Nom)
         .ToList();

        }

        private bool IsMeubleVisibleWithCurrentTags(Meuble m) {
            foreach (Tag tag in tagsSelection) {
                if (!m.tags.Contains(tag)) {
                    return false;
                }
            }
            return true;

        }
    }
}
