using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_PIIA.Controleurs {
    public enum EditMode {
        Meuble,
        Mur,
    }

    public class ModeControler {

        public event Action ModeChangedActions = delegate { };
        public EditMode ModeEdition { get; private set; } = EditMode.Mur;

        public void SwitchMode() {
            ModeEdition = (ModeEdition == EditMode.Meuble) ? EditMode.Mur : EditMode.Meuble;
            OnModeChanged();
        }

        protected virtual void OnModeChanged() {
            ModeChangedActions?.Invoke();
        }

    }
}
