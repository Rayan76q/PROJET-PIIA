using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_PIIA
{
    public class Position
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

        public bool is_valid()
        {
            return (x >= 0 && y >= 0);
        }

        public Position RotatePoint((float, float) origin, float angleRad)
        {
            float x = origin.Item1 + X * (float)Math.Cos(angleRad) - Y * (float)Math.Sin(angleRad);
            float y = origin.Item2 + X * (float)Math.Sin(angleRad) + Y * (float)Math.Cos(angleRad);
            return new Position(x, y);
        }
    }

    public class Murs
    {

        List<Position> perimetre;
        List<Porte> portes;
        List<Fenetre> fenetres;


        public List<Position> getPerimetre() { return perimetre; }

        
            public static bool checkMurs(List<Position> points)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var p1 = points[i];
                    var q1 = points[i + 1];

                    for (int j = i + 2; j < points.Count - 1; j++)
                    {
                        if (i == j - 1) continue; // Skip adjacent segments

                        var p2 = points[j];
                        var q2 = points[j + 1];

                        if (SegmentsIntersect(p1, q1, p2, q2))
                            return true;
                    }
                }
                return false;
            }

            public static bool SegmentsIntersect(Position p1, Position q1, Position p2, Position q2)
            {
                int o1 = Orientation(p1, q1, p2);
                int o2 = Orientation(p1, q1, q2);
                int o3 = Orientation(p2, q2, p1);
                int o4 = Orientation(p2, q2, q1);

                // General case
                if (o1 != o2 && o3 != o4)
                    return true;

                // Special cases
                if (o1 == 0 && OnSegment(p1, p2, q1)) return true;
                if (o2 == 0 && OnSegment(p1, q2, q1)) return true;
                if (o3 == 0 && OnSegment(p2, p1, q2)) return true;
                if (o4 == 0 && OnSegment(p2, q1, q2)) return true;

                return false;
            }

            private static int Orientation(Position p, Position q, Position r)
            {
                float val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
                if (Math.Abs(val) < 1e-6) return 0;  // Colinear
                return (val > 0) ? 1 : 2;           // Clockwise or Counterclockwise
            }

            private static bool OnSegment(Position p, Position q, Position r)
            {
                return q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                       q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);
            }
        




        public Murs(List<Position> perimetre, List<Porte> portes, List<Fenetre> fenetres)
        {
            if (checkMurs(perimetre))
                throw new ArgumentOutOfRangeException("Les mures s'intersectent, impossible de générer la cuisine.");
            this.perimetre = perimetre;
            this.portes = portes;
            this.fenetres = fenetres;
        }

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


        public List<(Position, Position)> GetSegments()
        {
            var segments = new List<(Position, Position)>();
            for (int i = 0; i < perimetre.Count; i++)
            {
                segments.Add((perimetre[i], perimetre[(i + 1) % perimetre.Count]));
            }
            return segments;
        }

        public void allongerSegment(Position start, Position end, float distance) {
            int startIndex = perimetre.IndexOf(start);
            int endIndex = perimetre.IndexOf(end);

            if (startIndex == -1 || endIndex == -1 || Math.Abs(startIndex - endIndex) != 1 && Math.Abs(startIndex - endIndex) != perimetre.Count - 1) {
                throw new ArgumentException("Les positions spécifiées ne sont pas adjacentes.");
            }

            var (dx, dy) = start.calculerVecteur(end);
            float longueur = start.distance(end);

            if (longueur > 0) {
                float ux = dx / longueur;
                float uy = dy / longueur;

                Position newStart = new Position(start.X - ux * distance, start.Y - uy * distance);
                Position newEnd = new Position(end.X + ux * distance, end.Y + uy * distance);

                perimetre[startIndex] = newStart;
                perimetre[endIndex] = newEnd;
            }
        }




    }

    abstract public class ElemMur
    {
        static protected int idCounter = 0;
        protected int _id = idCounter;
        public int Id {
            get { return _id; }
        }

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

        
        public virtual void afficher()
        {
            Console.WriteLine("ID : " + _id);
            Console.WriteLine("Position : " + distPos);
            Console.WriteLine("Largeur : " + largeur);
            
        }
    }

    public class Porte : ElemMur
    {
        public Porte(float position, float largeur)
        {
            this.DistPos = position;
            this.Largeur = largeur;
            idCounter++;
        }

   
        public override void afficher()
        {
            Console.WriteLine("Porte:");
            base.afficher();
        }
    }


    public class Fenetre : ElemMur
    {
        public Fenetre(float position, float largeur)
        {
            this.DistPos = position;
            this.Largeur = largeur;
            idCounter++;
        }


        public override void afficher()
        {
            Console.WriteLine("Fenetre:");
            base.afficher();
        }
    }
}
