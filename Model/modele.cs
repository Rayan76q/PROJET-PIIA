using System.Text;

namespace PROJET_PIIA.Model {
    public class Modele {
        public Catalogue Catalogue { get; }

        public Plan planActuel;
        public Compte? compteActuel = null;


        // Ques ce que ça fou là svp, Soit a foutre dans un controleur, soit une classe 
        // je pense plutot un controleur pour la page login, psq là modele.creerCompte... ça napas trop de sens imo
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

        // pourquoi ? on ne passe pas par l'id ? vaut mieux une liste/dict de plan dans model
        // ou bien dans user, ilfautcreer une classe User
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

        public void creerPlan(Murs m, string nom) {
            Plan plan = new Plan(nom);
            planActuel = plan;
        }


        public Modele() {
            Catalogue = new Catalogue();
            this.planActuel = new Plan(); // il a un nom par defaut selon son id
            this.compteActuel = null;
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