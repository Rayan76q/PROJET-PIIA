using System.Diagnostics;
using System.Drawing;
using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Extensions;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.View {
    public partial class PlanView : UserControl {

        private PointF _offset = new(0, 0);       // Décalage du plan
        private PointF _dragStart;                      // PointF de départ du clic
        private DragMode _dragMode = DragMode.None;
        private const int DeleteButtonSize = 16;


        private Meuble? _selectedMeuble = null;
        private int _selectedWall = -1;
        private PointF _meubleOffset;

        internal PlanControleur planController;
        private UndoRedoControleur undoRedoControleur;

        private float _initialMouseAngle = 0f;  


        enum DragMode {
            None,
            Pan, // bouger dans le plan
            /*DrawPolygon,*/
            RotateMeuble,
            MoveMeuble,
            MoveWall
            
        }

        private PointF PlanToScreen(PointF p) =>
            new(p.X * planController.ZoomFactor + _offset.X,
                p.Y * planController.ZoomFactor + _offset.Y);

        private PointF ScreenToPlan(PointF p) =>
            new((p.X - _offset.X) / planController.ZoomFactor,
                (p.Y - _offset.Y) / planController.ZoomFactor);

        public PlanView(PlanControleur controleurplan, UndoRedoControleur undoRedoControleur) {
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

            this.undoRedoControleur = undoRedoControleur;
            this.undoRedoControleur.OnActionUndoRedo += OnMurChanged;
            this.Invalidate();
        }


        private void OnMurChanged() {
            this.Invalidate();
        }


        private void PlanView_DragEnter(object? sender, DragEventArgs e) {
            if (e.Data != null && e.Data.GetDataPresent(typeof(Meuble))) {
                e.Effect = DragDropEffects.Copy;
            } else {

                e.Effect = DragDropEffects.None;
            }
        }

        private void PlanView_DragOver(object? sender, DragEventArgs e) {
            
            if (e.Data != null && e.Data.GetDataPresent(typeof(Meuble))) {
                e.Effect = DragDropEffects.Copy;
            } else
                e.Effect = DragDropEffects.None;
        }

        
        private void PlanView_DragDrop(object? sender, DragEventArgs e) {
            if (e.Data != null && e.Data.GetDataPresent(typeof(Meuble))) {
                var original = e.Data.GetData(typeof(Meuble)) as Meuble;
                if (original == null) return;

                PointF clientPt = this.PointToClient(new Point(e.X, e.Y));
                PointF planPt = ScreenToPlan(clientPt);

                Meuble copie = original.Copier(false);

                if (copie.IsMural) {
                    Murs murs = planController.ObtenirMurs();
                    if (murs != null && murs.Perimetre.Count >= 2) {
                        planController.PlaceMeubleAtPosition(copie, planPt);
                    }
                    
                } else {
                    planPt.X -= copie.Width / 2;
                    planPt.Y -= copie.Height / 2;
                    copie.Position = planPt;
                    planController.PlaceMeubleAtPosition(copie, planPt);
                }

                undoRedoControleur.Add(new AjoutMeuble(copie, planPt));
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

        private PointF GetDeleteButtonPosition() {
            if (_selectedMeuble == null || _selectedMeuble.Position == null)
                throw new InvalidOperationException("Aucun meuble sélectionné ou position non définie.");
            PointF center = _selectedMeuble.GetCenter();
            (float dirX, float dirY) = _selectedMeuble.Orientation;
            float distanceX = _selectedMeuble.Width / 2;
            float distanceY = -_selectedMeuble.Height / 2;
            float buttonX = center.X + dirX * distanceX + dirY * distanceY;
            float buttonY = center.Y + dirY * distanceX - dirX * distanceY;
            PointF screenPos = PlanToScreen(new PointF(buttonX, buttonY));
            return new PointF(screenPos.X - DeleteButtonSize / 2, screenPos.Y - DeleteButtonSize / 2);
        }


        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            //Debug.WriteLine("Affichage !");

            Graphics g = e.Graphics;
            g.Clear(Color.White);

            // dessine la grille
            if (planController.isGridVisible()) {
                DrawGrid(g);
            }

            // dessin des murs
            List<PointF> points = planController.ObtenirMurs().Perimetre;
            var c = (planController.escequelesmursecroisent()) ? Pens.Red : Pens.Blue;
            if (points.Count > 1) {
                for (int i = 0; i < points.Count - 1; i++) {
                    g.DrawLine(c, PlanToScreen(points[i]), PlanToScreen(points[i + 1]));
                }
                g.DrawLine(c, PlanToScreen(points.Last()), PlanToScreen(points.First()));

                if (_selectedWall != -1) {
                    (int, int) seg = (_selectedWall, (_selectedWall+1)%planController.ObtenirMurs().Perimetre.Count());
                    PointF p1 = planController.ObtenirMurs().Perimetre[seg.Item1];
                    PointF p2 = planController.ObtenirMurs().Perimetre[seg.Item2];
                    g.DrawLine(new Pen(Color.Green, 3), PlanToScreen(p1), PlanToScreen(p2));
                }
            }

            // dessine les meubles
            DrawMeubles(g);

            // dessine l'handle et le bouton suppr
            if (_selectedMeuble != null) {
                PointF center = _selectedMeuble.GetCenter();
                PointF screenCenter = PlanToScreen(center);

                PointF delbuttonPos = GetDeleteButtonPosition();                
                using (SolidBrush redBrush = new SolidBrush(Color.Red)) {
                    g.FillEllipse(redBrush, delbuttonPos.X, delbuttonPos.Y, DeleteButtonSize, DeleteButtonSize);
                    g.DrawString("X", new Font("Arial", 8), Brushes.White, delbuttonPos.X + 2, delbuttonPos.Y);
                }
                // dessin de la poignée de rotation
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
        }

        private void DrawGrid(Graphics g) {
            int gridSize = 50;
            Pen gridPen = new(Color.LightGray, 1);
            PointF topLeft = ScreenToPlan(new PointF(0, 0));
            PointF bottomRight = ScreenToPlan(new PointF(this.Width, this.Height));
            int startX = (int)(Math.Floor(topLeft.X / gridSize) * gridSize);
            int startY = (int)(Math.Floor(topLeft.Y / gridSize) * gridSize);
            for (float x = startX; x < bottomRight.X; x += gridSize) {
                PointF p1 = PlanToScreen(new PointF(x, topLeft.Y));
                PointF p2 = PlanToScreen(new PointF(x, bottomRight.Y));
                g.DrawLine(gridPen, p1, p2);
            }
            for (float y = startY; y < bottomRight.Y; y += gridSize) {
                PointF p1 = PlanToScreen(new PointF(topLeft.X, y));
                PointF p2 = PlanToScreen(new PointF(bottomRight.X, y));
                g.DrawLine(gridPen, p1, p2);
            }
            gridPen.Dispose();
        }


        private Pen GetMeubleBorderStyle(Meuble m) {
            bool isColliding = m.CheckMeubleCollision(planController.ObtenirMeublePlacé(), planController.ObtenirMurs());
            bool isSelected = _selectedMeuble == m;
            bool isInSalle = planController.estDansSalle(m);


            Color borderColor = Color.Red;
            if (isSelected) {
                if (isInSalle && !isColliding) {
                    borderColor = Color.Green;
                }
            } else if (isInSalle && !isColliding) {
                borderColor = Color.Transparent;
            }

            return new Pen(borderColor, 2);
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
                    float scale = PlanToScreen(new PointF(1, 0)).X - PlanToScreen(new PointF(0, 0)).X;
                    float pixelWidth = meuble.Width * scale;
                    float pixelHeight = meuble.Height * scale;
                    PointF draw = new(-pixelWidth / 2, -pixelHeight / 2);

                    bool isPorteOrFenetre = meuble.IsPorte || meuble.IsFenetre ||
                                          meuble.GetType().Name.Contains("Porte") ||
                                          meuble.GetType().Name.Contains("Fenetre");

                    if (isPorteOrFenetre) {
                        using (Pen redPen = new Pen(Color.Red, 3)) {
                            // For doors and windows, draw as a line spanning the width or height
                            // depending on orientation (use width as it's likely the longer dimension)
                            g.DrawLine(redPen, draw.X, draw.Y + meuble.Height / 2,
                                       draw.X + meuble.Width, draw.Y + meuble.Height / 2);
                        }
                    } else {
                        Image img = ImageLoader.GetImageOfMeuble(meuble);
                        g.DrawImage(img, draw.X, draw.Y, pixelWidth, pixelHeight);

                        bool isSelected = _selectedMeuble == meuble;
                        bool isColliding = meuble.CheckMeubleCollision(planController.ObtenirMeublePlacé(), planController.ObtenirMurs());
                        using SolidBrush overlay = new SolidBrush(Color.FromArgb(100, pen.Color));
                        g.FillRectangle(overlay, draw.X, draw.Y, pixelWidth, pixelHeight);
                        
                        g.DrawRectangle(pen, draw.X, draw.Y, pixelWidth, pixelHeight);
                    }
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

       
        private void SupprimeMeubleSelection() {
            if (_selectedMeuble != null) {
                PointF safePosition = _selectedMeuble.Position ?? new PointF(-1, -1);
                float angle = _selectedMeuble.getAngle();

                undoRedoControleur.Add(new SuppressionMeuble(_selectedMeuble, safePosition, angle));
                planController.SupprimerMeuble(_selectedMeuble);

                _selectedMeuble = null;
                this.Invalidate(); 
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
            planController.SetMurs(newPoints);

        }


        public void undo() {
            _selectedMeuble = null;
            undoRedoControleur.Undo();
        }

        public void redo() {
            _selectedMeuble = null;
            undoRedoControleur.Redo();
        }

        public void SetCurrentPlan(Plan plan) {
            try {
                if (plan == null)
                    return;
                planController.SetCurrentPlan(plan);
                _selectedMeuble = null;
                _selectedWall = -1;
                resetUndoRedo();  //reset stack
               
                Invalidate();
            } catch (Exception ex) {
                MessageBox.Show($"Erreur lors du chargement du plan dans la vue: {ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void resetUndoRedo() {
            undoRedoControleur = new UndoRedoControleur(planController);
        }


    }
}