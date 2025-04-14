﻿using System.Text;

namespace PROJET_PIIA.Model {

    public class Catalogue {
        // tous les meubles sont dans cette liste
        // ouion pourrait faire un dict
        public List<Meuble> Meubles { get; private set; }

        public Catalogue() {
            Meubles = new List<Meuble>();
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
