using PROJET_PIIA.Modele;

namespace PROJET_PIIA.Model {
    public class Meuble {

        private bool IsValidPositive(float value) => value > 0;
        private bool IsNonEmpty(string value) => !string.IsNullOrEmpty(value);

        private string _nom;
        public string Nom {
            get => _nom;
            set => _nom = IsNonEmpty(value) ? value : _nom;
        }

        private Categorie _type;
        public Categorie Type => _type;

        private float _prix;
        public float Prix {
            get => _prix;
            set => _prix = IsValidPositive(value) ? value : _prix;
        }

        private string _description;
        public string Description {
            get => _description;
            set => _description = value;
        }

        private string _image;
        public string Image {
            get => _image;
            set => _image = value;
        }

        private bool _isMural;
        public bool IsMural {
            get => _isMural;
            set => _isMural = value;
        }

        private Point? _position;
        public Point? Position {
            get => _position;
            set {
                if (value != null && GeometrieUtils.is_valid(value.Value))
                    _position = value;
            }
        }

        private (float, float) _dimensions;
        public (float, float) Dimensions {
            get => _dimensions;
            set => _dimensions = IsValidPositive(value.Item1) && IsValidPositive(value.Item2) ? value : _dimensions;
        }

        private (float, float) _orientation;
        public (float, float) Orientation {
            get => _orientation;
            set => _orientation = IsValidPositive(value.Item1) && IsValidPositive(value.Item2) ? value : _orientation;
        }





        public Meuble(string nom, Categorie type, float prix, string description, string image, bool mural, (float, float) dim) {
            Nom = nom;
            _type = type;
            Prix = prix;
            Description = description;
            Image = image;
            IsMural = mural;
            Position = null;  //pas encore placé dans le plan
            Dimensions = dim;
            Orientation = (0, 0);
        }


        public bool ChevaucheMur(Murs murs) {
            if (Position == null)
                return false; // pas encore placé

            Point meublePos = Position.Value;
            float largeur = Dimensions.Item1;
            float hauteur = Dimensions.Item2;


            float angleRad = (float)Math.Atan2(Orientation.Item2, Orientation.Item1);


            List<Point> corners = new List<Point>();
            corners.Add(GeometrieUtils.RotatePoint(meublePos, (0, 0), angleRad));
            corners.Add(GeometrieUtils.RotatePoint(meublePos, (largeur, 0), angleRad));
            corners.Add(GeometrieUtils.RotatePoint(meublePos, (largeur, hauteur), angleRad));
            corners.Add(GeometrieUtils.RotatePoint(meublePos, (0, hauteur), angleRad));


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


        public bool chevaucheMeuble(Meuble autre) {
            if (Position == null || autre.Position == null)
                return false; // au moins un meuble n'est pas placé

            var (x1, y1) = (Position?.X, Position?.Y);
            var (w1, h1) = Dimensions;

            var (x2, y2) = (autre.Position?.X, autre.Position?.Y);
            var (w2, h2) = autre.Dimensions;

            bool overlapX = x1 < x2 + w2 && x1 + w1 > x2;
            bool overlapY = y1 < y2 + h2 && y1 + h1 > y2;

            return overlapX && overlapY;
        }

        public void deplacer(Point nouvellePos) {
            if (GeometrieUtils.is_valid(nouvellePos)) {
                Position = nouvellePos;
            } else {
                throw new ArgumentException("La nouvelle position n'est pas valide.");
            }
        }

        public void tourner(float angle) {
            if (angle >= 0 && angle <= 360) {
                float angleRad = (float)(angle * Math.PI / 180);
                Orientation = ((float)Math.Cos(angleRad), (float)Math.Sin(angleRad));
            } else {
                throw new ArgumentException("L'angle doit être entre 0 et 360 degrés.");
            }
        }

        public override string ToString() {
            return $"Meuble [Nom: {Nom}, Type: {Type.GetDisplayName()}, Prix: {Prix}€," +
                   $" Description: {Description}, Image: {Image}, " +
                   $"Mural: {IsMural}, Position: {Position}, " +
                   $"Dimensions: ({Dimensions.Item1}x{Dimensions.Item2}), " +
                   $"Orientation: ({Orientation.Item1}, {Orientation.Item2})]";
        }




    }
}
