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
    public partial class Menu : Form
    {
        public string Selection { get; set; }

        public Menu()
        {
            InitializeComponent();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Selection = "restart";
            Close();
        }

        private void btnQuitToDesktop_Click(object sender, EventArgs e)
        {
            Selection = "desktop";
            Close();
        }

        private void btnToMenu_Click(object sender, EventArgs e)
        {
            Selection = "menu";
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Selection = "cancel";
            Close();
        }
    }
}
