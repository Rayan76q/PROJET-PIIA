﻿
using PROJET_PIIA.Model;
using PROJET_PIIA.View;

namespace PROJET_PIIA {
    public class Controleur : Control {
        public  Modele modele;
        public  MainView MainView;


        public Controleur(Modele m, MainView v) {
            this.modele = m;
            this.MainView = v;
        }


        public void setMurs(Murs m) {
            modele.planActuel.Murs = m;
        }


    }
}
