using System;
using System.Windows.Forms;

namespace CSC578_Project
{
    public partial class MainGamesMenu : Form
    {
        private string selection;
        public MainGamesMenu()
        {
            InitializeComponent();
            PopulateListView();
        }

        private void PopulateListView()
        {
            lvwListGames.Items.Clear();
            var games = AvailableGames.GetAvailableGames();
            for (int i = 0; i < games.Count; i++)
            {
                var item = new ListViewItem(new[] { (i + 1).ToString(), AvailableGames.GameNameWithoutExtension(games[i]), "?" });
                item.Tag = games[i];
                lvwListGames.Items.Add(item);
            }

        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lvwListGames_ItemActivate(object sender, EventArgs e)
        {
            btnPlay.Enabled = true;
            selection = lvwListGames.SelectedItems[0].Tag.ToString();
        }
    }
}
