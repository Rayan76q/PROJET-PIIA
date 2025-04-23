using PROJET_PIIA.Model;

namespace PROJET_PIIA.Controleurs {
    public class PlanControleur {

        Plan plan;
        public event Action PlanChanged = delegate { };

        public PlanControleur(Modele m) {
            plan = m.planActuel;
        }

        public List<Meuble> ObtenirMeublePlacé() {
            return plan.Meubles;
        }

        public Meuble? FindMeubleAtPoint(PointF planPoint) {
            return plan.FindMeubleAtPoint(planPoint);
        }


        public void PlaceMeubleAtPosition(Meuble m, PointF position) {
            this.plan.PlacerMeuble(m, position);
            OnPlanChanged();
        }

        public void SupprimerMeuble(Meuble m) {
            this.plan.SupprimerMeuble(m);
            OnPlanChanged();
        }

        protected virtual void OnPlanChanged() {
            PlanChanged?.Invoke();
        }

        public Murs ObtenirMurs() {
            return plan.Murs;
        }


    }
}
