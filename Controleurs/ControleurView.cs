using System.Reflection;
using PROJET_PIIA.Model;
using PROJET_PIIA.View;
namespace PROJET_PIIA.Controleurs {
    public class ControleurMainView : Control {
        public event Action ModeChanged;
        public event Action PerimeterChanged;
        public PlanMode ModeEdition { get; private set; } = PlanMode.Normal;
        public Plan plan { get; private set; }
        public Catalogue catalogue { get; private set; }

        // Property that indicates if wall interactions should be allowed
        public bool WallInteractionsAllowed => !(ModeEdition == PlanMode.Meuble);

        public ControleurMainView(Modele m) {
            this.plan = m.planActuel;
            this.catalogue = m.Catalogue;
        }

        public void ChangerMode() {
            // Only allow changing drawing mode if not in furniture mode
                ModeEdition = ModeEdition == PlanMode.Normal ? PlanMode.DessinPolygone : PlanMode.Normal;
                OnModeChanged();
            
        }

        public void ChangerModeMeuble() {
            ModeEdition = ModeEdition == PlanMode.Normal ? PlanMode.Meuble : PlanMode.Normal;
            OnModeChanged();
        }

        protected virtual void OnModeChanged() {
            ModeChanged?.Invoke();
        }

        public List<Point> ObtenirPerimetre() {
            return plan.Murs.Perimetre;
        }

        public void setMurs(List<Point> p) {
            // Completely block wall modifications when in furniture mode
            if (!(ModeEdition == PlanMode.Meuble)) {
                plan.Murs.perimetre = p;
                OnPerimeterChanged();
            }
        }

        protected virtual void OnPerimeterChanged() {
            PerimeterChanged?.Invoke();
        }

        public void UpdatePerimetre(List<Point> nouveauPerimetre) {
            // Completely block perimeter updates when in furniture mode
            if (!(ModeEdition == PlanMode.Meuble)) {
                plan.Murs.perimetre = nouveauPerimetre;
                PerimeterChanged?.Invoke();
            }
        }

       
        public void PlaceMeubleAtPosition(Meuble m, Point position) {
            this.plan.placerMeuble(m, position);
        }

        public List<Meuble> ObtenirMeubles() {
            return this.plan.Meubles;
        }

        // Method to check if cursor should change near walls
        // Returns false when in furniture mode to prevent cursor change
        public bool ShouldChangeCursorForWalls() {
            return !(ModeEdition == PlanMode.Meuble);
        }

        // Add a method to ensure all wall interaction operations check before proceeding
        public bool CanModifyWalls() {
            return !(ModeEdition == PlanMode.Meuble);
        }
    }
}