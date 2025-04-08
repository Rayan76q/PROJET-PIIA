namespace PROJET_PIIA {

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

        private Position _position;
        public Position? Position {
            get => _position;
            set => _position = value.is_valid() ? value : _position;
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
            IsMural = false;
            Position = null;
            Dimensions = dim;
            Orientation = (0, 0);
        }
    }


    class Catalogue {
        public Dictionary<Categorie, List<Meuble>> CategoryToMeubles { get; set; }

        public Catalogue() {

            CategoryToMeubles = new Dictionary<Categorie, List<Meuble>>();


        }
    }
}
