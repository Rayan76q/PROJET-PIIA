namespace PROJET_PIIA.Model {
    public class Compte {
        private static string DEFAULT_AVATAR = "";
        private static int _idCounter = 0;
        public static Dictionary<int, Compte> comptes = new Dictionary<int, Compte>(); 

        public List<int> Favorites;

        public static bool nomDispo(string nom) {
            foreach (Compte compte in comptes.Values) {
                if (compte.Name == nom) {
                    return false;
                }
            }
            return true;
        }


        private string _name;
        public string Name {
            get => _name;
            set {
                if (!nomDispo(value))
                    throw new ArgumentException("Nom indisponible.");
                _name = value;
            }
        }

        public readonly int Id;


        private string _password;
        public string Password {
            get => _password;
            set {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Password cannot be null or empty.");
                _password = value;
            }
        }


        private List<Plan> _plans;
        public List<Plan> Plans {
            get => _plans;
        }


        public void addPlan(Plan p) {
            if (p == null) {
                throw new ArgumentNullException(nameof(p), "Plan cannot be null.");
            }
            _plans.Add(p);
        }

        public void deletePlan(Plan p) {
            if (p == null) {
                throw new ArgumentNullException(nameof(p), "Plan cannot be null.");
            }
            _plans.Remove(p);
        }

        public string Avatar { get; set; } // j'aienvie de dire null par defaut pour etre sur qu'il n'y a pas d'image ?

        public bool Connected {get;set;}


        public void changeAvatar(string avatar) {
            if (string.IsNullOrEmpty(avatar))
                Avatar = DEFAULT_AVATAR;
            Avatar = avatar;
        }

        public Compte() {
            Id = _idCounter++;
            Name = "Invité";
            Password = "0000";
            _plans = new List<Plan>();
            Avatar = DEFAULT_AVATAR;
            comptes.Add(Id, this);
            Connected = false;
            Favorites = new();
        }

        public Compte(string name, string password) {

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty.");

            Id = _idCounter++;
            Name = name;
            Password = password;
            _plans = new List<Plan>();
            Avatar = DEFAULT_AVATAR;
            comptes.Add(Id, this);
            Connected = false;
            Favorites = new();
        }

        public override string ToString() {
            var plansStr = Plans.Count > 0
                ? string.Join(", ", Plans.Select(p => p.ToString()))
                : "Aucun plan";

            return $"Compte [ID: {Id}, Nom: {Name}, Connecté: {Connected}, " +
                   $"Avatar: {Avatar}, Plans: [{plansStr}]]";
        }

    }
}
