#nullable disable

using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Model;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System;
using PROJET_PIIA.CustomControls;
using System.Diagnostics;

namespace PROJET_PIIA.View {


    public partial class MainView : Form {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private PlanView planView;

        // creer un user control expres ?
        private ToolStripButton convertButton;
        private ToolStripTextBox searchToolBox;
        private ToolStripButton searchButton;
        private ToolStripButton downloadButton;
        private ToolStripButton shareButton;
        private ToolStripButton commentButton;
        private ToolStripButton newButton;
        private ToolStripButton modifyButton;
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
        private SidePanelMeuble MubleSidePanel;
        private MurSidePanel MurSidePanel;

        private UserControl _currentEditor;

        public MainView(Modele m) {
            this.DoubleBuffered = true;
            InitializeComponent();
            modeControler = new ModeControler();

            //ctrg = new ControleurMainView(m);
            var pctr = new PlanControleur(m);
            planView = new PlanView(pctr);

            MubleSidePanel = new SidePanelMeuble(m);
            MurSidePanel = new MurSidePanel(m, planView, pctr);
            _currentEditor = MubleSidePanel;
            _currentEditor.Dock = DockStyle.Fill;

            spliterSidePlan.Panel1.Controls.Add(MubleSidePanel);
            spliterSidePlan.Panel2.Controls.Add(planView);
            modeControler.ModeChangedActions += SwitchSidePanel;
            

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
                case EditMode.Meuble :
                    _currentEditor = MubleSidePanel;
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
            modifyButton = new ToolStripButton();
            convertButton = new ToolStripButton();
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
            mainToolStrip.Items.AddRange(new ToolStripItem[] { appNameLabel, newButton, modifyButton, convertButton, searchToolBox, searchButton, downloadButton, shareButton, commentButton, emailButton, rightAlignSeparator, avatarButton });
            mainToolStrip.Location = new Point(0, 0);
            mainToolStrip.Name = "mainToolStrip";
            mainToolStrip.RenderMode = ToolStripRenderMode.System;
            mainToolStrip.Size = new Size(1128, 35);
            mainToolStrip.TabIndex = 0;
            // 
            // appNameLabel
            // 
            appNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            appNameLabel.Name = "appNameLabel";
            appNameLabel.Size = new Size(85, 32);
            appNameLabel.Text = "PLANCHA!";
            // 
            // newButton
            // 
            newButton.Name = "newButton";
            newButton.Size = new Size(72, 32);
            newButton.Text = "Nouveau";
            // 
            // modifyButton
            // 
            modifyButton.Name = "modifyButton";
            modifyButton.Size = new Size(70, 32);
            modifyButton.Text = "Modifier";
            // 
            // convertButton
            // 
            convertButton.Name = "convertButton";
            convertButton.Size = new Size(73, 32);
            convertButton.Text = "Convertir";
            // 
            // searchToolBox
            // 
            searchToolBox.Name = "searchToolBox";
            searchToolBox.Size = new Size(114, 35);
            // 
            // searchButton
            // 
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(34, 32);
            searchButton.Text = "🔍";
            // 
            // downloadButton
            // 
            downloadButton.Name = "downloadButton";
            downloadButton.Size = new Size(29, 32);
            downloadButton.Text = "⬇";
            // 
            // shareButton
            // 
            shareButton.Name = "shareButton";
            shareButton.Size = new Size(29, 32);
            shareButton.Text = "↗";
            // 
            // commentButton
            // 
            commentButton.Name = "commentButton";
            commentButton.Size = new Size(34, 32);
            commentButton.Text = "💬";
            // 
            // emailButton
            // 
            emailButton.Name = "emailButton";
            emailButton.Size = new Size(34, 32);
            emailButton.Text = "✉";
            // 
            // rightAlignSeparator
            // 
            rightAlignSeparator.Alignment = ToolStripItemAlignment.Right;
            rightAlignSeparator.Name = "rightAlignSeparator";
            rightAlignSeparator.Size = new Size(6, 35);
            // 
            // avatarButton
            // 
            avatarButton.Alignment = ToolStripItemAlignment.Right;
            avatarButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            avatarButton.DropDownItems.AddRange(new ToolStripItem[] { loginMenuItem, signupMenuItem });
            avatarButton.Font = new Font("Segoe UI", 12F);
            avatarButton.Name = "avatarButton";
            avatarButton.Size = new Size(49, 32);
            avatarButton.Text = "👤";
            avatarButton.ToolTipText = "Account options";
            // 
            // loginMenuItem
            // 
            loginMenuItem.Name = "loginMenuItem";
            loginMenuItem.Size = new Size(168, 32);
            loginMenuItem.Text = "Login";
            // 
            // signupMenuItem
            // 
            signupMenuItem.Name = "signupMenuItem";
            signupMenuItem.Size = new Size(168, 32);
            signupMenuItem.Text = "Sign Up";
            // 
            // spliterSidePlan
            // 
            spliterSidePlan.Dock = DockStyle.Fill;
            spliterSidePlan.Location = new Point(0, 35);
            spliterSidePlan.Margin = new Padding(3, 4, 3, 4);
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
            spliterSidePlan.Size = new Size(1128, 585);
            spliterSidePlan.SplitterDistance = 270;
            spliterSidePlan.SplitterWidth = 5;
            spliterSidePlan.TabIndex = 1;
            spliterSidePlan.TabStop = false;
            // 
            // modeSelector1
            // 
            modeSelector1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            modeSelector1.BackColor = Color.LightGray;
            modeSelector1.Location = new Point(642, 525);
            modeSelector1.Modes.Add("Meublage");
            modeSelector1.Modes.Add("Murage");
            modeSelector1.Name = "modeSelector1";
            modeSelector1.SelectedIndex = 0;
            modeSelector1.Size = new Size(186, 38);
            modeSelector1.TabIndex = 12;
            modeSelector1.Click += modeSelector1_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(629, 37);
            label1.Name = "label1";
            label1.Size = new Size(45, 20);
            label1.TabIndex = 8;
            label1.Text = "utile :";
            // 
            // labelsurface
            // 
            labelsurface.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelsurface.AutoSize = true;
            labelsurface.Location = new Point(629, 19);
            labelsurface.Name = "labelsurface";
            labelsurface.Size = new Size(82, 20);
            labelsurface.TabIndex = 7;
            labelsurface.Text = "Superficie :";
            // 
            // zoombar
            // 
            zoombar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            zoombar.Location = new Point(3, 507);
            zoombar.Name = "zoombar";
            zoombar.Size = new Size(150, 56);
            zoombar.TabIndex = 5;
            // 
            // showgrid
            // 
            showgrid.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            showgrid.Location = new Point(778, 468);
            showgrid.Name = "showgrid";
            showgrid.Size = new Size(50, 51);
            showgrid.TabIndex = 4;
            showgrid.Text = "⊞";
            showgrid.UseVisualStyleBackColor = true;
            // 
            // Redo
            // 
            Redo.Location = new Point(78, 7);
            Redo.Name = "Redo";
            Redo.Size = new Size(50, 51);
            Redo.TabIndex = 2;
            Redo.Text = "↪";
            Redo.UseVisualStyleBackColor = true;
            // 
            // Undo
            // 
            Undo.Location = new Point(3, 7);
            Undo.Name = "Undo";
            Undo.Size = new Size(50, 51);
            Undo.TabIndex = 1;
            Undo.Text = "↩";
            Undo.UseVisualStyleBackColor = true;
            // 
            // toggleButton
            // 
            toggleButton.Anchor = AnchorStyles.Left;
            toggleButton.CausesValidation = false;
            toggleButton.Location = new Point(3, 252);
            toggleButton.Name = "toggleButton";
            toggleButton.Size = new Size(32, 101);
            toggleButton.TabIndex = 0;
            toggleButton.Text = "<";
            toggleButton.UseVisualStyleBackColor = true;
            toggleButton.Click += button1_Click;
            // 
            // MainView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1128, 620);
            Controls.Add(spliterSidePlan);
            Controls.Add(mainToolStrip);
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
            using (LoginView loginDialog = new LoginView()) {
                DialogResult result = loginDialog.ShowDialog(this);
                if (result == DialogResult.OK) {
                    UpdateAvatarForLoggedInUser();
                }
            }
        }
        private void SignupMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Sign up functionality will be implemented here");
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

        //private void zoombar_Scroll(object sender, EventArgs e) {
        //    float valeur = zoombar.Value / 100f;  // ex: trackbar de 10 à 300 → 0.1 à 3.0
        //    ctrg.ChangerZoom(valeur);
        //}

        private void modeSelector1_Click(object sender, EventArgs e) {
            modeControler.SwitchMode();
        }
    }
}