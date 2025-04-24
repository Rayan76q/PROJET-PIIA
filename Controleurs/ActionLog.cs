using PROJET_PIIA.Model;

namespace PROJET_PIIA.Controleurs {

    public enum PlanAction {
        DeplacementMeuble,
        DeplacementMur,
        RotationMeuble,
        SuppressionMeuble,
        AjoutMeuble,
        AjoutMurs
    }

    public abstract class ActionLog {
        public PlanAction Action;

        public abstract void undo(PlanControleur plan);
        public abstract void redo(PlanControleur plan);
    }

    public class AjoutMurs : ActionLog {
        public readonly List<PointF> pointsMurs;
        public readonly List<PointF> save;

        public AjoutMurs(List<PointF> pointsMurs, List<PointF> save) {
            this.pointsMurs = new List<PointF>(pointsMurs);
            this.save = new List<PointF>(save);
            this.Action = PlanAction.AjoutMurs;
        }

        public override void undo(PlanControleur p) {
            p.SetMurs(save);
        }

        public override void redo(PlanControleur p) {
            p.SetMurs(pointsMurs);
        }
    }

    public abstract class ActionLogMur : ActionLog {
        public (int, int) segmentMur;
    }

    public class DeplacementMur : ActionLogMur {
        public readonly (PointF, PointF) segmentPosition;
        public readonly (PointF, PointF) save;

        public DeplacementMur((int, int) segmentMur, (PointF, PointF) segmentPosition, (PointF, PointF) save) {
            this.segmentMur = segmentMur;
            this.segmentPosition = segmentPosition;
            this.save = save;
            this.Action = PlanAction.DeplacementMur;
        }

        public override void undo(PlanControleur p) {
            // grab the current closed-loop list of points:
            List<PointF> perimetre = p.ObtenirMurs().Perimetre;

            // restore the two endpoints of the moved segment:
            perimetre[segmentMur.Item1] = save.Item1;
            perimetre[segmentMur.Item2] = save.Item2;

            // if we just moved point 0 or point (N−1), mirror it on the other end
            int last = perimetre.Count - 1;
            if (segmentMur.Item1 == 0 || segmentMur.Item2 == 0) {
                // moved the “first” point → update the duplicate at the end
                perimetre[last] = perimetre[0];
            } else if (segmentMur.Item1 == last || segmentMur.Item2 == last) {
                // moved the duplicate endpoint → update the “first” point
                perimetre[0] = perimetre[last];
            }

            p.SetMurs(perimetre);
        }

        public override void redo(PlanControleur p) {
            // grab the current closed-loop list of points:
            List<PointF> perimetre = p.ObtenirMurs().Perimetre;

            // apply the new positions to that same segment:
            perimetre[segmentMur.Item1] = segmentPosition.Item1;
            perimetre[segmentMur.Item2] = segmentPosition.Item2;

            // if we just moved point 0 or point (N−1), mirror it on the other end
            int last = perimetre.Count - 1;
            if (segmentMur.Item1 == 0 || segmentMur.Item2 == 0) {
                perimetre[last] = perimetre[0];
            } else if (segmentMur.Item1 == last || segmentMur.Item2 == last) {
                perimetre[0] = perimetre[last];
            }

            p.SetMurs(perimetre);
        }

    }

    public abstract class ActionLogMeuble : ActionLog {
        public Meuble objet;
    }

    public class DeplacementMeuble : ActionLogMeuble {
        public readonly PointF position;
        public readonly PointF save;

        public DeplacementMeuble(Meuble objet, PointF position, PointF save) {
            this.objet = objet.Copier(true);
            this.position = position;
            this.save = save;
            this.Action = PlanAction.DeplacementMeuble;
        }

        public override void undo(PlanControleur p) {
            p.SupprimerMeuble(objet);
            p.PlaceMeubleAtPosition(objet, save);
        }

        public override void redo(PlanControleur p) {
            p.SupprimerMeuble(objet);
            p.PlaceMeubleAtPosition(objet, position);
        }
    }

    public class RotationMeuble : ActionLogMeuble {
        public readonly float angle;


        public RotationMeuble(Meuble objet, float angle) {
            this.objet = objet;
            this.angle = angle;
            this.Action = PlanAction.RotationMeuble;
        }

        public override void undo(PlanControleur p) {
            p.tournerMeuble(objet, -angle, true);
        }

        public override void redo(PlanControleur p) {
            p.tournerMeuble(objet, angle, true);
        }
    }

    public class SuppressionMeuble : ActionLogMeuble {
        public readonly PointF position;
        public readonly float angle;

        public SuppressionMeuble(Meuble objet, PointF position, float angle) {
            this.objet = objet.Copier(true);
            this.position = position;
            this.angle = angle;
            this.Action = PlanAction.SuppressionMeuble;
        }

        public override void undo(PlanControleur p) {
            p.PlaceMeubleAtPosition(objet, position);
            p.tournerMeuble(objet, angle, true);
        }

        public override void redo(PlanControleur p) {
            p.SupprimerMeuble(objet);

        }
    }

    public class AjoutMeuble : ActionLogMeuble {
        public readonly PointF position;

        public AjoutMeuble(Meuble objet, PointF position) {
            this.objet = objet.Copier(true);
            this.position = position;
            this.Action = PlanAction.AjoutMeuble;
        }

        public override void undo(PlanControleur p) {
            p.SupprimerMeuble(objet);
        }

        public override void redo(PlanControleur p) {
            p.PlaceMeubleAtPosition(objet, position);
        }
    }
}