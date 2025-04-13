using System.Reflection;
using PROJET_PIIA.Model;
using PROJET_PIIA.View;

namespace PROJET_PIIA.Controleurs {

    public class ControleurMainView : Control{
        public event Action ModeChanged; // onen fait qu'un et tout le monde s'abonne ?
        public event Action PerimeterChanged;

        public PlanMode ModeEdition { get; private set; } = PlanMode.Deplacement;// mettre dans model ?
        public bool ModeMeuble { get; private set; } = false; 

        public Plan plan { get; private set; } // set pour quand on echangera les plans

        public ControleurMainView(Modele m) {
            this.plan = m.planActuel;
        }


        public void ChangerMode() {
            ModeEdition = ModeEdition == PlanMode.Deplacement ? PlanMode.DessinPolygone : PlanMode.Deplacement;
            OnModeChanged();
        }


        public void ChangerModeMeuble() {
            ModeMeuble = !ModeMeuble;
            if (ModeMeuble) {
                ModeEdition = PlanMode.Deplacement;
            } 
            OnModeChanged();
        }



        protected virtual void OnModeChanged() {
            ModeChanged?.Invoke();  // Appel à tous les abonnés
        }

       

        public List<Point> ObtenirPerimetre() {
            return plan.Murs.Perimetre;
        }

        public void setMurs(List<Point> p) {
            plan.Murs.perimetre = p;
            OnPerimeterChanged();
        }

        protected virtual void OnPerimeterChanged() {
            PerimeterChanged?.Invoke();  // Appel à tous les abonnés
        }
    }


}
