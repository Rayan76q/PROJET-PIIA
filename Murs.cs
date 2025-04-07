using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_PIIA
{
    class Position
    {
        private float x;
        public float X
        {
            get { return x; }
            set
            {
                if (value < 0) throw new ArgumentException("X cannot be negative.");
                x = value;
            }
        }

        private float y;
        public float Y
        {
            get { return y; }
            set
            {
                if (value < 0) throw new ArgumentException("Y cannot be negative.");
                y = value;
            }
        }

        public Position(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public (float, float) calculerVecteur(Position p)
        {
            return (p.x - this.x, p.y - this.y);
        }

        public float distance(Position p)
        {
            (float dx, float dy) = this.calculerVecteur(p);
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }

    class Murs
    {

        public bool contient(ElemMur e)
        {
            // TODO : vérifier si l'élément est dans l'espace
            return true;
        }

        List<Position> perimetre;
        List<Porte> portes;
        List<Fenetre> fenetres;

        float GetPerimetreLength()
        {
            float total = 0;
            for (int i = 0; i < perimetre.Count; i++)
            {
                Position a = perimetre[i];
                Position b = perimetre[(i + 1) % perimetre.Count];
                total += a.distance(b);
            }
            return total;
        }

        /// Renvoie l'indice du segment où la porte est placée et la position normalisée (t) de la porte dans ce segment.
        public (int segmentIndex, float t) GetSegmentForPorte(ElemMur porte)
        {
            float globalOffset = porte.DistPos;
            float current = 0;
            for (int i = 0; i < perimetre.Count; i++)
            {
                Position a = perimetre[i];
                Position b = perimetre[(i + 1) % perimetre.Count];
                float segmentLength = a.distance(b);

                if (globalOffset <= current + segmentLength)
                {
                    // position localisée de la porte sur le segment
                    float localOffset = globalOffset - current;
                    float t = localOffset / segmentLength;
                    float endOffset = globalOffset + porte.Largeur; // bout de la porte

                    // si la porte dépasse la fin du segment
                    if (endOffset > current + segmentLength)
                    {
                        // Si oui, on ajuste la position de la porte pour la mettre à la fin du segment
                        globalOffset = current + segmentLength - porte.Largeur;
                        t = (segmentLength - porte.Largeur) / segmentLength;
                    }
                    return (i, t);
                }

                current += segmentLength;
            }

            return (perimetre.Count - 1, 0);
        }

        /// renvoit la vraie position d'un point
        public Position GetPositionForOffset(float offset)
        {
            float current = 0;
            for (int i = 0; i < perimetre.Count; i++)
            {
                Position a = perimetre[i];
                Position b = perimetre[(i + 1) % perimetre.Count];
                float segmentLength = a.distance(b);

                // Si l'offset tombe dans ce segment
                if (offset <= current + segmentLength)
                {
                    float t = (offset - current) / segmentLength;
                    // position sur le segment
                    float x = a.X + t * (b.X - a.X);
                    float y = a.Y + t * (b.Y - a.Y);
                    return new Position(x, y);
                }
                current += segmentLength;
            }
            Position last = perimetre.Last();
            return new Position(last.X, last.Y);
        }
    }

    abstract class ElemMur
    {
        protected float distPos;
        public float DistPos
        {
            get { return distPos; }
            set
            {
                if (value < 0) throw new ArgumentException("Distance cannot be negative.");
                distPos = value;
            }
        }

        protected float largeur;
        public float Largeur
        {
            get { return largeur; }
            set
            {
                if (value < 0) throw new ArgumentException("Largeur cannot be negative.");
                largeur = value;
            }
        }

        public bool est_dans_mur(Murs MursCuisine)
        {
            return MursCuisine.contient(this);
        }

        //pourrait être utile pour débugguer
        public void afficher()
        {
            Console.WriteLine("Position : " + distPos);
            Console.WriteLine("Largeur : " + largeur);
            
        }
    }

    class Porte : ElemMur
    {
        public Porte(float position, float largeur)
        {
            this.DistPos = position;
            this.Largeur = largeur;
        }
    }


    class Fenetre : ElemMur
    {
        public Fenetre(float position, float largeur)
        {
            this.DistPos = position;
            this.Largeur = largeur;
        }
    }
}
