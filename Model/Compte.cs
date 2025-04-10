using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accessibility;

namespace PROJET_PIIA.Model
{
    class Compte
    {
        private static string DEFAULT_AVATAR = "";
        private static int _idCounter = 0; 
        public static Dictionary<int, Compte> comptes = new Dictionary<int, Compte>();

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

        private string _avatar;
        public string Avatar {
            get => _avatar;
        }


        private bool _connected;
        public bool Connected {
            get => _connected;
            set => _connected = value;
        }


        public void changeAvatar(string avatar) {
            if (string.IsNullOrEmpty(avatar))
                _avatar = DEFAULT_AVATAR;
            _avatar = avatar;
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
            _avatar = DEFAULT_AVATAR; 
            comptes.Add(Id, this);
            Connected = false;
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
