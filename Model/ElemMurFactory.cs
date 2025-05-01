using PROJET_PIIA.Extensions;
using Newtonsoft.Json;
using System.Drawing;

namespace PROJET_PIIA.Model {
    public class ElemMurFactory {
        private static int idCounter = 0;

        

        public static Meuble CreatePorte(float distPos=0, float largeur=10, string nom = "Porte",
            List<Tag> tags = null, float? prix = null, string description = "", string imagePath = null) {

            Meuble porte = new Meuble(
                nom,
                tags ?? new List<Tag> { Tag.Porte },
                prix ?? 0,
                description,
                imagePath,
                (largeur, 0), 
                idCounter++
            ) {
                IsMural = true, 
                DistPos = distPos, 
                IsPorte = true, 
                ImagePath = "Images/portes.png"
            };

            return porte;
        }

        public static Meuble CreateFenetre(float distPos=0, float largeur=10, string nom = "Fenêtre",
            List<Tag> tags = null, float? prix = null, string description = "", string imagePath = null) {

            Meuble fenetre = new Meuble(
                nom,
                tags ?? new List<Tag> { Tag.Fenetre },
                prix ?? 0,
                description,
                imagePath,
                (largeur, 0),
                idCounter++
            ) {
                IsMural = true, 
                DistPos = distPos, 
                IsFenetre = true,
                ImagePath = "Images/fenetre.png"
            };

            return fenetre;
        }

        
    }
}