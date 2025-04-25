using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROJET_PIIA.Model;
using PROJET_PIIA.View;

namespace PROJET_PIIA.Controleurs {
    public class MurSidePanelControler {

        Plan plan;
        PlanView pv;
        public MurSidePanelControler(Modele m,PlanView pv) { 
            this.plan =m.planActuel;
            this.pv = pv;
        }

        public void SetMurs(List<PointF> p) {
            plan.Murs = new Murs(p);  // Enleve les elements muraux

            pv.Invalidate();
        }

        /*public List<PointF> GetMurs() {
            return murs.Perimetre;
        }*/

        public float GetSurperficie() {
            return plan.Murs.Area();
        }

        public void resizeWallsToArea(float targetArea) {
            
            float currentArea = plan.Murs.Area();
            float scaleFactor = (float)Math.Sqrt(targetArea / currentArea);
           
            pv.rescalePlan(scaleFactor);

        }

        public void setCurrentPlan(Plan p) {
            this.plan = p;
        }

    }
}
