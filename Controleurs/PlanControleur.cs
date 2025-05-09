﻿using System;
using System.Diagnostics;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.Controleurs {
    public class PlanControleur {

        Plan plan;
        public event Action PlanChanged = delegate { };
        private bool show_grid;
        private float _zoomFactor = 1.0f;

        public float ZoomFactor {
            get { return _zoomFactor; }
            set { ChangerZoom(value); }
        }

        public PlanControleur(Modele m) {
            plan = m.planActuel;
            show_grid = true;
        }

        public List<Meuble> ObtenirMeublePlacé() {
            return plan.Meubles;
        }

        public Meuble? FindMeubleAtPoint(PointF planPoint) {
            return plan.FindMeubleAtPoint(planPoint);
        }

        public int FindMurAtPoint(PointF planPoint) {
            return plan.FindMurAtPoint(planPoint);
        }


        public void PlaceMeubleAtPosition(Meuble m, PointF position) {
            this.plan.PlacerMeuble(m, position);
            OnPlanChanged();
        }

        public void SupprimerMeuble(Meuble m) {
            this.plan.SupprimerMeuble(m);
            OnPlanChanged();
        }

        public virtual void OnPlanChanged() {
            PlanChanged?.Invoke();
        }

        public void tournerMeuble(Meuble meuble, float angle, bool fixeddirection) {
            plan.tournerMeuble(meuble, angle, fixeddirection);
            OnPlanChanged();
        }

        public Murs ObtenirMurs() {
            return plan.Murs;
        }

        public void SetMurs(List<PointF> points) {
            Murs murs = plan.Murs;
            if (points.Count < 3) return;

            List<PointF> oldPoints = new List<PointF>(murs.Perimetre);
            murs.Perimetre = points;


            OnPlanChanged();
        }


        public bool isGridVisible() {
            return show_grid;
        }

        public void toggleGrid() {
            show_grid = !show_grid;
            OnPlanChanged();
        }


        public void ChangerZoom(float zoom) {
            if (zoom >= 0.1f && zoom <= 3.0f) {
                _zoomFactor = zoom;
                OnPlanChanged();
            }
        }
        public void SetCurrentPlan(Plan plan) {
            if (plan == null)
                return;

            
            this.plan = plan;
        }

        public bool estDansSalle(Meuble m) {
            return this.plan.EstDansEspaceDesMurs(m);
        }

        public bool escequelesmursecroisent() {
            return this.plan.MursSeCroisent();
        }


    }
}
