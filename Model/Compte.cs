using System.Text;
using System.Xml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Serialization;
using Formatting = Newtonsoft.Json.Formatting;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace PROJET_PIIA.Model {
    public class Compte {
        private static string DEFAULT_AVATAR = "";
        private static int _idCounter = 0;
        public static Dictionary<int, Compte> comptes;

        static Compte() {
            comptes = loadAccounts() ?? new Dictionary<int, Compte>();
        }

        public List<int> Favorites;

        public static bool nomDispo(string nom) {
            if (string.IsNullOrWhiteSpace(nom))
                return false;
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

        public int Id;


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

        public string Avatar { get; set; } 

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

            saveAccount();
        }

        //pour ne pas resave , utile pour load
        private Compte(int id,
                  string name,
                  string password,
                  string avatar,
                  bool connected,
                  List<int> favorites,
                  List<Plan> plans) {
            Id = id;
            _name = name;
            _password = password;
            Avatar = avatar;
            Connected = connected;
            Favorites = favorites;
            _plans = plans;
        }

        public void saveAccount() {
            try {
                string exeDir = Application.StartupPath;
                string savesDir = Path.Combine(exeDir, "SavedPlans");
                Directory.CreateDirectory(savesDir);

                string registryPath = Path.Combine(savesDir, "accounts.json");

                JArray accountsArray;
                if (File.Exists(registryPath)) {
                    string existingJson = File.ReadAllText(registryPath, Encoding.UTF8);
                    accountsArray = JArray.Parse(existingJson);
                } else {
                    accountsArray = new JArray();
                }

                bool alreadyRegistered = false;
                foreach (var token in accountsArray) {
                    if (token.Type == JTokenType.Object &&
                        token["Name"]?.ToString().Equals(this.Name, StringComparison.OrdinalIgnoreCase) == true) {
                        alreadyRegistered = true;
                        break;
                    }
                }

                if (alreadyRegistered) {
                    return;
                }

                var jo = new JObject {
                    ["Id"] = this.Id,
                    ["Password"] = this.Password,
                    ["Name"] = this.Name,
                    ["Avatar"] = this.Avatar ?? string.Empty,
                    ["Created"] = DateTime.Now.ToString("o")
                };

                accountsArray.Add(jo);
                File.WriteAllText(registryPath,
                                  accountsArray.ToString(Formatting.Indented),
                                  Encoding.UTF8);
            } catch (Exception ex) {
                MessageBox.Show(
                    $"Erreur lors de l'enregistrement du compte :\n{ex.Message}",
                    "Erreur Registry",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        private static Dictionary<int, Compte> loadAccounts() {
            var dict = new Dictionary<int, Compte>();
            string exeDir = Application.StartupPath;
            string savesDir = Path.Combine(exeDir, "SavedPlans");
            Directory.CreateDirectory(savesDir);
            string registry = Path.Combine(savesDir, "accounts.json");

            if (!File.Exists(registry))
                return dict;

            var array = JArray.Parse(File.ReadAllText(registry, Encoding.UTF8));
            foreach (JObject token in array) {
                int id = token["Id"]!.Value<int>();
                string name = token["Name"]!.Value<string>();
                string password = token["Password"]!.Value<string>();
                string avatar = token["Avatar"]?.Value<string>() ?? DEFAULT_AVATAR;

                var c = new Compte(
                    id,
                    name,
                    password,
                    avatar,
                    connected: false,
                    favorites: new List<int>(),
                    plans: new List<Plan>());

                dict.Add(id, c);
            }

            if (dict.Count > 0)
                _idCounter = dict.Keys.Max() + 1;

            return dict;
        }




        public void savePlan(Plan plan) {
            try {
                if (plan == null) {
                    throw new ArgumentNullException(nameof(plan), "Le plan ne peut pas être null.");
                }

                if (!Plans.Contains(plan)) {
                    Plans.Add(plan);
                }

                string executablePath = Application.StartupPath;
                string userDirectory = Path.Combine(executablePath, "SavedPlans", this.Name);

                if (!Directory.Exists(userDirectory)) {
                    Directory.CreateDirectory(userDirectory);
                }

                string sanitizedPlanName = string.Join("_", plan.Nom.Split(Path.GetInvalidFileNameChars()));
                string filename = $"{sanitizedPlanName}_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                string filePath = Path.Combine(userDirectory, filename);

                string planJson = PROJET_PIIA.Helpers.JsonHelper.SerializeObject(plan);

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                using (StreamWriter writer = new StreamWriter(fs)) {
                    writer.Write(planJson);
                    writer.Flush();
                }

                UpdatePlanRegistry(plan, filePath);
            } catch (Exception ex) {
                MessageBox.Show($"Erreur lors de la sauvegarde du plan: {ex.Message}", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<Plan> LoadPlans() {
            List<Plan> loadedPlans = new List<Plan>();

            try {
                string userDirectory = Path.Combine(Application.StartupPath, "SavedPlans", this.Name);

                if (!Directory.Exists(userDirectory)) {
                    return loadedPlans; 
                }

                string[] planFiles = Directory.GetFiles(userDirectory, "*.json");
                foreach (string file in planFiles) {
                    try {
                        Plan loadedPlan = PROJET_PIIA.Helpers.JsonHelper.LoadPlanFromJson(file);

                        if (loadedPlan != null) {
                            loadedPlans.Add(loadedPlan);
                        }
                    } catch (Exception ex) {
                        Debug.WriteLine($"File deserialization error: {ex.Message}");
                        continue;
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Erreur lors du chargement des plans: {ex.Message}", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return loadedPlans;
        }

        private void UpdatePlanRegistry(Plan plan, string filePath) {
            try {
                string registryPath = Path.Combine(Application.StartupPath, "SavedPlans", "registry.json");
                Dictionary<string, List<string>> registry = new Dictionary<string, List<string>>();

                if (File.Exists(registryPath)) {
                    string json = File.ReadAllText(registryPath);
                    registry = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json)
                              ?? new Dictionary<string, List<string>>();
                }

                if (!registry.ContainsKey(this.Name)) {
                    registry[this.Name] = new List<string>();
                }

                registry[this.Name].Add(filePath);

                string updatedJson = JsonConvert.SerializeObject(registry, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(registryPath, updatedJson);
            } catch {
            }
        }


        public void deletePlan(Plan p) {
            if (p == null) {
                throw new ArgumentNullException(nameof(p), "Plan cannot be null.");
            }
            _plans.Remove(p);
        }

        public override string ToString() {
            var plansStr = Plans.Count > 0
                ? string.Join(", ", Plans.Select(p => p.ToString()))
                : "Aucun plan";

            return $"Compte [ID: {Id}, Nom: {Name}, pwd:{Password}, Connecté: {Connected}" +
                   $"Avatar: {Avatar}, Plans: [{plansStr}]]";
        }

    }
}
