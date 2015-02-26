using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace RarFiles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Process1 = new Process {StartInfo = {FileName = "Winrar.exe", CreateNoWindow = true}};
        }

        public Process Process1 { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                var di = new DirectoryInfo(textBox1.Text);
                if (di.Exists)
                {
                    TraverseFolders(di);
                }
            }
        }

        private void TraverseFolders(DirectoryInfo di)
        {
            var files = di.GetFiles();
            foreach (var fileInfo in files)
            {
                if (fileInfo.Extension != ".rar")
                {
                    var filepath = fileInfo.FullName;
                    var zippath = filepath.Substring(0, filepath.LastIndexOf('.')) + ".rar";
                    
                    Process1.StartInfo.Arguments = "m -ep -plqhgsllj " + zippath + " " + filepath;
                    Process1.Start();

                    while (true)
                    {
                        if (Process1.HasExited)
                        {
                            break;
                        }
                        Thread.Sleep(5000);
                    }
                }
            }

            var dirs = di.GetDirectories();
            foreach (var directoryInfo in dirs)
            {
                TraverseFolders(directoryInfo);
            }
        }
    }
}
