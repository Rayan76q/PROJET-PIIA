using PROJET_PIIA.Extensions;

namespace PROJET_PIIA.Model {
    public class Meuble {
        public List<Tag> tags { get; } = new();

        public float? Prix { get; set; }
        public string Description { get; set; } = "";
        public string Nom { get; }
        public string? ImagePath = null;

        public bool IsMural = false; // si doit etre posé contre un mur
        public PointF? Position { get; set; } = null;

        public (float, float) Dimensions { get; set; }
        public float Width => Dimensions.Item1;
        public float Height => Dimensions.Item2;


        public (float, float) Orientation { get; set; } = (1, 0);

        private bool IsValidPositive(float value) => value > 0;
        private bool IsNonEmpty(string value) => !string.IsNullOrEmpty(value);


        //Constructeur
        public Meuble(string nom, (float, float) dim) {
            Nom = nom ?? throw new ArgumentNullException(nameof(nom));

            Dimensions = dim;
        }
        //Full
        public Meuble(string nom, List<Tag> tags, float prix, string description, string image, (float, float) dim) {
            Nom = nom;
            this.tags = tags ?? new List<Tag>();
            Prix = prix;
            Description = description;
            this.ImagePath = image;
            Position = new Point(-1, -1);  //pas encore placé dans le plan
            Dimensions = dim;
        }


        // Helper method to check if a point is inside a meuble
        public bool IsPointInMeuble(PointF point) {
            if (Position == null) return false;
            var pos = Position.Value;
            return point.X >= pos.X &&
                   point.X <= pos.X + Width &&
                   point.Y >= pos.Y &&
                   point.Y <= pos.Y + Height;
        }

        public bool ChevaucheMur(Murs murs) {
            if (!Position.HasValue) return false;

            var pos = Position.Value;
            var angleRad = (float)Math.Atan2(Orientation.Item2, Orientation.Item1);
            var corners = new List<PointF> {
                pos,
                new ((int)(pos.X + Width), pos.Y),
                new ((int)(pos.X + Width), (int)(pos.Y + Height)),
                new (pos.X, (int)(pos.Y + Height))
               };
            var meubleSegments = new List<(PointF, PointF)> {
                (corners[0], corners[1]),
                (corners[1], corners[2]),
                (corners[2], corners[3]),
                (corners[3], corners[0])
            };
            return meubleSegments
                .Any(segMeuble => murs.GetSegments()
                    .Any(segMur => Murs.SegmentsIntersect(segMeuble.Item1, segMeuble.Item2, segMur.Item1, segMur.Item2)));
        }

        public bool ChevaucheMeuble(Meuble autre) {
            if (!Position.HasValue || !autre.Position.HasValue) return false;

            var p1 = Position.Value;
            var p2 = autre.Position.Value;

            return p1.X < p2.X + autre.Width && p1.X + Width > p2.X &&
                   p1.Y < p2.Y + autre.Height && p1.Y + Height > p2.Y;
        }

        public void deplacer(PointF nouvellePos) {
            if (nouvellePos.is_valid()) Position = nouvellePos;
            else throw new ArgumentException("La nouvelle position n'est pas valide.");
        }



        public void tourner(float deltaAngleDegrees, bool fixedDirection) {
            float angleRad = deltaAngleDegrees * (float)Math.PI / 180f;
            float x = (float)Math.Cos(angleRad);
            float y = (float)Math.Sin(angleRad);
            if (fixedDirection) {
                var directions = new (float, float)[]{
                    (-1, -1), (0, -1),(1, -1),  // NW, N, NE
                    (-1, 0), /*   */ (1, 0),    //  W,  , E
                    (-1, 1), (0, 1), (1, 1),   //  SW, S, SE
                };
                (float bestX, float bestY) = directions
                    .OrderBy(dir => Math.Abs(dir.Item1 - x) + Math.Abs(dir.Item2 - y)) 
                    .First();
                Orientation = (bestX, bestY);
            } else {
                (float ox,float oy) = (Orientation.Item1,  Orientation.Item2);
                if (Math.Abs(ox) < 0.001f && Math.Abs(oy) < 0.001f) {
                    ox = 1;
                    oy = 0;
                }
                (float newX, float newY) = (ox * x - oy * y, ox *y + oy * x);
                float magnitude = (float)Math.Sqrt(newX * newX + newY * newY);
                newX /= magnitude; newY /= magnitude;
                Orientation = (newX, newY);
            }
        }


        public float getAngle() {
            // Handle invalid orientation
            if (Math.Abs(Orientation.Item1) < 0.001f &&
                Math.Abs(Orientation.Item2) < 0.001f) {
                return 0f;
            }

            float angleRad = (float)Math.Atan2(Orientation.Item2, Orientation.Item1);
            float angleDeg = angleRad * (180f / (float)Math.PI);

            // Convert to 0-360 range
            if (angleDeg < 0) angleDeg += 360f;
            return angleDeg;
        }

        // Check if a meuble collides with walls or other meubles
        public bool CheckMeubleCollision(List<Meuble> meubles, Murs murs) {
            // Check collision with walls
            if (this.ChevaucheMur(murs)) {
                return true;
            }

            // Check collision with other meubles
            foreach (var otherMeuble in meubles) {
                if (otherMeuble != this && this.ChevaucheMeuble(otherMeuble)) {
                    return true;
                }
            }

            return false;
        }

        public PointF GetCenter() {
            if (Position == null) throw new InvalidOperationException("GetCenter() : Position non définie.");
            return new PointF(Position.Value.X + Width / 2f, Position.Value.Y + Height / 2f);
        }

        public Meuble Copier() {
            return new Meuble(
                this.Nom,
                new List<Tag>(this.tags), 
                this.Prix ?? 0,
                this.Description,
                this.ImagePath,
                this.Dimensions
            ) {
                Position = this.Position,  
                Orientation = this.Orientation 
            };
        }


        public override string ToString() {
            string s = "[ ";
            foreach (Tag tag in tags) {
                s += TagExtensions.GetDisplayName(tag) + " ";
            }
            s += " ]";

            return $"Meuble [Nom: {Nom}, Type: {s}, Prix: {Prix}€," +
                   $" Description: {Description}, Image: {ImagePath}, " +
                   $" Position: {Position}, " +
                   $"Dimensions: ({Dimensions.Item1}x{Dimensions.Item2}), " +
                   $"Orientation: ({Orientation.Item1}, {Orientation.Item2})]";
        }




    }
}
