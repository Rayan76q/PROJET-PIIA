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

        // ElemMur constructor
        protected ElemMur(float position, float largeur) {
            DistPos = position;
            Largeur = largeur;
            Id = idCounter++;
        }

        public virtual void afficher() {
            Console.WriteLine("ID : " + Id);
            Console.WriteLine("Position : " + DistPos);
            Console.WriteLine("Largeur : " + Largeur);
        }
    }

    class Porte : ElemMur {
        public Porte(float position, float largeur)
            : base(position, largeur) { }

        public override void afficher() {
            Console.WriteLine("Porte:");
            base.afficher();
        }
    }

    class Fenetre : ElemMur {
        public Fenetre(float position, float largeur)
            : base(position, largeur) { }

        public override void afficher() {
            Console.WriteLine("Fenetre:");
            base.afficher();
        }
    }
}
