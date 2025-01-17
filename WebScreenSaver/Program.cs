namespace WebScreenSaver
{
    internal static class Program
    {
        private const string ConfigFilePath = "screensaver_config.txt";
        
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string savedUrl = LoadUrl();

            if (args.Length == 0 || args[0].ToLower() == "/c")
            {
                // Default to configuration mode if no arguments are provided
                using (var configForm = new ConfigForm(savedUrl))
                {
                    if (configForm.ShowDialog() == DialogResult.OK)
                    {
                        SaveUrl(configForm.ConfiguredUrl);
                    }
                }
            }
            else if (args[0].ToLower() == "/s")
            {
                // Screensaver mode
                Application.Run(new Form1(savedUrl));
            }
            else if (args[0].ToLower().StartsWith("/p"))
            {
                // Preview mode (not fully implemented)
            }
            else
            {
                // Fallback for unknown arguments
                Application.Run(new Form1(savedUrl));
            }
        }

        private static string LoadUrl()
        {
            if (File.Exists(ConfigFilePath))
            {
                return File.ReadAllText(ConfigFilePath).Trim();
            }

            // Default URL if no config file exists
            return "https://google.com";
        }

        private static void SaveUrl(string url)
        {
            File.WriteAllText(ConfigFilePath, url);
        }
    }
}