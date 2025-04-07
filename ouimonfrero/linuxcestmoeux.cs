public struct Point
{
    public float X, Y;

    public Point(float x, float y){X = x;Y = y;}
}

public class Porte {
    float position;
    float largeur;
}

public class Murs {
    List<Point> perim;
    List<Porte> portes;

    float GetPerimetreLength(){
        float total = 0;
        for (int i = 0; i < Perimetre.Count; i++){
            Point a = Perimetre[i];
            Point b = Perimetre[(i + 1) % Perimetre.Count];
            total += Distance(a, b);
        }
        return total;
    }

    /// Renvoie l'indice du segment où la porte est placée et la position normalisée (t) de la porte dans ce segment.
    public (int segmentIndex, float t) GetSegmentForPorte(Porte porte){
        float globalOffset = porte.position;
        float current = 0;
        for (int i = 0; i < Perimetre.Count; i++){
            Point a = Perimetre[i];
            Point b = Perimetre[(i + 1) % Perimetre.Count];
            float segmentLength = Distance(a, b);

            if (globalOffset <= current + segmentLength)
            {
                // position localisée de la porte sur le segment
                float localOffset = globalOffset - current;
                float t = localOffset / segmentLength;
                float endOffset = globalOffset + porte.Largeur; // bout de la porte

                // si la porte dépasse la fin du segment
                if (endOffset > current + segmentLength) {
                    // Si oui, on ajuste la position de la porte pour la mettre à la fin du segment
                    globalOffset = current + segmentLength - porte.Largeur;
                    t = (segmentLength - porte.Largeur) / segmentLength;  
                }
                return (i, t);
            }

            current += segmentLength;
        }

        return (Perimetre.Count - 1, 0);
    }

    /// renvoitla vraie position d'un point
    public Point GetPositionForOffset(float offset) {
        float current = 0;
        for (int i = 0; i < Perimetre.Count; i++){
            Point a = Perimetre[i];
            Point b = Perimetre[(i + 1) % Perimetre.Count];
            float segmentLength = Distance(a, b);

            // Si l'offset tombe dans ce segment
            if (offset <= current + segmentLength){
                float t = (offset - current) / segmentLength; 
                // position sur le segment
                float x = a.X + t * (b.X - a.X);
                float y = a.Y + t * (b.Y - a.Y);
                return new Point(x, y);
            }
            current += segmentLength;
        }
        Point last = Perimetre.Last();
        return new Point(last.X, last.Y);
    }   

    float Distance(Point a, Point b){
        float dx = a.X - b.X;
        float dy = a.Y - b.Y;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }
}
