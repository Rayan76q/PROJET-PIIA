using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJET_PIIA.View {
    public partial class MurSidePanel : UserControl {
        public MurSidePanel() {
            InitializeComponent();
            InitializePresets();
        }

        private readonly List<string> _presets = new List<string> {
            "Carré","Rectangle","L-Shape","U-Shape"
        };

        private void InitializePresets() {
            
            //Debug.WriteLine("" + panel.Width + ", " + panel.Height);
            flowLayoutPanel1.WrapContents = false;
            Button futureButton = new Button {
                Text = "🔧 Ajouter plus tard...",
                Height = 80,
                Margin = new Padding(5),
                Enabled = false // pour indiquer qu'il est "inutile" pour l'instant
            };
            flowLayoutPanel1.Controls.Add(futureButton);
            
            

            foreach (var preset in _presets) {
                Button btn = new Button {
                    Text = preset,
                    Anchor = AnchorStyles.Left | AnchorStyles.Right,
                    Width = this.flowLayoutPanel1.Width - 15,
                    Height = 80,
                    Margin = new Padding(5),
                };
                btn.Click += (sender, e) => OnPresetSelected(preset);

                flowLayoutPanel1.Controls.Add(btn);
            }

            flowLayoutPanel1.SizeChanged += (sender, e) => {
                int scrollFix = SystemInformation.VerticalScrollBarWidth + 5;
                foreach (Control ctrl in flowLayoutPanel1.Controls) {
                    ctrl.Width = flowLayoutPanel1.ClientSize.Width - ctrl.Margin.Left - ctrl.Margin.Right - scrollFix;
                }
            };

            this.Invalidate();
        }

        public event Action<string>? PresetSelected;

        private void OnPresetSelected(string presetName) {
            PresetSelected?.Invoke(presetName);
        }


    }
}
