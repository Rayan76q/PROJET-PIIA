using PROJET_PIIA.Controleurs;
using PROJET_PIIA.Model;

namespace PROJET_PIIA.View {
    public partial class MeubleSidePanel : UserControl {

        ControleurSidePanelMeuble ctr;
        public MeubleSidePanel(ControleurSidePanelMeuble ctr) {
            this.ctr = ctr;
            InitializeComponent();
            InitializeMeuble();
            CreateFilterPanel();
            this.Invalidate();
        }

        private void InitializeMeuble() {
            searchBox.TextChanged += (sender, e) => update_meubles();
            filterButton.Click += filterButton_Click;

            update_meubles();


            flowLayoutPanel1.SizeChanged += (sender, e) => {
                int scrollFix = SystemInformation.VerticalScrollBarWidth + 5;
                foreach (Control ctrl in flowLayoutPanel1.Controls) {
                    ctrl.Width = flowLayoutPanel1.ClientSize.Width - ctrl.Margin.Left - ctrl.Margin.Right - scrollFix;
                }
            };
        }

        private void update_meubles() {
            foreach (var m in ctr.getMeubles()) {
                AddMeubleToPanel(m);
            }
            Invalidate();
        }


        private void AddMeubleToPanel(Meuble m) {
            string meubleName = m.Nom;

            Panel meublePanel = new Panel {
                Width = flowLayoutPanel1.Width,
                Height = 90,
                Margin = new Padding(2, 5, 10, 0),
                BackColor = Color.WhiteSmoke,
            };

            Button meubleButton = new Button {
                Text = "🪑", // icône par défaut
                Width = 60,
                Height = 60,
                Location = new Point(10, 5),
                FlatStyle = FlatStyle.Flat,
                Tag = m
            };
            meubleButton.MouseDown += (sender, e) => {
                if (e.Button == MouseButtons.Left) {
                    meubleButton.DoDragDrop(m, DragDropEffects.Copy);
                }
            };

            Label nameLabel = new Label {
                Text = meubleName,
                Location = new Point(80, 10),
                AutoSize = true,
            };

            Button starButton = new Button {
                Text = ctr.IsFavorite(m) ? "★" : "☆",
                Width = 30,
                Height = 30,
                Location = new Point(160, 50),
                FlatStyle = FlatStyle.Flat,
                Tag = m
            };
            starButton.Click += ToggleFavorite_Click;

            meublePanel.Controls.Add(meubleButton);
            meublePanel.Controls.Add(nameLabel);
            meublePanel.Controls.Add(starButton);

            flowLayoutPanel1.Controls.Add(meublePanel);
        }

        private void ToggleFavorite_Click(object sender, EventArgs e) {
            Button btn = (Button)sender;
            Meuble m = (Meuble)btn.Tag;
            ctr.SwitchFavorite(m);
            btn.Text = ctr.IsFavorite(m) ? "★" : "☆";
        }

        private void filterButton_Click(object sender, EventArgs e) {
            ctr.CollapseFilterSelection();
            this.splitfilterpanel.Panel2Collapsed = ctr.filterSelectionColapsed;
            if (ctr.filterSelectionColapsed) {
                this.splitfilterpanel.SplitterDistance = 45;
            } else {
                //this.splitfilterpanel.SplitterDistance = 300;
            }
        }


        ///

        private void CreateFilterPanel() {

        }

        // Mise à jour des tags sélectionnés
        private void RefreshSelectedTagsPanel() {
            // Logique pour afficher les tags sélectionnés
        }

        // Mise à jour des tags disponibles
        private void RefreshAvailableTagsPanel() {
            // Logique pour afficher les tags disponibles
        }


    }
}
