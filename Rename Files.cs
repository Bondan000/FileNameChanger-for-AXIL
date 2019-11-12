using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace TestWinForm
{
    public partial class Rename_Files : Form
    {
        public Rename_Files()
        {
            InitializeComponent();
        }

        private string fpath;
        private string orgfilenames;
        private string[] orgfnPath;
        private string nName;
        private string fileName = "\\UndoFile.dat";
        private string batchfileName = "\\axil.txt";
        private string fitNo = null;
        private bool rninitiated = false;
        
        private void button1_Click(object sender, EventArgs e)
        {
            if(fbd1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fpath = fbd1.SelectedPath;
                    folderpath1.Text = fbd1.SelectedPath;

                    orgfilenames = string.Join("\r\n", Directory.EnumerateFiles(fpath, "*.spe").OrderBy(fsort => fsort).Select(p => Path.GetFileName(p)));
                    textBox1.Text = orgfilenames;
                    orgfnPath = Directory.EnumerateFiles(fpath, "*.spe").OrderBy(fsort=>fsort).ToArray();
                    textBox3.Text = orgfnPath.Length.ToString();

                    if (orgfnPath.Length != 0)
                    {
                        textBox2.Enabled = true;
                        if (File.Exists(fpath + fileName))
                        {
                            string[] lines = File.ReadAllLines(fpath + fileName);
                            var tbl = lines.Select(a => new { SecColmn = a.Split('\t')[1] });

                            int count = 0;

                            foreach (var value in tbl)
                            {
                                if (orgfnPath[count] == value.SecColmn)
                                {
                                    MessageBox.Show("Files already renamed!", "Warning");
                                    textBox2.Enabled = false;
                                    break;
                                }
                                count += 1;
                            }  
                        }

                        rninitiated = false;
                        textBox2.Clear();
                        textBox4.Clear();
                        textBox4.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No '.spe' files found in this folder", "Error!");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message,"Error!");  
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ScrollBars = ScrollBars.Vertical;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (StreamWriter outputFile = new StreamWriter(fpath + fileName))
            {
                for (int i = 0; i <= orgfnPath.Length - 1; i++)
                {
                    File.Move(orgfnPath[i], @fpath + "\\"+ nName + i.ToString().PadLeft(4,'0') + ".spe");
                    outputFile.WriteLine(orgfnPath[i] + "\t" + @fpath + "\\" + nName + i.ToString().PadLeft(4,'0') + ".spe");
                }
            }

            //textBox2.Clear();
            textBox2.Enabled = false;
            button2.Enabled = false;
            MessageBox.Show("Process complete!", "Info");
            System.Diagnostics.Process.Start("notepad.exe", @fpath + fileName);
            rninitiated = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = textBox2.Text != string.Empty;
            nName = textBox2.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (StreamWriter outputFile = new StreamWriter(fpath + batchfileName))
            {
                if (rninitiated == true)
                {
                    for (int i = 0; i <= orgfnPath.Length - 1; i++)
                    {
                        outputFile.WriteLine("LOAD SPEC=" + nName + i.ToString().PadLeft(4,'0') + "\r\n" + "FIT N=" + fitNo + "\r\n" + "REPORT FULL SAVE");
                    }
                }
                else
                {
                    for (int i = 0; i <= orgfnPath.Length - 1; i++)
                    {
                        outputFile.WriteLine("LOAD SPEC="+ Path.GetFileNameWithoutExtension(orgfnPath[i]) + "\r\n" + "FIT N="+ fitNo + "\r\n" + "REPORT FULL SAVE");
                    }
                }   
            }

            //textBox4.Clear();
            textBox4.Enabled = false;
            button4.Enabled = false;
            fitNo = null;
            MessageBox.Show("Batch file created!", "Info");
            System.Diagnostics.Process.Start("notepad.exe", @fpath + batchfileName);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            button4.Enabled = textBox4.Text != string.Empty;
            fitNo = textBox4.Text;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char inp = e.KeyChar;
            if (!Char.IsDigit(inp) && inp != 8 && inp != 46)
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char nNinp = e.KeyChar;
            if (nNinp == 32 || nNinp == 34 || nNinp == 42 || nNinp == 46 || nNinp == 47 || nNinp == 58 || nNinp == 60 || nNinp == 62 || nNinp == 63 || nNinp == 124)
            {
                e.Handled = true;
            }
        }
    }
}
