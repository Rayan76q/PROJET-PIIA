using PROJET_PIIA.Extensions;

namespace PROJET_PIIA.Model {

    public class Murs {

        public List<Point> perimetre;
        List<ElemMur> elemsmuraux;



        public List<Point> Perimetre {
            get => perimetre;
        }

        public Murs() {
            List<Point> points = new List<Point>();
            // etoile ☼
            points.Add(new Point(300, 150)); // sommet haut ↑
            points.Add(new Point(320, 200));
            points.Add(new Point(370, 200));
            points.Add(new Point(330, 230));
            points.Add(new Point(350, 280));
            points.Add(new Point(300, 250)); // centre bas ↓
            points.Add(new Point(250, 280));
            points.Add(new Point(270, 230));
            points.Add(new Point(230, 200));
            points.Add(new Point(280, 200));
            this.perimetre = points;
            this.elemsmuraux = new List<ElemMur>();
        }


        // mettre dans un controleur ?
        public static bool checkMurs(List<Point> points) {
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

        // mettre dans un controleur ?
        public static bool SegmentsIntersect(Point p1, Point q1, Point p2, Point q2) {
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

        private static int Orientation(Point p, Point q, Point r) {
            float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (Math.Abs(val) < 1e-6) return 0;  // Colinear
            return val > 0 ? 1 : 2;           // Clockwise or Counterclockwise
        }

        private static bool OnSegment(Point p, Point q, Point r) {
            return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                   q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
        }







        public Murs(List<Point> perimetre) {
            //if(perimetre.Count < 3)
            //    throw new ArgumentOutOfRangeException("Le périmètre doit contenir au moins 3 points.");

            if (checkMurs(perimetre))
                throw new ArgumentOutOfRangeException("Les mures s'intersectent, impossible de générer la cuisine.");
            this.perimetre = perimetre;
            this.elemsmuraux = new List<ElemMur>();
        }

        float GetPerimetreLength() {
            float total = 0;
            for (int i = 0; i < perimetre.Count; i++) {
                Point a = perimetre[i];
                Point b = perimetre[(i + 1) % perimetre.Count];
                total += a.DistanceTo(b);
            }
            return total;
        }

        /// Renvoie l'indice du segment où la porte est placée et la position normalisée (t) de la porte dans ce segment.
        public (int segmentIndex, float t) getSegmentForElem(ElemMur e) {
            float globalOffset = e.DistPos;
            float current = 0;
            for (int i = 0; i < perimetre.Count; i++) {
                Point a = perimetre[i];
                Point b = perimetre[(i + 1) % perimetre.Count];
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

            return (perimetre.Count - 1, 0);
        }

        /// renvoit la vraie position d'un point 
        /// mettre dans uncontroleur ?
        public Point GetPositionForOffset(float offset) {
            float current = 0;
            for (int i = 0; i < perimetre.Count; i++) {
                Point a = perimetre[i];
                Point b = perimetre[(i + 1) % perimetre.Count];
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
            Point last = perimetre.Last();
            return new Point(last.X, last.Y);
        }


        public List<(Point, Point)> GetSegments() {
            var segments = new List<(Point, Point)>();
            for (int i = 0; i < perimetre.Count; i++) {
                segments.Add((perimetre[i], perimetre[(i + 1) % perimetre.Count]));
            }
            return segments;
        }

        public void addElemMur(ElemMur e) {
            (int, float) pos = getSegmentForElem(e);

            if (pos == (perimetre.Count - 1, 0)) {  //check si le dernier segment peut accueillir l'elem
                if (perimetre.Last().DistanceTo(perimetre[0]) <= e.Largeur) {
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


        public override string ToString() {
            string pointsStr = string.Join(", ", perimetre.Select(p => p.ToString()));

            string segmentsStr = string.Join("\n  ", GetSegments()
                .Select((seg, i) => $"Segment {i}: {seg.Item1} -> {seg.Item2}"));

            string elemsStr = elemsmuraux.Count > 0
                ? string.Join("\n  ", elemsmuraux.Select((e, i) => $"{i}: {e}"))
                : "Aucun élément mural.";

            return $"MURS DEBUG:\n" +
                   $"- Périmètre ({perimetre.Count} points): [{pointsStr}]\n" +
                   $"- Segments:\n  {segmentsStr}\n" +
                   $"- Éléments muraux ({elemsmuraux.Count}):\n  {elemsStr}";
        }



    }


}
