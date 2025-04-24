using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.ConstrainedExecution;
using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Extensions;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.View {
    public partial class PlanView : UserControl {

        private PointF _offset = new(0, 0);       // Décalage du plan
        private PointF _dragStart;                      // PointF de départ du clic
        private DragMode _dragMode = DragMode.None;

        

        //private List<(PointF Start, PointF End)> _lignes = new();
        //private PointF? _currentStart = null; // point de départ d'une ligne en cours
        //private PointF _mousePosition;

        //private int? segmentProche = null;
        //private bool _resizing = false;
        //private int? _segmentResize = null;
        //private PointF _resizeStart;

        // Properties for meuble interactions
        private Meuble? _selectedMeuble = null;
        //private bool _movingMeuble = false;
        private PointF _meubleOffset;

        private PlanControleur planController;

        private float _initialMouseAngle = 0f;  // The angle from the meuble's center to the mouse at rotation start.
        

        enum DragMode {
            None,
            Pan, // bouger dans le plan
            /*DrawPolygon,*/
            RotateMeuble,
            MoveMeuble,
            // … à étendre
        }

        private PointF PlanToScreen(PointF p) =>
            new(p.X * planController.ZoomFactor + _offset.X,
                p.Y * planController.ZoomFactor + _offset.Y);

        private PointF ScreenToPlan(PointF p) =>
            new((p.X - _offset.X) / planController.ZoomFactor,
                (p.Y - _offset.Y) / planController.ZoomFactor);

        public PlanView(PlanControleur controleurplan) {
            this.DoubleBuffered = true;
            this.Dock = DockStyle.Fill;

            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;
            this.MouseEnter += (s, e) => this.Focus();
            this.KeyDown += PlanView_KeyDown;
            this.KeyUp += PlanView_KeyUp;

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

            this.planController = controleurplan;
            planController.PlanChanged += OnMurChanged;
            
            ImageLoader.LoadImagesOfFolder("images");
            
            this.Invalidate();
        }


        private void OnMurChanged() {
            this.Invalidate();
        }


        // Handle drag operations
        private void PlanView_DragEnter(object? sender, DragEventArgs e) {
            if (e.Data != null && e.Data.GetDataPresent(typeof(Meuble))) {
                //Meuble ?meuble = e.Data.GetData(typeof(Meuble)) as Meuble;         
                e.Effect = DragDropEffects.Copy;
            } else {
         
                e.Effect = DragDropEffects.None;
            }
        }

        private void PlanView_DragOver(object? sender, DragEventArgs e) {
            if (e.Data != null && e.Data.GetDataPresent(typeof(Meuble))) {
                //Meuble? meuble = e.Data.GetData(typeof(Meuble)) as Meuble;
                e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void PlanView_DragDrop(object? sender, DragEventArgs e) {
            if (e.Data != null && e.Data.GetDataPresent(typeof(Meuble))) {
                Meuble? meuble = e.Data.GetData(typeof(Meuble)) as Meuble;
                if (meuble != null) {
                    PointF clientPoint = this.PointToClient(new Point(e.X, e.Y));
                    PointF planPoint = ScreenToPlan(clientPoint);
                    planPoint.X -= (int)(meuble.Dimensions.Item1 / 2);
                    planPoint.Y -= (int)(meuble.Dimensions.Item2 / 2);

                    Meuble meubleCopie = meuble.Copier();
                    planController.PlaceMeubleAtPosition(meubleCopie, planPoint);
                }

                this.Invalidate();
            }
        }



        private PointF GetMeubleCenter() {
            if (_selectedMeuble == null || _selectedMeuble.Position == null)
                throw new InvalidOperationException("Aucun meuble sélectionné ou position non définie.");
            PointF p = _selectedMeuble.GetCenter();
            return new PointF((int)p.X, (int)p.Y);
        }


        private PointF GetHandleCenter() {
            if (_selectedMeuble == null || _selectedMeuble.Position == null)
                throw new InvalidOperationException("Aucun meuble sélectionné ou position non définie.");

            PointF center = _selectedMeuble.GetCenter();
            (float dirX, float dirY) = _selectedMeuble.Orientation;
            float distance = _selectedMeuble.Height; //todo
            // Positionner le handle dans la direction perpendiculaire au meuble (par exemple "au-dessus")
            // La direction perpendiculaire = (dirY, -dirX)
            float handleX = center.X + dirY * distance;
            float handleY = center.Y - dirX * distance;
            return new PointF((int)handleX, (int)handleY);
        }



        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            //Debug.WriteLine("Affichage !");

            Graphics g = e.Graphics;
            g.Clear(Color.White);

            // dessin des murs
            List<PointF> points = planController.ObtenirMurs().Perimetre;
            if (points.Count > 1) {
                for (int i = 0; i < points.Count - 1; i++) {
                    g.DrawLine(Pens.Blue, PlanToScreen(points[i]), PlanToScreen(points[i + 1]));
                }
                g.DrawLine(Pens.Blue, PlanToScreen(points.Last()), PlanToScreen(points.First()));
            }


            // dessin de la poignée de rotation
            if (_selectedMeuble != null) {
                PointF screenCenter = PlanToScreen(GetMeubleCenter());
                PointF screenHandle = PlanToScreen(GetHandleCenter());
                e.Graphics.DrawLine(Pens.Gray, screenCenter, screenHandle);
                int handleSize = 12;
                Rectangle handleRect = new(
                    (int)screenHandle.X - handleSize / 2,
                    (int)screenHandle.Y - handleSize / 2,
                    handleSize, handleSize
                );
                e.Graphics.FillEllipse(Brushes.Blue, handleRect);
                e.Graphics.DrawEllipse(Pens.White, handleRect);
                e.Graphics.DrawString("↻", new Font("Arial", 8), Brushes.White,
                    handleRect.X + 2, handleRect.Y);
            }

            // ligne pus epaisse pour le murle plus proche
            /*if (ctrg.ModeEdition == PlanMode.Normal && segmentProche != null) {
                List<PointF> p = ctrg.ObtenirMurs().perimetre;
                PointF p1 = p[segmentProche.Value];
                PointF p2 = p[(segmentProche.Value + 1) % p.Count];
                g.DrawLine(new Pen(Color.Green, 3), PlanToScreen(p1), PlanToScreen(p2));
            }*/

            // dessine les meubles
            DrawMeubles(g);

            // dessine la grille
            if (planController.isGridVisible()) {
                DrawGrid(g);
            }

        }

        private void DrawGrid(Graphics g) {
            int gridSize = 50; 
            Pen gridPen = new(Color.LightGray, 1);
            for (int x = 0; x < this.Width; x += gridSize) {
                g.DrawLine(gridPen, x, 0, x, this.Height);
            }
            for (int y = 0; y < this.Height; y += gridSize) {
                g.DrawLine(gridPen, 0, y, this.Width, y);
            }
            gridPen.Dispose();
        }

        private Pen GetMeubleBorderStyle(Meuble m) {
            bool isSelected = _selectedMeuble == m;
            bool isColliding = m.CheckMeubleCollision(planController.ObtenirMeublePlacé(), planController.ObtenirMurs());
            var border = isColliding ? Color.DarkRed : isSelected ? Color.Green : Color.Blue;
            return new Pen(border, 2);
        }


        private void DrawMeubles(Graphics g) {
            var meubles = planController.ObtenirMeublePlacé();
            if (meubles == null || meubles.Count == 0) return;
            foreach (var meuble in meubles) {
                DrawMeuble(g, meuble);
            }
        }

        private void DrawMeuble(Graphics g, Meuble meuble) {
            if (meuble == null || meuble.Position == null) return;

            var state = g.Save();
            PointF p = meuble.Position.Value;
            PointF screenPos = PlanToScreen(p);
            PointF center = meuble.GetCenter();
            PointF screenCenter = PlanToScreen(center);

            var pen = GetMeubleBorderStyle(meuble);
            
            using (pen)
            using (Font font = new Font("Arial", 8)) {

                try {
                    g.TranslateTransform(screenCenter.X, screenCenter.Y);
                    g.RotateTransform(meuble.getAngle());

                    PointF draw = new(-meuble.Width / 2, -meuble.Height / 2);
                    Image img = ImageLoader.GetImageOfMeuble(meuble);
                    g.DrawImage(img, draw.X, draw.Y, meuble.Width, meuble.Height);

                    // 🎯 Overlay status : selection / collision
                    bool isSelected = _selectedMeuble == meuble;
                    bool isColliding = meuble.CheckMeubleCollision(planController.ObtenirMeublePlacé(), planController.ObtenirMurs());

                    if (isColliding || isSelected) {
                        Color overlayColor = isColliding ? Color.FromArgb(100, Color.Red) : Color.FromArgb(100, Color.Green);
                        using SolidBrush overlay = new SolidBrush(overlayColor);
                        g.FillRectangle(overlay, draw.X, draw.Y, meuble.Width, meuble.Height);
                    }

                    g.DrawRectangle(pen, draw.X, draw.Y, meuble.Width, meuble.Height);
                } finally {
                    g.Restore(state);
                }

                // nom du meuble
                if (!string.IsNullOrEmpty(meuble.Nom)) {
                    g.DrawString(meuble.Nom, font, Brushes.Black, screenPos.X, screenPos.Y - 15);
                }
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                ImageLoader.ClearCache();
            }
            base.Dispose(disposing);
        }

        // Ajouter un botonrouge quin'est visible que quand un meuble est selectionné
        private void SupprimeMeubleSelection() {
            if (_selectedMeuble != null) {
                planController.SupprimerMeuble(_selectedMeuble);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData == Keys.Delete && _selectedMeuble != null) {
                SupprimeMeubleSelection();
                Invalidate();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void toggleGrid() {
            planController.toggleGrid();
        }


        public void ChangerZoom(float newZoom) {
            
            PointF screenCenter = new PointF(this.Width / 2f, this.Height / 2f);
            PointF planCenter = ScreenToPlan(screenCenter);

           
            float oldZoom = planController.ZoomFactor;
            planController.ChangerZoom(newZoom);
            _offset.X = screenCenter.X - planCenter.X * newZoom;
            _offset.Y = screenCenter.Y - planCenter.Y * newZoom;
            
            Invalidate();
        }

        public void rescalePlan(float scale) {
          
            List<PointF> currentPoints = planController.ObtenirMurs().Perimetre;
            PointF center = PointExtensions.FindCenterPoint(currentPoints);
            List<PointF> newPoints = PointExtensions.ApplyHomothety(currentPoints, center, scale);
            planController.SetMurs(new Murs(newPoints));

        }



    }
}