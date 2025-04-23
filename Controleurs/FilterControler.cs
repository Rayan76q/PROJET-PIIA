using PROJET_PIIA.Model;

namespace PROJET_PIIA.Controleurs {
    public class FilterControler {
        Catalogue cataloge;
        public List<Tag> tagsSelection;
        public List<Tag> tagsDisponible;

        public event Action TagsModifies;

        public FilterControler(Modele m) { 
            cataloge = m.Catalogue;
            tagsSelection = new List<Tag>();
            tagsDisponible = Enum.GetValues(typeof(Tag)).Cast<Tag>().ToList();
        }

        public List<Meuble> getMeubleToDisplay() {
            if (tagsSelection.Count == 0) {
                return cataloge.Meubles;
            }
            return cataloge.getWithTags(tagsSelection);
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
    }
}
