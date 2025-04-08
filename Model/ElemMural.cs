using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_PIIA.Model {
    abstract class ElemMur {
        static protected int idCounter = 0;
        protected int id = idCounter;


        protected float distPos;
        public float DistPos {
            get { return distPos; }
            set {
                if (value < 0) throw new ArgumentException("Distance cannot be negative.");
                distPos = value;
            }
        }

        protected float largeur;
        public float Largeur {
            get { return largeur; }
            set {
                if (value < 0) throw new ArgumentException("Largeur cannot be negative.");
                largeur = value;
            }
        }


        public virtual void afficher() {
            Console.WriteLine("ID : " + id);
            Console.WriteLine("Position : " + distPos);
            Console.WriteLine("Largeur : " + largeur);

        }
    }

    class Porte : ElemMur {
        public Porte(float position, float largeur) {
            DistPos = position;
            Largeur = largeur;
            idCounter++;
        }


        public override void afficher() {
            Console.WriteLine("Porte:");
            base.afficher();
        }
    }


    class Fenetre : ElemMur {
        public Fenetre(float position, float largeur) {
            DistPos = position;
            Largeur = largeur;
            idCounter++;
        }


        public override void afficher() {
            Console.WriteLine("Fenetre:");
            base.afficher();
        }
    }
}
