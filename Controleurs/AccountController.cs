using PROJET_PIIA.Model;
using PROJET_PIIA.View;

namespace PROJET_PIIA.Controleurs {
    public class AccountController {
        public Modele _modele { get; }
        private MainView _mainView;

        public event Action AccountStateChanged;
        public event Action PlanChanged;
        private static Dictionary<string, string> validCredentials;

        public static void setValideCredentials() {
            Dictionary<string, string> credentials = new Dictionary<string, string>();
            foreach (Compte c in Compte.comptes.Values) {
                credentials.Add(c.Name, c.Password);
            }

            validCredentials = credentials;
        }

        public AccountController(Modele modele) {
            _modele = modele;
        }

        public void SetMainView(MainView mainView) {
            _mainView = mainView;
        }

        // ================ Login/Logout Functionality ================

        public bool Login(string username, string password) {
            // Check if any account matches the provided credentials
            foreach (Compte compte in Compte.comptes.Values) {
                if (compte.Name == username && compte.Password == password) {
                    // Set this account as the active account
                    _modele.compteActuel = compte;
                    compte.Connected = true;

                    // Notify subscribers that account state has changed
                    AccountStateChanged?.Invoke();
                    return true;
                }
            }
            return false;
        }

        public void Logout() {
            if (_modele.compteActuel != null) {
                _modele.compteActuel.Connected = false;
                _modele.compteActuel = new Compte(); // Create a new default guest account
                AccountStateChanged?.Invoke();
            }
        }

        // ================ Registration Functionality ================

        public bool RegisterAccount(string username, string password, string confirmPassword) {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(username)) {
                MessageBox.Show("Le nom d'utilisateur ne peut pas être vide.", "Erreur d'inscription",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(password)) {
                MessageBox.Show("Le mot de passe ne peut pas être vide.", "Erreur d'inscription",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (password != confirmPassword) {
                MessageBox.Show("Les mots de passe ne correspondent pas.", "Erreur d'inscription",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if username is available
            if (!Compte.nomDispo(username)) {
                MessageBox.Show("Ce nom d'utilisateur est déjà pris.", "Erreur d'inscription",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try {
                // Create new account
                Compte newAccount = new Compte(username, password);

                // Auto-login the new user
                _modele.compteActuel = newAccount;
                newAccount.Connected = true;

                // Notify subscribers that account state has changed
                AccountStateChanged?.Invoke();
                return true;
            } catch (Exception ex) {
                MessageBox.Show($"Erreur lors de la création du compte: {ex.Message}", "Erreur d'inscription",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ================ Plan Management ================

        public void SavePlan(Plan plan) {
            if (_modele.compteActuel == null) {
                // If no active account, prompt for login or account creation
                if (MessageBox.Show("Vous devez être connecté pour sauvegarder un plan. Souhaitez-vous vous connecter ?",
                        "Connexion requise", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    ShowLoginDialog();
                }
                return;
            }

            try {
                _modele.compteActuel.savePlan(plan);
                MessageBox.Show("Plan sauvegardé avec succès.", "Sauvegarde",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                //handled
            }
        }

        public void DeletePlan(Plan plan) {
            if (_modele.compteActuel == null || plan == null) {
                return;
            }

            try {
                _modele.compteActuel.deletePlan(plan);
                MessageBox.Show("Plan supprimé avec succès.", "Suppression",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show($"Erreur lors de la suppression du plan: {ex.Message}", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<Plan> GetUserPlans() {
            return _modele.compteActuel.LoadPlans();
        }

        public void selectPlan(Plan plan) {
            if (plan == null) {
                return;
            } else { _modele.planActuel = plan; }
            
            PlanChanged?.Invoke();
        }


        // ================ UI Helpers ================

        public void UpdateAvatarForLoggedInUser() {
            _mainView.UpdateAvatarForLoggedInUser();
            AccountStateChanged?.Invoke();
        }

        public bool ShowLoginDialog() {
            if (_mainView == null) return false;

            using (LoginView loginDialog = new LoginView(this)) {
                loginDialog.StartPosition = FormStartPosition.CenterParent;
                DialogResult result = loginDialog.ShowDialog(_mainView);
                if (result == DialogResult.OK) {
                    AccountStateChanged?.Invoke();
                    return true;
                }
                return false;
            }
        }

        public bool ShowSignupDialog() {
            if (_mainView == null) return false;

            using (SignupView signDialog = new SignupView(this)) {
                signDialog.StartPosition = FormStartPosition.CenterParent;
                DialogResult result = signDialog.ShowDialog(_mainView);
                if (result == DialogResult.OK) {
                    AccountStateChanged?.Invoke();
                    return true;
                }
                return false;
            }
        }

        public bool IsUserLoggedIn() {
            return _modele.compteActuel != null && _modele.compteActuel.Connected && _modele.compteActuel.Name != "Invité";
        }

        public string GetCurrentUserName() {
            return _modele.compteActuel?.Name ?? "Invité";
        }



    }
}