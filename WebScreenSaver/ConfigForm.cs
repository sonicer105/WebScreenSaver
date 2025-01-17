using System;
using System.Windows.Forms;

namespace WebScreenSaver
{
    public partial class ConfigForm : Form
    {
        public string ConfiguredUrl { get; private set; }

        public ConfigForm(string currentUrl)
        {
            InitializeComponent();
            // Set the TextBox to the current URL
            urlTextBox.Text = currentUrl;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            ConfiguredUrl = urlTextBox.Text.Trim();
            if (Uri.IsWellFormedUriString(ConfiguredUrl, UriKind.Absolute))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid URL.", "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
