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
using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Model;
using PROJET_PIIA.Extensions;

namespace PROJET_PIIA.View {
    public partial class MurSidePanel : UserControl {

        MurSidePanelControler ctr;

        public MurSidePanel(Modele m, PlanView pv, PlanControleur pctr) {
            ctr = new MurSidePanelControler(m, pv);
            InitializeComponent();
            InitializePresets();
            pctr.PlanChanged += updateSupeficie;
            updateSupeficie();
        }

        private readonly List<string> _presets = new List<string> {
            "carre","rectangle","lshape","ushape"
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
                btn.Click += (sender, e) => {
                    ApplyPreset(btn.Text);
                };

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

        private void OnPresetSelected(string presetName) {
            //SetMurs(List<PointF> p)
            this.searchBox.Text = ctr.GetSurperficie().ToString();
        }

        public void ApplyPreset(string type) {
            List<PointF> points = type.ToLower() switch {
                "carre" => new List<PointF> {
            new PointF(0, 0),
            new PointF(200, 0),
            new PointF(200, 200),
            new PointF(0, 200),
            new PointF(0, 0),
        },
                "rectangle" => new List<PointF> {
            new PointF(0, 0),
            new PointF(300, 0),
            new PointF(300, 150),
            new PointF(0, 150),
            new PointF(0, 0),
        },
                "lshape" => new List<PointF> {
            new PointF(0, 0),
            new PointF(200, 0),
            new PointF(200, 100),
            new PointF(100, 100),
            new PointF(100, 200),
            new PointF(0, 200),
            new PointF(0, 0),
        },
                "ushape" => new List<PointF> {
            new PointF(0, 0),
            new PointF(300, 0),
            new PointF(300, 50),
            new PointF(200, 50),
            new PointF(200, 150),
            new PointF(100, 150),
            new PointF(100, 50),
            new PointF(0, 50),
            new PointF(0, 0),
        },
                _ => null
            };

            if (points != null) {
                ctr.SetMurs(points);
            }
            updateSupeficie();
        }

        void updateSupeficie() { this.searchBox.Text = (Math.Round(ctr.GetSurperficie())).ToString(); }

        private void ApplyAreaButton_Click(object sender, EventArgs e) {

        }

        public void setCurrentPlan(Plan p) {
            ctr.setCurrentPlan(p);
            updateSupeficie();
        }

        private void buttonPorte_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                Meuble porte = ElemMurFactory.CreatePorte();
                buttonPorte.DoDragDrop(porte, DragDropEffects.Copy);
            }
        }

        private void buttonFenetre_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                Meuble fenetre = ElemMurFactory.CreateFenetre();
                buttonFenetre.DoDragDrop(fenetre, DragDropEffects.Copy);
            }
        }

        private void buttonScaling_Click(object sender, EventArgs e) {
            if (int.TryParse(searchBox.Text, out int targetArea) && targetArea > 0) {
                ctr.resizeWallsToArea(targetArea);
            }
            updateSupeficie();
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                ApplyAreaButton_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;  // Prevents the "ding" sound
            }
        }

        private void splittousPorteFenetre_SplitterMoved(object sender, SplitterEventArgs e) {

        }
    }
}