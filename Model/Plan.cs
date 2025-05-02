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
            
            if (meuble.IsMural) {
                Murs.placerElem(meuble, position);
            } else {
                meuble.Position = position;
            }
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
            Meuble? r = findMeuble(m);
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



        public bool EstDansEspaceDesMurs(Meuble meuble) {
            if (meuble == null || Murs.Perimetre == null || Murs.Perimetre.Count < 2)
                return false;

            // Obtenir les coins du meuble avant rotation
            List<PointF> coins = GetCoins(meuble);

            // Appliquer la rotation du meuble sur ses coins
            var rotatedCoins = new List<PointF>();
            foreach (var coin in coins) {
                rotatedCoins.Add(RotaterPoint(coin, meuble.Position.Value, meuble.Orientation));
            }

            // Vérifier si chaque coin est dans l'espace défini par les murs
            foreach (var rotatedCoin in rotatedCoins) {
                if (!EstPointDansPlan(rotatedCoin)) {
                    return false;
                }
            }

            return true;
        }

       
        private List<PointF> GetCoins(Meuble meuble) {
            List<PointF> coins = new List<PointF>();

            if (meuble.Position.HasValue) {
                float x = meuble.Position.Value.X;
                float y = meuble.Position.Value.Y;
                float width = meuble.Width;
                float height = meuble.Height;
                coins.Add(new PointF(x, y)); 
                coins.Add(new PointF(x + width, y));
                coins.Add(new PointF(x, y + height)); 
                coins.Add(new PointF(x + width, y + height));
            }

            return coins;
        }

        // Méthode pour appliquer la rotation d'un point autour d'un centre donné
        private PointF RotaterPoint(PointF point, PointF center, (float, float) orientation) {
            float radian = (float)Math.Atan2(orientation.Item2, orientation.Item1);
            float cosAngle = (float)Math.Cos(radian);
            float sinAngle = (float)Math.Sin(radian);

            float dx = point.X - center.X;
            float dy = point.Y - center.Y;

            // Rotation en 2D
            float xRot = center.X + cosAngle * dx - sinAngle * dy;
            float yRot = center.Y + sinAngle * dx + cosAngle * dy;

            return new PointF(xRot, yRot);
        }

        // Méthode pour vérifier si un point est à l'intérieur des murs du plan
        private bool EstPointDansPlan(PointF point) {
            for (int i = 0; i < Murs.Perimetre.Count; i++) {
                PointF murStart = Murs.Perimetre[i];
                PointF murEnd = Murs.Perimetre[(i + 1) % Murs.Perimetre.Count];

                if (!EstPointAVersMurs(point, murStart, murEnd)) {
                    return false;
                }
            }
            return true;
        }

        // Vérifie si un point est du bon côté d'un segment de mur (en utilisant un produit vectoriel)
        private bool EstPointAVersMurs(PointF point, PointF murStart, PointF murEnd) {
            // Calcul de la direction du mur et de la direction du point par rapport à ce mur
            float dxMur = murEnd.X - murStart.X;
            float dyMur = murEnd.Y - murStart.Y;
            float dxPoint = point.X - murStart.X;
            float dyPoint = point.Y - murStart.Y;

            // Calcul du produit vectoriel (déterminant) pour savoir de quel côté du mur est le point
            float produitCroise = dxMur * dyPoint - dyMur * dxPoint;

            // Si le produit est positif, le point est à gauche, sinon à droite (on veut les points à gauche des murs)
            return produitCroise >= 0;
        }

        private bool SegmentsSeCroisent(PointF p1, PointF p2, PointF q1, PointF q2) {
            // Fonction pour calculer l'orientation
            int Orientation(PointF a, PointF b, PointF c) {
                float val = (b.Y - a.Y) * (c.X - b.X) - (b.X - a.X) * (c.Y - b.Y);
                if (val == 0) return 0;  // colinéaire
                return (val > 0) ? 1 : 2;  // horaire ou anti-horaire
            }

            bool SurSegment(PointF a, PointF b, PointF c) {
                return b.X <= Math.Max(a.X, c.X) && b.X >= Math.Min(a.X, c.X) &&
                       b.Y <= Math.Max(a.Y, c.Y) && b.Y >= Math.Min(a.Y, c.Y);
            }

            int o1 = Orientation(p1, p2, q1);
            int o2 = Orientation(p1, p2, q2);
            int o3 = Orientation(q1, q2, p1);
            int o4 = Orientation(q1, q2, p2);

            // Cas général
            if (o1 != o2 && o3 != o4)
                return true;

            // Cas spéciaux (colinéarité)
            if (o1 == 0 && SurSegment(p1, q1, p2)) return true;
            if (o2 == 0 && SurSegment(p1, q2, p2)) return true;
            if (o3 == 0 && SurSegment(q1, p1, q2)) return true;
            if (o4 == 0 && SurSegment(q1, p2, q2)) return true;

            return false;
        }

        public bool MursSeCroisent() {
            var perimetre = Murs?.Perimetre;
            if (perimetre == null || perimetre.Count < 4)
                return false;
            for (int i = 0; i < perimetre.Count; i++) {
                PointF p1 = perimetre[i];
                PointF p2 = perimetre[(i + 1) % perimetre.Count];

                for (int j = i + 1; j < perimetre.Count; j++) {
                    PointF q1 = perimetre[j];
                    PointF q2 = perimetre[(j + 1) % perimetre.Count];
                    if (SegmentsOntUnPointEnCommun(p1, p2, q1, q2))
                        continue;
                    if (SegmentsSeCroisent(p1, p2, q1, q2)) {
                        return true;
                    }
                }
            }
            return false;
        }

        bool SegmentsOntUnPointEnCommun(PointF a1, PointF a2, PointF b1, PointF b2) {
            return a1 == b1 || a1 == b2 || a2 == b1 || a2 == b2;
        }


    }
}
