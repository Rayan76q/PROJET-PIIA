using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using PROJET_PIIA.Controleurs;
using System.Threading.Tasks.Sources;
using PROJET_PIIA.Model;
using System.Diagnostics;

namespace PROJET_PIIA.View {
    internal class LoginView : Form {
        private System.ComponentModel.IContainer components = null;


        private readonly Color defaultBoxColor;
        private readonly System.Windows.Forms.Timer shakeTimer;
        private readonly int shakeTotalSteps = 10;
        private Point originalBoxLocation;
        private int shakeStep;
        private AccountController accountController;





        public LoginView(AccountController c) {
            InitializeComponent();
            accountController = c;
            AccountController.setValideCredentials();

            defaultBoxColor = boxPanel.BackColor;
            originalBoxLocation = boxPanel.Location;

            shakeTimer = new System.Windows.Forms.Timer { Interval = 20 };
            shakeTimer.Tick += ShakeTimer_Tick;
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

        private void InitializeComponent() {
            panel = new Panel();
            errorText = new Label();
            boxPanel = new Panel();
            txtUsername = new TextBox();
            textPassword = new TextBox();
            label3 = new Label();
            registerButton = new Button();
            label2 = new Label();
            connectButton = new Button();
            label1 = new Label();
            panel.SuspendLayout();
            boxPanel.SuspendLayout();
            SuspendLayout();
            // 
            // panel
            // 
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel.BackColor = SystemColors.ActiveBorder;
            panel.Controls.Add(errorText);
            panel.Controls.Add(boxPanel);
            panel.Controls.Add(label3);
            panel.Controls.Add(registerButton);
            panel.Controls.Add(label2);
            panel.Controls.Add(connectButton);
            panel.Controls.Add(label1);
            panel.Location = new Point(12, 12);
            panel.Name = "panel";
            panel.Size = new Size(823, 552);
            panel.TabIndex = 1;
            panel.Paint += panel2_Paint;
            // 
            // errorText
            // 
            errorText.Anchor = AnchorStyles.None;
            errorText.AutoSize = true;
            errorText.ForeColor = Color.Firebrick;
            errorText.Location = new Point(279, 250);
            errorText.Name = "errorText";
            errorText.Size = new Size(242, 15);
            errorText.TabIndex = 9;
            errorText.Text = "Nom d'utilisateur ou mot de passe incorrect.";
            errorText.Visible = false;
            // 
            // boxPanel
            // 
            boxPanel.Anchor = AnchorStyles.None;
            boxPanel.BackColor = Color.DimGray;
            boxPanel.Controls.Add(txtUsername);
            boxPanel.Controls.Add(textPassword);
            boxPanel.Location = new Point(274, 268);
            boxPanel.Name = "boxPanel";
            boxPanel.Size = new Size(258, 84);
            boxPanel.TabIndex = 8;
            boxPanel.Paint += panel1_Paint;
            // 
            // txtUsername
            // 
            txtUsername.Anchor = AnchorStyles.None;
            txtUsername.Cursor = Cursors.IBeam;
            txtUsername.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic);
            txtUsername.Location = new Point(8, 9);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Username";
            txtUsername.Size = new Size(239, 25);
            txtUsername.TabIndex = 0;
            txtUsername.TextChanged += textBox1_TextChanged_2;
            // 
            // textPassword
            // 
            textPassword.Anchor = AnchorStyles.None;
            textPassword.Cursor = Cursors.IBeam;
            textPassword.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic);
            textPassword.Location = new Point(8, 52);
            textPassword.Name = "textPassword";
            textPassword.PasswordChar = '*';
            textPassword.PlaceholderText = "Password";
            textPassword.Size = new Size(239, 25);
            textPassword.TabIndex = 1;
            textPassword.TextChanged += textPassword_TextChanged;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Location = new Point(274, 474);
            label3.Name = "label3";
            label3.Size = new Size(157, 15);
            label3.TabIndex = 7;
            label3.Text = "Vous n'avez pas de compte ?";
            // 
            // registerButton
            // 
            registerButton.Anchor = AnchorStyles.None;
            registerButton.BackColor = Color.FromArgb(0, 0, 0, 0);
            registerButton.Cursor = Cursors.Hand;
            registerButton.FlatAppearance.BorderSize = 0;
            registerButton.FlatStyle = FlatStyle.Flat;
            registerButton.Font = new Font("Segoe UI", 9F, FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point, 0);
            registerButton.ForeColor = Color.Transparent;
            registerButton.Location = new Point(428, 469);
            registerButton.Name = "registerButton";
            registerButton.Size = new Size(61, 24);
            registerButton.TabIndex = 6;
            registerButton.Text = "S'inscrire";
            registerButton.UseVisualStyleBackColor = false;
            registerButton.Click += registerButton_Click;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(275, 162);
            label2.Name = "label2";
            label2.Size = new Size(194, 40);
            label2.TabIndex = 5;
            label2.Text = "Se connecter";
            // 
            // connectButton
            // 
            connectButton.Anchor = AnchorStyles.None;
            connectButton.BackColor = SystemColors.ActiveCaption;
            connectButton.Cursor = Cursors.Hand;
            connectButton.FlatStyle = FlatStyle.Flat;
            connectButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            connectButton.Location = new Point(275, 422);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(177, 32);
            connectButton.TabIndex = 4;
            connectButton.Text = "Se connecter";
            connectButton.UseVisualStyleBackColor = false;
            connectButton.Click += connectButton_Click_1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Haettenschweiler", 72F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(259, 61);
            label1.Name = "label1";
            label1.Size = new Size(283, 101);
            label1.TabIndex = 2;
            label1.Text = "PlanCha!";
            label1.Click += label1_Click;
            // 
            // LoginView
            // 
            ClientSize = new Size(847, 576);
            Controls.Add(panel);
            Name = "LoginView";
            panel.ResumeLayout(false);
            panel.PerformLayout();
            boxPanel.ResumeLayout(false);
            boxPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e) {

        }

        private void textBox1_TextChanged_2(object sender, EventArgs e) {

        }

        private void panel2_Paint(object sender, PaintEventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void button2_Click(object sender, EventArgs e) {

        }

        private void button2_Click_1(object sender, EventArgs e) {

        }

        private Panel panel;
        private TextBox textPassword;
        private TextBox txtUsername;
        private Label label1;
        private Button connectButton;
        private Label label2;
        private Button registerButton;
        private Panel boxPanel;
        private Label label3;
        private Label errorText;

        private void textPassword_TextChanged(object sender, EventArgs e) {

        }

        private void panel1_Paint(object sender, PaintEventArgs e) {

        }









        private void connectButton_Click_1(object sender, EventArgs e) {
            errorText.Visible = false;
            boxPanel.BackColor = defaultBoxColor;
            boxPanel.Location = originalBoxLocation;

            string username = txtUsername.Text;
            string password = textPassword.Text;

            // Use the AccountController to perform the login instead of the static dictionary
            bool isAuthenticated = accountController.Login(username, password);

            if (isAuthenticated) {
                this.DialogResult = DialogResult.OK;
                this.Close();
            } else {
                errorText.Visible = true;
                boxPanel.BackColor = errorText.ForeColor;
                shakeStep = 0;
                shakeTimer.Start();
            }
        }

        private void ShakeTimer_Tick(object sender, EventArgs e) {
            shakeStep++;
            // amplitude de 5px, oscillation sinusoïdale
            int amplitude = 5;
            double angle = shakeStep * Math.PI * 2 / shakeTotalSteps;
            int offsetX = (int)(amplitude * Math.Sin(angle));

            boxPanel.Location = new Point(originalBoxLocation.X + offsetX, originalBoxLocation.Y);

            if (shakeStep >= shakeTotalSteps) {
                // fin de l’animation → retour à la position d’origine
                shakeTimer.Stop();
                boxPanel.Location = originalBoxLocation;
            }
        }

        private void registerButton_Click(object sender, EventArgs e) {
            this.Hide();
            this.Close();
            if (accountController.ShowSignupDialog()) {
                accountController.UpdateAvatarForLoggedInUser();
            }
        }
    }
}
