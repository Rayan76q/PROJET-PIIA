using System.Diagnostics;
using System.Text;
using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Extensions;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace PROJET_PIIA.Model {
    public class Plan {
       

        public int Id { get; }
        public string Nom { get; set; }
        public Murs Murs { get; set;  }

        [JsonProperty("Meubles")]
        public List<Meuble> Meubles { get; private set; }


        [JsonConstructor]
        public Plan(int id, string nom, Murs murs, List<Meuble> meubles) {
            Id = id;
            Nom = string.IsNullOrEmpty(nom) ? "Plan" : nom;
            Murs = murs ?? new Murs();

            // Initialize the collection if null
            if (meubles == null) {
                Meubles = new List<Meuble>();
            } else {
                // Important: We need to create a new list here to respect the read-only property
                Meubles = new List<Meuble>(meubles);
            }
        }


        public Plan(string nom = "") {
           
            Nom = nom=="" ? "Plan" : nom;
            Murs = new Murs();
            Meubles = new List<Meuble>();
        }


        public void PlacerMeuble(Meuble meuble, PointF position) { 
            meuble.Position = position;
            Meubles.Add(meuble);
        }


        // find a meuble at a specific point
        public Meuble? FindMeubleAtPoint(PointF planPoint) {
            if (Meubles == null || Meubles.Count == 0) return null;

            // Check each meuble in reverse order (so topmost is selected first)
            for (int i = Meubles.Count - 1; i >= 0; i--) {
                Meuble meuble = Meubles[i];
                if (meuble.IsPointInMeuble(planPoint)) {
                    return meuble;
                }
            }
            return null;
        }

        public int FindMurAtPoint(PointF planPoint) {
            if (Murs == null || Murs.Perimetre == null || Murs.Perimetre.Count < 2)
                return -1;

            return planPoint.TrouverSegmentProche(Murs.Perimetre);
        }

        public void tournerMeuble(Meuble meuble, float angle, bool fixeddirection) {
            if (Meubles.Contains(meuble)) {
                meuble.tourner(angle, fixeddirection);
            } else {
                throw new ArgumentException("Le meuble n'est pas présent dans le plan.");
            }
        }


        public void SupprimerMeuble(Meuble m) {
            Meuble r = findMeuble(m);
            if (r!=null) {
                Meubles.Remove(r);
            } else {
                throw new ArgumentException("Le meuble n'est pas présent dans le plan.");
            }
        }

        public Meuble? findMeuble(Meuble meub) {
            foreach(Meuble m in Meubles) {
                if(meub.Equals(m)) {
                    return m;
                }
            }
            return null;
        }

        


        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Plan ID: {Id}");
            sb.AppendLine($"Nom: {Nom}");
            sb.AppendLine("Murs: ");
            sb.AppendLine(Murs.ToString());  // Assuming Murs has a ToString method

            sb.AppendLine("Meubles: ");
            if (Meubles.Count == 0) {
                sb.AppendLine("  Aucun meuble");
            } else {
                foreach (var meuble in Meubles) {
                    sb.AppendLine($"  - {meuble.ToString()}");
                }
            }

            return sb.ToString();
        }





    }
}
