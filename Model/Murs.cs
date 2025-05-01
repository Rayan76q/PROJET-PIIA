using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Extensions;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace PROJET_PIIA.Model {

    public class Murs {
        List<Meuble> elemsMuraux; // Changed from ElemMur to Meuble
        public List<PointF> Perimetre { get; set; }

        public Murs() {
            List<PointF> points = new();

            //put something if you wanna test a special shape
            //points.Add(new (100, 10));   // sommet 1
            //points.Add(new (120, 70));  // creux 1
            //points.Add(new (180, 70));  // sommet 2
            //points.Add(new (130, 110)); // creux 2
            //points.Add(new (150, 170)); // sommet 3
            //points.Add(new (100, 130)); // creux 3
            //points.Add(new (50, 170));  // sommet 4
            //points.Add(new (70, 110));  // creux 4
            //points.Add(new (20, 70));   // sommet 5
            //points.Add(new (80, 70));   // creux 5

            this.Perimetre = points;
            this.elemsMuraux = new List<Meuble>();
        }

        public Murs(List<PointF> perimetre) {
            //if(perimetre.Count < 3)
            //    throw new ArgumentOutOfRangeException("Le périmètre doit contenir au moins 3 points.");

            //if (checkMurs(perimetre))
            //throw new ArgumentOutOfRangeException("Les mures s'intersectent, impossible de générer la cuisine.");
            this.Perimetre = perimetre;
            this.elemsMuraux = new List<Meuble>();
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

        /// Renvoie l'indice du segment où l'élément est placé et la position normalisée (t) de l'élément dans ce segment.
        public (int segmentIndex, float t) getSegmentForElem(Meuble e) {
            // Check if the element is a wall element (door or window)
            if (!e.IsMural || (!e.IsPorte && !e.IsFenetre)) {
                throw new ArgumentException("L'élément n'est pas un élément mural (porte ou fenêtre).");
            }

            float globalOffset = e.DistPos;
            float current = 0;
            for (int i = 0; i < Perimetre.Count; i++) {
                PointF a = Perimetre[i];
                PointF b = Perimetre[(i + 1) % Perimetre.Count];
                float segmentLength = a.DistanceTo(b);

                if (globalOffset <= current + segmentLength) {
                    // position localisée de l'élément sur le segment
                    float localOffset = globalOffset - current;
                    float t = localOffset / segmentLength;
                    if (t > 1)
                        throw new ArgumentException("L'element est trop large pour le Mur qui est censé l'abriter.");
                    float endOffset = globalOffset + e.Width; // bout de l'élément

                    // si l'élément dépasse la fin du segment
                    if (endOffset > current + segmentLength) {
                        // Si oui, on ajuste la position de l'élément pour le mettre à la fin du segment
                        globalOffset = current + segmentLength - e.Width;
                        t = (segmentLength - e.Width) / segmentLength;
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
                    return new PointF(x, y);
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

     

        public void supprimeElemMur(Meuble e) {
            elemsMuraux.Remove(e);
        }

        // Get all mural elements 
        public List<Meuble> GetElemsMuraux() {
            return elemsMuraux.Where(e => e.IsMural || (e.IsPorte || e.IsFenetre)).ToList();
        }

        // Get only doors
        public List<Meuble> GetPortes() {
            return elemsMuraux.Where(e => e.IsPorte).ToList();
        }

        // Get only windows
        public List<Meuble> GetFenetres() {
            return elemsMuraux.Where(e => e.IsFenetre).ToList();
        }



        public override string ToString() {
            string pointsStr = string.Join(", ", Perimetre.Select(p => p.ToString()));

            string segmentsStr = string.Join("\n  ", GetSegments()
                .Select((seg, i) => $"Segment {i}: {seg.Item1} -> {seg.Item2}"));

            string elemsStr = elemsMuraux.Count > 0
                ? string.Join("\n  ", elemsMuraux
                    .Where(e => e.IsMural && (e.IsPorte || e.IsFenetre))
                    .Select((e, i) => $"{i}: {e}"))
                : "Aucun élément mural.";

            return $"MURS DEBUG:\n" +
                   $"- Périmètre ({Perimetre.Count} points): [{pointsStr}]\n" +
                   $"- Segments:\n  {segmentsStr}\n" +
                   $"- Éléments muraux ({elemsMuraux.Count(e => e.IsMural && (e.IsPorte || e.IsFenetre))}):\n  {elemsStr}";
        }

        public void UpdateElementPositions() {
            // Recalculate positions for all wall elements

            foreach (var elem in elemsMuraux) {
                if (elem.IsMural) {
                    try {
                        // Get the position on the updated wall
                        PointF newPosition = GetPositionForOffset(elem.DistPos);

                        // If it's a door or window, position it on the wall
                        if (elem.IsPorte || elem.IsFenetre || elem.IsMural) {
                            // Get segment details
                            (int segmentIndex, float t) = getSegmentForElem(elem);
                            PointF segmentStart = Perimetre[segmentIndex];
                            PointF segmentEnd = Perimetre[(segmentIndex + 1) % Perimetre.Count];

                            // Calculate angle to align with the wall segment direction
                            float dx = segmentEnd.X - segmentStart.X;
                            float dy = segmentEnd.Y - segmentStart.Y;
                            float angle = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);

                            // Update element position and orientation
                            elem.Position = newPosition;
                            elem.tourner(angle, true);
                        }
                    } catch (ArgumentException ex) {
                        // Log error or handle exceptions
                        System.Diagnostics.Debug.WriteLine($"Error updating element position: {ex.Message}");
                    }
                }
            }
        }



        public float GetOffsetForPosition(PointF position, float epsilon = 1e-2f) {
            float cumulative = 0f;

            for (int i = 0; i < Perimetre.Count; i++) {
                var a = Perimetre[i];
                var b = Perimetre[(i + 1) % Perimetre.Count];

                // Vector from A to B and A to the query point
                float vx = b.X - a.X, vy = b.Y - a.Y;
                float wx = position.X - a.X, wy = position.Y - a.Y;

                float segLenSq = vx * vx + vy * vy;
                if (segLenSq < 1e-9f) {
                    // Degenerate segment, skip
                    continue;
                }

                // Project W onto V, parameterized by t in [0,1]
                float t = (vx * wx + vy * wy) / segLenSq;

                // Check if projection lies on the segment (with tolerance)
                if (t >= -epsilon && t <= 1 + epsilon) {
                    // Compute the closest point on AB
                    t = Math.Clamp(t, 0f, 1f);
                    var projX = a.X + t * vx;
                    var projY = a.Y + t * vy;

                    // Check distance from actual point to projection
                    var dx = position.X - projX;
                    var dy = position.Y - projY;
                    if (dx * dx + dy * dy <= epsilon * epsilon) {
                        // It's on this segment: return cumulative length + local offset
                        float segmentLength = (float)Math.Sqrt(segLenSq);
                        return cumulative + t * segmentLength;
                    }
                }

                cumulative += (float)Math.Sqrt(segLenSq);
            }

            throw new ArgumentException("La position n'appartient à aucun segment du mur.");
        }

      
        public Murs Clone() {
            // 1) Copy perimeter points
            var perimeterCopy = Perimetre
                .Select(p => new PointF(p.X, p.Y))
                .ToList();

            // 2) Create the new Murs with that perimeter
            var copy = new Murs(perimeterCopy);

            // 3) Copy over the meubles
            //    If you want a deep‐copy of each Meuble, replace `e` with e.Clone()
            foreach (var e in elemsMuraux)
                copy.elemsMuraux.Add(e);

            return copy;
        }


        public void placerElem(Meuble e, PointF planPt) {
            try {
                // Check if this is a dragging operation - in this case, just update the position
                // This preserves the ability to freely move furniture with the mouse
                if (e.Position != null && elemsMuraux.Contains(e)) {
                    e.Position = planPt;
                    return;
                }

                // Initial placement - stick to wall
                int closestSegmentIndex = PointExtensions.TrouverSegmentProche(planPt, Perimetre);
                if (closestSegmentIndex != -1) {
                    PointF segmentStart = Perimetre[closestSegmentIndex];
                    PointF segmentEnd = Perimetre[(closestSegmentIndex + 1) % Perimetre.Count];
                    PointF closestProjection = PointExtensions.ProjectPointOntoSegment(planPt, (segmentStart, segmentEnd));

                    // Calculate wall direction and angle
                    float dx = segmentEnd.X - segmentStart.X;
                    float dy = segmentEnd.Y - segmentStart.Y;
                    float angle = (float)(Math.Atan2(dy, dx) * 180 / Math.PI);
                    e.tourner(angle, true);

                    // Calculate normal vector (perpendicular to wall, pointing inward)
                    float normalX = -dy;
                    float normalY = dx;
                    float normalLength = (float)Math.Sqrt(normalX * normalX + normalY * normalY);
                    normalX /= normalLength;
                    normalY /= normalLength;

                    // Check if normal points inside or outside - flip if needed
                    PointF testPoint = new PointF(
                        closestProjection.X + normalX,
                        closestProjection.Y + normalY
                    );

                    if (!IsPointInsidePerimeter(testPoint)) {
                        normalX = -normalX;
                        normalY = -normalY;
                    }

                    
                        // For furniture, we need to account for its dimensions and top-left position
                        // Calculate where to place the element so its closest edge is against the wall

                        // First determine which edge will be against the wall based on rotation
                        float angleRad = angle * (float)Math.PI / 180f;
                        float sin = (float)Math.Sin(angleRad);
                        float cos = (float)Math.Cos(angleRad);

                        // Offset needed to align the furniture with the wall
                        float offsetX = 0;
                        float offsetY = 0;

                        // Handle different wall orientations
                        if (Math.Abs(sin) > Math.Abs(cos)) {
                            // Wall is more vertical
                            angle += 90;
                            if (sin > 0) {
                                // Wall is facing right, so place left edge of furniture against wall
                                offsetX = -e.Width;
                                offsetY = 0;
                                
                            } else {
                                // Wall is facing left, so place right edge of furniture against wall
                                offsetX = 0;
                                offsetY = 0;
                            }
                        } else {
                            // Wall is more horizontal
                            if (cos > 0) {
                                // Wall is facing down, so place top edge of furniture against wall
                                offsetX = 0;
                                offsetY = 0;
                            } else {
                                // Wall is facing up, so place bottom edge of furniture against wall
                                offsetX = 0;
                                offsetY = -e.Height;
                            }
                        }

                        // Calculate final position
                        e.Position = new PointF(
                            closestProjection.X + offsetX,
                            closestProjection.Y + offsetY
                        );
                    

                    e.DistPos = GetOffsetForPosition(closestProjection);
                    e.tourner(angle, false);

                    if (!elemsMuraux.Contains(e)) {
                        Debug.WriteLine(e.Nom);
                        elemsMuraux.Add(e);
                    }

                    Debug.WriteLine(this);
                }
            } catch (ArgumentException ex) {
                Debug.WriteLine($"Placement failed: {ex.Message}");
                return;
            }
        }

        // Helper method to determine if a point is inside the perimeter
        private bool IsPointInsidePerimeter(PointF point) {
            int crossings = 0;
            for (int i = 0; i < Perimetre.Count; i++) {
                PointF p1 = Perimetre[i];
                PointF p2 = Perimetre[(i + 1) % Perimetre.Count];

                // Check if point is on an edge
                if (IsPointOnSegment(point, p1, p2)) {
                    return true; // Point is on the perimeter
                }

                // Ray-casting algorithm
                if (((p1.Y <= point.Y) && (p2.Y > point.Y)) || ((p1.Y > point.Y) && (p2.Y <= point.Y))) {
                    float intersectX = p1.X + (point.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y);
                    if (point.X < intersectX) {
                        crossings++;
                    }
                }
            }

            // Odd number of crossings means inside
            return (crossings % 2 != 0);
        }

        // Helper method to check if a point is on a line segment
        private bool IsPointOnSegment(PointF point, PointF segStart, PointF segEnd) {
            float tolerance = 0.001f; // Adjust as needed

            // Use your existing DistancePointToSegment method
            float distanceToSegment = PointExtensions.DistancePointSegment(point, segStart, segEnd);
            return distanceToSegment < tolerance;
        }


    }
}