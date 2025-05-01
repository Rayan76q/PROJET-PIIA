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

        public abstract void Undo(PlanControleur plan);
        public abstract void Redo(PlanControleur plan);
    }

    public class AjoutMurs : ActionLog {
        public readonly List<PointF> pointsMurs;
        public readonly List<PointF> save;

        public AjoutMurs(List<PointF> pointsMurs, List<PointF> save) {
            this.pointsMurs = new List<PointF>(pointsMurs);
            this.save = new List<PointF>(save);
            this.Action = PlanAction.AjoutMurs;
        }

        public override void Undo(PlanControleur p) {
            p.SetMurs(save);
        }

        public override void Redo(PlanControleur p) {
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

        private void ApplySegment(PlanControleur p, (PointF, PointF) points) {
            var perimetre = p.ObtenirMurs().Perimetre;

            perimetre[segmentMur.Item1] = points.Item1;
            perimetre[segmentMur.Item2] = points.Item2;

            int last = perimetre.Count - 1;
            if (segmentMur.Item1 == 0 || segmentMur.Item2 == 0) {
                perimetre[last] = perimetre[0];
            } else if (segmentMur.Item1 == last || segmentMur.Item2 == last) {
                perimetre[0] = perimetre[last];
            }

            p.SetMurs(perimetre);
        }

        public override void Undo(PlanControleur p) => ApplySegment(p, save);
        public override void Redo(PlanControleur p) => ApplySegment(p, segmentPosition);

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

        public override void Undo(PlanControleur p) {
            p.SupprimerMeuble(objet);
            p.PlaceMeubleAtPosition(objet, save);
        }

        public override void Redo(PlanControleur p) {
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

        public override void Undo(PlanControleur p) {
            p.tournerMeuble(objet, -angle, true);
        }

        public override void Redo(PlanControleur p) {
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

        public override void Undo(PlanControleur p) {
            p.PlaceMeubleAtPosition(objet, position);
            p.tournerMeuble(objet, angle, true);
        }

        public override void Redo(PlanControleur p) {
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

        public override void Undo(PlanControleur p) {
            p.SupprimerMeuble(objet);
        }

        public override void Redo(PlanControleur p) {
            p.PlaceMeubleAtPosition(objet, position);
        }
    }
}