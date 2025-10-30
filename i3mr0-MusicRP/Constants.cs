using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace i3mr0_MusicRP {
    internal static class Constants {
        public static string ProgramVersionBase {
            get {
                try {
                    var exePath = Process.GetCurrentProcess().MainModule?.FileName;
                    if (exePath == null) {
                        return "";
                    }
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(exePath);
                    return $"v{fvi.FileVersion}";
                } catch (Exception ex) {
                    new i3mr0_MusicRP.Logger().Log($"Error getting version string: {ex}");
                    return "";
                }
            }
        } 
#if RELEASE
        public static string  ProgramVersion = ProgramVersionBase;
#else
        public static string  ProgramVersion = $"{ProgramVersionBase}-dev";
#endif                        
        public static int    MaxLogFiles                    = 10; // files
        public static int    RefreshPeriod                  = 3; // seconds (faster updates)
        public static int    NumFailedSearchesBeforeAbandon = 5; // attempts
        public static int    MaxRetryAttempts              = 3; // retry failed operations
        public static int    CacheTimeoutMinutes           = 30; // cache timeout for web requests
        public static string AppDataFolderName              = "i3mr0-MusicRP";
        public static string DiscordClientID                = "1066220978406953012"; // You'll need to create your own Discord app
        public static string DiscordAppleMusicImageKey      = "musicnote1024x";
        public static string DiscordAppleMusicPlayImageKey  = "play1024x";
        public static string DiscordAppleMusicPauseImageKey = "pause1024x";
        public static string LastFMCredentialTargetName     = "Last FM Password";
        public static int    LastFMTimeBeforeScrobbling     = 20; // seconds
        public static string GithubReleasesApiUrl           = "https://api.github.com/repos/i3mr0/MusicRP/releases/latest";
        public static string GithubReleasesUrl              = "https://github.com/i3mr0/MusicRP/releases";
        public static string DefaultAppleMusicRegion        = "US";
        public static string WindowsStartupFolder => Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        public static string WindowsAppDataFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string AppDataFolder => Path.Combine(WindowsAppDataFolder, AppDataFolderName);
        public static string AppShortcutPath => Path.Join(WindowsStartupFolder, "i3mr0-MusicRP.lnk");
        public static string? ExePath => Process.GetCurrentProcess().MainModule?.FileName;

        public static readonly HttpClient HttpClient = new();

        public static string[] ValidAppleMusicRegions = [
            "ae","ag","ai","am","ao","ar","at","au",
            "az","ba","bb","be","bg","bh","bj","bm",
            "bo","br","bs","bt","bw","by","bz","ca",
            "cd","cg","ch","ci","cl","cm","cn","co",
            "cr","cv","cy","cz","de","dk","dm","do",
            "dz","ec","ee","eg","es","fi","fj","fm",
            "fr","ga","gb","gd","ge","gh","gm","gr",
            "gt","gw","gy","hk","hn","hr","hu","id",
            "ie","il","in","iq","is","it","jm","jo",
            "jp","ke","kg","kh","kn","kr","kw","ky",
            "kz","la","lb","lc","lk","lr","lt","lu",
            "lv","ly","ma","md","me","mg","mk","ml",
            "mm","mn","mo","mr","ms","mt","mu","mv",
            "mw","mx","my","mz","na","ne","ng","ni",
            "nl","no","np","nz","om","pa","pe","pg",
            "ph","pl","pt","py","qa","ro","rs","ru",
            "rw","sa","sb","sc","se","sg","si","sk",
            "sl","sn","sr","sv","sz","tc","td","th",
            "tj","tm","tn","to","tr","tt","tw","tz",
            "ua","ug","us","uy","uz","vc","ve","vg",
            "vn","vu","xk","ye","za","zm","zw"
        ];
    }
}