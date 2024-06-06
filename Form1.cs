using System.Diagnostics;

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

            Button btnStartSetup = new()
            {
                Text = "Start Setup Process",
                Location = new Point(0, 25),
                Width = 250
            };
            btnStartSetup.Click += BtnStartSetupClick!;
            this.Controls.Add(btnStartSetup);
        }

        private void BtnStartSetupClick(object sender, EventArgs e)
        {
            GitSetup();
        }

        private static void GitSetup()
        {
            using var folderBrowserDialog = new FolderBrowserDialog();
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

                    StartProcess(processGitInit);

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

                    StartProcess(processGitAddAll);

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

                    StartProcess(processGitCommit);

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

                    StartProcess(processGitBranch);

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

                    StartProcess(processGitRemote);

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

                    StartProcess(processGitPush);

                }
            }
        }


        public class PromptDialog : Form
        {
            private readonly Label label;
            private readonly TextBox textBox;
            private readonly Button okButton;
            private readonly Button cancelButton;

            public string? URL { get; private set; }

            public PromptDialog(string title, string labelText)
            {
                this.Text = title;
                this.ClientSize = new System.Drawing.Size(300, 120);
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                label = new Label
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(15, 20),
                    Text = labelText
                };

                textBox = new TextBox
                {
                    Location = new System.Drawing.Point(15, 40),
                    Size = new System.Drawing.Size(270, 20)
                };

                okButton = new Button
                {
                    DialogResult = DialogResult.OK,
                    Location = new System.Drawing.Point(130, 70),
                    Text = "OK"
                };
                okButton.Click += OkButton_Click!;

                cancelButton = new Button
                {
                    DialogResult = DialogResult.Cancel,
                    Location = new System.Drawing.Point(210, 70),
                    Text = "Cancel"
                };

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

        private static string? PromptForURL()
        {
            var prompt = new PromptDialog("Enter Git Remote URL", "Please enter the Git remote URL:");
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                return prompt.URL;
            }
            return null;
        }

        private static void StartProcess(ProcessStartInfo processInfo)
        {
            using var process = new Process();
            process.StartInfo = processInfo;
            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            MessageBox.Show(output);
            var error = process.StandardError.ReadToEnd();
            MessageBox.Show(error);
        }
    }
}
