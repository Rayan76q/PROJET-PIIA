namespace PROJET_PIIA.Model {
    abstract class ElemMur {
        static protected int idCounter = 0;
        public readonly int Id;

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
