using System.Text;

namespace PROJET_PIIA.Model {
    public class Plan {
        private static int _idCounter = 0;

        public int Id { get; }

        string nom;
        Murs murs;
        List<Meuble> meubles;

        public List<Meuble> Meubles {
            get => meubles;
            set {
                if (value != null) {
                    meubles = new List<Meuble>(value);
                } else {
                    meubles = new List<Meuble>();
                }
            }
        }
        public Murs Murs {
            get => murs;
            set => murs = value;
        }

        // Constructeur
        public Plan() {
            this.nom = "Plan N°" + _idCounter;
            this.murs = new();
            meubles = new List<Meuble>();
            this.Id = _idCounter++;
        }
        public Plan(string nom) : this() {  // Appel constructeur par défaut
            this.nom = nom;
        }



        public void placerMeuble(Meuble meuble, Point position) {
            //place un meuble qui a été ajouté au plan
            bool placing = false;

            if (!meubles.Contains(meuble)) {
                meubles.Add(meuble);
                placing = true;
            } else {
                Point old_pos = meuble.Position;
                meuble.Position = position;

                if (!meuble.ChevaucheMur(murs)) {

                    foreach (Meuble other in meubles) {
                        if (other != meuble && meuble.chevaucheMeuble(other)) {
                            meuble.Position = old_pos; // Revert to old position
                            if (placing) {
                                meubles.Remove(meuble); // Remove if it was just added
                            }
                            throw new ArgumentException("Le meuble ne peut pas être déplacé à cette position car il chevauche un autre meuble.");
                        }
                    }
                    return;

                } else {
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

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Plan ID: {Id}");
            sb.AppendLine($"Nom: {nom}");
            sb.AppendLine("Murs: ");
            sb.AppendLine(murs.ToString());  // Assuming Murs has a ToString method

            sb.AppendLine("Meubles: ");
            if (meubles.Count == 0) {
                sb.AppendLine("  Aucun meuble");
            } else {
                foreach (var meuble in meubles) {
                    sb.AppendLine($"  - {meuble.ToString()}");
                }
            }

            return sb.ToString();
        }





    }
}
