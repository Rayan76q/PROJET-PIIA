﻿using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Model;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System;

namespace PROJET_PIIA.View {
    public enum PlanMode { // jsp où mettre
        Deplacement,
        DessinPolygone,
        
    }

    public partial class MainView : Form {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private PlanView planView;
        private Button switchmodebutton;
        private Button button2;
        private ControleurMainView ctrg;
        private Panel filterPanel;
        private Button filterButton;
        private FlowLayoutPanel meubleListPanel;
        private FlowLayoutPanel tagsPanel;
        private bool isFilterPanelVisible = false;
        private List<string> availableTags = TagExtensions.allStrings();
        private List<string> selectedTags = new List<string>();
        private List<Meuble> selectedMeubles = new List<Meuble>();
        private FlowLayoutPanel availableTagsPanel;
        private FlowLayoutPanel selectedTagsPanel;
        private TextBox searchBox;

        // Furniture categories and tags
        private List<string> categories = TagExtensions.allStrings();


        private List<string> frequentlyUsedItems = new List<string> {
        };

        private List<string> positionTags = new List<string> {
            "Mural", "Sol"
        };

        public MainView(Modele m) {
            this.DoubleBuffered = true;
            InitializeComponent();
            ctrg = new ControleurMainView(m);
            planView = new PlanView(ctrg);
            selectedMeubles = ctrg.catalogue.Meubles;
            splitContainer1.Panel2.Controls.Add(planView);
        }

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            splitContainer1 = new SplitContainer();
            sdb_murs_label1 = new Label();
            sdb_murs_bt1 = new Button();
            button2 = new Button();
            switchmodebutton = new Button();
            label1 = new Label();
            labelsurface = new Label();
            rotate = new Button();
            zoombar = new TrackBar();
            showgrid = new Button();
            button3 = new Button();
            Redo = new Button();
            Undo = new Button();
            toggleButton = new Button();
            fontDialog1 = new FontDialog();
            toolStrip1 = new ToolStrip();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)zoombar).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 25);
            splitContainer1.Margin = new Padding(3, 2, 3, 2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = Color.Silver;
            splitContainer1.Panel1.Controls.Add(sdb_murs_label1);
            splitContainer1.Panel1.Controls.Add(sdb_murs_bt1);
            splitContainer1.Panel1.Paint += splitContainer1_Panel1_Paint;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(button2);
            splitContainer1.Panel2.Controls.Add(switchmodebutton);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(labelsurface);
            splitContainer1.Panel2.Controls.Add(rotate);
            splitContainer1.Panel2.Controls.Add(zoombar);
            splitContainer1.Panel2.Controls.Add(showgrid);
            splitContainer1.Panel2.Controls.Add(button3);
            splitContainer1.Panel2.Controls.Add(Redo);
            splitContainer1.Panel2.Controls.Add(Undo);
            splitContainer1.Panel2.Controls.Add(toggleButton);
            splitContainer1.Panel2.Paint += splitContainer1_Panel2_Paint;
            splitContainer1.Panel1.SizeChanged += Panel1_SizeChanged;
            splitContainer1.Size = new Size(952, 389);
            splitContainer1.SplitterDistance = 229;
            splitContainer1.TabIndex = 1;
            splitContainer1.TabStop = false;
            // 
            // sdb_murs_label1
            // 
            sdb_murs_label1.AutoSize = true;
            sdb_murs_label1.Location = new Point(3, 103);
            sdb_murs_label1.Name = "sdb_murs_label1";
            sdb_murs_label1.Size = new Size(123, 15);
            sdb_murs_label1.TabIndex = 1;
            sdb_murs_label1.Text = "Rectangulaire 600x400";
            sdb_murs_label1.TextAlign = ContentAlignment.MiddleCenter;
            sdb_murs_label1.Click += label2_Click;
            // 
            // sdb_murs_bt1
            // 
            sdb_murs_bt1.Location = new Point(3, 32);
            sdb_murs_bt1.Name = "sdb_murs_bt1";
            sdb_murs_bt1.Size = new Size(214, 68);
            sdb_murs_bt1.TabIndex = 0;
            sdb_murs_bt1.Text = "Plan Rectangulaire";
            sdb_murs_bt1.UseVisualStyleBackColor = true;
            sdb_murs_bt1.Click += sidebar_bt1_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Right;
            button2.Enabled = false;
            button2.Location = new Point(658, 237);
            button2.Name = "button2";
            button2.Size = new Size(43, 23);
            button2.TabIndex = 10;
            button2.Text = "Reset";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // switchmodebutton
            // 
            switchmodebutton.Anchor = AnchorStyles.Bottom;
            switchmodebutton.Location = new Point(295, 331);
            switchmodebutton.Margin = new Padding(3, 2, 3, 2);
            switchmodebutton.Name = "switchmodebutton";
            switchmodebutton.Size = new Size(144, 38);
            switchmodebutton.TabIndex = 9;
            switchmodebutton.Text = "Dessin";
            switchmodebutton.UseVisualStyleBackColor = true;
            switchmodebutton.Click += switchmodebutton_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(532, 28);
            label1.Name = "label1";
            label1.Size = new Size(36, 15);
            label1.TabIndex = 8;
            label1.Text = "utile :";
            // 
            // labelsurface
            // 
            labelsurface.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelsurface.AutoSize = true;
            labelsurface.Location = new Point(532, 14);
            labelsurface.Name = "labelsurface";
            labelsurface.Size = new Size(65, 15);
            labelsurface.TabIndex = 7;
            labelsurface.Text = "Superficie :";
            labelsurface.Click += labelsurface_Click;
            // 
            // rotate
            // 
            rotate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            rotate.Location = new Point(658, 286);
            rotate.Margin = new Padding(3, 2, 3, 2);
            rotate.Name = "rotate";
            rotate.Size = new Size(44, 38);
            rotate.TabIndex = 6;
            rotate.Text = "button1";
            rotate.UseVisualStyleBackColor = true;
            // 
            // zoombar
            // 
            zoombar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            zoombar.Location = new Point(3, 331);
            zoombar.Margin = new Padding(3, 2, 3, 2);
            zoombar.Name = "zoombar";
            zoombar.Size = new Size(131, 45);
            zoombar.TabIndex = 5;
            // 
            // showgrid
            // 
            showgrid.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            showgrid.Location = new Point(658, 337);
            showgrid.Margin = new Padding(3, 2, 3, 2);
            showgrid.Name = "showgrid";
            showgrid.Size = new Size(44, 38);
            showgrid.TabIndex = 4;
            showgrid.Text = "button4";
            showgrid.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(3, 47);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(109, 38);
            button3.TabIndex = 3;
            button3.Text = "Meuble";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Redo
            // 
            Redo.Location = new Point(68, 5);
            Redo.Margin = new Padding(3, 2, 3, 2);
            Redo.Name = "Redo";
            Redo.Size = new Size(44, 38);
            Redo.TabIndex = 2;
            Redo.Text = "button2";
            Redo.UseVisualStyleBackColor = true;
            // 
            // Undo
            // 
            Undo.Location = new Point(3, 5);
            Undo.Margin = new Padding(3, 2, 3, 2);
            Undo.Name = "Undo";
            Undo.Size = new Size(44, 38);
            Undo.TabIndex = 1;
            Undo.Text = "button1";
            Undo.UseVisualStyleBackColor = true;
            Undo.Click += Undo_Click;
            // 
            // toggleButton
            // 
            toggleButton.Anchor = AnchorStyles.Left;
            toggleButton.CausesValidation = false;
            toggleButton.Location = new Point(3, 184);
            toggleButton.Margin = new Padding(3, 2, 3, 2);
            toggleButton.Name = "toggleButton";
            toggleButton.Size = new Size(28, 76);
            toggleButton.TabIndex = 0;
            toggleButton.Text = ">";
            toggleButton.UseVisualStyleBackColor = true;
            toggleButton.Click += button1_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(952, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(952, 414);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainView";
            Text = "Form1";
            Load += MainView_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)zoombar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e) {
            if (splitContainer1.Panel1Collapsed) {
                splitContainer1.Panel1Collapsed = false;
                toggleButton.Text = "<";  // Barre ouverte, bouton pointant à droite
            } else {
                splitContainer1.Panel1Collapsed = true;
                toggleButton.Text = ">";  // Barre fermée, bouton pointant à gauche
            }
        }
        private SplitContainer splitContainer1;
        private FontDialog fontDialog1;
        private Button button3;
        private Button Redo;
        private Button Undo;
        private Button showgrid;
        private TrackBar zoombar;
        private Label labelsurface;
        private Button rotate;
        private Label label1;
        private ToolStrip toolStrip1;
        private Button sdb_murs_bt1;
        private Label sdb_murs_label1;
        private Button toggleButton;

        private void labelsurface_Click(object sender, EventArgs e) {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e) {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e) {

        }

        private void MainView_Load(object sender, EventArgs e) {

        }

        private void Undo_Click(object sender, EventArgs e) {

        }

        private void sidebar_bt1_Click(object sender, EventArgs e) {
            List<Point> p = [new Point(0, 0), new Point(600, 0), new Point(600, 400), new Point(0, 400)];
            ctrg.setMurs(p);
        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void switchmodebutton_Click(object sender, EventArgs e) {
            this.ctrg.ChangerMode();
            if (PlanMode.DessinPolygone == ctrg.ModeEdition) {
                button2.Enabled = true;
            } else {
                button2.Enabled = false;
            }

            ((Button)sender).Text = (ctrg.ModeEdition == PlanMode.Deplacement) ? "Dessin" : "Deplacement";
        }

        private void button2_Click(object sender, EventArgs e) {
            this.ctrg.setMurs(new List<Point>());
        }

        private void button3_Click(object sender, EventArgs e) {
            this.ctrg.ChangerModeMeuble();
            splitContainer1.Panel1.Controls.Clear();
            if (ctrg.ModeMeuble) {
                button3.Text = "Murs";
                InitializeSidePanelMeubles();
            } else {
                button3.Text = "Meuble";
                InitializeSidePanelMurs();
            }
        }


        private void Panel1_SizeChanged(object sender, EventArgs e) {
            // Force panel to redraw
            splitContainer1.Panel1.Invalidate();

            // If you have the meubleListPanel, update its items
            if (meubleListPanel != null) {
                foreach (Control control in meubleListPanel.Controls) {
                    if (control is Panel meublePanel) {
                        meublePanel.Width = meubleListPanel.ClientSize.Width - 10;

                        // Reposition any anchored controls like star buttons
                        foreach (Control c in meublePanel.Controls) {
                            if (c is Button starButton && (starButton.Text == "★" || starButton.Text == "☆")) {
                                starButton.Location = new Point(meublePanel.Width - 40, starButton.Location.Y);
                            }
                        }
                    }
                }
            }

            // Update filter panels if they exist
            if (selectedTagsPanel != null) {
                selectedTagsPanel.Width = splitContainer1.Panel1.Width - 10;
            }

            if (availableTagsPanel != null) {
                availableTagsPanel.Width = splitContainer1.Panel1.Width - 10;
                UpdateTagPanelsLayout();
            }

            if (filterButton != null) {
                filterButton.Location = new Point(splitContainer1.Panel1.Width - 50, filterButton.Location.Y);
            }
        }

        private void InitializeSidePanelMurs() {
            splitContainer1.Panel1.Controls.Clear();
            splitContainer1.Panel1.Controls.Add(sdb_murs_label1);
            splitContainer1.Panel1.Controls.Add(sdb_murs_bt1);
            splitContainer1.Panel1.Invalidate();
        }

        private void AddMeubleToPanel(Meuble m, FlowLayoutPanel panel) {
            string meubleName = m.Nom;
            // Create panel for the meuble item
            Panel meublePanel = new Panel {
                Width = panel.Width,
                Height = 90,
                Margin = new Padding(2,5,10,0),
                BackColor = Color.WhiteSmoke,
            };

            // Create button with furniture icon that supports dragging
            Button meubleButton = new Button {
                Text = "🪑", // Default furniture icon
                Width = 60,
                Height = 60,
                Location = new Point(10, 5),
                FlatStyle = FlatStyle.Flat,
                Tag = m // Store meuble object for reference
            };

            // Enable button to be dragged
            meubleButton.MouseDown += (sender, e) => {
                if (e.Button == MouseButtons.Left) {
                    // Start drag operation with the meuble object
                    meubleButton.DoDragDrop(m, DragDropEffects.Copy);
                }
            };

            // Create label for furniture name
            Label nameLabel = new Label {
                Text = meubleName,
                Location = new Point(80, 10),
                AutoSize = true,
            };

            // Create favorite button
            Button starButton = new Button {
                Text = frequentlyUsedItems.Contains(meubleName) ? "★" : "☆",
                Width = 30,
                Height = 30,
                Location = new Point(160, 50),
                FlatStyle = FlatStyle.Flat,
                Tag = meubleName
            };
            starButton.Click += ToggleFavorite_Click;

            // Add all controls to the meuble panel
            meublePanel.Controls.Add(meubleButton);
            meublePanel.Controls.Add(nameLabel);
            meublePanel.Controls.Add(starButton);

            // Add meuble panel to the parent panel
            panel.Controls.Add(meublePanel);
        }

        private void InitializeSidePanelMeubles() {
            splitContainer1.Panel1.Controls.Clear();

            // Barre d'en-tête avec la zone de recherche et le bouton de filtre
            Panel headerPanel = new Panel {
                Dock = DockStyle.Top,
                Height = 40
            };

            // Création de la zone de recherche
            searchBox = new TextBox {
                Width = 150,
                Location = new Point(10, 10),
                PlaceholderText = "Recherche"
            };

            // À chaque modification du texte, on rafraîchit la liste
            searchBox.TextChanged += SearchBox_TextChanged;
            headerPanel.Controls.Add(searchBox);

            // Ajoutez le bouton de filtre si besoin
            filterButton = new Button {
                Text = "🔍︎",
                Width = 40,
                Height = 30,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Location = new Point(160, 5),
                FlatStyle = FlatStyle.Flat
            };
            filterButton.Click += FilterButton_Click;
            headerPanel.Controls.Add(filterButton);

            splitContainer1.Panel1.Controls.Add(headerPanel);

            // Panel d'affichage des meubles
            meubleListPanel = new FlowLayoutPanel {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                Width = splitContainer1.Panel1.Width - 20, 
                AutoScroll = true,
                WrapContents = false,
                Padding = new Padding(5, 40, 5, 5),
            };

            // Remplissage initial de la liste en fonction de la recherche (initialement vide)
            FilterMeubles();
            CreateFilterPanel();
            splitContainer1.Panel1.Controls.Add(meubleListPanel);
        }

        // Méthode appelée à chaque changement dans la barre de recherche
        private void SearchBox_TextChanged(object sender, EventArgs e) {
            FilterMeubles();
        }

        // Méthode de rafraîchissement de la liste des meubles selon la recherche
        private void FilterMeubles() {
            // Vider la liste actuelle
            meubleListPanel.Controls.Clear();
            // Récupérer le texte de la recherche en minuscule
            string query = searchBox.Text.Trim().ToLower();

            // Parcourir tous les meubles de la collection
            foreach (Meuble m in ctrg.catalogue.Meubles) {
                // Appliquer les filtres éventuels sur les tags avec votre méthode existante
                if (!IsMeubleVisibleWithCurrentTags(m))
                    continue;

                // Si une recherche est active, vérifier que le nom contient le texte
                if (!string.IsNullOrEmpty(query) && !m.Nom.ToLower().Contains(query))
                    continue;

                // Si le meuble correspond aux critères, l'ajouter au panel
                AddMeubleToPanel(m, meubleListPanel);
            }
        }





        private void CreateFilterPanel() {
            filterPanel = new Panel {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke,
                Visible = false
            };

            // Selected tags section with dynamic sizing
            Label selectedTagsLabel = new Label {
                Text = "Tags sélectionnés",
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 40)
            };
            filterPanel.Controls.Add(selectedTagsLabel);

            selectedTagsPanel = new FlowLayoutPanel {
                Location = new Point(5, 65),
                Width = splitContainer1.Panel1.Width - 10, // Almost full width
                Height = splitContainer1.Panel1.Height / 3,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = false, // Start with no scrolling
                Padding = new Padding(2)
            };

            // Add selected tags
            RefreshSelectedTagsPanel();
            filterPanel.Controls.Add(selectedTagsPanel);

            // Available tags section
            Label availableTagsLabel = new Label {
                Text = "Tags disponibles",
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 65 + selectedTagsPanel.Height + 10)
            };
            filterPanel.Controls.Add(availableTagsLabel);

            // Position available tags panel below selected tags panel
            availableTagsPanel = new FlowLayoutPanel {
                Location = new Point(5, 65 + selectedTagsPanel.Height + 35),
                Width = splitContainer1.Panel1.Width - 10, // Almost full width
                Height = splitContainer1.Panel1.Height / 3,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = false, // Start with no scrolling
                Padding = new Padding(2)
            };

            // Add available tags to grid
            RefreshAvailableTagsPanel();
            filterPanel.Controls.Add(availableTagsPanel);

            splitContainer1.Panel1.Controls.Add(filterPanel);

            // Add resize handler to update panel widths when the form or splitter changes size
            splitContainer1.Panel1.SizeChanged += (sender, e) => {
                selectedTagsPanel.Width = splitContainer1.Panel1.Width - 10;
                availableTagsPanel.Width = splitContainer1.Panel1.Width - 10;
                UpdateTagPanelsScrolling();
            };
        }

        // Add this method to check if scrolling is needed
        private void UpdateTagPanelsScrolling() {
            // For selected tags panel
            bool needsVerticalScroll = false;
            foreach (Control ctrl in selectedTagsPanel.Controls) {
                if (ctrl.Bottom > selectedTagsPanel.Height) {
                    needsVerticalScroll = true;
                    break;
                }
            }
            selectedTagsPanel.AutoScroll = needsVerticalScroll;

            // For available tags panel
            needsVerticalScroll = false;
            foreach (Control ctrl in availableTagsPanel.Controls) {
                if (ctrl.Bottom > availableTagsPanel.Height) {
                    needsVerticalScroll = true;
                    break;
                }
            }
            availableTagsPanel.AutoScroll = needsVerticalScroll;
        }

        private void UpdateTagPanelsLayout() {
            // Calculate a reasonable height for the selected tags panel
            int selectedTagsCount = selectedTags.Count;
            int rowHeight = 40; // Height of each tag + margin
            int minRows = 1;
            int maxRows = 5;
            int tagsPerRow = splitContainer1.Panel1.Width > 200 ? 3 : 2; 

            int rows = Math.Min(maxRows, Math.Max(minRows, (int)Math.Ceiling((double)selectedTagsCount / tagsPerRow)));
            int selectedPanelHeight = (rows+1) * rowHeight;

            // Update selected tags panel height
            selectedTagsPanel.Height = selectedPanelHeight;

            // Update the location and size of available tags label and panel
            int availableLabelY = 65 + selectedPanelHeight + 10;
            foreach (Control control in filterPanel.Controls) {
                if (control is Label label && label.Text == "Tags disponibles") {
                    label.Location = new Point(10, availableLabelY);
                    break;
                }
            }

            availableTagsPanel.Location = new Point(0, availableLabelY + 25);
            availableTagsPanel.Height = 290 - availableLabelY;
        }

        private void AddTagWithRemoveButton(string tagName, FlowLayoutPanel panel) {
            // Calculate width based on text length (min 80px, max 150px)
            int tagWidth = Math.Max(80, Math.Min(150, TextRenderer.MeasureText(tagName, SystemFonts.DefaultFont).Width + 40));

            RoundedPanel tagContainer = new RoundedPanel {
                Width = tagWidth,
                Height = 30,
                Margin = new Padding(3, 5, 3, 5),
                BackColor = Color.LightGray,
                BorderRadius = 15,
                Tag = tagName
            };

            Label tagLabel = new Label {
                Text = tagName,
                AutoSize = true,
                Location = new Point(10, 7),
                BackColor = Color.Transparent
            };

            Button removeButton = new Button {
                Text = "×",
                Width = 20,
                Height = 20,
                Location = new Point(tagContainer.Width - 25, 5),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 8, FontStyle.Bold),
                BackColor = Color.Transparent,
                Tag = tagName
            };
            removeButton.FlatAppearance.BorderSize = 0;
            removeButton.Click += RemoveTag_Click;

            tagContainer.Controls.Add(tagLabel);
            tagContainer.Controls.Add(removeButton);
            panel.Controls.Add(tagContainer);
        }

        // Similarly update the AddTagWithAddButton method
        private void AddTagWithAddButton(string tagName, FlowLayoutPanel panel) {
            // Calculate width based on text length (min 80px, max 150px)
            int tagWidth = Math.Max(80, Math.Min(150, TextRenderer.MeasureText(tagName, SystemFonts.DefaultFont).Width + 40));

            RoundedPanel tagContainer = new RoundedPanel {
                Width = tagWidth,
                Height = 30,
                Margin = new Padding(3, 5, 3, 5),
                BackColor = Color.LightGray,
                BorderRadius = 15,
                Tag = tagName
            };

            Label tagLabel = new Label {
                Text = tagName,
                AutoSize = true,
                Location = new Point(10, 7),
                BackColor = Color.Transparent
            };

            Button addButton = new Button {
                Text = "+",
                Width = 20,
                Height = 20,
                Location = new Point(tagContainer.Width - 25, 5),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 8, FontStyle.Bold),
                BackColor = Color.Transparent,
                Tag = tagName
            };
            addButton.FlatAppearance.BorderSize = 0;
            addButton.Click += AddTag_Click;

            tagContainer.Controls.Add(tagLabel);
            tagContainer.Controls.Add(addButton);
            panel.Controls.Add(tagContainer);
        }

        private void AddTag_Click(object sender, EventArgs e) {
            Button button = (Button)sender;
            string tagName = button.Tag.ToString();

            // Move tag from available to selected
            if (availableTags.Contains(tagName)) {
                availableTags.Remove(tagName);
                selectedTags.Add(tagName);

                // Update UI
                RefreshTagPanels();
                UpdateTagPanelsLayout();

                // Apply tag filters
                ApplyTagFilters();
            }
        }

        private void RemoveTag_Click(object sender, EventArgs e) {
            Button button = (Button)sender;
            string tagName = button.Tag.ToString();

            // Move tag from selected to available
            if (selectedTags.Contains(tagName)) {
                selectedTags.Remove(tagName);
                availableTags.Add(tagName);

                // Update UI
                RefreshTagPanels();
                UpdateTagPanelsLayout();

                // Apply tag filters
                ApplyTagFilters();
            }
        }

        private void RefreshTagPanels() {
            RefreshSelectedTagsPanel();
            RefreshAvailableTagsPanel();
            UpdateTagPanelsScrolling(); // Check if scrolling is needed
        }

        private void RefreshSelectedTagsPanel() {
            selectedTagsPanel.Controls.Clear();
            foreach (string tag in selectedTags) {
                AddTagWithRemoveButton(tag, selectedTagsPanel);
            }
        }

        private void RefreshAvailableTagsPanel() {
            availableTagsPanel.Controls.Clear();
            foreach (string tag in availableTags) {
                AddTagWithAddButton(tag, availableTagsPanel);
            }
        }

        private void ApplyTagFilters() {
            selectedMeubles = new List<Meuble>();
            foreach(Meuble m in ctrg.catalogue.Meubles) {
                if(IsMeubleVisibleWithCurrentTags(m)) selectedMeubles.Add(m);
            }
        }

        private void FilterButton_Click(object sender, EventArgs e) {
            if (!isFilterPanelVisible) {
                meubleListPanel.Visible = false;
                filterPanel.Visible = true;
                isFilterPanelVisible = true;

                // Update layout when showing filter panel
                UpdateTagPanelsLayout();
            } else {
                filterPanel.Visible = false;
                InitializeSidePanelMeubles();
                meubleListPanel.Visible = true;
                isFilterPanelVisible = false;
            }
        }

        // Custom panel with rounded corners
        public class RoundedPanel : Panel {
            public int BorderRadius { get; set; } = 20;

            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);

                GraphicsPath path = new GraphicsPath();
                Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

                path.AddArc(rect.X, rect.Y, BorderRadius, BorderRadius, 180, 90);
                path.AddArc(rect.X + rect.Width - BorderRadius, rect.Y, BorderRadius, BorderRadius, 270, 90);
                path.AddArc(rect.X + rect.Width - BorderRadius, rect.Y + rect.Height - BorderRadius, BorderRadius, BorderRadius, 0, 90);
                path.AddArc(rect.X, rect.Y + rect.Height - BorderRadius, BorderRadius, BorderRadius, 90, 90);
                path.CloseAllFigures();

                this.Region = new Region(path);

                // Draw border if needed
                using (Pen pen = new Pen(Color.LightGray, 1)) {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private void ToggleFavorite_Click(object sender, EventArgs e) {
            Button btn = (Button)sender;
            string meuble = btn.Tag.ToString();
            if (frequentlyUsedItems.Contains(meuble)) {
                frequentlyUsedItems.Remove(meuble);
                btn.Text = "☆";
            } else {
                frequentlyUsedItems.Add(meuble);
                btn.Text = "★";
            }
        }

        private bool IsMeubleVisibleWithCurrentTags(Meuble meuble) {
            if (selectedTags.Count == 0) return true;
            if (meuble.tags.Count == 0) return true;

            foreach(Tags t in meuble.tags) {
                if (selectedTags.Contains(t.GetDisplayName())) return true;
            }

            return false;
        }


    }
}