namespace PROJET_PIIA.Model {
    abstract public class ElemMur {
        static protected int idCounter = 0;
        public readonly int Id;

        // là aussi y'a vrm besoin ? je ne comprends pas trop
        protected float distPos;
        public float DistPos {
            get => distPos;
            set {
                if (value < 0) throw new ArgumentException("Distance cannot be negative.");
                distPos = value;
            }
        }

        protected float largeur;
        public float Largeur {
            get => largeur;
            set {
                if (value < 0) throw new ArgumentException("Largeur cannot be negative.");
                largeur = value;
            }
        }

        // I guess faut autoriser null ? genre si on est trop loin d'un mur flemme de faire une projection
        // on l'affiche sur le curseurs -> verifier si c'est un endroit valide audepot de la porte
        protected ElemMur(float position, float largeur) {
            DistPos = position;
            Largeur = largeur;
            Id = idCounter++;
        }

        public override string ToString() {
            return $"ID: {Id}, Position: {DistPos}, Largeur: {Largeur}";
        }
    }

    class Porte : ElemMur {
        public Porte(float position, float largeur)
            : base(position, largeur) { }

        public override string ToString() {
            return $"Porte → {base.ToString()}";
        }
    }

    class Fenetre : ElemMur {
        public Fenetre(float position, float largeur)
            : base(position, largeur) { }

        public override string ToString() {
            return $"Fenetre → {base.ToString()}";
        }
    }

}
