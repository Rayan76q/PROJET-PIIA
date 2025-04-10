namespace PROJET_PIIA.Model {
    class Modele {
        private Catalogue _cat;
        public Catalogue cat {
            get => _cat;
        }
        public Plan? planActuel = null;
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

        
        public Modele(Catalogue c) {
            _cat = c;
        }
    }
}