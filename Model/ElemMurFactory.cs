using PROJET_PIIA.Extensions;
using Newtonsoft.Json;
using System.Drawing;

namespace PROJET_PIIA.Model {
    public class ElemMurFactory {
        private static int idCounter = 0;

        

        // Factory method for creating doors
        public static Meuble CreatePorte(float distPos=0, float largeur=10, string nom = "Porte",
            List<Tag> tags = null, float? prix = null, string description = "", string imagePath = null) {

            // Create a door as a special type of furniture
            Meuble porte = new Meuble(
                nom,
                tags ?? new List<Tag> { Tag.Porte },
                prix ?? 0,
                description,
                imagePath,
                (largeur, 0), // Width is the door width, height is set to 0 for mural elements
                idCounter++
            ) {
                IsMural = true, // Mark it as a wall element
                DistPos = distPos, // Store the position along the wall
                IsPorte = true, // Specific property to identify as a door
                ImagePath = "Images/portes.png"
            };

            return porte;
        }

        // Factory method for creating windows
        public static Meuble CreateFenetre(float distPos=0, float largeur=10, string nom = "Fenêtre",
            List<Tag> tags = null, float? prix = null, string description = "", string imagePath = null) {

            // Create a window as a special type of furniture
            Meuble fenetre = new Meuble(
                nom,
                tags ?? new List<Tag> { Tag.Fenetre },
                prix ?? 0,
                description,
                imagePath,
                (largeur, 0), // Width is the window width, height is set to 0 for mural elements
                idCounter++
            ) {
                IsMural = true, // Mark it as a wall element
                DistPos = distPos, // Store the position along the wall
                IsFenetre = true, // Specific property to identify as a window
                ImagePath = "Images/fenetre.png"
            };

            return fenetre;
        }

        
    }
}