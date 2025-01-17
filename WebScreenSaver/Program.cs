namespace WebScreenSaver
{
    internal static class Program
    {
        private const string ConfigFilePath = "screensaver_config.txt";
        
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.SetCompatibleTextRenderingDefault(false);

            string savedUrl = LoadUrl();

            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "/s": // Screensaver mode
                        Application.Run(new Form1(savedUrl)); // Replace Form1 with your main form
                        break;
                    case "/c": // Configuration mode
                        using (var configForm = new ConfigForm(savedUrl))
                        {
                            if (configForm.ShowDialog() == DialogResult.OK)
                            {
                                SaveUrl(configForm.ConfiguredUrl);
                            }
                        }
                        break;
                    case "/p": // Preview mode
                        Application.Exit();
                        break;
                    default:
                        Application.Exit();
                        break;
                }
            }
            else
            {
                // Default behavior (optional)
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