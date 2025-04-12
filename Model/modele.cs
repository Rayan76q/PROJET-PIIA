using System.Text;

namespace PROJET_PIIA.Modele {
    public class Modele {
        private Catalogue _cat;
        public Catalogue cat {
            get => _cat;
        }
        public Plan planActuel;
        public Compte? compteActuel = null;

        public Compte creerCompte(string nom, string password) {
            try {
                Compte compte = new Compte(nom, password);
                return compte;
            } catch (ArgumentException e) {
                throw e;
            }
        }

        public bool supprimerCompte(Compte compte) {
            return Compte.comptes.Remove(compte.Id);
        }

        public void selectionnerPlan(Plan p) {
            planActuel = p;
        }

        public List<Plan> getPlansCompteAct() {
            if (compteActuel != null) {
                return compteActuel.Plans;
            }
            throw new InvalidOperationException("Aucun compte sélectionné.");
        }

        public void seConnecter(string nom, string password) {
            foreach (Compte compte in Compte.comptes.Values) {
                if (compte.Name == nom && compte.Password == password) {
                    compteActuel = compte;
                    return; 
                }
            }
            
            throw new ArgumentException("Nom d'utilisateur ou mot de passe incorrect.");
        }

       public  void creerPlan(Murs m, string nom) {
            Plan plan = new Plan(m, nom);
            planActuel = plan;
        }


        public Modele() {
            _cat = new Catalogue();
           this.planActuel = new Plan(new Murs(), "Untitled 1");
           this.compteActuel = null;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Modèle:");
            sb.AppendLine($"Catalogue: {cat.ToString()}");

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