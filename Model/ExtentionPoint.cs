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

            // Exemple de validité : coordonnées finies
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
    }
}
