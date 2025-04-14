﻿using System.Diagnostics;
using System.Drawing.Drawing2D;
using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Extensions;
using PROJET_PIIA.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

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

        public PlanView(ControleurMainView ctrg) {
            this.DoubleBuffered = true;
            this.Dock = DockStyle.Fill;

            // Abonnement aux événements souris
            this.MouseDown += PlanView_MouseDown;
            this.MouseMove += PlanView_MouseMove;
            this.MouseUp += PlanView_MouseUp;

            // Enable drag and drop
            this.AllowDrop = true;
            this.DragEnter += PlanView_DragEnter;
            this.DragOver += PlanView_DragOver;
            this.DragDrop += PlanView_DragDrop;

            this.ctrg = ctrg;
            ctrg.ModeChanged += OnModeChanged;
            ctrg.PerimeterChanged += OnMurChanged;
            LoadImages("images");
            this.Invalidate();
        }

        private void OnModeChanged() {
            this.Invalidate();
        }

        private void OnMurChanged() {
            this.Invalidate();
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
                // Get the meuble from the drag data
                Meuble meuble = (Meuble)e.Data.GetData(typeof(Meuble));

                // Convert screen coordinates to client coordinates
                Point clientPoint = this.PointToClient(new Point(e.X, e.Y));

                // Convert to plan coordinates
                Point planPoint = ScreenToPlan(clientPoint);
               
                // Place the meuble in the plan
                this.ctrg.plan.placerMeuble(meuble, planPoint);

                // Redraw
                this.Invalidate();
            }
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
                if (_movingMeuble) {
                    _movingMeuble = false;
                    _collisionDetected = false;
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void PlanView_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (ctrg.ModeEdition == PlanMode.Deplacement) {
                    // Check if we clicked on a meuble first
                    Point planPoint = ScreenToPlan(e.Location);
                    Meuble clickedMeuble = FindMeubleAtPoint(planPoint);

                    if (clickedMeuble != null) {
                        _selectedMeuble = clickedMeuble;
                        _movingMeuble = true;
                        _meubleOffset = new Point(planPoint.X - clickedMeuble.Position.X,
                                                planPoint.Y - clickedMeuble.Position.Y);
                        this.Cursor = Cursors.Hand;
                        return; // Early exit so no other action is taken
                    }

                    // If a segment is close to the cursor, initiate resizing.
                    if (segmentProche != null && ctrg.CanModifyWalls()) {
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

            // Moving a meuble takes priority
            if (_movingMeuble && _selectedMeuble != null) {
                Point planPoint = ScreenToPlan(e.Location);
                Point newPosition = new Point(
                    planPoint.X - _meubleOffset.X,
                    planPoint.Y - _meubleOffset.Y
                );

                // Store original position to restore if there's a collision
                Point originalPosition = _selectedMeuble.Position;

                // Update meuble's position
                _selectedMeuble.Position = newPosition;

                // Check for collisions
                _collisionDetected = CheckMeubleCollision(_selectedMeuble);

                this.Invalidate();
                return; // Exit early since we're handling meuble movement
            }

            // Si on est en resizing, agir sur le périmètre :
            if (_resizing && _segmentResize != null && !ctrg.ModeMeuble) {
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

                if (segmentProche != null && !ctrg.ModeMeuble) {
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
                    if (this.Cursor != Cursors.SizeAll && this.Cursor != Cursors.Hand) {
                        this.Cursor = Cursors.Default;
                    }

                }

                if (ancienSegment != segmentProche && !ctrg.ModeMeuble) {
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
            float halfWidth = meuble.Dimensions.Item1 / 2;
            float halfHeight = meuble.Dimensions.Item2 / 2;

            // Simple bounding box check for now - we could enhance with rotation later
            return point.X >= meuble.Position.X - halfWidth &&
                   point.X <= meuble.Position.X + halfWidth &&
                   point.Y >= meuble.Position.Y - halfHeight &&
                   point.Y <= meuble.Position.Y + halfHeight;
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

            // Draw meubles
            DrawMeubles(g);
        }

        // Method to draw all meubles from the controller
        private void DrawMeubles(Graphics g) {
            var meubles = ctrg.ObtenirMeubles();
            if (meubles == null || meubles.Count == 0) return;

            foreach (var meuble in meubles) {
                // Check if this is the selected meuble with a collision
                bool isColliding = _selectedMeuble == meuble && _collisionDetected;
                DrawMeuble(g, meuble, isColliding);
            }
        }

        // Method to draw a single meuble
        private void DrawMeuble(Graphics g, Meuble meuble, bool isColliding = false) {
            // Skip if meuble is null or has no position
            if (meuble == null) return;

            (float width, float height) = meuble.Dimensions;

            // Get screen position from plan position
            Point screenPos = PlanToScreen(meuble.Position);

            // Color for meuble outline or fill - red if colliding
            Color meubleColor = isColliding ? Color.Red : Color.LightBlue;
            Brush meubleBrush = new SolidBrush(meubleColor);
            Pen meublePen = new Pen(isColliding ? Color.DarkRed : Color.Blue, 2);

            // If meuble has an image path, draw the image
            if (!string.IsNullOrEmpty(meuble.ImagePath)) {
                try {
                    // Load image from cache or from file
                    Image img = GetCachedImage(meuble.ImagePath);

                    // Apply rotation if needed
                    float orientation = (float)Math.Tan(meuble.Orientation.Item2 / meuble.Orientation.Item1);

                    if (orientation != 0) {
                        // Save current state
                        Matrix oldMatrix = g.Transform;

                        // Create a new matrix for rotation
                        Matrix rotationMatrix = new Matrix();
                        rotationMatrix.RotateAt(orientation, new PointF(screenPos.X, screenPos.Y));
                        g.Transform = rotationMatrix;

                        // Draw rotated image
                        g.DrawImage(img, screenPos.X - width / 2, screenPos.Y - height / 2, width, height);

                        // Draw red outline if colliding
                        if (isColliding) {
                            g.DrawRectangle(meublePen, screenPos.X - width / 2, screenPos.Y - height / 2, width, height);
                        }

                        // Restore original transformation
                        g.Transform = oldMatrix;
                    } else {
                        // Draw image normally
                        g.DrawImage(img, screenPos.X - width / 2, screenPos.Y - height / 2, width, height);

                        // Draw red outline if colliding
                        if (isColliding) {
                            g.DrawRectangle(meublePen, screenPos.X - width / 2, screenPos.Y - height / 2, width, height);
                        }
                    }
                } catch (Exception ex) {
                    // If image loading fails, draw a placeholder rectangle
                    using (Brush brush = new SolidBrush(isColliding ? Color.Red : Color.LightGray)) {
                        g.FillRectangle(brush, screenPos.X - width / 2, screenPos.Y - height / 2,
                                       width, height);
                    }

                    // Draw a text indicating image error
                    using (Font font = new Font("Arial", 8)) {
                        g.DrawString("Image Error", font, Brushes.Red, screenPos.X - 30, screenPos.Y);
                    }

                    Debug.WriteLine($"Error loading image for meuble: {ex.Message}");
                }
            } else {
                // If no image path, draw a simple rectangle representation
                using (Brush brush = meubleBrush) {
                    g.FillRectangle(brush, screenPos.X - width / 2, screenPos.Y - height / 2,
                                   width, height);
                }

                // Add a border
                g.DrawRectangle(meublePen, screenPos.X - width / 2, screenPos.Y - height / 2,
                               width, height);
            }

            // Optionally, draw the name or type of the meuble
            if (!string.IsNullOrEmpty(meuble.Nom)) {
                using (Font font = new Font("Arial", 8)) {
                    g.DrawString(meuble.Nom, font, Brushes.Black,
                                 screenPos.X - width / 2, screenPos.Y - height / 2 - 15);
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
            }
            base.Dispose(disposing);
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
}