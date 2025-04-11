using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJET_PIIA.View {
    internal class LoginView : Form {
        private System.ComponentModel.IContainer components = null;

        public LoginView() {
            InitializeComponent();
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
            panel2 = new Panel();
            txtUsername = new TextBox();
            textPassword = new TextBox();
            label1 = new Label();
            button1 = new Button();
            button2 = new Button();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = SystemColors.ActiveBorder;
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button1);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(textPassword);
            panel2.Controls.Add(txtUsername);
            panel2.Location = new Point(12, 12);
            panel2.Name = "panel2";
            panel2.Size = new Size(637, 583);
            panel2.TabIndex = 1;
            panel2.Paint += panel2_Paint;
            // 
            // txtUsername
            // 
            txtUsername.Anchor = AnchorStyles.None;
            txtUsername.Cursor = Cursors.IBeam;
            txtUsername.Font = new Font("Castellar", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(182, 279);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Username";
            txtUsername.Size = new Size(239, 22);
            txtUsername.TabIndex = 0;
            txtUsername.TextChanged += textBox1_TextChanged_2;
            // 
            // textPassword
            // 
            textPassword.Anchor = AnchorStyles.None;
            textPassword.Cursor = Cursors.IBeam;
            textPassword.Font = new Font("Castellar", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            textPassword.Location = new Point(182, 340);
            textPassword.Name = "textPassword";
            textPassword.PasswordChar = '*';
            textPassword.PlaceholderText = "Password";
            textPassword.Size = new Size(239, 22);
            textPassword.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Castellar", 48F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(172, 147);
            label1.Name = "label1";
            label1.Size = new Size(263, 77);
            label1.TabIndex = 2;
            label1.Text = "Login";
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.None;
            button1.BackColor = Color.FromArgb(0, 0, 0, 0);
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Italic | FontStyle.Underline, GraphicsUnit.Point, 0);
            button1.Location = new Point(195, 368);
            button1.Name = "button1";
            button1.Size = new Size(121, 24);
            button1.TabIndex = 3;
            button1.Text = "Mot de passe oublié";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.None;
            button2.BackColor = SystemColors.ActiveCaption;
            button2.Cursor = Cursors.Hand;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Castellar", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            button2.Location = new Point(182, 437);
            button2.Name = "button2";
            button2.Size = new Size(177, 32);
            button2.TabIndex = 4;
            button2.Text = "Se connecter";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click_1;
            // 
            // LoginView
            // 
            ClientSize = new Size(661, 607);
            Controls.Add(panel2);
            Name = "LoginView";
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
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

        private Panel panel2;
        private TextBox textPassword;
        private TextBox txtUsername;
        private Label label1;
        private Button button2;
        private Button button1;
    }
}
