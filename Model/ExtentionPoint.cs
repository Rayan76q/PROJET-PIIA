namespace PROJET_PIIA.Extensions {
    public static class PointExtensions {
        // merci chat gpt pour tout ces petits tricks (Connais tu Jésus ?)

        // Calcule la distance entre deux points
        public static float DistanceTo(this Point p1, Point p2) {
            (int dx, int dy)= (p2.X - p1.X, p2.Y - p1.Y);
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        // Retourne un nouveau point après une translation
        public static Point Translate(this Point p, int dx, int dy) {
            return new Point(p.X + dx, p.Y + dy);
        }

        // Conversion en Size
        public static Size ToSize(this Point p) {
            return new Size(p.X, p.Y);
        }

/*        public static float distance(Point p1, Point p2) {
            (float dx, float dy) = (p1.X - p2.X, p1.Y - p2.Y);
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }*/

        public static bool is_valid(this Point p) {
            return !float.IsNaN(p.X) && !float.IsNaN(p.Y)
                && !float.IsInfinity(p.X) && !float.IsInfinity(p.Y);
        }

        public static Point RotatePoint(this Point p, (float, float) origin, float angleRad) {

            float translatedX = p.X - origin.Item1;
            float translatedY = p.Y - origin.Item2;


            float rotatedX = translatedX * (float)Math.Cos(angleRad) - translatedY * (float)Math.Sin(angleRad);
            float rotatedY = translatedX * (float)Math.Sin(angleRad) + translatedY * (float)Math.Cos(angleRad);


            float finalX = rotatedX + origin.Item1;
            float finalY = rotatedY + origin.Item2;

            return new Point((int)finalX, (int)finalY);
        }

        public static float DistancePointSegment(this Point p, Point a, Point b) {
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

        public static int? TrouverSegmentProche(this Point sourisPlan, List<Point> perimetre, float seuilProximité = 10f) {
            if (perimetre == null || perimetre.Count < 2) return null;

            for (int i = 0; i < perimetre.Count; i++) {
                Point p1 = perimetre[i];
                Point p2 = perimetre[(i + 1) % perimetre.Count];

                float distance = sourisPlan.DistancePointSegment(p1, p2);
                if (distance < seuilProximité) {
                    return i;
                }
            }

            return null;
        }
    }
}
