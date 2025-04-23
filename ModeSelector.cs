using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PROJET_PIIA.CustomControls {
    public partial class ModeSelector : UserControl {

        private List<string> _modes = new();
        private int _selectedIndex = 0;

        [Browsable(true)]
        [Category("Behavior")]
        [Description("Se déclenche quand l'utilisateur change de mode.")]
        public event EventHandler ModeChanged;


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<string> Modes {
            get => _modes;
            set {
                _modes = value ?? new List<string>();
                _selectedIndex = 0;
                Invalidate(); // Redessine le contrôle
            }
        }

        [Browsable(true)]
        [Category("Behavior")]
        public int SelectedIndex {
            get => _selectedIndex;
            set {
                if (value >= 0 && value < _modes.Count) {
                    _selectedIndex = value;
                    Invalidate();
                    ModeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        public string SelectedValue => (_modes.Count > 0) ? _modes[_selectedIndex] : string.Empty;

        public ModeSelector() {
            //InitializeComponent();
            this.DoubleBuffered = true;
            this.Cursor = Cursors.Hand;
            this.Size = new Size(150, 30);
            this.BackColor = Color.LightGray;
            this.Click += ModeSelector_Click;
            this.SetStyle(ControlStyles.StandardDoubleClick, false);
        }

        protected override void OnCreateControl() {
            base.OnCreateControl();
            // Si la liste est vide, ajoute des modes par défaut
            if (_modes.Count == 0) {
                _modes = new List<string> { "Mode1", "Mode2", "Mode3" };
            }
        }

        private void ModeSelector_Click(object? sender, EventArgs e) {
            if (_modes.Count == 0) return;
            _selectedIndex = (_selectedIndex + 1) % _modes.Count;
            Invalidate();
            ModeChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            e.Graphics.Clear(BackColor);

            if (_modes.Count == 0) return;

            int buttonWidth = Width / _modes.Count;
            for (int i = 0; i < _modes.Count; i++) {
                Rectangle rect = new Rectangle(i * buttonWidth, 0, buttonWidth, Height);

                using Brush bgBrush = new SolidBrush(i == _selectedIndex ? Color.DimGray : Color.LightGray);
                e.Graphics.FillRectangle(bgBrush, rect);

                TextRenderer.DrawText(
                    e.Graphics,
                    _modes[i],
                    Font,
                    rect,
                    i == _selectedIndex ? Color.White : Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );
            }
        }
    }
}
