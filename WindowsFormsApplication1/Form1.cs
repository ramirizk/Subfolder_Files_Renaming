using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;

using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private string dirOld;
        private string dirNew;
        private string dirPath;
        private string origPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog( );
            FBD.Description = "Select Subject that has folders to be renamed";

            if (FBD.ShowDialog() == DialogResult.OK)
            {
                
                origPath = FBD.SelectedPath;
                listBox1.Items.Clear();
                string[] files = Directory.GetFiles(FBD.SelectedPath);
                string[] dirs = Directory.GetDirectories(FBD.SelectedPath);
                foreach (string dir in dirs)
                {
                    listBox1.Items.Add(dir);
                }
            } else
            {
                Application.Exit();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            for (int x = listBox1.SelectedIndices.Count - 1; x >= 0; x--)
            {
                dirOld = listBox1.SelectedItems[x].ToString();
                dirPath= Directory.GetParent(dirOld).ToString();
                dirNew = dirPath + @"\" + Path.GetFileName(dirOld.TrimEnd(Path.DirectorySeparatorChar));
                string ReplaceDir = Form1.ShowDialog("What is new name for Folder?", "Changing " + dirNew + " to New",dirNew);
                Directory.CreateDirectory(dirPath + "\\" + ReplaceDir);
                Directory.Move(dirOld + "\\Configuration", dirPath + "\\" + ReplaceDir + "\\Configuration");
                DirectoryInfo hdSearch = new DirectoryInfo(dirOld + @"\Data");
                FileInfo[] filesInDir = hdSearch.GetFiles("*" + Path.GetFileName(dirOld.TrimEnd(Path.DirectorySeparatorChar)) + "*");
                foreach(FileInfo foundFile in filesInDir)
                {
                    string newName = hdSearch.FullName + @"\" + foundFile.Name.Replace(Path.GetFileName(dirOld.TrimEnd(Path.DirectorySeparatorChar)), ReplaceDir);
                    string oldName=foundFile.FullName;
                    File.Move(oldName,newName);
                }
                

                Directory.Move(dirOld + "\\Data", dirPath+ "\\" + ReplaceDir + @"\Data");
                Directory.Delete(dirOld);
                
            }

        }

        public static string ShowDialog(string text, string caption, string dirNew)
        {
            Form prompt = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                AutoSize=true
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, AutoSize = true };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400, Text= Path.GetFileName(dirNew.TrimEnd(Path.DirectorySeparatorChar))};
            Button confirmation = new Button() { Text = "ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";

        }
    


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string[] files = Directory.GetFiles(origPath);
            string[] dirs = Directory.GetDirectories(origPath);
            foreach (string dir in dirs)
            {
                listBox1.Items.Add(dir);
            }
        }
    }
    }

