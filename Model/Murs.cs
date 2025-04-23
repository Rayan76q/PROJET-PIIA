using PROJET_PIIA.Extensions;

namespace PROJET_PIIA.Model {

    public class Murs {

        
        List<ElemMur> elemsmuraux;
        public List<PointF> Perimetre { get; set; }

        public Murs() {
            List<PointF> points = new ();

            //put something if you wanna test a special shape
            points.Add(new (100, 10));   // sommet 1
            points.Add(new (120, 70));  // creux 1
            points.Add(new (180, 70));  // sommet 2
            points.Add(new (130, 110)); // creux 2
            points.Add(new (150, 170)); // sommet 3
            points.Add(new (100, 130)); // creux 3
            points.Add(new (50, 170));  // sommet 4
            points.Add(new (70, 110));  // creux 4
            points.Add(new (20, 70));   // sommet 5
            points.Add(new (80, 70));   // creux 5

            this.Perimetre = points;
            this.elemsmuraux = new List<ElemMur>();
        }

        public Murs(List<PointF> perimetre) {
            //if(perimetre.Count < 3)
            //    throw new ArgumentOutOfRangeException("Le périmètre doit contenir au moins 3 points.");

            //if (checkMurs(perimetre))
                //throw new ArgumentOutOfRangeException("Les mures s'intersectent, impossible de générer la cuisine.");
            this.Perimetre = perimetre;
            this.elemsmuraux = new List<ElemMur>();
        }



        public static bool checkMurs(List<PointF> points) {
            for (int i = 0; i < points.Count - 1; i++) {
                var p1 = points[i];
                var q1 = points[i + 1];

                for (int j = i + 2; j < points.Count - 1; j++) {
                    if (i == j - 1) continue; // Skip adjacent segments

                    var p2 = points[j];
                    var q2 = points[j + 1];

                    if (SegmentsIntersect(p1, q1, p2, q2))
                        return true;
                }
            }
            return false;
        }

       
        public static bool SegmentsIntersect(PointF p1, PointF q1, PointF p2, PointF q2) {
            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special cases
            if (o1 == 0 && OnSegment(p1, p2, q1)) return true;
            if (o2 == 0 && OnSegment(p1, q2, q1)) return true;
            if (o3 == 0 && OnSegment(p2, p1, q2)) return true;
            if (o4 == 0 && OnSegment(p2, q1, q2)) return true;

            return false;
        }

        private static int Orientation(PointF p, PointF q, PointF r) {
            float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (Math.Abs(val) < 1e-6) return 0;  // Colinear
            return val > 0 ? 1 : 2;           // Clockwise or Counterclockwise
        }

        private static bool OnSegment(PointF p, PointF q, PointF r) {
            return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                   q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
        }

        float GetPerimetreLength() {
            float total = 0;
            for (int i = 0; i < Perimetre.Count; i++) {
                PointF a = Perimetre[i];
                PointF b = Perimetre[(i + 1) % Perimetre.Count];
                total += a.DistanceTo(b);
            }
            return total;
        }

        public float Area() {
            int n = Perimetre.Count;
            float area = 0;
            for (int i = 0; i < n; i++) {
                PointF p1 = Perimetre[i];
                PointF p2 = Perimetre[(i + 1) % n];
                area += (p1.X * p2.Y) - (p2.X * p1.Y);
            }
            return Math.Abs(area) / 2;
        }

        /// Renvoie l'indice du segment où la porte est placée et la position normalisée (t) de la porte dans ce segment.
        public (int segmentIndex, float t) getSegmentForElem(ElemMur e) {
            float globalOffset = e.DistPos;
            float current = 0;
            for (int i = 0; i < Perimetre.Count; i++) {
                PointF a = Perimetre[i];
                PointF b = Perimetre[(i + 1) % Perimetre.Count];
                float segmentLength = a.DistanceTo(b);

                if (globalOffset <= current + segmentLength) {
                    // position localisée de la porte sur le segment
                    float localOffset = globalOffset - current;
                    float t = localOffset / segmentLength;
                    if(t > 1)
                        throw new ArgumentException("L'element est trop large pour le Mur qui est censé l'abriter.");
                    float endOffset = globalOffset + e.Largeur; // bout de la porte/fenetre

                    // si la porte dépasse la fin du segment
                    if (endOffset > current + segmentLength) {
                        // Si oui, on ajuste la position de la porte pour la mettre à la fin du segment
                        globalOffset = current + segmentLength - e.Largeur;
                        t = (segmentLength - e.Largeur) / segmentLength;
                    }
                    return (i, t);
                }

                current += segmentLength;
            }

            return (Perimetre.Count - 1, 0);
        }

        /// renvoit la vraie position d'un point 
        public PointF GetPositionForOffset(float offset) {
            float current = 0;
            for (int i = 0; i < Perimetre.Count; i++) {
                PointF a = Perimetre[i];
                PointF b = Perimetre[(i + 1) % Perimetre.Count];
                float segmentLength = a.DistanceTo(b);

                // Si l'offset tombe dans ce segment
                if (offset <= current + segmentLength) {
                    float t = (offset - current) / segmentLength;
                    // position sur le segment
                    float x = a.X + t * (b.X - a.X);
                    float y = a.Y + t * (b.Y - a.Y);
                    return new Point((int)x, (int)y);
                }
                current += segmentLength;
            }
            PointF last = Perimetre.Last();
            return new PointF(last.X, last.Y);
        }


        public List<(PointF, PointF)> GetSegments() {
            var segments = new List<(PointF, PointF)>();
            for (int i = 0; i < Perimetre.Count; i++) {
                segments.Add((Perimetre[i], Perimetre[(i + 1) % Perimetre.Count]));
            }
            return segments;
        }

        public void addElemMur(ElemMur e) {
            (int, float) pos = getSegmentForElem(e);

            if (pos == (Perimetre.Count - 1, 0)) {  //check si le dernier segment peut accueillir l'elem
                if (Perimetre.Last().DistanceTo(Perimetre[0]) <= e.Largeur) {
                    throw new ArgumentException("La porte ou la fenêtre ne peut pas être placée car trop large.");
                } else {
                    elemsmuraux.Add(e);
                }
            } 
            else {
                elemsmuraux.Add(e);
            }


        }

        public void supprimeElemMur(ElemMur e) {
            elemsmuraux.Remove(e);
        }


        //C'est pour les élongations
        public void ModifierSegment(int indexSegment, float distance) {
            if (indexSegment < 0 || indexSegment >= Perimetre.Count)
                throw new ArgumentOutOfRangeException(nameof(indexSegment));

            PointF a = Perimetre[indexSegment];
            PointF b = Perimetre[(indexSegment + 1) % Perimetre.Count];

            // Vecteur du segment
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;

            // Normale (orthogonale) - on inverse dx/dy et on change un signe
            float nx = -dy;
            float ny = dx;

            // Normalisation
            float length = MathF.Sqrt(nx * nx + ny * ny);
            nx /= length;
            ny /= length;

            // Appliquer le déplacement le long de la normale
            Point newA = new Point((int)(a.X + nx * distance), (int)(a.Y + ny * distance));
            Point newB = new Point((int)(b.X + nx * distance), (int)(b.Y + ny * distance));

            Perimetre[indexSegment] = newA;
            Perimetre[(indexSegment + 1) % Perimetre.Count] = newB;

            // (optionnel) throw si les murs s'intersectent
            if (Murs.checkMurs(Perimetre))
                throw new ArgumentException("La modification crée des intersections entre murs.");
        }



        public override string ToString() {
            string pointsStr = string.Join(", ", Perimetre.Select(p => p.ToString()));

            string segmentsStr = string.Join("\n  ", GetSegments()
                .Select((seg, i) => $"Segment {i}: {seg.Item1} -> {seg.Item2}"));

            string elemsStr = elemsmuraux.Count > 0
                ? string.Join("\n  ", elemsmuraux.Select((e, i) => $"{i}: {e}"))
                : "Aucun élément mural.";

            return $"MURS DEBUG:\n" +
                   $"- Périmètre ({Perimetre.Count} points): [{pointsStr}]\n" +
                   $"- Segments:\n  {segmentsStr}\n" +
                   $"- Éléments muraux ({elemsmuraux.Count}):\n  {elemsStr}";
        }



    }


}
