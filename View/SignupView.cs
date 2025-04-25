using System;
using System.Drawing;
using System.Windows.Forms;
using PROJET_PIIA.Controleurs;

namespace PROJET_PIIA.View {
    public class SignupView : Form {
        private System.ComponentModel.IContainer components = null;
        private Panel panel;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Label labelTitle;
        private Button registerButton;
        private Label labelSubtitle;
        private Button loginLinkButton;
        private Panel boxPanel;
        private Label errorText;
        private AccountController accountController;

        private readonly Color defaultBoxColor;
        private readonly System.Windows.Forms.Timer shakeTimer;
        private readonly int shakeTotalSteps = 10;
        private Point originalBoxLocation;
        private int shakeStep;

        public SignupView(AccountController controller) {
            InitializeComponent();
            accountController = controller;

            defaultBoxColor = boxPanel.BackColor;
            originalBoxLocation = boxPanel.Location;

            shakeTimer = new System.Windows.Forms.Timer { Interval = 20 };
            shakeTimer.Tick += ShakeTimer_Tick;
        }

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            panel = new Panel();
            errorText = new Label();
            boxPanel = new Panel();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            labelSubtitle = new Label();
            loginLinkButton = new Button();
            labelTitle = new Label();
            registerButton = new Button();

            panel.SuspendLayout();
            boxPanel.SuspendLayout();
            SuspendLayout();

            // panel
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel.BackColor = SystemColors.ActiveBorder;
            panel.Controls.Add(errorText);
            panel.Controls.Add(boxPanel);
            panel.Controls.Add(labelSubtitle);
            panel.Controls.Add(loginLinkButton);
            panel.Controls.Add(labelTitle);
            panel.Controls.Add(registerButton);
            panel.Location = new Point(12, 12);
            panel.Name = "panel";
            panel.Size = new Size(823, 552);
            panel.TabIndex = 1;

            // errorText
            errorText.Anchor = AnchorStyles.None;
            errorText.AutoSize = true;
            errorText.ForeColor = Color.Firebrick;
            errorText.Location = new Point(279, 250);
            errorText.Name = "errorText";
            errorText.Size = new Size(242, 15);
            errorText.TabIndex = 9;
            errorText.Text = "Veuillez remplir tous les champs correctement.";
            errorText.Visible = false;

            // boxPanel
            boxPanel.Anchor = AnchorStyles.None;
            boxPanel.BackColor = Color.DimGray;
            boxPanel.Controls.Add(txtUsername);
            boxPanel.Controls.Add(txtPassword);
            boxPanel.Controls.Add(txtConfirmPassword);
            boxPanel.Location = new Point(274, 268);
            boxPanel.Name = "boxPanel";
            boxPanel.Size = new Size(258, 120);
            boxPanel.TabIndex = 8;

            // txtUsername
            txtUsername.Anchor = AnchorStyles.None;
            txtUsername.Cursor = Cursors.IBeam;
            txtUsername.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic);
            txtUsername.Location = new Point(8, 10);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Nom d'utilisateur";
            txtUsername.Size = new Size(239, 25);
            txtUsername.TabIndex = 0;

            // txtPassword
            txtPassword.Anchor = AnchorStyles.None;
            txtPassword.Cursor = Cursors.IBeam;
            txtPassword.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic);
            txtPassword.Location = new Point(8, 50);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.PlaceholderText = "Mot de passe";
            txtPassword.Size = new Size(239, 25);
            txtPassword.TabIndex = 1;

            // txtConfirmPassword
            txtConfirmPassword.Anchor = AnchorStyles.None;
            txtConfirmPassword.Cursor = Cursors.IBeam;
            txtConfirmPassword.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic);
            txtConfirmPassword.Location = new Point(8, 90);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '*';
            txtConfirmPassword.PlaceholderText = "Confirmez le mot de passe";
            txtConfirmPassword.Size = new Size(239, 25);
            txtConfirmPassword.TabIndex = 2;

            // labelSubtitle
            labelSubtitle.Anchor = AnchorStyles.None;
            labelSubtitle.AutoSize = true;
            labelSubtitle.Location = new Point(274, 474);
            labelSubtitle.Name = "labelSubtitle";
            labelSubtitle.Size = new Size(142, 15);
            labelSubtitle.TabIndex = 7;
            labelSubtitle.Text = "Vous avez déjà un compte ?";

            // loginLinkButton
            loginLinkButton.Anchor = AnchorStyles.None;
            loginLinkButton.BackColor = Color.FromArgb(0, 0, 0, 0);
            loginLinkButton.Cursor = Cursors.Hand;
            loginLinkButton.FlatAppearance.BorderSize = 0;
            loginLinkButton.FlatStyle = FlatStyle.Flat;
            loginLinkButton.Font = new Font("Segoe UI", 9F, FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point, 0);
            loginLinkButton.Location = new Point(416, 469);
            loginLinkButton.Name = "loginLinkButton";
            loginLinkButton.Size = new Size(80, 24);
            loginLinkButton.TabIndex = 6;
            loginLinkButton.Text = "Se connecter";
            loginLinkButton.UseVisualStyleBackColor = false;
            loginLinkButton.Click += loginLinkButton_Click;

            // labelTitle
            labelTitle.Anchor = AnchorStyles.None;
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTitle.Location = new Point(275, 162);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(165, 40);
            labelTitle.TabIndex = 5;
            labelTitle.Text = "S'inscrire";

            // registerButton
            registerButton.Anchor = AnchorStyles.None;
            registerButton.BackColor = SystemColors.ActiveCaption;
            registerButton.Cursor = Cursors.Hand;
            registerButton.FlatStyle = FlatStyle.Flat;
            registerButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            registerButton.Location = new Point(275, 420);
            registerButton.Name = "registerButton";
            registerButton.Size = new Size(177, 32);
            registerButton.TabIndex = 4;
            registerButton.Text = "S'inscrire";
            registerButton.UseVisualStyleBackColor = false;
            registerButton.Click += registerButton_Click;

            // SignupView
            ClientSize = new Size(847, 576);
            Controls.Add(panel);
            Name = "SignupView";
            Text = "PlanCha! - Inscription";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            boxPanel.ResumeLayout(false);
            boxPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void registerButton_Click(object sender, EventArgs e) {
            errorText.Visible = false;
            boxPanel.BackColor = defaultBoxColor;
            boxPanel.Location = originalBoxLocation;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (accountController.RegisterAccount(username, password, confirmPassword)) {
                this.DialogResult = DialogResult.OK;
                AccountController.setValideCredentials();  //refresh for new acocunts
                this.Close();
            } else {
                errorText.Visible = true;
                boxPanel.BackColor = errorText.ForeColor;
                shakeStep = 0;
                shakeTimer.Start();
            }
        }

        private void loginLinkButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

            // Open login dialog
            using (LoginView loginDialog = new LoginView(accountController)) {
                loginDialog.ShowDialog(this.Owner);
            }
        }

        private void ShakeTimer_Tick(object sender, EventArgs e) {
            shakeStep++;
            int amplitude = 5;
            double angle = shakeStep * Math.PI * 2 / shakeTotalSteps;
            int offsetX = (int)(amplitude * Math.Sin(angle));

            boxPanel.Location = new Point(originalBoxLocation.X + offsetX, originalBoxLocation.Y);

            if (shakeStep >= shakeTotalSteps) {
                shakeTimer.Stop();
                boxPanel.Location = originalBoxLocation;
            }
        }
    }
}