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
        public required string Name {
            get => _name;
            set {
                if (!nomDispo(value))
                    throw new ArgumentException("Nom indisponible.");
                _name = value;
            }
        }

        public readonly int Id;


        private string _password;
        public required string Password {
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
            Name = string.IsNullOrEmpty(name) ? "Compte " + Id : name;
            Password = password;
            _plans = new List<Plan>();
            _avatar = DEFAULT_AVATAR;
            comptes.Add(Id, this);
        }










    }
}
