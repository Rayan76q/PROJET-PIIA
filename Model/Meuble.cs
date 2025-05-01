using PROJET_PIIA.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PROJET_PIIA.Helpers;
namespace PROJET_PIIA.Model {

    public class Meuble {
        public float? Prix { get; set; }
        public string Description { get; set; } = "";
        public string Nom { get; }
        public string? ImagePath = null;

        public bool IsMural = false; // True if the furniture needs to be placed against a wall
        public bool IsPorte = false; // True if it's a door
        public bool IsFenetre = false; // True if it's a window

        // Properties for wall elements (doors and windows)
        private float distPos;
        public float DistPos {
            get => distPos;
            set {
                if (value < 0) throw new ArgumentException("Distance cannot be negative.");
                distPos = value;
            }
        }

        public PointF? Position { get; set; } = null;

        public (float, float) Dimensions { get; set; }
        public float Width;
        public float Height;

        public (float, float) Orientation { get; set; } = (1, 0);

        private bool IsValidPositive(float value) => value > 0;
        private bool IsNonEmpty(string value) => !string.IsNullOrEmpty(value);

        private static int id_counter = 0;
        public readonly int id;
        public int catRef;

        [JsonProperty("tags")]
        [JsonConverter(typeof(Helpers.TagsEnumConverter))]
        public List<Tag> tags { get; private set; }

        [JsonConstructor]
        public Meuble(
            string imagePath,
            bool isMural,
            int id,
            int catRef,
            List<Tag> tags,
            double prix,
            string description,
            string nom,
            PointF? position,
            (float, float) dimensions,
            float width,
            float height,
            (float, float) orientation,
            bool isPorte,
            bool isFenetre,
            float distPos = 0
        ) {
            ImagePath = imagePath;
            IsMural = isMural;
            IsPorte = isPorte;
            IsFenetre = isFenetre;
            this.id = id;
            this.catRef = catRef;
            this.tags = tags ?? new List<Tag>();
            Prix = (float)prix;
            Description = description;
            Nom = nom;
            Position = position;
            Dimensions = dimensions;
            Width = width;
            Height = height;
            Orientation = orientation;
            DistPos = distPos;
        }

        //Basic constructor
        public Meuble(string nom, (float, float) dim, int id) {
            Nom = nom ?? throw new ArgumentNullException(nameof(nom));
            Dimensions = dim;
            this.id = id == -1 ? id_counter++ : id;
         
            Width = dim.Item1;
            Height = dim.Item2;
            tags = new List<Tag>();
        }

        //Full constructor
        public Meuble(string nom, List<Tag> tags, float prix, string description, string image, (float, float) dim, int id) {
            Nom = nom;
            this.tags = tags ?? new List<Tag>();
            Prix = prix;
            Description = description;
            this.ImagePath = image;
            Position = new Point(-1, -1);
            Dimensions = dim;
            this.id = id == -1 ? id_counter++ : id;
            Width = dim.Item1;
            Height = dim.Item2;

            
            IsMural = tags.Contains(Tag.Mural) || tags.Contains(Tag.Porte) || tags.Contains(Tag.Fenetre);

            IsPorte = tags.Contains(Tag.Porte);
            IsFenetre = tags.Contains(Tag.Fenetre);

           
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
            //pas de collision pour les muraux
            if (IsMural || IsPorte || IsFenetre) return false;

            if (!Position.HasValue) return false;

            var pos = Position.Value;
            var angleRad = (float)Math.Atan2(Orientation.Item2, Orientation.Item1);
            var corners = new List<PointF> {
                pos,
                new PointF((pos.X + Width), pos.Y),
                new PointF((pos.X + Width), (pos.Y + Height)),
                new PointF(pos.X, (pos.Y + Height))
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
            // Wall elements don't collide with each other in the same way as regular furniture
            if ((IsMural && (IsPorte || IsFenetre)) || (autre.IsMural && (autre.IsPorte || autre.IsFenetre))) {
                // If both are wall elements, check if they overlap on the wall
                if ((IsMural && (IsPorte || IsFenetre)) && (autre.IsMural && (autre.IsPorte || autre.IsFenetre))) {
                    float start1 = DistPos;
                    float end1 = DistPos + Width;
                    float start2 = autre.DistPos;
                    float end2 = autre.DistPos + autre.Width;

                    // Check for overlap on the wall
                    return !(end1 <= start2 || end2 <= start1);
                }
                return false;
            }

            if (!Position.HasValue || !autre.Position.HasValue) return false;

            var p1 = Position.Value;
            var p2 = autre.Position.Value;

            return p1.X < p2.X + autre.Width && p1.X + Width > p2.X &&
                   p1.Y < p2.Y + autre.Height && p1.Y + Height > p2.Y;
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
                (float ox, float oy) = (Orientation.Item1, Orientation.Item2);
                if (Math.Abs(ox) < 0.001f && Math.Abs(oy) < 0.001f) {
                    ox = 1;
                    oy = 0;
                }
                (float newX, float newY) = (ox * x - oy * y, ox * y + oy * x);
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
            // Wall elements have special collision handling
            if (IsMural && (IsPorte || IsFenetre)) {
                // Check collision with other wall elements only
                foreach (var otherMeuble in meubles) {
                    if (otherMeuble != this && otherMeuble.IsMural && (otherMeuble.IsPorte || otherMeuble.IsFenetre)) {
                        if (this.ChevaucheMeuble(otherMeuble)) {
                            return true;
                        }
                    }
                }
                return false;
            }

            // Regular furniture collision check
            if (this.ChevaucheMur(murs)) {
                return true;
            }

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

        public Meuble Copier(bool copieID) {
            var copy = new Meuble(
                this.Nom,
                new List<Tag>(this.tags),
                this.Prix ?? 0,
                this.Description,
                this.ImagePath,
                this.Dimensions,
                (copieID ? this.id : id_counter++)
            ) {
                Position = this.Position,
                Orientation = this.Orientation,
                IsMural = this.IsMural,
                IsPorte = this.IsPorte,
                IsFenetre = this.IsFenetre,
                DistPos = this.DistPos
            };

            return copy;
        }

        public void setCatRef(int n) {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "Catégorie de référence invalide.");
            this.catRef = n;
        }

        public bool Equals(Meuble? other) {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return ImagePath == other.ImagePath
                && IsMural == other.IsMural
                && IsPorte == other.IsPorte
                && IsFenetre == other.IsFenetre
                && id == other.id
                && Prix == other.Prix
                && Description == other.Description
                && Nom == other.Nom
                && Dimensions == other.Dimensions
                && ((tags == null && other.tags == null)
                    || (tags != null && other.tags != null
                        && tags.Count == other.tags.Count
                        && tags.SequenceEqual(other.tags)));
        }

        public override bool Equals(object? obj)
            => Equals(obj as Meuble);

        public override string ToString() {
            string s = "[ ";
            foreach (Tag tag in tags) {
                s += TagExtensions.GetDisplayName(tag) + " ";
            }
            s += " ]";

            string typeInfo = IsMural ? (IsPorte ? "Porte" : (IsFenetre ? "Fenêtre" : "Mural")) : "Standard";

            return $"Meuble [{typeInfo}] [Nom: {Nom}, ID: {id}, Type: {s}, Prix: {Prix}€," +
                   $" Description: {Description}, Image: {ImagePath}, " +
                   (IsMural ? $" DistPos: {DistPos}, Largeur: {Width}" :
                   $" Position: {Position}, Dimensions: ({Width}x{Height}), " +
                   $"Orientation: ({Orientation.Item1}, {Orientation.Item2})");
        }
    }
}