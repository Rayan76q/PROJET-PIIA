using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.Controleurs {
    public class UndoRedoControleur(PlanControleur planControleur) {
        private PlanControleur _planControleur = planControleur;
        List<ActionLog> _actionLogs = [];
        int _currentIndex = -1;
        int currentChainMax = -1;

        public event Action OnActionUndoRedo = delegate { };

        public void Add(ActionLog actionLog) {
            if (actionLog == null) throw new ArgumentNullException(nameof(actionLog));

            // Si on a fait des undo, supprimer tout ce qui suit
            if (_currentIndex + 1 < _actionLogs.Count) {
                _actionLogs.RemoveRange(_currentIndex + 1, _actionLogs.Count - (_currentIndex + 1));
            }

            _actionLogs.Add(actionLog);
            _currentIndex++;

            Debug.WriteLine(ToString());
            OnUndoRedo();
        }

        public bool Undo() {
            if (_currentIndex < 0) return false;

            _actionLogs[_currentIndex].Undo(_planControleur);
            _currentIndex--;

            Debug.WriteLine(ToString());
            OnUndoRedo();
            return true;
        }

        public bool Redo() {
            if (_currentIndex + 1 >= _actionLogs.Count) return false;

            _currentIndex++;
            _actionLogs[_currentIndex].Redo(_planControleur);

            Debug.WriteLine(ToString());
            OnUndoRedo();
            return true;
        }

        private void OnUndoRedo() {
            OnActionUndoRedo?.Invoke();
        }

        public bool HasPrevious => _currentIndex >= 0;
        public bool HasNext => _currentIndex + 1 < _actionLogs.Count;



        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== Undo/Redo Stack ===");
            sb.AppendLine($"Current Index: {_currentIndex}");
            sb.AppendLine($"Max Index: {currentChainMax}");
            sb.AppendLine("Actions:");

            for (int i = 0; i < _actionLogs.Count; i++) {
                var log = _actionLogs[i];
                string prefix = i == _currentIndex ? "►" : " ";
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