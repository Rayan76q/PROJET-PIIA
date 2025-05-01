using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Extensions;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.View {
    public partial class PlanView {
        private bool _isMouseOverDeleteButton = false;
        private PointF _startAction;
        private float _angleAction;
        private (PointF, PointF) _selectedWallPoints1;
        private (PointF, PointF) _selectedWallPoints2;

        private void PlanView_MouseClick(object? sender, MouseEventArgs e) {
            // la selection du meuble de fais quand on souleve le click (MouseUp)

            if (e.Button == MouseButtons.Left && _selectedMeuble != null) {
                PointF mousePos = new PointF(e.X, e.Y);

                // Check if the click was on the delete button
                if (IsPointOverDeleteButton(mousePos)) {
                    SupprimeMeubleSelection();
                    return;
                }
            }
        }



        private void PlanView_MouseDown(object? sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                _startAction = new PointF(e.Location.X, e.Location.Y);
                PointF planPoint = ScreenToPlan(e.Location);

                if ((Control.ModifierKeys & Keys.Shift) != 0) {
                    _dragMode = DragMode.Pan;
                    _dragStart = e.Location;
                    this.Cursor = Cursors.SizeAll;
                    return;
                }

                var meuble = planController.FindMeubleAtPoint(planPoint);
                if (meuble != null && meuble.Position != null) {
                    _selectedMeuble = meuble;
                    _dragMode = DragMode.MoveMeuble;
                    _dragStart = e.Location;
                    
                    PointF p = meuble.Position.Value;
                    _meubleOffset = new PointF(
                        planPoint.X - p.X,
                        planPoint.Y - p.Y
                    );

                    this.Cursor = Cursors.Hand;
                }

                if (_selectedMeuble != null) {
                    PointF screenHandle = PlanToScreen(GetHandleCenter());
                    Rectangle handleRect = new Rectangle(
                        (int)screenHandle.X - 6, (int)screenHandle.Y - 6, 12, 12);
                    if (handleRect.Contains(e.Location)) {
                        _dragMode = DragMode.RotateMeuble;
                        _dragStart = e.Location;
                        PointF center = PlanToScreen(GetMeubleCenter());
                        PointF p = new(e.Location.X - center.X, e.Location.Y - center.Y);
                        _initialMouseAngle = (float)Math.Atan2(p.Y, p.X);

                        this.Cursor = Cursors.Hand;
                        return;
                    }
                } else {
                    int segmentIndex = planController.FindMurAtPoint(planPoint);
                    if (segmentIndex != -1) {
                        _selectedWall = segmentIndex;
                        List<PointF> perimetre = planController.ObtenirMurs().Perimetre;
                        int i1 = _selectedWall;
                        int i2 = (i1 + 1) % perimetre.Count;
                        PointF p1 = perimetre[i1];
                        PointF p2 = perimetre[i2];
                        _selectedWallPoints2 = (p1, p2);

                        _dragMode = DragMode.MoveWall;
                        _dragStart = e.Location;
                        this.Cursor = Cursors.Hand;
                    }
                }


            }
            Invalidate();
        }

        private void PlanView_MouseMove(object? sender, MouseEventArgs e) {
            PointF mousePos = new PointF(e.X, e.Y);

            // Check if mouse is over delete button
            bool wasOverDeleteButton = _isMouseOverDeleteButton;
            _isMouseOverDeleteButton = _selectedMeuble != null && IsPointOverDeleteButton(mousePos);

            // Change cursor and redraw if needed
            if (wasOverDeleteButton != _isMouseOverDeleteButton) {
                this.Cursor = _isMouseOverDeleteButton ? Cursors.Hand : Cursors.Default;
                this.Invalidate();
            }

            switch (_dragMode) {
                case DragMode.Pan:
                    float dx = e.X - _dragStart.X;
                    float dy = e.Y - _dragStart.Y;
                    _offset = new PointF(_offset.X + dx, _offset.Y + dy);
                    _dragStart = e.Location;
                    Invalidate();
                    break;


                case DragMode.MoveMeuble:
                    if (_selectedMeuble != null) {
                        PointF planDansPoint = ScreenToPlan(e.Location);
                        _selectedMeuble.Position = new PointF(
                            planDansPoint.X - _meubleOffset.X,
                            planDansPoint.Y - _meubleOffset.Y
                        );

                        if (_selectedMeuble.IsMural) {
                            Murs murs = planController.ObtenirMurs();
                            murs.placerElem(_selectedMeuble, (PointF)_selectedMeuble.Position);
                        }
                        this.Cursor = Cursors.Hand;
                        Invalidate();
                    }
                    break;

                case DragMode.RotateMeuble:
                    if (_selectedMeuble != null) {
                        PointF screenCenter = PlanToScreen(GetMeubleCenter());
                        float currentAngle = (float)Math.Atan2(
                            e.Location.Y - screenCenter.Y,
                            e.Location.X - screenCenter.X
                        );
                        float angleDelta = (currentAngle - _initialMouseAngle) * (180f / (float)Math.PI);
                        _selectedMeuble.tourner(angleDelta, false); 
                        _angleAction = angleDelta;
                        _initialMouseAngle = currentAngle;

                        Invalidate();
                    }
                    break;


                case DragMode.MoveWall:
                    PointF current = ScreenToPlan(new PointF(e.Location.X, e.Location.Y));
                    PointF start = ScreenToPlan(_dragStart);
                    PointF delta = new PointF(current.X - start.X, current.Y - start.Y);

                    Murs mursData = planController.ObtenirMurs();

                    List<PointF> perimetre = mursData.Perimetre;
                    Murs oldMurs = mursData.Clone();

                    if (perimetre.Count <= 2) return;

                    int i1 = _selectedWall;
                    int i2 = (i1 + 1) % perimetre.Count;
                    PointF p1 = perimetre[i1];
                    PointF p2 = perimetre[i2];

                    PointF newP1 = new PointF(p1.X + delta.X, p1.Y + delta.Y);
                    PointF newP2 = new PointF(p2.X + delta.X, p2.Y + delta.Y);

                    perimetre[i1] = newP1;
                    perimetre[i2] = newP2;
                    if (i1 == 0)
                        perimetre[^1] = newP1;
                    else if (i1 == perimetre.Count - 2)
                        perimetre[0] = newP2;

                   
                    
                    planController.SetMurs(perimetre);

                    List<Meuble> elemsMuraux = mursData.GetElemsMuraux();
                    foreach (Meuble elem in elemsMuraux) {
                        PointF newpos = new PointF(((PointF)elem.Position).X + delta.X, ((PointF)elem.Position).Y + delta.Y);
                        mursData.placerElem(elem, newpos);
                    }

                   
                    _selectedWallPoints1 = (newP1, newP2);
                    _dragStart = e.Location;
                    
                    break;


                case DragMode.None:
                    PointF planPoint = ScreenToPlan(e.Location);
                    this.Cursor = (planController.FindMeubleAtPoint(planPoint) != null) ? Cursors.Hand : Cursors.Default;
                    break;
            }
            Invalidate();
        }

        private void PlanView_MouseUp(object? sender, MouseEventArgs e) {
            if (_dragMode == DragMode.None) {
                if ((Control.ModifierKeys & Keys.Shift) == 0 && e.Button == MouseButtons.Left) {
                    PointF planPoint = ScreenToPlan(e.Location);
                    Meuble? meuble = planController.FindMeubleAtPoint(planPoint);

                    if (meuble != null && meuble != _selectedMeuble) {
                        _selectedMeuble = meuble;
                    } else {
                        _selectedMeuble = null;
                    }
                    Invalidate();
                }
            }

            

            switch (_dragMode) {
                case DragMode.Pan:
                    Cursor = Cursors.Default;
                    _dragMode = DragMode.None;
                    break;
                case DragMode.MoveMeuble:
                    undoRedoControleur.add(new DeplacementMeuble(_selectedMeuble, (PointF)_selectedMeuble.Position, _dragStart));
                    Cursor = Cursors.Default;
                    _dragMode = DragMode.None;
                    break;
                case DragMode.RotateMeuble:
                    undoRedoControleur.add(new RotationMeuble(_selectedMeuble, _angleAction));
                    Cursor = Cursors.Default;
                    _dragMode = DragMode.None;
                    break;
                case DragMode.MoveWall:
                    undoRedoControleur.add(new DeplacementMur((_selectedWall, (_selectedWall+1)%planController.ObtenirMurs().Perimetre.Count()) , _selectedWallPoints1, _selectedWallPoints2));
                    Cursor = Cursors.Default;
                    _dragMode = DragMode.None;
                    break;
            }
            _selectedWall = -1;
            Invalidate();
        }

        private void PlanView_KeyDown(object? sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.ShiftKey)
                this.Cursor = Cursors.SizeAll;  // curseur “pan”
        }

        private void PlanView_KeyUp(object? sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.ShiftKey)
                this.Cursor = Cursors.Default;  // retour curseur normal
        }


        private PointF ScreenDeltaToPlanDelta(PointF screenDelta) {
            float scale = 1; // GetCurrentZoom();  // par exemple : 1.0 = 100%, 2.0 = zoom x2
            return new PointF(
                (int)(screenDelta.X / scale),
                (int)(screenDelta.Y / scale)
            );
        }

        private bool IsPointOverDeleteButton(PointF screenPoint) {
            if (_selectedMeuble == null) return false;

            // Get the center of the meuble in screen coordinates
            PointF center = _selectedMeuble.GetCenter();
            PointF screenCenter = PlanToScreen(center);

            // Create a matrix for the transformations
            Matrix matrix = new Matrix();
            matrix.Translate(screenCenter.X, screenCenter.Y);
            matrix.Rotate(_selectedMeuble.getAngle());

            // Transform the screen point to the meuble's coordinate system
            PointF[] points = new PointF[] { screenPoint };
            matrix.Invert();
            matrix.TransformPoints(points);
            PointF transformedPoint = points[0];

            // Calculate button rectangle in meuble's coordinates
            float buttonX = _selectedMeuble.Width * (3 / 2) - DeleteButtonSize;
            float buttonY = -_selectedMeuble.Height * (3 / 2);
            RectangleF buttonRect = new RectangleF(buttonX, buttonY, DeleteButtonSize, DeleteButtonSize);

            // Check if the transformed point is inside the button rectangle
            return buttonRect.Contains(transformedPoint);
        }

    }
}
