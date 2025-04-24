using PROJET_PIIA.Model;

namespace PROJET_PIIA.Controleurs {
    public class PlanControleur {

        Plan plan;
        public event Action PlanChanged = delegate { };
        private bool show_grid;
        private float _zoomFactor = 1.0f;

        public float ZoomFactor {
            get { return _zoomFactor; }
            set { ChangerZoom(value); }
        }

        public PlanControleur(Modele m) {
            plan = m.planActuel;
            show_grid = false;
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

        public virtual void OnPlanChanged() {
            PlanChanged?.Invoke();
        }

        public Murs ObtenirMurs() {
            return plan.Murs;
        }

        public void SetMurs(Murs m) {
            plan.Murs = m;
            OnPlanChanged();
        }


        public bool isGridVisible() {
            return show_grid;
        }

        public void toggleGrid() {
            show_grid = !show_grid;
            OnPlanChanged();
        }


        public void ChangerZoom(float zoom) {
            if (zoom >= 0.1f && zoom <= 3.0f) {
                _zoomFactor = zoom;
                OnPlanChanged();
            }
        }


    }
}
