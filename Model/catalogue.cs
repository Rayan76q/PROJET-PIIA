using System.Text;

namespace PROJET_PIIA.Model {

    public class Catalogue {
        // tous les meubles sont dans cette liste
        // ouion pourrait faire un dict
        public List<Meuble> Meubles { get; private set; }

        public Catalogue() {
            Meubles = new List<Meuble>();

            Meubles.Add(new Meuble(
                "Chaise",
                new List<Tags> { Tags.Chaise, Tags.Sol },
                49.99f,
                "Chaise en bois confortable",
                "Images/chaise.png",
                (45, 45)
            ));

            Meubles.Add(new Meuble(
                "Table",
                new List<Tags> { Tags.Table, Tags.Sol },
                149.99f,
                "Table à manger 4 personnes",
                "Images/table.png",
                
                (120, 80)
            ));

            Meubles.Add(new Meuble(
                "Réfrigérateur",
                new List<Tags> { Tags.Electroménager, Tags.Mural },
                299.99f,
                "Frigo grande capacité",
                "Images/frigo.png",
                
                (60, 60)
            ));

            Meubles.Add(new Meuble(
                "Évier",
                new List<Tags> { Tags.Plomberie, Tags.Mural },
                89.99f,
                "Évier de cuisine inox",
                "Images/evier.png",
                
                (80, 50)
            ));

            Meubles.Add(new Meuble(
                "Plan de travail classique",
                new List<Tags> { Tags.PlanDeTravail, Tags.Mural },
                199.99f,
                "Plan de travail stratifié",
                "Images/plan_travail.png",
               
                (150, 60)
            ));

            Meubles.Add(new Meuble(
                "Lampe 1",
                new List<Tags> { Tags.Eclairage, Tags.Decoration },
                29.99f,
                "Lampe sur pied LED",
                "Images/lampe.png",
                
                (30, 30)
            ));
        }


        public void AjouterMeuble(Meuble meuble) {
            Meubles.Add(meuble);
        }

        public List<Meuble> ObtenirMeublesParCategorie(Tags categorie) {
            return Meubles.Where(meuble => meuble.tags.Contains(categorie)).ToList();
        }

       
        // meubles non catégorisés
        public List<Meuble> ObtenirMeublesNonCategorises() {
            return Meubles.Where(meuble => !meuble.tags.Any()).ToList();
        }


        public override string ToString() {
            StringBuilder sb = new();
            // meubles par catégorie
            foreach (Tags categorie in Enum.GetValues(typeof(Tags))) {
                sb.AppendLine($"Catégorie: {categorie}");
                var meublesParCategorie = ObtenirMeublesParCategorie(categorie);
                if (meublesParCategorie.Any()) {
                    foreach (var meuble in meublesParCategorie) {
                        sb.AppendLine($" - {meuble}");
                    }
                } else {
                    sb.AppendLine(" Aucun meuble");
                }
            }

            // meubles non catégorisés
            var meublesNonCategorises = ObtenirMeublesNonCategorises();
            if (meublesNonCategorises.Any()) {
                sb.AppendLine("\nMeubles non catégorisés:");
                foreach (var meuble in meublesNonCategorises) {
                    sb.AppendLine($" - {meuble}");
                }
            }

            return sb.ToString();
        }



    }
}
