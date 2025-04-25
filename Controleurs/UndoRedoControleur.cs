using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.Controleurs {
    public class UndoRedoControleur {
        Plan p;
        PlanControleur planControleur;
        List<ActionLog> actionLogs;
        int currentIndex;
        int currentChainMax;

        public event Action undoRedo = delegate { };

        public UndoRedoControleur(PlanControleur planControleur) {
            this.planControleur = planControleur;
            actionLogs = new List<ActionLog>();
            currentIndex = -1;
            currentChainMax = -1;
        }

        public void add(ActionLog actionLog) {
            if (currentIndex < currentChainMax) {
                actionLogs[currentIndex + 1] = actionLog;

                if (currentIndex + 1 < actionLogs.Count - 1) {
                    actionLogs.RemoveRange(currentIndex + 2, actionLogs.Count - currentIndex - 2);
                }
            } else {
                actionLogs.Add(actionLog);
            }

            currentIndex++;
            currentChainMax = currentIndex;
            Debug.WriteLine(this);

        }

        public bool undo() {
            
            if (currentIndex < 0) return false;
            actionLogs[currentIndex].undo(planControleur);
            currentIndex--;
            OnUndoRedo();
            Debug.WriteLine(this);
            return true;
            
        }

        public bool redo() {
            if (currentIndex >= currentChainMax)
                return false;
            currentIndex++;
            actionLogs[currentIndex].redo(planControleur);
            OnUndoRedo();
            Debug.WriteLine(this);
            return true;
        }

        public virtual void OnUndoRedo() {
            undoRedo?.Invoke();
        }

        
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== Undo/Redo Stack ===");
            sb.AppendLine($"Current Index: {currentIndex}");
            sb.AppendLine($"Max Index: {currentChainMax}");
            sb.AppendLine("Actions:");

            for (int i = 0; i < actionLogs.Count; i++) {
                var log = actionLogs[i];
                string prefix = i == currentIndex ? "►" : " ";
                string state = i <= currentChainMax ? "[Active] " : "[Undone] ";

                string details = "";
                if (log is ActionLogMeuble meubleLog) {
                    details = $"Meuble {meubleLog.objet.id}";
                    if (log is DeplacementMeuble dp) details += $" from ({dp.save.X:N1},{dp.save.Y:N1}) to ({dp.position.X:N1},{dp.position.Y:N1})";
                    else if (log is RotationMeuble rt) details += $" by {rt.angle}°";
                } else if (log is ActionLogMur murLog) {
                    details = $"Wall segment {murLog.segmentMur}";
                }

                sb.AppendLine($"{prefix} {state}{i}: {log.Action} {details}");
            }
            return sb.ToString();
        }
    }
}