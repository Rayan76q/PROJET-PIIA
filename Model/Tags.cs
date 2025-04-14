using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_PIIA.Model {
    public enum Tags {
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
        PlanDeTravail,

        [Display(Name = "Eclairage")]
        Eclairage,

        [Display(Name = "Mural")]
        Mural,

        [Display(Name = "Sol")]
        Sol
    }

    public static class TagExtensions {
        public static List<String> allStrings() {
            return Enum.GetValues(typeof(Tags))
                .Cast<Tags>()
                .Select(c => c.GetDisplayName())
                .ToList();
        }
        public static string GetDisplayName(this Enum value) {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? value.ToString();
        }

        public static Tags fromString(string value) {
            foreach (Tags c in Enum.GetValues(typeof(Tags))) {
                if (c.GetDisplayName() == value) {
                    return c;
                }
            }
            throw new ArgumentException($"La catégorie '{value}' n'existe pas.");
        }

    }
}
