namespace WebScreenSaver
{
    internal static class Program
    {
        
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string configFilePath = GetConfigFilePath();
            string savedUrl = LoadUrl(configFilePath);

            if (args.Length == 0 || args[0].ToLower().StartsWith("/c"))
            {
                // Default to configuration mode if no arguments are provided
                using (var configForm = new ConfigForm(savedUrl))
                {
                    if (configForm.ShowDialog() == DialogResult.OK)
                    {
                        SaveUrl(configForm.ConfiguredUrl, configFilePath);
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

        private static string GetConfigFilePath()
        {
            // Get the directory of the .scr file
            string scrDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            return Path.Combine(scrDirectory, "screensaver_config.txt");
        }

        private static string LoadUrl(string ConfigFilePath)
        {
            if (File.Exists(ConfigFilePath))
            {
                return File.ReadAllText(ConfigFilePath).Trim();
            }

            // Default URL if no config file exists
            return "https://google.com";
        }

        private static void SaveUrl(string url, string ConfigFilePath)
        {
            File.WriteAllText(ConfigFilePath, url);
        }
    }
}