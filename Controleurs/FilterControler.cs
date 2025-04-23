using PROJET_PIIA.Model;

namespace PROJET_PIIA.Controleurs {
    public class FilterControler {
        Catalogue cataloge;
        List<Tag> tagsSelection;
        List<Tag> tagsDisponible;

        public FilterControler(Modele m) { 
            cataloge = m.Catalogue;
            tagsSelection = new List<Tag>();
            tagsDisponible = Enum.GetValues(typeof(Tag)).Cast<Tag>().ToList();
        }

        void getMeubleToDisplay() {
            cataloge.getWithTags(tagsSelection);
        }

        void toggleTag(Tag tag) {
            tagsDisponible.Remove(tag);
            tagsSelection.Add(tag);
        }
    }
}
