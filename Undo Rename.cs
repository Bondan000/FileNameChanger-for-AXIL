using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace TestWinForm
{
    public partial class Undo_Rename : Form
    {
        public Undo_Rename()
        {
            InitializeComponent();
        }

        private string fpath;
        private string dirPath;
        private string[] dirFiles;
        private string fileName = "UndoFile.dat";
        private string[] lines;

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Data Files (.dat)|*.dat";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fpath = openFileDialog1.FileName;
                    textBox1.Text = fpath;
                    FileInfo fn = new FileInfo(fpath);

                    if (fn.Name != fileName)
                    {
                        MessageBox.Show("Selected file '" + fn.Name + "' is not matching!\r\nPlease select the correct file!", "Error!");
                        button2.Enabled = false;
                    }

                    else
                    {
                        if (fn.Length <= 40)
                        {
                            MessageBox.Show("Selected file '" + fn.Name + "' is empty or corrupt!\r\nCan not process undo!", "Error!");
                            button2.Enabled = false;
                        }
                        else
                        {
                            dirPath = Path.GetDirectoryName(fpath);
                            dirFiles = Directory.EnumerateFiles(dirPath, "*.spe").OrderBy(fsort => fsort).ToArray();

                            lines = File.ReadAllLines(fpath);

                            if (lines.Length == dirFiles.Length)
                            {
                                button2.Enabled = true;
                            }
                            else
                            {
                                button2.Enabled = false;
                                MessageBox.Show("The elements in the file '" + fn.Name + "' is not matching with the number of '.spe' files in the folder!\r\nPlease check the '" + fn.Name + "'!", "Error!");
                            }
                        }
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool errIni = false;
            int count = 0;
            var tbl = lines.Select(a => new { FstColmn = a.Split('\t').First(), SecColmn = a.Split('\t')[1] });
            foreach (var value in tbl)
            {
                if (dirFiles[count] == value.SecColmn)
                {
                    File.Move(value.SecColmn, value.FstColmn);
                    count += 1;
                }
                else
	            {
                    MessageBox.Show("Files already renamed!", "Info");
                    errIni = true;
                    break;
                }
            }

            if (errIni==false)
            {
                MessageBox.Show("Files renamed!", "Info");
            } 
            button2.Enabled = false;
            System.Diagnostics.Process.Start("explorer.exe", dirPath);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
