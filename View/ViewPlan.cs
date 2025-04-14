using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Extensions;
using PROJET_PIIA.Model;


namespace PROJET_PIIA.View {
    public partial class PlanView : UserControl {

        private Point _offset = new Point(0, 0);       // Décalage du plan
        private Point _dragStart;                      // Point de départ du clic
        private bool _dragging = false;

        private List<(Point Start, Point End)> _lignes = new();
        private Point? _currentStart = null; // point de départ d'une ligne en cours
        private Point _mousePosition;


        private int? segmentProche = null;
        private const float seuilProximité = 10f;
        private bool _resizing = false;
        private int? _segmentResize = null;
        private Point _resizeStart;



        private ControleurMainView ctrg;


        public PlanView(ControleurMainView ctrg) {
            this.DoubleBuffered = true;
            this.Dock = DockStyle.Fill;

            // Abonnement aux événements souris
            this.MouseDown += PlanView_MouseDown;
            this.MouseMove += PlanView_MouseMove;
            this.MouseUp += PlanView_MouseUp;

            this.ctrg = ctrg;
            ctrg.ModeChanged += OnModeChanged;
            ctrg.PerimeterChanged += OnMurChanged;
            this.Invalidate();
        }

        private void OnModeChanged() {
            this.Invalidate();
        }
        private void OnMurChanged() {
            this.Invalidate();
        }








        private void PlanView_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (_resizing) {
                    _resizing = false;
                    _segmentResize = null;
                }
                if (_dragging) {
                    _dragging = false;
                }
                this.Cursor = Cursors.Default;
            }
        }


        private void PlanView_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (ctrg.ModeEdition == PlanMode.Deplacement) {
                    // If a segment is close to the cursor, initiate resizing.
                    if (segmentProche != null) {
                        _resizing = true;
                        _segmentResize = segmentProche;
                        _resizeStart = ScreenToPlan(e.Location);
                        // Optionally update the cursor here for resizing feedback.
                        return; // Early exit so that no dragging occurs.
                    } else {
                        // No segment detected, so start dragging (panning) the view.
                        _dragging = true;
                        _dragStart = e.Location;
                        this.Cursor = Cursors.SizeAll;
                    }
                } else if (ctrg.ModeEdition == PlanMode.DessinPolygone) {
                    // In polygon drawing mode.
                    if (_currentStart == null) {
                        _currentStart = ScreenToPlan(e.Location);
                    } else {
                        Point end = ScreenToPlan(e.Location);
                        _lignes.Add((_currentStart.Value, end));
                        _currentStart = end;
                    }
                    this.Invalidate();
                }
            }

            // Right-click in polygon drawing mode to close the polygon.
            if (ctrg.ModeEdition == PlanMode.DessinPolygone && e.Button == MouseButtons.Right
                && _currentStart != null && _lignes.Count > 0) {
                var firstPoint = _lignes[0].Start;
                var lastPoint = _currentStart.Value;
                _lignes.Add((lastPoint, firstPoint));
                _currentStart = null;
                this.Invalidate();

                if (_lignes.Count > 0) {List<Point> ps = new();
                    ps.Add(_lignes[0].Start);
                    foreach (var ligne in _lignes) {
                        ps.Add(ligne.End);
                    }

                    ctrg.setMurs(ps);
                    _lignes = new List<(Point Start, Point End)>();
                }

            }
        }



        private void PlanView_MouseMove(object sender, MouseEventArgs e) {
            _mousePosition = e.Location;

            // Si on est en resizing, agir sur le périmètre :
            if (_resizing && _segmentResize != null) {
                Point current = ScreenToPlan(e.Location);
                Point delta = new Point(current.X - _resizeStart.X, current.Y - _resizeStart.Y);

                List<Point> perimetre = ctrg.ObtenirPerimetre();

                int i1 = _segmentResize.Value;
                int i2 = (i1 + 1) % perimetre.Count;

                Point p1 = perimetre[i1];
                Point p2 = perimetre[i2];

                // Calcul du vecteur du segment, puis de sa normale
                float vx = p2.X - p1.X;
                float vy = p2.Y - p1.Y;
                float nx = -vy;
                float ny = vx;
                float length = MathF.Sqrt(nx * nx + ny * ny);
                if (length < 1e-6) return;
                nx /= length;
                ny /= length;

                // Projection du delta sur la normale
                float d = delta.X * nx + delta.Y * ny;

                // Mettre à jour les points du segment
                Point newP1 = new Point((int)(p1.X + d * nx), (int)(p1.Y + d * ny));
                Point newP2 = new Point((int)(p2.X + d * nx), (int)(p2.Y + d * ny));
                perimetre[i1] = newP1;
                perimetre[i2] = newP2;

                ctrg.UpdatePerimetre(perimetre);
                _resizeStart = current;
                this.Invalidate();
                return;
            }

            // Sinon, traiter la détection de segment et le déplacement de la vue si nécessaire
            if (ctrg.ModeEdition == PlanMode.Deplacement) {
                // Détection du segment proche pour changement de curseur
                var perimetre = ctrg.ObtenirPerimetre();
                var ancienSegment = segmentProche;
                segmentProche = TrouverSegmentProche(e.Location, perimetre);

                if (segmentProche != null) {
                    var (a, b) = (perimetre[segmentProche.Value], perimetre[(segmentProche.Value + 1) % perimetre.Count]);
                    float dx = b.X - a.X;
                    float dy = b.Y - a.Y;
                    double angle = Math.Atan2(dy, dx) * 180 / Math.PI;
                    angle = (angle + 360) % 360;

                    if ((angle >= 337.5 || angle < 22.5) || (angle >= 157.5 && angle < 202.5)) {
                        this.Cursor = Cursors.SizeNS;
                    } else if ((angle >= 67.5 && angle < 112.5) || (angle >= 247.5 && angle < 292.5)) {
                        this.Cursor = Cursors.SizeWE;
                    } else if ((angle >= 22.5 && angle < 67.5) || (angle >= 202.5 && angle < 247.5)) {
                        this.Cursor = Cursors.SizeNESW;
                    } else {
                        this.Cursor = Cursors.SizeNWSE;
                    }
                } else {
                    if(this.Cursor != Cursors.SizeAll) {
                        this.Cursor = Cursors.Default;
                    }
                    
                }

                if (ancienSegment != segmentProche) {
                    this.Invalidate();
                }

                // Déplacement de la vue si l'utilisateur fait un drag classique (et pas resizing)
                if (_dragging) {
                    int dx = e.X - _dragStart.X;
                    int dy = e.Y - _dragStart.Y;
                    _offset.X += dx;
                    _offset.Y += dy;
                    _dragStart = e.Location;
                    this.Invalidate();
                }
            }

            // Autres modes (ex. dessin polygone)…
            else if (ctrg.ModeEdition == PlanMode.DessinPolygone && _currentStart != null) {
                this.Invalidate();
            }
        }


        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            var offset = _offset;

            // Lignes existantes
            foreach (var ligne in _lignes) {
                g.DrawLine(Pens.Blue, PlanToScreen(ligne.Start), PlanToScreen(ligne.End));
            }

            // Ligne en cours
            if (_currentStart != null) {
                g.DrawLine(Pens.Red, PlanToScreen(_currentStart.Value), _mousePosition);
            }

            if (ctrg.ModeEdition == PlanMode.Deplacement && segmentProche != null) {
                List<Point> p = ctrg.ObtenirPerimetre();
                Point p1 = p[segmentProche.Value];
                Point p2 = p[(segmentProche.Value + 1) % p.Count];
                g.DrawLine(new Pen(Color.Red, 3), PlanToScreen(p1), PlanToScreen(p2));
            }



            List<Point> points = ctrg.ObtenirPerimetre();
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



        private int? TrouverSegmentProche(Point souris, List<Point> perimetre) {
            Point sourisPlan = ScreenToPlan(souris);

            for (int i = 0; i < perimetre.Count; i++) {
                Point p1 = perimetre[i];
                Point p2 = perimetre[(i + 1) % perimetre.Count];

                float distance = DistancePointSegment(sourisPlan, p1, p2);
                if (distance < seuilProximité) {
                    return i;
                }
            }

            return null;
        }

        private float DistancePointSegment(Point p, Point a, Point b) {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;

            if (dx == 0 && dy == 0) return p.DistanceTo(a);

            float t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));

            float projX = a.X + t * dx;
            float projY = a.Y + t * dy;

            float distX = p.X - projX;
            float distY = p.Y - projY;

            return MathF.Sqrt(distX * distX + distY * distY);
        }

    }

}







