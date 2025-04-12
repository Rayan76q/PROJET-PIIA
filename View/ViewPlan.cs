

using PROJET_PIIA;
using PROJET_PIIA.Modele;

public partial class PlanView : UserControl {

    private Point _offset = new Point(0, 0);       // Décalage du plan
    private Point _dragStart;                      // Point de départ du clic
    private bool _dragging = false;

    private List<(Point Start, Point End)> _lignes = new();
    private Point? _currentStart = null; // point de départ d'une ligne en cours
    private Point _mousePosition;
    private PlanMode _mode = PlanMode.Deplacement;

    private Controleur ctr;


    public PlanView(Controleur ctr) {
        this.DoubleBuffered = true;
        this.Dock = DockStyle.Fill;

        // Abonnement aux événements souris
        this.MouseDown += PlanView_MouseDown;
        this.MouseMove += PlanView_MouseMove;
        this.MouseUp += PlanView_MouseUp;

        this.ctr = ctr;
        this.Invalidate();
    }






    private void PlanView_MouseUp(object sender, MouseEventArgs e) {
        if (e.Button == MouseButtons.Left && _mode == PlanMode.Deplacement) {
            _dragging = false;
            this.Cursor = Cursors.Default;
        }
    }

    private void PlanView_MouseDown(object sender, MouseEventArgs e) {
        if (e.Button == MouseButtons.Left) {
            if (_mode == PlanMode.Deplacement) {
                _dragging = true;
                _dragStart = e.Location;
                this.Cursor = Cursors.SizeAll;
            } else if (_mode == PlanMode.DessinPolygone) {
                if (_currentStart == null) {
                    _currentStart = ScreenToPlan(e.Location);
                } else {
                    var end = ScreenToPlan(e.Location);
                    _lignes.Add((_currentStart.Value, end));
                    _currentStart = end;
                    this.Invalidate();
                }
            }
        }

        if (_mode == PlanMode.DessinPolygone && e.Button == MouseButtons.Right && _currentStart != null && _lignes.Count > 0) {
            // Relier le dernier au point initial
            var firstPoint = _lignes[0].Start;
            var lastPoint = _currentStart.Value;
            _lignes.Add((lastPoint, firstPoint));
            _currentStart = null;
            this.Invalidate();
        }
    }


    private void PlanView_MouseMove(object sender, MouseEventArgs e) {
        _mousePosition = e.Location;

        if (_mode == PlanMode.Deplacement && _dragging) {
            int dx = e.X - _dragStart.X;
            int dy = e.Y - _dragStart.Y;
            _offset.X += dx;
            _offset.Y += dy;
            _dragStart = e.Location;
            this.Invalidate();
        } else if (_mode == PlanMode.DessinPolygone && _currentStart != null) {
            this.Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e) {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.Clear(Color.White);

        var offset = _offset;

        /*// Mur
        g.DrawRectangle(Pens.Black, offset.X + 10, offset.Y + 10, 300, 200);

        // Meuble
        g.FillRectangle(Brushes.LightGray, offset.X + 50, offset.Y + 50, 60, 40);
        g.DrawRectangle(Pens.Black, offset.X + 50, offset.Y + 50, 60, 40);
*/
        // Lignes existantes
        foreach (var ligne in _lignes) {
            g.DrawLine(Pens.Blue, PlanToScreen(ligne.Start), PlanToScreen(ligne.End));
        }

        // Ligne en cours
        if (_currentStart != null) {
            g.DrawLine(Pens.Red, PlanToScreen(_currentStart.Value), _mousePosition);
        }



        List<Point> points = ctr.modele.planActuel.Murs.Perimetre;
        if (points.Count > 1) {
            for (int i = 0; i < points.Count - 1; i++) {
                g.DrawLine(Pens.Blue, PlanToScreen(points[i]), PlanToScreen(points[i + 1]));
            }
            // Fermer le polygone
            g.DrawLine(Pens.Blue, PlanToScreen(points.Last()), PlanToScreen(points.First()));
        }

    }
    private Point PlanToScreen(Point p) => new Point(p.X + _offset.X, p.Y + _offset.Y);
    private Point ScreenToPlan(Point p) => new Point(p.X - _offset.X, p.Y - _offset.Y);

    

    public void SetMode(PlanMode mode) {
        _mode = mode;
        _currentStart = null; // annuler tout tracé en cours si on change de mode
        this.Cursor = mode == PlanMode.Deplacement ? Cursors.SizeAll : Cursors.Cross;
    }
}


public enum PlanMode {
    Deplacement,
    DessinPolygone
}





