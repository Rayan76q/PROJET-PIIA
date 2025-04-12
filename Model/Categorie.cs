using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_PIIA.Model {
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
}
