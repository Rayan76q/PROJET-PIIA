#nullable disable

using System.Diagnostics;
using PROJET_PIIA.Controleurs;
using PROJET_PIIA.CustomControls;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.View {


    public partial class MainView : Form {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private PlanView planView;
        private Plan p;
        // creer un user control expres ?
        private ToolStripButton modifyButton;
        private ToolStripTextBox searchToolBox;
        private ToolStripButton searchButton;
        private ToolStripButton downloadButton;
        private ToolStripButton shareButton;
        private ToolStripButton commentButton;
        private ToolStripButton newButton;
        private ToolStripButton loadButton;
        private ToolStripButton emailButton;
        private ToolStrip mainToolStrip;
        private ToolStripLabel appNameLabel;
        private ToolStripDropDownButton avatarButton;
        private ToolStripMenuItem loginMenuItem;
        private ToolStripSeparator rightAlignSeparator;
        private ToolStripMenuItem signupMenuItem;
        //

        private CustomControls.ModeSelector modeSelector1;

        ModeControler modeControler;
        AccountController compteControleur;
        private SidePanelMeuble MeubleSidePanel;
        private MurSidePanel MurSidePanel;
        private UserControl _currentEditor;

        public MainView(Modele m) {
            this.DoubleBuffered = true;
            InitializeComponent();
            modeControler = new ModeControler();
            compteControleur = new AccountController(m);
            compteControleur.SetMainView(this);
            //ctrg = new ControleurMainView(m);
            var pctr = new PlanControleur(m);
            var urctr = new UndoRedoControleur(pctr);
            planView = new PlanView(pctr, urctr);

            MeubleSidePanel = new SidePanelMeuble(m);
            MurSidePanel = new MurSidePanel(m, planView, pctr);
            _currentEditor = MeubleSidePanel;
            _currentEditor.Dock = DockStyle.Fill;

            spliterSidePlan.Panel1.Controls.Add(MurSidePanel);
            spliterSidePlan.Panel2.Controls.Add(planView);
            modeControler.ModeChangedActions += SwitchSidePanel;
            signupMenuItem.Click += SignupMenuItem_Click;

            p = m.planActuel;


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



        private void SwitchSidePanel() {
            spliterSidePlan.Panel1.Controls.Clear();
            switch (modeControler.ModeEdition) {
                case EditMode.Meuble:
                    _currentEditor = MeubleSidePanel;
                    break;
                case EditMode.Mur:
                    _currentEditor = MurSidePanel;
                    break;
            }
            spliterSidePlan.Panel1.Controls.Add(_currentEditor);
            _currentEditor.Dock = DockStyle.Fill;
            spliterSidePlan.Panel1.Invalidate();
            Invalidate();
        }



        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            mainToolStrip = new ToolStrip();
            appNameLabel = new ToolStripLabel();
            newButton = new ToolStripButton();
            loadButton = new ToolStripButton();
            modifyButton = new ToolStripButton();
            searchToolBox = new ToolStripTextBox();
            searchButton = new ToolStripButton();
            downloadButton = new ToolStripButton();
            shareButton = new ToolStripButton();
            commentButton = new ToolStripButton();
            emailButton = new ToolStripButton();
            rightAlignSeparator = new ToolStripSeparator();
            avatarButton = new ToolStripDropDownButton();
            loginMenuItem = new ToolStripMenuItem();
            signupMenuItem = new ToolStripMenuItem();
            spliterSidePlan = new SplitContainer();
            modeSelector1 = new ModeSelector();
            label1 = new Label();
            labelsurface = new Label();
            zoombar = new TrackBar();
            showgrid = new Button();
            Redo = new Button();
            Undo = new Button();
            toggleButton = new Button();
            fontDialog1 = new FontDialog();
            mainToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spliterSidePlan).BeginInit();
            spliterSidePlan.Panel2.SuspendLayout();
            spliterSidePlan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)zoombar).BeginInit();
            SuspendLayout();
            // 
            // mainToolStrip
            // 
            mainToolStrip.ImageScalingSize = new Size(20, 20);
            mainToolStrip.Items.AddRange(new ToolStripItem[] { appNameLabel, newButton, loadButton, modifyButton, searchToolBox, searchButton, downloadButton, shareButton, commentButton, emailButton, rightAlignSeparator, avatarButton });
            mainToolStrip.Location = new Point(0, 0);
            mainToolStrip.Name = "mainToolStrip";
            mainToolStrip.RenderMode = ToolStripRenderMode.System;
            mainToolStrip.Size = new Size(987, 28);
            mainToolStrip.TabIndex = 0;
            // 
            // appNameLabel
            // 
            appNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            appNameLabel.Name = "appNameLabel";
            appNameLabel.Size = new Size(65, 25);
            appNameLabel.Text = "PLANCHA!";
            // 
            // newButton
            // 
            newButton.Name = "newButton";
            newButton.Size = new Size(59, 25);
            newButton.Text = "Nouveau";
            // 
            // loadButton
            // 
            loadButton.Name = "loadButton";
            loadButton.Size = new Size(53, 25);
            loadButton.Text = "Charger";
            loadButton.Click += loadButton_Click;
            // 
            // modifyButton
            // 
            modifyButton.Name = "modifyButton";
            modifyButton.Size = new Size(56, 25);
            modifyButton.Text = "Modifier";
            modifyButton.Click += modifyButton_Click;
            // 
            // searchToolBox
            // 
            searchToolBox.Name = "searchToolBox";
            searchToolBox.Size = new Size(100, 28);
            // 
            // searchButton
            // 
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(23, 25);
            searchButton.Text = "🔍";
            // 
            // downloadButton
            // 
            downloadButton.Name = "downloadButton";
            downloadButton.Size = new Size(23, 25);
            downloadButton.Text = "⬇";
            downloadButton.Click += downloadButton_Click;
            // 
            // shareButton
            // 
            shareButton.Name = "shareButton";
            shareButton.Size = new Size(23, 25);
            shareButton.Text = "↗";
            // 
            // commentButton
            // 
            commentButton.Name = "commentButton";
            commentButton.Size = new Size(23, 25);
            commentButton.Text = "💬";
            // 
            // emailButton
            // 
            emailButton.Name = "emailButton";
            emailButton.Size = new Size(23, 25);
            emailButton.Text = "✉";
            // 
            // rightAlignSeparator
            // 
            rightAlignSeparator.Alignment = ToolStripItemAlignment.Right;
            rightAlignSeparator.Name = "rightAlignSeparator";
            rightAlignSeparator.Size = new Size(6, 28);
            // 
            // avatarButton
            // 
            avatarButton.Alignment = ToolStripItemAlignment.Right;
            avatarButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            avatarButton.DropDownItems.AddRange(new ToolStripItem[] { loginMenuItem, signupMenuItem });
            avatarButton.Font = new Font("Segoe UI", 12F);
            avatarButton.Name = "avatarButton";
            avatarButton.Size = new Size(45, 25);
            avatarButton.Text = "👤";
            avatarButton.ToolTipText = "Account options";
            // 
            // loginMenuItem
            // 
            loginMenuItem.Name = "loginMenuItem";
            loginMenuItem.Size = new Size(135, 26);
            loginMenuItem.Text = "Login";
            loginMenuItem.Click += LoginMenuItem_Click;
            // 
            // signupMenuItem
            // 
            signupMenuItem.Name = "signupMenuItem";
            signupMenuItem.Size = new Size(135, 26);
            signupMenuItem.Text = "Sign Up";
            // 
            // spliterSidePlan
            // 
            spliterSidePlan.Dock = DockStyle.Fill;
            spliterSidePlan.Location = new Point(0, 28);
            spliterSidePlan.Name = "spliterSidePlan";
            // 
            // spliterSidePlan.Panel2
            // 
            spliterSidePlan.Panel2.Controls.Add(modeSelector1);
            spliterSidePlan.Panel2.Controls.Add(label1);
            spliterSidePlan.Panel2.Controls.Add(labelsurface);
            spliterSidePlan.Panel2.Controls.Add(zoombar);
            spliterSidePlan.Panel2.Controls.Add(showgrid);
            spliterSidePlan.Panel2.Controls.Add(Redo);
            spliterSidePlan.Panel2.Controls.Add(Undo);
            spliterSidePlan.Panel2.Controls.Add(toggleButton);
            spliterSidePlan.Size = new Size(987, 437);
            spliterSidePlan.SplitterDistance = 244;
            spliterSidePlan.TabIndex = 1;
            spliterSidePlan.TabStop = false;
            // 
            // modeSelector1
            // 
            modeSelector1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            modeSelector1.BackColor = Color.LightGray;
            modeSelector1.Location = new Point(554, 394);
            modeSelector1.Margin = new Padding(3, 2, 3, 2);
            modeSelector1.Modes.Add("Murage");
            modeSelector1.Modes.Add("Meublage");
            modeSelector1.Name = "modeSelector1";
            modeSelector1.SelectedIndex = 0;
            modeSelector1.Size = new Size(163, 26);
            modeSelector1.TabIndex = 12;
            modeSelector1.Load += modeSelector1_Load;
            modeSelector1.Click += modeSelector1_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(543, 28);
            label1.Name = "label1";
            label1.Size = new Size(36, 15);
            label1.TabIndex = 8;
            label1.Text = "utile :";
            // 
            // labelsurface
            // 
            labelsurface.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelsurface.AutoSize = true;
            labelsurface.Location = new Point(543, 14);
            labelsurface.Name = "labelsurface";
            labelsurface.Size = new Size(65, 15);
            labelsurface.TabIndex = 7;
            labelsurface.Text = "Superficie :";
            // 
            // zoombar
            // 
            zoombar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            zoombar.Location = new Point(3, 378);
            zoombar.Margin = new Padding(3, 2, 3, 2);
            zoombar.Maximum = 300;
            zoombar.Minimum = 10;
            zoombar.Name = "zoombar";
            zoombar.Size = new Size(131, 45);
            zoombar.TabIndex = 5;
            zoombar.TickFrequency = 10;
            zoombar.Value = 100;
            zoombar.Scroll += zoombar_Scroll;
            // 
            // showgrid
            // 
            showgrid.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            showgrid.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            showgrid.Location = new Point(673, 349);
            showgrid.Margin = new Padding(3, 2, 3, 2);
            showgrid.Name = "showgrid";
            showgrid.Size = new Size(44, 38);
            showgrid.TabIndex = 4;
            showgrid.Text = "⊞";
            showgrid.UseVisualStyleBackColor = true;
            showgrid.Click += showgrid_Click;
            // 
            // Redo
            // 
            Redo.Location = new Point(68, 5);
            Redo.Margin = new Padding(3, 2, 3, 2);
            Redo.Name = "Redo";
            Redo.Size = new Size(44, 38);
            Redo.TabIndex = 2;
            Redo.Text = "↪";
            Redo.UseVisualStyleBackColor = true;
            Redo.Click += Redo_Click;
            // 
            // Undo
            // 
            Undo.Location = new Point(3, 5);
            Undo.Margin = new Padding(3, 2, 3, 2);
            Undo.Name = "Undo";
            Undo.Size = new Size(44, 38);
            Undo.TabIndex = 1;
            Undo.Text = "↩";
            Undo.UseVisualStyleBackColor = true;
            Undo.Click += Undo_Click;
            // 
            // toggleButton
            // 
            toggleButton.Anchor = AnchorStyles.Left;
            toggleButton.CausesValidation = false;
            toggleButton.Location = new Point(3, 188);
            toggleButton.Margin = new Padding(3, 2, 3, 2);
            toggleButton.Name = "toggleButton";
            toggleButton.Size = new Size(28, 76);
            toggleButton.TabIndex = 0;
            toggleButton.Text = "<";
            toggleButton.UseVisualStyleBackColor = true;
            toggleButton.Click += button1_Click;
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(987, 465);
            Controls.Add(spliterSidePlan);
            Controls.Add(mainToolStrip);
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainView";
            Text = "Kitchen Design App";
            mainToolStrip.ResumeLayout(false);
            mainToolStrip.PerformLayout();
            spliterSidePlan.Panel2.ResumeLayout(false);
            spliterSidePlan.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spliterSidePlan).EndInit();
            spliterSidePlan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)zoombar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e) {
            if (spliterSidePlan.Panel1Collapsed) {
                spliterSidePlan.Panel1Collapsed = false;
                toggleButton.Text = "<";  // Barre ouverte, bouton pointant à droite
                MeubleSidePanel.update_meubles();

            } else {
                spliterSidePlan.Panel1Collapsed = true;
                toggleButton.Text = ">";  // Barre fermée, bouton pointant à gauche
            }
        }
        private SplitContainer spliterSidePlan;
        private FontDialog fontDialog1;
        private Button Redo;
        private Button Undo;
        private Button showgrid;
        private TrackBar zoombar;
        private Label labelsurface;
        private Label label1;
        private Button toggleButton;



        // a metre dans un usercontrol/ controleur spé 
        private void LoginMenuItem_Click(object sender, EventArgs e) {
            if (compteControleur.ShowLoginDialog()) {
                UpdateAvatarForLoggedInUser();
            }
        }
        private void SignupMenuItem_Click(object sender, EventArgs e) {
            if (compteControleur.ShowSignupDialog()) {
                UpdateAvatarForLoggedInUser();
            }
        }

        private void UpdateAvatarForLoggedInUser() {
            ToolStripDropDownButton avatarButton = null;
            foreach (ToolStripItem item in mainToolStrip.Items) {
                if (item is ToolStripDropDownButton && item.Name == "avatarButton") {
                    avatarButton = (ToolStripDropDownButton)item;
                    break;
                }
            }

            if (avatarButton != null) {
                // Change icon or text for logged in state
                // avatarButton.Text = "👤 User"; // Example with username
                // Or set an image instead:
                // avatarButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
                // avatarButton.Image = Properties.Resources.UserAvatar;

                // Clear existing dropdown items
                avatarButton.DropDownItems.Clear();

                // Add user-specific menu items
                ToolStripMenuItem profileMenuItem = new ToolStripMenuItem("My Profile");
                profileMenuItem.Click += (s, e) => MessageBox.Show("Profile page will be shown");

                ToolStripMenuItem settingsMenuItem = new ToolStripMenuItem("Settings");
                settingsMenuItem.Click += (s, e) => MessageBox.Show("Settings page will be shown");

                ToolStripMenuItem logoutMenuItem = new ToolStripMenuItem("Logout");
                logoutMenuItem.Click += (s, e) => {
                    // Logout logic
                    MessageBox.Show("Logging out...");
                    ResetAvatarForLoggedOutUser();
                };

                // Add items to dropdown
                avatarButton.DropDownItems.Add(profileMenuItem);
                avatarButton.DropDownItems.Add(settingsMenuItem);
                avatarButton.DropDownItems.Add(new ToolStripSeparator());
                avatarButton.DropDownItems.Add(logoutMenuItem);
            }
        }

        // Method to reset avatar after logout
        private void ResetAvatarForLoggedOutUser() {
            // Find the avatar button
            ToolStripDropDownButton avatarButton = null;
            foreach (ToolStripItem item in mainToolStrip.Items) {
                if (item is ToolStripDropDownButton && item.Name == "avatarButton") {
                    avatarButton = (ToolStripDropDownButton)item;
                    break;
                }
            }

            if (avatarButton != null) {
                // Reset to default state
                avatarButton.Text = "👤";
                avatarButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
                avatarButton.Image = null;

                // Clear existing dropdown items
                avatarButton.DropDownItems.Clear();

                // Add default menu items for non-logged in users
                ToolStripMenuItem loginMenuItem = new ToolStripMenuItem("Login");
                loginMenuItem.Click += LoginMenuItem_Click;

                ToolStripMenuItem signupMenuItem = new ToolStripMenuItem("Sign Up");
                signupMenuItem.Click += SignupMenuItem_Click;

                // Add items to dropdown
                avatarButton.DropDownItems.Add(loginMenuItem);
                avatarButton.DropDownItems.Add(signupMenuItem);
            }
        }



        private void modeSelector1_Click(object sender, EventArgs e) {
            modeControler.SwitchMode();
        }

        private void modeSelector1_Load(object sender, EventArgs e) {

        }

        private void showgrid_Click(object sender, EventArgs e) {
            planView.toggleGrid();

        }

        private void zoombar_Scroll(object sender, EventArgs e) {
            float valeur = zoombar.Value / 100f;
            planView.ChangerZoom(valeur);
        }

        private void delete_meuble_Click(object sender, EventArgs e) {

        }

        private void Undo_Click(object sender, EventArgs e) {
            planView.undo();
        }

        private void Redo_Click(object sender, EventArgs e) {
            planView.redo();
        }

        private void downloadButton_Click(object sender, EventArgs e) {
            compteControleur.SavePlan(p);
        }

        private void modifyButton_Click(object sender, EventArgs e) {

        }

        private void loadButton_Click(object sender, EventArgs e) {
            try {
                // Get the list of plans for the current user
                List<Plan> plans = compteControleur.GetUserPlans();
               
                // Check if there are any plans to load
                if (plans == null || plans.Count == 0) {
                    MessageBox.Show("Vous n'avez aucun plan sauvegardé.", "Aucun plan",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Create a form to display the plans
                using (Form planSelectionForm = new Form()) {
                    planSelectionForm.Text = "Sélectionnez un plan";
                    planSelectionForm.Size = new Size(400, 300);
                    planSelectionForm.StartPosition = FormStartPosition.CenterScreen;
                    planSelectionForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    planSelectionForm.MaximizeBox = false;
                    planSelectionForm.MinimizeBox = false;

                    // Create a ListBox to display plans
                    ListBox planListBox = new ListBox();
                    planListBox.Dock = DockStyle.Fill;
                    planListBox.Items.AddRange(plans.Select(p => p.Nom).ToArray());
                    planListBox.SelectedIndex = 0; // Select first plan by default

                    // Create buttons panel
                    Panel buttonPanel = new Panel();
                    buttonPanel.Dock = DockStyle.Bottom;
                    buttonPanel.Height = 50;

                    // Create OK button
                    Button okButton = new Button();
                    okButton.Text = "Charger";
                    okButton.DialogResult = DialogResult.OK;
                    okButton.Width = 100;
                    okButton.Location = new Point(buttonPanel.Width / 2 - 110, 10);

                    // Create Cancel button
                    Button cancelButton = new Button();
                    cancelButton.Text = "Annuler";
                    cancelButton.DialogResult = DialogResult.Cancel;
                    cancelButton.Width = 100;
                    cancelButton.Location = new Point(buttonPanel.Width / 2 + 10, 10);

                    // Add buttons to panel
                    buttonPanel.Controls.Add(okButton);
                    buttonPanel.Controls.Add(cancelButton);

                    // Add controls to form
                    planSelectionForm.Controls.Add(planListBox);
                    planSelectionForm.Controls.Add(buttonPanel);

                    // Show the form and get the result
                    DialogResult result = planSelectionForm.ShowDialog();

                    // Handle the result
                    if (result == DialogResult.OK && planListBox.SelectedIndex >= 0) {
                        int selectedIndex = planListBox.SelectedIndex;
                        Plan selectedPlan = plans[selectedIndex];

                        // Load the selected plan into the application
                        LoadPlanIntoApplication(selectedPlan);
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Erreur lors du chargement des plans: {ex.Message}", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        
        private void LoadPlanIntoApplication(Plan selectedPlan) {
            try {
                if (selectedPlan == null) {
                    MessageBox.Show("Le plan sélectionné est invalide.", "Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                compteControleur.selectPlan(selectedPlan);
                planView.SetCurrentPlan(selectedPlan);
                MurSidePanel.setCurrentPlan(selectedPlan);
                MeubleSidePanel.update_meubles(); //in case
                planView.Invalidate();

                MessageBox.Show($"Plan '{selectedPlan.Nom}' chargé avec succès.", "Plan chargé",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show($"Erreur lors du chargement du plan: {ex.Message}", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}