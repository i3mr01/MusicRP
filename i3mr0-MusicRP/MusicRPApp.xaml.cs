using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.VisualBasic;

namespace i3mr0_MusicRP {
    /// <summary>
    /// Interaction logic for MusicRPApp.xaml
    /// </summary>
    public partial class MusicRPApp : Application {

        [STAThread]
        public static void Main() {
            try {
                // Create crash log file
                var crashLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "i3mr0-MusicRP-CrashLog.txt");
                File.WriteAllText(crashLogPath, $"i3mr0 MusicRP Crash Log - {DateTime.Now}\n");
                File.AppendAllText(crashLogPath, "Starting application...\n");
                
                var app = new MusicRPApp();
                File.AppendAllText(crashLogPath, "App created successfully\n");
                
                app.InitializeComponent();
                File.AppendAllText(crashLogPath, "InitializeComponent completed\n");
                
                app.Run();
                File.AppendAllText(crashLogPath, "App.Run completed\n");
            } catch (Exception ex) {
                var crashLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "i3mr0-MusicRP-CrashLog.txt");
                File.AppendAllText(crashLogPath, $"CRASH: {ex.Message}\n");
                File.AppendAllText(crashLogPath, $"Stack Trace: {ex.StackTrace}\n");
                File.AppendAllText(crashLogPath, $"Inner Exception: {ex.InnerException?.Message}\n");
                
                MessageBox.Show($"Application failed to start!\n\nError: {ex.Message}\n\nFull details saved to:\n{crashLogPath}", 
                    "i3mr0 MusicRP - Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private TaskbarIcon? taskbarIcon;
        private MusicClientScraper amScraper;
        private MusicDiscordClient discordClient;
        private MusicLastFmScrobbler? lastFmScrobblerClient;
        private MusicListenBrainzScrobbler? listenBrainzScrobblerClient;
        private Logger? logger;

        public LastFmCredentials lastFmCredentials {
            get {
                var creds = new LastFmCredentials();
                creds.apiKey = i3mr0_MusicRP.Properties.Settings.Default.LastfmAPIKey;
                creds.apiSecret = i3mr0_MusicRP.Properties.Settings.Default.LastfmSecret;
                creds.username = i3mr0_MusicRP.Properties.Settings.Default.LastfmUsername;
                creds.password = MusicRPSettings.GetLastFMPassword();
                return creds;
            }
        }

        public ListenBrainzCredentials listenBrainzCredentials {
            get {
                var creds = new ListenBrainzCredentials();
                creds.userToken = i3mr0_MusicRP.Properties.Settings.Default.ListenBrainzUserToken;
                return creds;
            }
        }

        public MusicRPApp() {
            var crashLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "i3mr0-MusicRP-CrashLog.txt");
            
            try {
                File.AppendAllText(crashLogPath, "Constructor started\n");
                
                // make logger
                try {
                    File.AppendAllText(crashLogPath, "Creating logger...\n");
                    logger = new Logger();
                    logger.Log("Application started");
                    File.AppendAllText(crashLogPath, "Logger created successfully\n");
                } catch (Exception ex) {
                    File.AppendAllText(crashLogPath, $"Logger failed: {ex.Message}\n");
                    logger?.Log($"Logger failed: {ex.Message}");
                    logger = null;
                }

                // try to auto detect region
                try {
                    File.AppendAllText(crashLogPath, "Auto-detecting region...\n");
                    if (i3mr0_MusicRP.Properties.Settings.Default.AppleMusicRegion == "") {
                        var region = RegionInfo.CurrentRegion.Name.ToLower();
                        if (Constants.ValidAppleMusicRegions.Contains(region)) {
                            i3mr0_MusicRP.Properties.Settings.Default.AppleMusicRegion = region;
                        } else {
                            i3mr0_MusicRP.Properties.Settings.Default.AppleMusicRegion = Constants.DefaultAppleMusicRegion;
                        }
                        i3mr0_MusicRP.Properties.Settings.Default.Save();
                    }
                    logger?.Log($"Using region {i3mr0_MusicRP.Properties.Settings.Default.AppleMusicRegion}");
                    File.AppendAllText(crashLogPath, $"Region set to: {i3mr0_MusicRP.Properties.Settings.Default.AppleMusicRegion}\n");
                } catch (Exception ex) {
                    File.AppendAllText(crashLogPath, $"Region detection failed: {ex.Message}\n");
                }

                // check for updates
                try {
                    File.AppendAllText(crashLogPath, "Checking for updates...\n");
                    if (i3mr0_MusicRP.Properties.Settings.Default.CheckForUpdatesOnStartup) {
                        _ = CheckForUpdatesSafe();
                    }
                    File.AppendAllText(crashLogPath, "Update check completed\n");
                } catch (Exception ex) {
                    File.AppendAllText(crashLogPath, $"Update check failed: {ex.Message}\n");
                }

                // start Discord RPC
                try {
                    File.AppendAllText(crashLogPath, "Creating Discord client...\n");
                    var subtitleOptions = (MusicDiscordClient.RPSubtitleDisplayOptions)i3mr0_MusicRP.Properties.Settings.Default.RPSubtitleChoice;
                    var classicalComposerAsArtist = i3mr0_MusicRP.Properties.Settings.Default.ClassicalComposerAsArtist;
                    discordClient = new(Constants.DiscordClientID, enabled: false, subtitleOptions: subtitleOptions, logger: logger);
                    File.AppendAllText(crashLogPath, "Discord client created\n");
                } catch (Exception ex) {
                    File.AppendAllText(crashLogPath, $"Discord client failed: {ex.Message}\n");
                    throw;
                }

                // start Last.FM scrobbler
                try {
                    File.AppendAllText(crashLogPath, "Creating Last.FM scrobbler...\n");
                    var amRegion = i3mr0_MusicRP.Properties.Settings.Default.AppleMusicRegion;
                    lastFmScrobblerClient = new MusicLastFmScrobbler(region: amRegion, logger: logger);
                    _ = lastFmScrobblerClient.init(lastFmCredentials);
                    File.AppendAllText(crashLogPath, "Last.FM scrobbler created\n");
                } catch (Exception ex) {
                    File.AppendAllText(crashLogPath, $"Last.FM scrobbler failed: {ex.Message}\n");
                }

                var lastFMApiKey = i3mr0_MusicRP.Properties.Settings.Default.LastfmAPIKey;
                if (lastFMApiKey == null || lastFMApiKey == "") {
                    logger?.Log("No Last.FM API key found");
                }

                // start ListenBrainz scrobbler
                try {
                    File.AppendAllText(crashLogPath, "Creating ListenBrainz scrobbler...\n");
                    var amRegion = i3mr0_MusicRP.Properties.Settings.Default.AppleMusicRegion;
                    listenBrainzScrobblerClient = new MusicListenBrainzScrobbler(region: amRegion, logger: logger);
                    _ = listenBrainzScrobblerClient.init(listenBrainzCredentials);
                    File.AppendAllText(crashLogPath, "ListenBrainz scrobbler created\n");
                } catch (Exception ex) {
                    File.AppendAllText(crashLogPath, $"ListenBrainz scrobbler failed: {ex.Message}\n");
                }

                // start Apple Music scraper
                try {
                    File.AppendAllText(crashLogPath, "Creating Apple Music scraper...\n");
                    var subtitleOptions = (MusicDiscordClient.RPSubtitleDisplayOptions)i3mr0_MusicRP.Properties.Settings.Default.RPSubtitleChoice;
                    var classicalComposerAsArtist = i3mr0_MusicRP.Properties.Settings.Default.ClassicalComposerAsArtist;
                    amScraper = new(lastFMApiKey, Constants.RefreshPeriod, classicalComposerAsArtist, i3mr0_MusicRP.Properties.Settings.Default.AppleMusicRegion, (newInfo) => {

                        // don't update scraper if Apple Music is paused or not open
                        if (newInfo != null && (i3mr0_MusicRP.Properties.Settings.Default.ShowRPWhenMusicPaused || !newInfo.IsPaused)) {

                            // Discord RP update
                            if (i3mr0_MusicRP.Properties.Settings.Default.EnableDiscordRP) {
                                discordClient.Enable();
                                discordClient.SetPresence(newInfo, i3mr0_MusicRP.Properties.Settings.Default.ShowAppleMusicIcon, i3mr0_MusicRP.Properties.Settings.Default.EnableRPCoverImages);
                            } else {
                                discordClient.Disable();
                            }

                            // Last.FM scrobble update
                            if (i3mr0_MusicRP.Properties.Settings.Default.LastfmEnable && lastFmScrobblerClient != null) {
                                lastFmScrobblerClient.Scrobbleit(newInfo);
                            }

                            // ListenBrainz scrobble update
                            if (i3mr0_MusicRP.Properties.Settings.Default.ListenBrainzEnable && listenBrainzScrobblerClient != null) {
                                listenBrainzScrobblerClient.Scrobbleit(newInfo);
                            }
                        } else {
                            discordClient.Disable();
                        }
                    }, logger);
                    File.AppendAllText(crashLogPath, "Apple Music scraper created\n");
                } catch (Exception ex) {
                    File.AppendAllText(crashLogPath, $"Apple Music scraper failed: {ex.Message}\n");
                    throw;
                }

                logger?.Log("Application initialization completed successfully");
                File.AppendAllText(crashLogPath, "Constructor completed successfully\n");
            } catch (Exception ex) {
                File.AppendAllText(crashLogPath, $"Constructor failed: {ex.Message}\n");
                File.AppendAllText(crashLogPath, $"Stack Trace: {ex.StackTrace}\n");
                logger?.Log($"Application initialization failed: {ex.Message}");
                MessageBox.Show($"Failed to initialize application: {ex.Message}", 
                    "i3mr0 MusicRP - Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        protected override void OnStartup(StartupEventArgs e) {
            var crashLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "i3mr0-MusicRP-CrashLog.txt");
            
            try {
                File.AppendAllText(crashLogPath, "OnStartup called\n");
                base.OnStartup(e);
                File.AppendAllText(crashLogPath, "base.OnStartup completed\n");
                
                File.AppendAllText(crashLogPath, "Looking for TaskbarIcon resource...\n");
                
                // Create TaskbarIcon programmatically instead of using XAML resources
                taskbarIcon = new TaskbarIcon();
                taskbarIcon.IconSource = new BitmapImage(new Uri("pack://application:,,,/Resources/MusicRP.ico"));
                taskbarIcon.ToolTipText = "i3mr0 MusicRP";
                
                // Create context menu programmatically
                var sysTrayMenu = new ContextMenu();
                sysTrayMenu.Items.Add(new MenuItem { Header = "i3mr0 MusicRP", IsEnabled = false });
                sysTrayMenu.Items.Add(new Separator());
                sysTrayMenu.Items.Add(new MenuItem { Header = "Settings" });
                sysTrayMenu.Items.Add(new Separator());
                sysTrayMenu.Items.Add(new MenuItem { Header = "Exit" });
                
                taskbarIcon.ContextMenu = sysTrayMenu;
                
                File.AppendAllText(crashLogPath, "TaskbarIcon created programmatically\n");
                
                // Set up event handlers
                if (taskbarIcon != null) {
                    File.AppendAllText(crashLogPath, "Setting up TaskbarIcon event handlers\n");
                    taskbarIcon.TrayMouseDoubleClick += TaskbarIcon_DoubleClick;
                    
                    // Get the context menu and set up event handlers
                    var contextMenu = taskbarIcon.ContextMenu;
                    if (contextMenu != null) {
                        foreach (var item in contextMenu.Items) {
                            if (item is MenuItem menuItem) {
                                if (menuItem.Header.ToString() == "Settings") {
                                    menuItem.Click += MenuItemSettings_Click;
                                } else if (menuItem.Header.ToString() == "Exit") {
                                    menuItem.Click += MenuItemExit_Click;
                                }
                            }
                        }
                    }
                    File.AppendAllText(crashLogPath, "Event handlers set up successfully\n");
                }
            } catch (Exception ex) {
                File.AppendAllText(crashLogPath, $"OnStartup failed: {ex.Message}\n");
                File.AppendAllText(crashLogPath, $"Stack Trace: {ex.StackTrace}\n");
                MessageBox.Show($"OnStartup failed: {ex.Message}", "i3mr0 MusicRP - Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private MusicRPSettings? settingsWindow;

        private void ShowWindow() {
            try { 
                settingsWindow!.Show(); 
                settingsWindow!.Focus();
            } catch (Exception e) when (e is NullReferenceException || e is InvalidOperationException) { 
                settingsWindow = new MusicRPSettings(); 
                settingsWindow.Show();
                settingsWindow.Focus();
            }
        }

        internal void TaskbarIcon_DoubleClick(object sender, RoutedEventArgs e) {
            ShowWindow();
        }

        internal void MenuItemSettings_Click(object sender, RoutedEventArgs e) {
            ShowWindow();
        }

        internal void MenuItemExit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void Application_Exit(object sender, ExitEventArgs e) {
            taskbarIcon?.Dispose();
            discordClient.Disable();
            logger?.Log("Application finished");
        }

        internal void UpdateRPSubtitleDisplay(MusicDiscordClient.RPSubtitleDisplayOptions newVal) {
            discordClient.subtitleOptions = newVal;
        }

        internal async Task<bool> UpdateLastfmCreds() {
            return lastFmScrobblerClient != null ? await lastFmScrobblerClient.UpdateCredsAsync(lastFmCredentials) : false;
        }

        internal async Task<bool> UpdateListenBrainzCreds() {
            return listenBrainzScrobblerClient != null ? await listenBrainzScrobblerClient.UpdateCredsAsync(listenBrainzCredentials) : false;
        }

        internal void UpdateRegion() {
            var region = i3mr0_MusicRP.Properties.Settings.Default.AppleMusicRegion;
            logger?.Log($"Changed region to {region}");
            amScraper.ChangeRegion(region);
        }

        internal void UpdateScraperPreferences(bool composerAsArtist) {
            amScraper.composerAsArtist = composerAsArtist;
        }

        private async Task CheckForUpdatesSafe() {
            try {
            static int StringVerToInt(string v) {
                var verStr = v[1..].Split("b")[0].Replace(".", "").PadRight(4, '0');
                return int.Parse(verStr);
            }
            Constants.HttpClient.DefaultRequestHeaders.Add("User-Agent", "i3mr0-MusicRP");
            var result = await Constants.HttpClient.GetStringAsync(Constants.GithubReleasesApiUrl);
            var json = JsonDocument.Parse(result);

            var verLocal = Constants.ProgramVersionBase;
            var verRemote = json.RootElement.GetProperty("name").GetString()!;

            var numverLocal = StringVerToInt(verLocal);
            var numverRemote = StringVerToInt(verRemote);

            // TODO add support for multiple beta versions (i.e. b1 and b2)
            if (numverRemote > numverLocal || (numverRemote == numverLocal && verLocal.Contains('b') && !verRemote.Contains('b'))) {
                var res = MessageBox.Show("A new update for i3mr0 MusicRP is available.\nWould you like to view the releases?", "New update available", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (res == MessageBoxResult.Yes) {
                    Process.Start(new ProcessStartInfo {
                        FileName = Constants.GithubReleasesUrl,
                        UseShellExecute = true
                    });
                }
            }
            } catch (Exception ex) {
                logger?.Log($"Update check failed: {ex.Message}");
            }
        }
    }
}
