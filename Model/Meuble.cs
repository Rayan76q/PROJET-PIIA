﻿using PROJET_PIIA.Extensions;

namespace PROJET_PIIA.Model {
    public class Meuble {
        public List<Tags> tags { get; }

        private float? _prix = null;
        public float? Prix {
            get => _prix;
            set {
                if (value.HasValue && IsValidPositive(value.Value)) _prix = value;
                else _prix = _prix;
            }
        }

        public string Description { get; set; } = "";

        private string _nom;
        public string Nom {
            get => _nom;
            set => _nom = IsNonEmpty(value) ? value : _nom;
        }

        public string? ImagePath = null;

        // si doit etre posé contre un mur
        public bool IsMural = false;
        

        private Point _position;
        public Point Position {
            get => _position;
            set {
                    _position = value;
            }
        }

        private (float, float) _dimensions;
        // verbeux les fonction imo
        public (float, float) Dimensions {
            get => _dimensions;
            set => _dimensions = IsValidPositive(value.Item1) && IsValidPositive(value.Item2) ? value : _dimensions;
        }

        private (float, float) _orientation = (1, 0);
        public (float, float) Orientation {
            get => _orientation;
            set {
                // Add vector magnitude check
                float magnitude = (float)Math.Sqrt(value.Item1 * value.Item1 + value.Item2 * value.Item2);
                if (magnitude > 0.9f && magnitude < 1.1f) { // Allow slight floating point imprecision
                    _orientation = value;
                }
            }
        }

        // en vrai je ne comprends pas à  100% de creer plusieurs attribut qui en englobe un


        private bool IsValidPositive(float value) => value > 0;
        private bool IsNonEmpty(string value) => !string.IsNullOrEmpty(value);


        //Constructeur
        public Meuble(string nom, (float, float) dim) {
            Nom = nom;
            Dimensions = dim;
        }
        //Full
        public Meuble(string nom, List<Tags> tags, float prix, string description, string image,  (float, float) dim) {
            Nom = nom;
            this.tags = tags ?? new List<Tags>(); 
            Prix = prix;
            Description = description;
            this.ImagePath = image;
            Position = new Point(-1,-1);  //pas encore placé dans le plan
            Dimensions = dim;
            Orientation = (0, 0);
        }



        // a mettre dans un controleur ?
        public bool ChevaucheMur(Murs murs) {
            if (Position == new Point(-1,-1))
                return false; // pas encore placé

            Point meublePos = Position;
            float largeur = Dimensions.Item1;
            float hauteur = Dimensions.Item2;


            float angleRad = (float)Math.Atan2(Orientation.Item2, Orientation.Item1);


            List<Point> corners = new List<Point> {
            meublePos, // Top-left
            new Point((int)(meublePos.X + largeur), meublePos.Y), // Top-right
            new Point((int)(meublePos.X + largeur), (int)(meublePos.Y + hauteur)), // Bottom-right
            new Point(meublePos.X, (int)(meublePos.Y + hauteur)) // Bottom-left
         };


            var meubleSegments = new List<(Point, Point)>
            {
                (corners[0], corners[1]),
                (corners[1], corners[2]),
                (corners[2], corners[3]),
                (corners[3], corners[0])
            };


            var mursSegments = murs.GetSegments();


            foreach (var segMeuble in meubleSegments) {
                foreach (var segMur in mursSegments) {
                    if (Murs.SegmentsIntersect(segMeuble.Item1, segMeuble.Item2, segMur.Item1, segMur.Item2))
                        return true;
                }
            }

            return false;
        }

        // a mettre dans un controleur ?
        public bool chevaucheMeuble(Meuble autre) {
            if (Position == null || autre.Position == null)
                return false; // au moins un meuble n'est pas placé

            var (x1, y1) = (Position.X, Position.Y);
            var (w1, h1) = Dimensions;

            var (x2, y2) = (autre.Position.X, autre.Position.Y);
            var (w2, h2) = autre.Dimensions;

            bool overlapX = x1 < x2 + w2 && x1 + w1 > x2;
            bool overlapY = y1 < y2 + h2 && y1 + h1 > y2;

            return overlapX && overlapY;
        }

        // a mettre dans un controleur ?
        public void deplacer(Point nouvellePos) {
            if (nouvellePos.is_valid()) {
                Position = nouvellePos;
            } else {
                throw new ArgumentException("La nouvelle position n'est pas valide.");
            }
        }

        // a mettre dans un controleur ?
        // In the Meuble.cs file, modify the tourner method:
        public void tourner(float deltaAngleDegrees) {
            float deltaAngleRad = deltaAngleDegrees * (float)Math.PI / 180f;

            // Get current orientation with fallback to (1,0)
            float ox = Orientation.Item1;
            float oy = Orientation.Item2;

            // Handle zero-vector edge case
            if (Math.Abs(ox) < 0.001f && Math.Abs(oy) < 0.001f) {
                ox = 1;
                oy = 0;
            }

            // Apply rotation matrix
            float cos = (float)Math.Cos(deltaAngleRad);
            float sin = (float)Math.Sin(deltaAngleRad);

            float newX = ox * cos - oy * sin;
            float newY = ox * sin + oy * cos;

            // Normalize vector
            float magnitude = (float)Math.Sqrt(newX * newX + newY * newY);
            newX /= magnitude;
            newY /= magnitude;

            Orientation = (newX, newY);
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



        public override string ToString() {
            string s = "[ ";
            foreach (Tags tag in tags) {
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
