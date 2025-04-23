using System.Reflection;
using PROJET_PIIA.Model;
using PROJET_PIIA.View;
namespace PROJET_PIIA.Controleurs {
    

    public class MainControler(Modele m) : Control {
        public Plan plan { get; private set; } = m.planActuel;

        //public event Action PlanChanged = delegate { };


        //public Catalogue catalogue { get; private set; } = m.Catalogue;
        //private float zoom = 1.0f;  // 1.0 = taille normale


        

        

    }
}