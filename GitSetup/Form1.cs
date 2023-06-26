using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;

namespace GitSetup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "GitSetup";

            Button btnStartSetup = new Button();
            btnStartSetup.Text = "Start Setup Process";
            btnStartSetup.Location = new Point(0, 25);
            btnStartSetup.Width = 250;
            btnStartSetup.Click += btnStartSetupClick;
            this.Controls.Add(btnStartSetup);
        }

        private void btnStartSetupClick(object sender, EventArgs e)
        {
            GitSetup();
        }

        private void GitSetup()
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                var url = PromptForURL();
                if (url != null)
                {
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        var workingDirectory = folderBrowserDialog.SelectedPath;

                        var processGitInit = new ProcessStartInfo // git init
                        {
                            FileName = "git",
                            Arguments = "init",
                            WorkingDirectory = workingDirectory,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        startProcess(processGitInit);

                        var processGitAddAll = new ProcessStartInfo     // git add -A
                        {
                            FileName = "git",
                            Arguments = "add -A",
                            WorkingDirectory = workingDirectory,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        startProcess(processGitAddAll);

                        var processGitCommit = new ProcessStartInfo     // git commit
                        {
                            FileName = "git",
                            Arguments = "commit -m \"" + "Project init" + "\"",
                            WorkingDirectory = workingDirectory,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        startProcess(processGitCommit);

                        var processGitBranch = new ProcessStartInfo     // git branch to main
                        {
                            FileName = "git",
                            Arguments = "branch -M main",
                            WorkingDirectory = workingDirectory,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        startProcess(processGitBranch);

                        var processGitRemote = new ProcessStartInfo // git remote add
                        {
                            FileName = "git",
                            Arguments = $"remote add origin {url}",
                            WorkingDirectory = workingDirectory,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        startProcess(processGitRemote);

                        var processGitPush = new ProcessStartInfo     // git push
                        {
                            FileName = "git",
                            Arguments = "push -u origin main",
                            WorkingDirectory = workingDirectory,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        startProcess(processGitPush);

                    }
                }
            }
        }
        

        public class PromptDialog : Form
        {
            private Label label;
            private TextBox textBox;
            private Button okButton;
            private Button cancelButton;

            public string URL { get; private set; }

            public PromptDialog(string title, string labelText)
            {
                this.Text = title;
                this.ClientSize = new System.Drawing.Size(300, 120);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                label = new Label();
                label.AutoSize = true;
                label.Location = new System.Drawing.Point(15, 20);
                label.Text = labelText;

                textBox = new TextBox();
                textBox.Location = new System.Drawing.Point(15, 40);
                textBox.Size = new System.Drawing.Size(270, 20);

                okButton = new Button();
                okButton.DialogResult = DialogResult.OK;
                okButton.Location = new System.Drawing.Point(130, 70);
                okButton.Text = "OK";
                okButton.Click += OkButton_Click;

                cancelButton = new Button();
                cancelButton.DialogResult = DialogResult.Cancel;
                cancelButton.Location = new System.Drawing.Point(210, 70);
                cancelButton.Text = "Cancel";

                this.Controls.Add(label);
                this.Controls.Add(textBox);
                this.Controls.Add(okButton);
                this.Controls.Add(cancelButton);
            }

            private void OkButton_Click(object sender, EventArgs e)
            {
                URL = textBox.Text;
            }
        }

        private string PromptForURL()
        {
            var prompt = new PromptDialog("Enter Git Remote URL", "Please enter the Git remote URL:");
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                return prompt.URL;
            }
            return null;
        }

        private void startProcess(ProcessStartInfo processInfo)
        {
            using (var process = new Process())
            {
                process.StartInfo = processInfo;
                process.Start();

                var output = process.StandardOutput.ReadToEnd();
                MessageBox.Show(output);
                var error = process.StandardError.ReadToEnd();
                MessageBox.Show(error);
            }
        }
    }
}
