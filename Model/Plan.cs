using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_PIIA.Model
{
    class Plan
    {
        private static int _idCounter = 0;

        private int _id = _idCounter;
        public int Id {
            get { return _id; }
        }

        string nom;
        Murs murs;
        List<Meuble> meubles;


        Plan(Murs murs, string nom) {
            if (Murs.checkMurs(murs.Perimetre)){
                this.murs = murs;
                meubles = new List<Meuble>();
                _idCounter++;

                if (nom != null) this.nom = nom;
                else this.nom = "Plan N°" + _idCounter;

            } else {
                throw new ArgumentException("Les murs s'interesectent, impossible de créer le plan.");
            }
        }



        public void placerMeuble(Meuble meuble, Position position) {
            //place un meuble qui a été ajouté au plan
            bool placing = false;

            if (!meubles.Contains(meuble)) {
                meubles.Add(meuble);
                placing = true;
            }
            else {
                Position old_pos = meuble.Position;
                meuble.Position = position;

                if (!meuble.ChevaucheMur(murs)) {

                    foreach (Meuble other in meubles) {
                        if (other != meuble && meuble.chevaucheMeuble(other)) {
                            meuble.Position = old_pos; // Revert to old position
                            if(placing) {
                                meubles.Remove(meuble); // Remove if it was just added
                            }
                            throw new ArgumentException("Le meuble ne peut pas être déplacé à cette position car il chevauche un autre meuble.");
                        }
                    }
                    return;

                } 
                else {
                    throw new ArgumentException("Le meuble ne peut pas être déplacé à cette position car il chevauche un mur.");
                }
            } 
        }


        public void tournerMeuble(Meuble meuble, float angle) {
            if (meubles.Contains(meuble)) {
                meuble.tourner(angle);
            } else {
                throw new ArgumentException("Le meuble n'est pas présent dans le plan.");
            }
        }


        public void supprimerMeuble(Meuble m) {
            if (meubles.Contains(m)) {
                meubles.Remove(m);
            } else {
                throw new ArgumentException("Le meuble n'est pas présent dans le plan.");
            }
        }


        

    }
}
