using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_PIIA.Model {

    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public enum Categorie {
        [Display(Name = "Plomberie")]
        Plomberie,

        [Display(Name = "Électroménager")]
        Electroménager,

        [Display(Name = "Chaise")]
        Chaise,

        [Display(Name = "Table")]
        Table,

        [Display(Name = "Rangement")]
        Rangement,

        [Display(Name = "Décoration")]
        Decoration,

        [Display(Name = "Plan de travail")]
        PlanDeTravail
    }

    public static class EnumExtensions {
        public static string GetDisplayName(this Enum value) {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? value.ToString();
        }

    }


    class Meuble {

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

        private Position? _position;
        public Position? Position {
            get => _position;
            set {
                if (value != null && value.is_valid())
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

            Position meublePos = Position;
            float largeur = Dimensions.Item1;
            float hauteur = Dimensions.Item2;


            float angleRad = (float)Math.Atan2(Orientation.Item2, Orientation.Item1);


            List<Position> corners = new List<Position>();
            corners.Add(meublePos.RotatePoint((0, 0), angleRad));
            corners.Add(meublePos.RotatePoint((largeur, 0), angleRad));
            corners.Add(meublePos.RotatePoint((largeur, hauteur), angleRad));
            corners.Add(meublePos.RotatePoint((0, hauteur), angleRad));


            var meubleSegments = new List<(Position, Position)>
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

            var (x1, y1) = (Position.X, Position.Y);
            var (w1, h1) = Dimensions;

            var (x2, y2) = (autre.Position.X, autre.Position.Y);
            var (w2, h2) = autre.Dimensions;

            bool overlapX = x1 < x2 + w2 && x1 + w1 > x2;
            bool overlapY = y1 < y2 + h2 && y1 + h1 > y2;

            return overlapX && overlapY;
        }



    }


    class Catalogue {
        public Dictionary<Categorie, List<Meuble>> CategoryToMeubles { get; set; }

        public Catalogue() {

            CategoryToMeubles = new Dictionary<Categorie, List<Meuble>>();


        }
    }
}
