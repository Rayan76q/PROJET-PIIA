using System.Numerics;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.Extensions {
    public static class PointExtensions {
        // merci chat gpt pour tout ces petits tricks (Connais tu Jésus ?)

        // Calcule la distance entre deux points
        public static float DistanceTo(this PointF p1, PointF p2) {
            (float dx, float dy) = (p2.X - p1.X, p2.Y - p1.Y);
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static PointF Translate(this PointF p, int dx, int dy) {
            return new PointF(p.X + dx, p.Y + dy);
        }

        // Conversion en Size
        //public static Size ToSize(this PointF p) {
        //    return new Size(p.X, p.Y);
        //}

        /*        public static float distance(PointF p1, PointF p2) {
                    (float dx, float dy) = (p1.X - p2.X, p1.Y - p2.Y);
                    return (float)Math.Sqrt(dx * dx + dy * dy);
                }*/

        public static bool is_valid(this PointF p) {
            return !float.IsNaN(p.X) && !float.IsNaN(p.Y)
                && !float.IsInfinity(p.X) && !float.IsInfinity(p.Y);
        }

        public static PointF RotatePoint(this PointF p, (float, float) origin, float angleRad) {

            float translatedX = p.X - origin.Item1;
            float translatedY = p.Y - origin.Item2;


            float rotatedX = translatedX * (float)Math.Cos(angleRad) - translatedY * (float)Math.Sin(angleRad);
            float rotatedY = translatedX * (float)Math.Sin(angleRad) + translatedY * (float)Math.Cos(angleRad);


            float finalX = rotatedX + origin.Item1;
            float finalY = rotatedY + origin.Item2;

            return new PointF((int)finalX, (int)finalY);
        }

        public static float DistancePointSegment(this PointF p, PointF a, PointF b) {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;

            if (dx == 0 && dy == 0) return p.DistanceTo(a);

            float t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));

            float projX = a.X + t * dx;
            float projY = a.Y + t * dy;

            float distX = p.X - projX;
            float distY = p.Y - projY;

            return MathF.Sqrt(distX * distX + distY * distY);
        }

        public static int TrouverSegmentProche(this PointF sourisPlan, List<PointF> perimetre, float seuilProximité = 10f) {
            if (perimetre == null || perimetre.Count < 2) return -1;

            float closestDistance = float.MaxValue;
            int closestSegmentIndex = -1;
            PointF closestProjection = PointF.Empty;

            // Find closest wall segment
            for (int i = 0; i < perimetre.Count; i++) {
                PointF start = perimetre[i];
                PointF end = perimetre[(i + 1) % perimetre.Count];

                // Project the point onto the current segment
                PointF projection = sourisPlan.ProjectPointOntoSegment((start, end));
                float distance = sourisPlan.DistanceTo(projection);

                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestSegmentIndex = i;
                    closestProjection = projection;
                }
            }

            return closestSegmentIndex;
        }

        public static PointF FindCenterPoint(List<PointF> points) {
            float sumX = 0, sumY = 0;

            foreach (var point in points) {
                sumX += point.X;
                sumY += point.Y;
            }

            return new PointF(sumX / points.Count, sumY / points.Count);
        }

        public static List<PointF> ApplyHomothety(List<PointF> points, PointF center, float scaleFactor) {
            List<PointF> scaledPoints = new List<PointF>();

            foreach (var point in points) {
                // Vector from center to point
                float dx = point.X - center.X;
                float dy = point.Y - center.Y;

                // Scale vector
                float scaledX = center.X + dx * scaleFactor;
                float scaledY = center.Y + dy * scaleFactor;

                scaledPoints.Add(new PointF(scaledX, scaledY));
            }

            return scaledPoints;
        }
        // Project point onto a segment (already in your code)
        public static PointF ProjectPointOntoSegment(this PointF point, (PointF Start, PointF End) segment) {
            Vector2 aToB = segment.End.Subtract(segment.Start);
            Vector2 aToP = point.Subtract(segment.Start);
            float t = Vector2.Dot(aToP, aToB) / aToB.LengthSquared();
            t = Math.Clamp(t, 0, 1);
            return segment.Start.Add(aToB.Multiply(t));
        }


        // Vector helpers (already in your code)
        public static Vector2 Subtract(this PointF a, PointF b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static PointF Add(this PointF a, Vector2 b) => new PointF(a.X + b.X, a.Y + b.Y);
        public static Vector2 Multiply(this Vector2 v, float scalar) => new Vector2(v.X * scalar, v.Y * scalar);
    }

}

