using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestWinForm
{
    public partial class File_Name_Changer : Form
    {
        public File_Name_Changer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rename_Files rnform = new Rename_Files();
            this.Hide();
            rnform.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Undo_Rename udform = new Undo_Rename();
            this.Hide();
            udform.ShowDialog();
            this.Show();
        }

        private void File_Name_Changer_Load(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 abform = new AboutBox1();
            this.Show();
            abform.ShowDialog();
            this.Show();
        }
    }
}
