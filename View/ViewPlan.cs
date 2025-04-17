using System.Diagnostics;
using System.Drawing.Drawing2D;
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

        // Properties for meuble interactions
        private Meuble _selectedMeuble = null;
        private bool _movingMeuble = false;
        private Point _meubleOffset;
        private bool _collisionDetected = false;

        private ControleurMainView ctrg;

        private Dictionary<string, Image> _imageCache = new Dictionary<string, Image>();

        // Carousel properties
        private bool _showCarousel = false;
        private Point _carouselPosition = Point.Empty;
        private List<CarouselOption> _carouselOptions = new List<CarouselOption>();
        private int? _selectedOptionIndex = null;
        private bool _wallSelected = false;
        private int? _selectedWallIndex = null;
        // New fields to support free-form rotation.
        private bool _rotating = false;
        private float _initialMouseAngle = 0f;  // The angle from the meuble's center to the mouse at rotation start.

        private System.Windows.Forms.Timer _hoverTimer;
        private string _currentTooltip = "";
        private Point _tooltipPosition = Point.Empty;

        public PlanView(ControleurMainView ctrg) {
            this.DoubleBuffered = true;
            this.Dock = DockStyle.Fill;

            // Abonnement aux événements souris
            this.MouseDown += PlanView_MouseDown;
            this.MouseMove += PlanView_MouseMove;
            this.MouseUp += PlanView_MouseUp;
            this.MouseClick += PlanView_MouseClick;

            // Enable drag and drop
            this.AllowDrop = true;
            this.DragEnter += PlanView_DragEnter;
            this.DragOver += PlanView_DragOver;
            this.DragDrop += PlanView_DragDrop;

            this.ctrg = ctrg;
            ctrg.ModeChanged += OnModeChanged;
            ctrg.PerimeterChanged += OnMurChanged;
            LoadImages("images");
            InitializeCarouselOptions();

            _hoverTimer = new System.Windows.Forms.Timer { Interval = 20 };
            _hoverTimer.Tick += HoverTimer_Tick;

            this.Invalidate();
        }

        private void CenterCursorOnSelectedMeuble() {
            if (_selectedMeuble == null) return;

            // Calculate center in plan coordinates
            int centerX = (int)(_selectedMeuble.Position.X + _selectedMeuble.Dimensions.Item1 / 2);
            int centerY = (int)(_selectedMeuble.Position.Y + _selectedMeuble.Dimensions.Item2 / 2);

            // Convert to screen coordinates
            Point screenPoint = PlanToScreen(new Point(centerX, centerY));

            // Move the cursor
            Cursor.Position = this.PointToScreen(screenPoint);
            _mousePosition = screenPoint;

            // Calculate offset between mouse and meuble position for dragging
            _meubleOffset = new Point(
                (int)(_selectedMeuble.Dimensions.Item1 / 2),
                (int)(_selectedMeuble.Dimensions.Item2 / 2)
            );
        }


        // Initialize carousel options with icons
        private void InitializeCarouselOptions() {
            _carouselOptions = new List<CarouselOption>
            {
                new CarouselOption("déplacer", GetMoveIcon(), (sender, e) => {
                    if (_wallSelected)
                    {
                        // Start wall resize logic
                        _resizing = true;
                        _segmentResize = _selectedWallIndex;
                        _resizeStart = ScreenToPlan(_mousePosition);
                    }
                    else if (_selectedMeuble != null)
                    {

                        CenterCursorOnSelectedMeuble();
                         _movingMeuble = true;
                        this.Cursor = Cursors.SizeAll;
                    }
                    HideCarousel();
                }),
                new CarouselOption("tourner", GetRotateIcon(), (sender, e) => {
                    if (_selectedMeuble != null)
                    {
                        CenterCursorOnSelectedMeuble();

                        _rotating = true;
                        // Determine the center (in plan coordinates) of the selected meuble.
                        int centerX = (int)(_selectedMeuble.Position.X + _selectedMeuble.Dimensions.Item1 / 2);
                        int centerY = (int)(_selectedMeuble.Position.Y + _selectedMeuble.Dimensions.Item2 / 2);

                        Point center = new Point(centerX, centerY);
                        // Convert the current mouse position (in screen coordinates) to plan coordinates.
                        Point planMouse = ScreenToPlan(_mousePosition);


                        HideCarousel();
                        this.Cursor = Cursors.PanSE;                 }
                }),



                new CarouselOption("supprimer", GetDeleteIcon(), (sender, e) => {
                    if (_wallSelected && _selectedWallIndex.HasValue && ctrg.CanModifyWalls())
                    {
                        // Delete the wall segment
                        List<Point> perimetre = ctrg.ObtenirPerimetre();
                        if (perimetre.Count > 3) // Ensure we don't delete too many points
                        {
                            perimetre.RemoveAt(_selectedWallIndex.Value);
                            ctrg.UpdatePerimetre(perimetre);
                        }
                    }
                    else if (_selectedMeuble != null)
                    {
                        // Remove the meuble
                        ctrg.plan.supprimerMeuble(_selectedMeuble);
                        _selectedMeuble = null;
                    }
                    HideCarousel();
                })
            };
        }


        private Image GetMoveIcon() {
            // Create a move icon (four directional arrows)
            Bitmap icon = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(icon)) {
                g.Clear(Color.Transparent);
                using (Pen pen = new Pen(Color.Black, 2)) {
                    // Draw arrows in four directions
                    // Up arrow
                    g.DrawLine(pen, 16, 4, 16, 28);
                    g.DrawLine(pen, 16, 4, 10, 10);
                    g.DrawLine(pen, 16, 4, 22, 10);

                    // Left-right arrow
                    g.DrawLine(pen, 4, 16, 28, 16);
                    g.DrawLine(pen, 4, 16, 10, 10);
                    g.DrawLine(pen, 4, 16, 10, 22);
                    g.DrawLine(pen, 28, 16, 22, 10);
                    g.DrawLine(pen, 28, 16, 22, 22);
                }
            }
            return icon;
        }

        private Image GetRotateIcon() {
            // Create a rotate icon (circular arrow)
            Bitmap icon = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(icon)) {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.Black, 2)) {
                    // Draw circular arrow
                    g.DrawArc(pen, 6, 6, 20, 20, 0, 270);
                    // Draw arrow head
                    g.DrawLine(pen, 26, 16, 26, 8);
                    g.DrawLine(pen, 26, 8, 20, 12);
                }
            }
            return icon;
        }

        private Image GetDeleteIcon() {
            // Create a delete icon (trash can)
            Bitmap icon = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(icon)) {
                g.Clear(Color.Transparent);
                using (Pen pen = new Pen(Color.Black, 2)) {
                    // Draw trash can
                    g.DrawRectangle(pen, 10, 10, 12, 16);
                    g.DrawLine(pen, 8, 10, 24, 10);
                    g.DrawLine(pen, 14, 10, 14, 6);
                    g.DrawLine(pen, 18, 10, 18, 6);
                    g.DrawLine(pen, 14, 6, 18, 6);

                    // Draw lines inside trash
                    g.DrawLine(pen, 13, 14, 13, 22);
                    g.DrawLine(pen, 16, 14, 16, 22);
                    g.DrawLine(pen, 19, 14, 19, 22);
                }
            }
            return icon;
        }

        // Show the carousel at the specified position
        private void ShowCarousel(Point position, bool isWall = false, int? wallIndex = null) {
            // Reset all hover timers when showing carousel
            foreach (var option in _carouselOptions) {
                option.HoverStartTime = null;
            }
            _currentTooltip = "";
            _hoverTimer.Stop();

            // Rest of existing code
            _showCarousel = true;
            _carouselPosition = position;
            _wallSelected = isWall;
            _selectedWallIndex = wallIndex;

            // Update visibility based on wall mode
            _carouselOptions.ForEach(opt => {
                opt.Visible = !isWall || opt.Name != "tourner";
            });

            this.Invalidate();
        }

        // Hide the carousel
        private void HideCarousel() {
            _showCarousel = false;
            _selectedOptionIndex = -1;
            _currentTooltip = "";
            _hoverTimer.Stop();

            // Reset all hover timers
            foreach (var option in _carouselOptions) {
                option.HoverStartTime = null;
            }

            this.Invalidate();
        }

        private void OnModeChanged() {
            HideCarousel();
            this.Invalidate();
        }

        private void OnMurChanged() {
            HideCarousel();
            this.Invalidate();
        }

        private void HoverTimer_Tick(object sender, EventArgs e) {
            if (!_showCarousel) return;

            // Get filtered list of visible options
            var visibleOptions = _carouselOptions.Where(o => o.Visible).ToList();

            if (_selectedOptionIndex.HasValue &&
                _selectedOptionIndex.Value < visibleOptions.Count) {
                var option = visibleOptions[_selectedOptionIndex.Value];

                if (option.HoverStartTime.HasValue &&
                    (DateTime.Now - option.HoverStartTime.Value).TotalSeconds >= 1) {
                    _currentTooltip = option.Name;
                    _tooltipPosition = _mousePosition;
                    this.Invalidate();
                }
            } else {
                // Reset if invalid index
                _currentTooltip = "";
                this.Invalidate();
            }
        }

        // Mouse click handler for carousel interaction
        private void PlanView_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                if (_movingMeuble || _rotating || _resizing) {
                    // If we are moving or rotating a meuble, cancel the operation
                    _movingMeuble = false;
                    _rotating = false;
                    _resizing = false;
                    _selectedMeuble = null;
                    this.Cursor = Cursors.Default;
                    this.Invalidate();
                    return;
                }

                // Check if carousel is already visible
                if (_showCarousel) {
                    // Check if an option was clicked
                    var visibleOptions = _carouselOptions.Where(o => o.Visible).ToList();
                    for (int i = 0; i < visibleOptions.Count; i++) {
                        Rectangle optionRect = GetOptionRectangle(i);
                        if (optionRect.Contains(e.Location)) {
                            int originalIndex = _carouselOptions.IndexOf(visibleOptions[i]);
                            _carouselOptions[originalIndex].OnClick?.Invoke(this, e);
                            return;
                        }
                    }

                    // If right-click outside carousel, hide it
                    if (!GetCarouselRectangle().Contains(e.Location)) {
                        HideCarousel();
                    }
                } else {
                    // Right-click to show carousel for meuble or wall if not already showing
                    Point planPoint = ScreenToPlan(e.Location);
                    Meuble clickedMeuble = FindMeubleAtPoint(planPoint);

                    if (clickedMeuble != null) {
                        _selectedMeuble = clickedMeuble;
                        ShowCarousel(e.Location);
                        return;
                    }

                    // Check if a segment is close to the cursor
                    var segmentNearMouse = TrouverSegmentProche(e.Location, ctrg.ObtenirPerimetre());
                    if (segmentNearMouse != null && ctrg.CanModifyWalls()) {
                        ShowCarousel(e.Location, true, segmentNearMouse);
                        return;
                    }
                }
            }
        }

        // Handle drag operations
        private void PlanView_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(Meuble)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void PlanView_DragOver(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(Meuble)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void PlanView_DragDrop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(typeof(Meuble))) {
                Meuble meuble = (Meuble)e.Data.GetData(typeof(Meuble));
                Point clientPoint = this.PointToClient(new Point(e.X, e.Y));
                Point planPoint = ScreenToPlan(clientPoint);

                // Place TOP-LEFT corner at mouse position (no offset)
                ctrg.plan.placerMeuble(meuble, planPoint);
                this.Invalidate();
            }
        }

        private void PlanView_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                bool needsRedraw = false;

                if (_rotating) {
                    _rotating = false;
                    _initialMouseAngle = 0f;
                    needsRedraw = true;
                }

                if (_resizing) {
                    _resizing = false;
                    _segmentResize = null;
                    needsRedraw = true;
                }

                if (_dragging) {
                    _dragging = false;
                    needsRedraw = true;
                }

                if (_movingMeuble) {
                    _movingMeuble = false;
                    _collisionDetected = false;
                    needsRedraw = true;
                }

                this.Cursor = Cursors.Default;

                if (needsRedraw) {
                    this.Invalidate();
                }
            }
        }

        private void PlanView_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (_showCarousel) {
                    // If carousel is shown, we handle it in MouseClick
                    return;
                }

                if (ctrg.ModeEdition == PlanMode.Deplacement) {
                    // Check if we clicked on a meuble first
                    Point planPoint = ScreenToPlan(e.Location);
                    Meuble clickedMeuble = FindMeubleAtPoint(planPoint);

                    if (clickedMeuble != null) {
                        _selectedMeuble = clickedMeuble;
                        ShowCarousel(e.Location);
                        return; // Early exit to show carousel
                    }

                    // If a segment is close to the cursor, show carousel for wall
                    if (segmentProche != null && ctrg.CanModifyWalls()) {
                        ShowCarousel(e.Location, true, segmentProche);
                        return; // Early exit to show carousel
                    } else {
                        // No segment detected, so start dragging (panning) the view.
                        _dragging = true;
                        _dragStart = e.Location;
                        this.Cursor = Cursors.SizeAll;
                    }
                } else if (ctrg.ModeEdition == PlanMode.DessinPolygone && ctrg.CanModifyWalls()) {
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
                && _currentStart != null && _lignes.Count > 0 && ctrg.CanModifyWalls()) {
                var firstPoint = _lignes[0].Start;
                var lastPoint = _currentStart.Value;
                _lignes.Add((lastPoint, firstPoint));
                _currentStart = null;
                this.Invalidate();

                if (_lignes.Count > 0) {
                    List<Point> ps = new();
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

            // If in rotation mode, update the rotation continuously.
            if (_rotating && _selectedMeuble != null) {
                int centerX = (int)(_selectedMeuble.Position.X + _selectedMeuble.Dimensions.Item1 / 2);
                int centerY = (int)(_selectedMeuble.Position.Y + _selectedMeuble.Dimensions.Item2 / 2);
                Point center = new Point(centerX, centerY);
                Point planMouse = ScreenToPlan(e.Location);

                // Calculate angle between center and mouse position
                float currentMouseAngleRad = (float)Math.Atan2(
                    planMouse.Y - center.Y,
                    planMouse.X - center.X
                );

                // Only set initial angle on first movement
                if (_initialMouseAngle == 0f) {
                    _initialMouseAngle = currentMouseAngleRad;
                }

                // Calculate delta in radians
                float deltaAngleRad = currentMouseAngleRad - _initialMouseAngle;

                // Convert to degrees for rotation
                float deltaAngleDeg = deltaAngleRad * (180f / (float)Math.PI);

                // Apply the rotation
                _selectedMeuble.tourner(deltaAngleDeg);

                // Reset initial angle for next movement calculation
                _initialMouseAngle = currentMouseAngleRad;

                this.Invalidate();
                return;
            }

            // Handle meuble movement
            if (_movingMeuble && _selectedMeuble != null) {
                Point planPoint = ScreenToPlan(e.Location);

                // Calculate new position using offset
                int newX = planPoint.X - _meubleOffset.X;
                int newY = planPoint.Y - _meubleOffset.Y;

                // Store original position in case we need to revert
                Point originalPosition = _selectedMeuble.Position;

                // Update position
                _selectedMeuble.Position = new Point(newX, newY);

                // Check for collisions
                _collisionDetected = CheckMeubleCollision(_selectedMeuble);

                this.Invalidate();
                return;
            }

            if (_showCarousel) {
                int visibleIndex = 0;
                int? newSelectedIndex = null;

                // Use filtered list of visible options
                var visibleOptions = _carouselOptions.Where(o => o.Visible).ToList();

                for (int i = 0; i < visibleOptions.Count; i++) {
                    Rectangle optRect = GetOptionRectangle(i);
                    if (optRect.Contains(e.Location)) {
                        newSelectedIndex = _carouselOptions.IndexOf(visibleOptions[i]);
                        break;
                    }
                }

                // Handle index changes
                if (_selectedOptionIndex != newSelectedIndex) {
                    // Reset previous option's hover time
                    if (_selectedOptionIndex.HasValue && _selectedOptionIndex.Value >= 0
                        && _selectedOptionIndex.Value < _carouselOptions.Count) {
                        _carouselOptions[_selectedOptionIndex.Value].HoverStartTime = null;
                    }

                    _selectedOptionIndex = newSelectedIndex;
                    _currentTooltip = "";
                    this.Invalidate();
                }

                // Update hover timing for current option
                if (_selectedOptionIndex.HasValue && _selectedOptionIndex.Value >= 0
                    && _selectedOptionIndex.Value < _carouselOptions.Count) {
                    var option = _carouselOptions[_selectedOptionIndex.Value];
                    if (option.HoverStartTime == null) {
                        option.HoverStartTime = DateTime.Now;
                        _hoverTimer.Start();
                    }
                }
                return;
            }

            // Si on est en resizing, agir sur le périmètre :
            if (_resizing && _segmentResize.HasValue && !ctrg.ModeMeuble) {
                Point current = ScreenToPlan(e.Location);
                Point delta = new Point(current.X - _resizeStart.X, current.Y - _resizeStart.Y);

                List<Point> perimetre = ctrg.ObtenirPerimetre();
                if (perimetre.Count <= 2) return;  // Prevent issues with invalid perimeter

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
                // Check if mouse is over a wall or meuble for the green cursor
                bool isOverElement = false;

                // Check if mouse is over a meuble
                Point planPoint = ScreenToPlan(e.Location);
                Meuble hoverMeuble = FindMeubleAtPoint(planPoint);
                if (hoverMeuble != null) {
                    this.Cursor = Cursors.Hand;
                    isOverElement = true;
                } else {
                    // Détection du segment proche pour changement de curseur
                    var perimetre = ctrg.ObtenirPerimetre();
                    var ancienSegment = segmentProche;
                    segmentProche = TrouverSegmentProche(e.Location, perimetre);

                    if (segmentProche != null && !ctrg.ModeMeuble) {
                        this.Cursor = Cursors.Hand;
                        isOverElement = true;
                    } else if (this.Cursor != Cursors.SizeAll && this.Cursor != Cursors.Hand && !isOverElement) {
                        this.Cursor = Cursors.Default;
                    }

                    if (ancienSegment != segmentProche && !ctrg.ModeMeuble) {
                        this.Invalidate();
                    }
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

        // Helper method to find a meuble at a specific point
        private Meuble FindMeubleAtPoint(Point planPoint) {
            var meubles = ctrg.ObtenirMeubles();
            if (meubles == null || meubles.Count == 0) return null;

            // Check each meuble in reverse order (so topmost is selected first)
            for (int i = meubles.Count - 1; i >= 0; i--) {
                Meuble meuble = meubles[i];
                if (IsPointInMeuble(planPoint, meuble)) {
                    return meuble;
                }
            }
            return null;
        }

        // Helper method to check if a point is inside a meuble
        private bool IsPointInMeuble(Point point, Meuble meuble) {
            // Check against TOP-LEFT based bounding box
            return point.X >= meuble.Position.X &&
                   point.X <= meuble.Position.X + meuble.Dimensions.Item1 &&
                   point.Y >= meuble.Position.Y &&
                   point.Y <= meuble.Position.Y + meuble.Dimensions.Item2;
        }

        // Check if a meuble collides with walls or other meubles
        private bool CheckMeubleCollision(Meuble meuble) {
            // Check collision with walls
            if (meuble.ChevaucheMur(ctrg.plan.Murs)) {
                return true;
            }

            // Check collision with other meubles
            var meubles = ctrg.ObtenirMeubles();
            foreach (var otherMeuble in meubles) {
                if (otherMeuble != meuble && meuble.chevaucheMeuble(otherMeuble)) {
                    return true;
                }
            }

            return false;
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
                g.DrawLine(new Pen(Color.Green, 3), PlanToScreen(p1), PlanToScreen(p2));
            }

            List<Point> points = ctrg.ObtenirPerimetre();
            if (points.Count > 1) {
                for (int i = 0; i < points.Count - 1; i++) {
                    g.DrawLine(Pens.Blue, PlanToScreen(points[i]), PlanToScreen(points[i + 1]));
                }
                // Fermer le polygone
                g.DrawLine(Pens.Blue, PlanToScreen(points.Last()), PlanToScreen(points.First()));
            }

            // Draw meubles
            DrawMeubles(g);

            // Draw carousel if needed
            if (_showCarousel) {
                DrawCarousel(g);
            }
        }

        // Draw the carousel of options
        private void DrawCarousel(Graphics g) {
            // Ensure smooth shapes
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Get overall carousel area
            Rectangle carouselRect = GetCarouselRectangle();

            // Draw the carousel background
            using (Brush bgBrush = new SolidBrush(Color.FromArgb(150, 240, 240, 240))) {
                g.FillEllipse(bgBrush, carouselRect);
            }
            g.DrawEllipse(Pens.Gray, carouselRect);

            // Count visible options
            int visibleCount = _carouselOptions.Count(o => o.Visible);

            // Draw each option
            int optionIndex = 0;
            foreach (var option in _carouselOptions) {
                if (!option.Visible) continue;

                // Calculate position
                Rectangle optRect = GetOptionRectangle(optionIndex);

                // Draw option background (highlight if selected)
                using (Brush optBrush = new SolidBrush(_selectedOptionIndex == optionIndex ?
                                                      Color.FromArgb(150, 200, 255) :
                                                      Color.FromArgb(150, 250, 250, 250))) {
                    g.FillEllipse(optBrush, optRect);
                }
                g.DrawEllipse(Pens.DarkGray, optRect);

                // Draw icon
                if (option.Icon != null) {
                    g.DrawImage(option.Icon,
                               optRect.X + (optRect.Width - option.Icon.Width) / 2,
                               optRect.Y + (optRect.Height - option.Icon.Height) / 2);
                }

                optionIndex++;
            }

            if (!GetCarouselRectangle().Contains(_mousePosition)) {
                _currentTooltip = "";
            }

            if (!string.IsNullOrEmpty(_currentTooltip)) {
                using (Font font = new Font("Arial", 9))
                using (Brush textBrush = new SolidBrush(Color.Black))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center }) {
                    var textSize = g.MeasureString(_currentTooltip, font);
                    RectangleF tooltipRect = new RectangleF(
                        _tooltipPosition.X - textSize.Width / 2,
                        _tooltipPosition.Y - 30,
                        textSize.Width,
                        textSize.Height
                    );

                    // Draw background
                    g.FillRectangle(Brushes.LightGoldenrodYellow, tooltipRect);
                    g.DrawRectangle(Pens.DarkGoldenrod, Rectangle.Round(tooltipRect));

                    // Draw text
                    g.DrawString(_currentTooltip, font, textBrush, tooltipRect, sf);
                }
            }


        }

        // Get the overall carousel rectangle
        private Rectangle GetCarouselRectangle() {
            int carouselSize = 180;
            return new Rectangle(
                _carouselPosition.X - carouselSize / 2,
                _carouselPosition.Y - carouselSize / 2,
                carouselSize, carouselSize);
        }

        // Get rectangle for a specific option
        private Rectangle GetOptionRectangle(int visibleIndex) {
            var visibleOptions = _carouselOptions.Where(o => o.Visible).ToList();
            if (visibleIndex >= visibleOptions.Count) return Rectangle.Empty;

            // Calculate position based on visible index
            double angle = 2 * Math.PI * visibleIndex / visibleOptions.Count;
            int radius = 60;
            int optionSize = 40;

            int x = _carouselPosition.X + (int)(radius * Math.Cos(angle)) - optionSize / 2;
            int y = _carouselPosition.Y + (int)(radius * Math.Sin(angle)) - optionSize / 2;

            return new Rectangle(x, y, optionSize, optionSize);
        }

        // Method to draw all meubles from the controller
        private void DrawMeubles(Graphics g) {
            var meubles = ctrg.ObtenirMeubles();
            if (meubles == null || meubles.Count == 0) return;

            foreach (var meuble in meubles) {
                // Check if this is the selected meuble with a collision
                bool isColliding = _selectedMeuble == meuble && _collisionDetected;
                bool isSelected = _selectedMeuble == meuble;
                DrawMeuble(g, meuble, isColliding, isSelected);
            }
        }

        // Method to draw a single meuble
        private void DrawMeuble(Graphics g, Meuble meuble, bool isColliding = false, bool isSelected = false) {
            if (meuble == null) return;

            // Get dimensions and define colors for fill/outline.
            (float width, float height) = meuble.Dimensions;
            Color meubleColor = isColliding ? Color.Red : (isSelected ? Color.LightGreen : Color.LightBlue);
            using Brush meubleBrush = new SolidBrush(meubleColor);
            using Pen meublePen = new Pen(isColliding ? Color.DarkRed : (isSelected ? Color.Green : Color.Blue), 2);

            // Save the current state of the graphics context
            var state = g.Save();

            try {
                // Compute the center point (screen coordinates).
                Point screenPos = PlanToScreen(meuble.Position);

                // Calculate center for rotation
                float centerX = screenPos.X + (width / 2);
                float centerY = screenPos.Y + (height / 2);

                // Apply transformations (in the correct order)
                g.TranslateTransform(centerX, centerY);
                g.RotateTransform(meuble.getAngle());

                // Draw relative to the center (origin after transformation)
                float drawX = -width / 2;
                float drawY = -height / 2;

                if (!string.IsNullOrEmpty(meuble.ImagePath)) {
                    try {
                        // Load the image from cache or file.
                        Image img = GetCachedImage(meuble.ImagePath);
                        // Draw the image centered at the origin.
                        g.DrawImage(img, drawX, drawY, width, height);

                        // Draw an outline if needed.
                        if (isColliding || isSelected) {
                            g.DrawRectangle(meublePen, drawX, drawY, width, height);
                        }
                    } catch (Exception ex) {
                        // In case of image-loading error, draw a placeholder.
                        g.FillRectangle(meubleBrush, drawX, drawY, width, height);
                        using (Font font = new Font("Arial", 8)) {
                            g.DrawString("Image Error", font, Brushes.Red, drawX + 5, drawY + 5);
                        }
                        Debug.WriteLine($"Error loading image for meuble: {ex.Message}");
                    }
                } else {
                    // No image provided: Draw a filled rectangle.
                    g.FillRectangle(meubleBrush, drawX, drawY, width, height);
                    g.DrawRectangle(meublePen, drawX, drawY, width, height);
                }
            } finally {
                // Restore the original state of the graphics context
                g.Restore(state);
            }

            // Draw the meuble name (unrotated)
            if (!string.IsNullOrEmpty(meuble.Nom)) {
                Point screenPos = PlanToScreen(meuble.Position);
                using (Font font = new Font("Arial", 8)) {
                    g.DrawString(meuble.Nom, font, Brushes.Black,
                                 screenPos.X, screenPos.Y - 15);
                }
            }
        }


        // Method to get image from cache or load it from file
        private Image GetCachedImage(string imagePath) {
            if (_imageCache.ContainsKey(imagePath)) {
                return _imageCache[imagePath];
            }

            // Load image and add to cache
            Image img = Image.FromFile(imagePath);
            _imageCache[imagePath] = img;
            return img;
        }

        // Add this method to clear the image cache when needed
        public void ClearImageCache() {
            // Dispose all images first
            foreach (var img in _imageCache.Values) {
                img.Dispose();
            }
            _imageCache.Clear();
        }

        // Make sure to call this when disposing the control
        protected override void Dispose(bool disposing) {
            if (disposing) {
                ClearImageCache();


                // Dispose option icons
                foreach (var option in _carouselOptions) {
                    option.Icon?.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private Point PlanToScreen(Point p) => new Point(p.X + _offset.X, p.Y + _offset.Y);
        private Point ScreenToPlan(Point p) => new Point(p.X - _offset.X, p.Y - _offset.Y);

        private int? TrouverSegmentProche(Point souris, List<Point> perimetre) {
            if (perimetre == null || perimetre.Count < 2) return null;

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

        private void LoadImages(string folderPath) {
            if (!Directory.Exists(folderPath))
                return;

            string[] files = Directory.GetFiles(folderPath, "*.png"); // or *.jpg, etc.
            foreach (string file in files) {
                try {
                    string key = Path.GetFileNameWithoutExtension(file); // use filename as key
                    Image img = Image.FromFile(file);
                    _imageCache[key] = img;
                } catch (Exception ex) {
                    // Handle invalid image or file access issues
                    Console.WriteLine($"Error loading image {file}: {ex.Message}");
                }
            }
        }

        public Image GetImage(string key) {
            return _imageCache.TryGetValue(key, out var img) ? img : null;
        }
    }

    // Class to represent a carousel option
    public class CarouselOption {
        public string Name { get; set; }
        public Image Icon { get; set; }
        public EventHandler OnClick { get; set; }
        public bool Visible { get; set; } = true;
        public DateTime? HoverStartTime { get; set; } // Track hover duration

        public CarouselOption(string name, Image icon, EventHandler onClick) {
            Name = name;
            Icon = icon;
            OnClick = onClick;
        }
    }
}