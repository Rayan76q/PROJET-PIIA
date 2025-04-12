using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_PIIA.Modele {

    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using PROJET_PIIA.Model;

    public enum Categorie {
        [Display(Name = "Plomberie")]
        Plomberie,

        [Display(Name = "Électroménager")]
        Electroménager,

        [Display(Name = "Chaise")]
        Chaise,

        [Display(Name = "Table")]
        Table,

        [Display(Name = "Rangement")]
        Rangement,

        [Display(Name = "Décoration")]
        Decoration,

        [Display(Name = "Plan de travail")]
        PlanDeTravail
    }

    public static class EnumExtensions {
        public static string GetDisplayName(this Enum value) {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? value.ToString();
        }

    }


    


    public class Catalogue {
        public Dictionary<Categorie, List<Meuble>> CategoryToMeubles { get; set; }

        public Catalogue() {

            CategoryToMeubles = new Dictionary<Categorie, List<Meuble>>();
        }

        public void addMeuble(Meuble meuble) {
            if (meuble == null)
                throw new ArgumentNullException(nameof(meuble), "Le meuble ne peut pas être nul.");
            if (!CategoryToMeubles.ContainsKey(meuble.Type)) {
                CategoryToMeubles[meuble.Type] = new List<Meuble>();
            }
            CategoryToMeubles[meuble.Type].Add(meuble);
        }

        public void supprimeMeuble(Meuble meuble) {
            if (meuble == null)
                throw new ArgumentNullException(nameof(meuble), "Le meuble ne peut pas être nul.");
            if (CategoryToMeubles.ContainsKey(meuble.Type)) {
                CategoryToMeubles[meuble.Type].Remove(meuble);
            }
        }
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Catalogue des Meubles:");

            foreach (var categorie in CategoryToMeubles) {
                sb.AppendLine($"Catégorie: {categorie.Key.GetDisplayName()}");
                foreach (var meuble in categorie.Value) {
                    sb.AppendLine($"  - {meuble.ToString()}");
                }
            }

            return sb.ToString();
        }



    }
}
