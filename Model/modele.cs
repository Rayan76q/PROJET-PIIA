using System.Text;

namespace PROJET_PIIA.Model {
    public class Modele {
        public Catalogue Catalogue { get; }

        public Plan planActuel { get; set; }
        public Compte compteActuel; 



        public Modele() {
            Catalogue = new Catalogue();
            this.planActuel = new Plan();
            this.compteActuel = new Compte();
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Modèle:");
            sb.AppendLine($"Catalogue: {Catalogue.ToString()}");

            if (planActuel != null) {
                sb.AppendLine($"Plan Actuel: {planActuel.ToString()}");
            } else {
                sb.AppendLine("Aucun plan actuel sélectionné.");
            }

            if (compteActuel != null) {
                sb.AppendLine($"Compte Actuel: {compteActuel.ToString()}");
            } else {
                sb.AppendLine("Aucun compte connecté.");
            }

            return sb.ToString();
        }



    }
}