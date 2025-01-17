using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.Diagnostics;

namespace WebScreenSaver
{
    public partial class Form1 : Form
    {
        private bool _canExit = false; // Tracks if input should trigger exit
        private System.Windows.Forms.Timer _debounceTimer; // Timer for debounce period
        private string _url;

        public Form1(string url)
        {
            _url = url;
            InitializeComponent();
            InitializeWebView();
            InitializeDebounceTimer();
        }

        private async void InitializeWebView()
        {
            // Create WebView2 control
            var webView = new WebView2
            {
                Dock = DockStyle.Fill
            };

            // Add WebView2 to the form
            Controls.Add(webView);

            // Configure the form for full-screen
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            //this.WindowState = FormWindowState.Normal;
            //this.FormBorderStyle = FormBorderStyle.Sizable;
            //this.TopMost = false;


            // Initialize WebView2 and inject JavaScript
            await webView.EnsureCoreWebView2Async();

            // Enable DevTools
            //webView.CoreWebView2.OpenDevToolsWindow();

            // Set up WebMessageReceived event
            webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            // Inject JavaScript after the DOM is fully loaded
            webView.CoreWebView2.DOMContentLoaded += async (s, e) =>
            {
                Debug.WriteLine("DOM content loaded. Injecting JavaScript...");

                string jsScript = @"
                    document.addEventListener('mousemove', () => {
                        console.log('Mouse moved'); // Debugging log
                        window.chrome.webview.postMessage('mouse');
                    });

                    document.addEventListener('keydown', () => {
                        console.log('Key pressed'); // Debugging log
                        window.chrome.webview.postMessage('key');
                    });
                ";
                await webView.CoreWebView2.ExecuteScriptAsync(jsScript);
                Debug.WriteLine("JavaScript injected successfully.");
            };

            // Set the website to display
            webView.Source = new Uri(_url);
        }

        private void InitializeDebounceTimer()
        {
            // Set up a timer to enable input detection after a delay
            _debounceTimer = new System.Windows.Forms.Timer();
            _debounceTimer.Interval = 3000; // 3 seconds debounce
            _debounceTimer.Tick += (s, e) =>
            {
                Debug.WriteLine("Debounce timer expired. Input will now trigger exit.");
                _canExit = true; // Allow exit after the timer completes
                _debounceTimer.Stop(); // Stop the timer
            };
            Debug.WriteLine("Debounce timer started.");
            _debounceTimer.Start();
        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var message = e.TryGetWebMessageAsString();
            Debug.WriteLine($"Received message from WebView: {message}");

            // Handle messages from JavaScript
            if (_canExit && (message == "mouse" || message == "key"))
            {
                Debug.WriteLine("Input detected and debounce period expired. Closing form.");
                this.Close(); // Close the form when input is detected
            }
            else if (!_canExit)
            {
                Debug.WriteLine("Input detected, but debounce period has not expired.");
            }
        }
    }
}
