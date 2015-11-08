using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CSC578_Project
{
    public partial class MainGamesMenu : Form
    {
        private GamePackage gameSelection;
        public MainGamesMenu()
        {
            InitializeComponent();
            PopulateListView();
        }

        private void OpenQuitDialogBox()
        {
            var dialogResult = MessageBox.Show("Are you sure you wish to exit?", "Quit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void PopulateListView()
        {
            lvwListGames.Items.Clear();
            List<GamePackage> games = AvailableGames.GetAvailableGames();
            for (int i = 0; i < games.Count; i++)
            {
                var item = new ListViewItem(new[] { (i + 1).ToString(), games[i].Name, "?" });
                item.Tag = games[i];
                lvwListGames.Items.Add(item);
            }

            if (games.Count == 1)
                lblGamesAvailable.Text = "1 Game Available";
            else
                lblGamesAvailable.Text = games.Count + " Games Available";
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            OpenQuitDialogBox();
        }

        private void lvwListGames_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lvwListGames.SelectedItems.Count == 1)
            {
                btnPlay.Enabled = true;
                gameSelection = (GamePackage)lvwListGames.SelectedItems[0].Tag;
            }
            else
            {
                btnPlay.Enabled = false;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
        }

        private void MainGamesMenu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                OpenQuitDialogBox();
            }
        }
    }
}
