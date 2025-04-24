using System.Text;
using PROJET_PIIA.Extensions;

namespace PROJET_PIIA.Model {
    public class Plan {
        private static int _idCounter = 0;

        public int Id { get; }
        public string Nom { get; set; }
        public Murs Murs { get; set;  }
        public List<Meuble> Meubles { get; }

        public Plan(string nom = "") {
            Id = _idCounter++;
            Nom = nom ?? $"Plan N°{Id}";
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
            if (Meubles.Contains(m)) {
                m.Position = new Point(-1, -1);
                m.Orientation = (1, 0);
                Meubles.Remove(m);
            } else {
                throw new ArgumentException("Le meuble n'est pas présent dans le plan.");
            }
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
