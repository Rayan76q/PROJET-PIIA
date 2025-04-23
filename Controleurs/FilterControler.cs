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
            tagsDisponible.Remove(tag);
            tagsSelection.Add(tag);
        }

        public bool EstActif(Tag tag) => tagsSelection.Contains(tag);
    }
}
