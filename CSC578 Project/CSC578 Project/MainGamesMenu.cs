using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSC578_Project
{
    public partial class MainGamesMenu : Form
    {    
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
                item.Tag = games[0];
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
        }
    }
}
