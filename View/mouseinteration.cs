using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROJET_PIIA.Extensions;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.View {
    public partial class PlanView : UserControl {
        private void PlanView_MouseClick(object? sender, MouseEventArgs e) {
            // la selection du meuble de fais quand on souleve le click (MouseUp)
        }



        private void PlanView_MouseDown(object? sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                Point planPoint = ScreenToPlan(e.Location);

                if ((Control.ModifierKeys & Keys.Shift) != 0) {
                    _dragMode = DragMode.Pan;
                    _dragStart = e.Location;
                    this.Cursor = Cursors.SizeAll;
                    return;
                }

                var meuble = ctrg.FindMeubleAtPoint(planPoint);
                if (meuble != null && meuble.Position != null) {
                    _selectedMeuble = meuble;
                    _dragMode = DragMode.MoveMeuble;
                    _dragStart = e.Location;
                    
                    Point p = meuble.Position.Value;
                    _meubleOffset = new Point(
                        planPoint.X - p.X,
                        planPoint.Y - p.Y
                    );

                    this.Cursor = Cursors.Hand;
                }

                if (_selectedMeuble != null) {
                    Point screenHandle = PlanToScreen(GetHandleCenter());
                    Rectangle handleRect = new Rectangle(
                        screenHandle.X - 6, screenHandle.Y - 6, 12, 12);
                    if (handleRect.Contains(e.Location)) {
                        _dragMode = DragMode.RotateMeuble;
                        _dragStart = e.Location;
                        Point center = PlanToScreen(GetMeubleCenter());
                        Point p = new(e.Location.X - center.X, e.Location.Y - center.Y);
                        _initialMouseAngle = (float)Math.Atan2(p.Y,p.X);

                        this.Cursor = Cursors.Hand;
                        return;
                    }
                }

            }
            Invalidate();
        }

        private void PlanView_MouseMove(object? sender, MouseEventArgs e) {
            //_mousePosition = e.Location;
            switch (_dragMode) {
                case DragMode.Pan:
                    int dx = e.X - _dragStart.X;
                    int dy = e.Y - _dragStart.Y;
                    _offset.Offset(dx, dy);
                    _dragStart = e.Location;
                    Invalidate();
                    break;


                case DragMode.MoveMeuble:
                    if (_selectedMeuble != null) {
                        Point planDansPoint = ScreenToPlan(e.Location);
                        _selectedMeuble.Position = new Point(
                            planDansPoint.X - _meubleOffset.X,
                            planDansPoint.Y - _meubleOffset.Y
                        );
                        this.Cursor = Cursors.Hand;
                        Invalidate();
                    }
                    break;

                case DragMode.RotateMeuble:
                    if (_selectedMeuble != null) {
                        Point screenCenter = PlanToScreen(GetMeubleCenter());
                        float currentAngle = (float)Math.Atan2(
                            e.Location.Y - screenCenter.Y,
                            e.Location.X - screenCenter.X
                        );
                        float angleDelta = (currentAngle - _initialMouseAngle) * (180f / (float)Math.PI);
                        _selectedMeuble.tourner(angleDelta, false); //todo
                        _initialMouseAngle = currentAngle;

                        Invalidate();
                    }
                    break;

                case DragMode.None:
                    Point planPoint = ScreenToPlan(e.Location);
                    this.Cursor = (ctrg.FindMeubleAtPoint(planPoint) != null) ? Cursors.Hand : Cursors.Default;
                    break;
            }
            Invalidate();
        }

        private void PlanView_MouseUp(object? sender, MouseEventArgs e) {
            if (_dragMode == DragMode.None) {
                if ((Control.ModifierKeys & Keys.Shift) == 0 && e.Button == MouseButtons.Left) {
                    Point planPoint = ScreenToPlan(e.Location);
                    Meuble? meuble = ctrg.FindMeubleAtPoint(planPoint);

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
                case DragMode.MoveMeuble:
                case DragMode.RotateMeuble:
                    Cursor = Cursors.Default;
                    _dragMode = DragMode.None;
                    break;
            }
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


        private Point ScreenDeltaToPlanDelta(Point screenDelta) {
            float scale = 1; // GetCurrentZoom();  // par exemple : 1.0 = 100%, 2.0 = zoom x2
            return new Point(
                (int)(screenDelta.X / scale),
                (int)(screenDelta.Y / scale)
            );
        }
    }
}
