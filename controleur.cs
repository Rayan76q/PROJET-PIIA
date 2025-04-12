
using PROJET_PIIA.Modele;

namespace PROJET_PIIA {
    public class Controleur : Control {
        public  Modele.Modele modele;
        public  MainView MainView;


        public Controleur(Modele.Modele m, MainView v) {
            this.modele = m;
            this.MainView = v;
        }


        public void setMurs(Murs m) {
            modele.planActuel.Murs = m;
        }


    }
}
