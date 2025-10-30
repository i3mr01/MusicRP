# i3mr0 MusicRP - Technical Documentation

## 🏗️ **Architecture Overview**

i3mr0 MusicRP is built on a modular architecture with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────┐
│                    i3mr0 MusicRP v2.0                      │
├─────────────────────────────────────────────────────────────┤
│  Core Application Layer                                     │
│  ├── MusicRPApp.xaml/cs          (Main Application)       │
│  ├── MusicRPSettings.xaml/cs     (Settings UI)            │
│  └── MusicRPTaskbar.cs            (System Tray)           │
├─────────────────────────────────────────────────────────────┤
│  Music Integration Layer                                    │
│  ├── MusicClientScraper.cs       (Apple Music Detection)  │
│  ├── MusicWebScraper.cs          (Metadata Scraping)      │
│  └── MusicScrobbler.cs           (Last.FM/ListenBrainz)   │
├─────────────────────────────────────────────────────────────┤
│  Discord Integration Layer                                 │
│  ├── MusicDiscordClient.cs       (Rich Presence)         │
│  └── DiscordRPC Integration      (External Library)      │
├─────────────────────────────────────────────────────────────┤
│  Enhanced Features Layer                                   │
│  ├── EnhancedFeatures.cs         (AI Features)           │
│  ├── ThemeManager.cs             (Theme System)          │
│  ├── PerformanceMonitor.cs        (Performance Tracking)  │
│  └── Constants.cs                 (Configuration)        │
└─────────────────────────────────────────────────────────────┘
```

## 🔧 **Core Components**

### **1. MusicRPApp (Main Application)**
- **Purpose**: Application entry point and orchestration
- **Key Features**:
  - Application lifecycle management
  - Component initialization and coordination
  - Settings persistence and management
  - Update checking and notification

### **2. MusicClientScraper (Apple Music Integration)**
- **Purpose**: Extract song information from Apple Music app
- **Technology**: Microsoft UI Automation (FlaUI)
- **Key Features**:
  - Real-time song detection
  - Playback status monitoring
  - Progress tracking and timestamps
  - Multi-window support (including virtual desktops)

### **3. MusicDiscordClient (Rich Presence)**
- **Purpose**: Update Discord Rich Presence status
- **Key Features**:
  - Custom format support (`{artist}`, `{album}`, `{song}`)
  - Album art integration
  - Progress bar display
  - Button integration (Apple Music links)

### **4. Enhanced Features System**
- **Purpose**: AI-powered enhancements and analytics
- **Components**:
  - **Mood Detection**: Analyze song mood and energy
  - **Genre Recognition**: Automatic genre classification
  - **Smart Recommendations**: AI-powered song suggestions
  - **Listening Analytics**: Detailed music habit reports

## 🎨 **Theme Management System**

### **Theme Architecture**
```csharp
public enum Theme {
    Default, Dark, Light, Neon, Minimal, Custom
}

public class ThemeColors {
    public Brush Background { get; set; }
    public Brush Foreground { get; set; }
    public Brush Accent { get; set; }
    public Brush Border { get; set; }
    public Brush ButtonBackground { get; set; }
    public Brush ButtonForeground { get; set; }
}
```

### **Theme Features**
- **Dynamic Switching**: Change themes without restart
- **Custom Themes**: Create your own color schemes
- **Consistent Application**: Automatic theme application across all UI elements
- **Persistence**: Theme preferences saved across sessions

## ⚡ **Performance Monitoring**

### **Monitoring Capabilities**
- **Memory Usage**: Track RAM consumption and detect leaks
- **CPU Usage**: Monitor processor utilization
- **Response Times**: Measure API call performance
- **Cache Management**: Intelligent caching with automatic cleanup

### **Performance Metrics**
```csharp
public class PerformanceMetric {
    public string Name { get; set; }
    public string Unit { get; set; }
    public List<double> Values { get; set; }
    public double Average { get; set; }
    public double Max { get; set; }
    public double Min { get; set; }
}
```

### **Auto-Optimization**
- **Memory Cleanup**: Automatic garbage collection
- **Cache Management**: Smart cache expiration
- **Resource Monitoring**: Real-time resource usage tracking
- **Alert System**: Performance issue detection and notification

## 🎵 **Music Integration**

### **Apple Music Detection**
```csharp
// Process detection
var amProcesses = Process.GetProcessesByName("AppleMusic");

// UI Automation
using (var automation = new UIA3Automation()) {
    var windows = automation.GetDesktop()
        .FindAllChildren(c => c.ByProcessId(processId));
}
```

### **Song Information Extraction**
- **Song Name**: Primary track title
- **Artist**: Primary artist name
- **Album**: Album name
- **Duration**: Track length
- **Progress**: Current playback position
- **Status**: Playing/Paused state

### **Metadata Enhancement**
- **Cover Art**: Fetch from Last.FM/Apple Music web
- **Genre Detection**: AI-powered genre classification
- **Mood Analysis**: Emotional content analysis
- **Popularity Scoring**: Track popularity metrics

## 🔗 **Discord Integration**

### **Rich Presence Features**
```csharp
public class RichPresence {
    public string Details { get; set; }        // Song name
    public string State { get; set; }          // Custom format
    public Assets Assets { get; set; }         // Images
    public ActivityType Type { get; set; }     // Listening
    public Timestamps? Timestamps { get; set; } // Progress
    public Button[]? Buttons { get; set; }     // Action buttons
}
```

### **Custom Format System**
- **Placeholders**: `{artist}`, `{album}`, `{song}`, `{year}`
- **Examples**:
  - `{artist} - {album}` → "Taylor Swift - 1989"
  - `🎵 {song} by {artist}` → "🎵 Shake It Off by Taylor Swift"
  - `{album} ({year})` → "1989 (2014)"

## 📊 **Analytics and Reporting**

### **Listening Reports**
```csharp
public class ListeningReport {
    public DateTime GeneratedAt { get; set; }
    public int TotalSongs { get; set; }
    public int UniqueArtists { get; set; }
    public int UniqueAlbums { get; set; }
    public string MostPlayedArtist { get; set; }
    public string MostPlayedAlbum { get; set; }
    public TimeSpan TotalListeningTime { get; set; }
}
```

### **Smart Recommendations**
- **Pattern Analysis**: Analyze listening habits
- **Genre Preferences**: Identify favorite genres
- **Artist Discovery**: Suggest similar artists
- **Trending Detection**: Identify popular tracks

## 🛠️ **Configuration Management**

### **Settings Structure**
```csharp
public class Settings {
    // Discord Settings
    public bool EnableDiscordRP { get; set; }
    public bool EnableRPCoverImages { get; set; }
    public bool ShowRPWhenMusicPaused { get; set; }
    public string CustomFormat { get; set; }
    
    // Scrobbling Settings
    public bool LastfmEnable { get; set; }
    public bool ListenBrainzEnable { get; set; }
    
    // Performance Settings
    public bool ShowListeningTime { get; set; }
    public bool ShowSongProgress { get; set; }
    public Theme CurrentTheme { get; set; }
}
```

### **Persistence**
- **Settings**: Stored in `%localappdata%\i3mr0-MusicRP\`
- **Logs**: Rotating log files with automatic cleanup
- **Cache**: Intelligent caching with expiration
- **Credentials**: Secure credential storage via Windows Credential Manager

## 🔒 **Security and Privacy**

### **Data Protection**
- **Local Processing**: All analysis done locally
- **No Data Collection**: No personal data sent to external servers
- **Secure Storage**: Credentials stored in Windows Credential Manager
- **Privacy First**: No tracking or analytics collection

### **Credential Management**
```csharp
// Last.FM credentials stored securely
public struct LastFmCredentials {
    public string apiKey;
    public string apiSecret;
    public string username;
    public string password; // Stored in Windows Credential Manager
}
```

## 🚀 **Performance Optimizations**

### **Caching Strategy**
- **Web Requests**: Cache API responses for 30 minutes
- **UI Elements**: Cache frequently accessed UI elements
- **Metadata**: Cache song metadata to reduce API calls
- **Images**: Cache album art URLs

### **Memory Management**
- **Automatic Cleanup**: Regular garbage collection
- **Cache Limits**: Maximum cache size limits
- **Resource Monitoring**: Real-time memory usage tracking
- **Leak Detection**: Automatic memory leak detection

### **Network Optimization**
- **Request Batching**: Batch multiple API requests
- **Timeout Management**: Configurable request timeouts
- **Retry Logic**: Automatic retry for failed requests
- **Connection Pooling**: Reuse HTTP connections

## 📱 **User Interface**

### **Modern UI Features**
- **Fluent Design**: Windows 11-style interface
- **Theme Support**: Multiple built-in themes
- **Responsive Layout**: Adaptive to different screen sizes
- **Accessibility**: Full accessibility support

### **System Tray Integration**
- **Context Menu**: Right-click options
- **Status Indicators**: Visual status feedback
- **Quick Actions**: Common actions accessible from tray
- **Notifications**: Toast notifications for important events

## 🔄 **Update System**

### **Automatic Updates**
- **Version Checking**: Check for updates on startup
- **Release Detection**: GitHub API integration
- **User Notification**: Prompt for available updates
- **Manual Updates**: User-controlled update process

### **Version Management**
```csharp
public static string ProgramVersionBase {
    get {
        var exePath = Process.GetCurrentProcess().MainModule?.FileName;
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(exePath);
        return $"v{fvi.FileVersion}";
    }
}
```

## 🧪 **Testing and Quality Assurance**

### **Testing Strategy**
- **Unit Tests**: Core functionality testing
- **Integration Tests**: Component interaction testing
- **Performance Tests**: Load and stress testing
- **User Acceptance Tests**: Real-world usage testing

### **Quality Metrics**
- **Code Coverage**: Comprehensive test coverage
- **Performance Benchmarks**: Response time targets
- **Memory Usage**: Memory consumption limits
- **Error Handling**: Comprehensive error recovery

## 📈 **Future Enhancements**

### **Planned Features**
- **Spotify Integration**: Support for Spotify alongside Apple Music
- **Mobile Companion**: Companion mobile app
- **Cloud Sync**: Settings synchronization across devices
- **Plugin System**: Third-party plugin support
- **API Integration**: REST API for external integrations

### **Technical Roadmap**
- **.NET 9.0**: Upgrade to latest .NET version
- **Performance**: Further optimization and caching improvements
- **AI Features**: Enhanced AI-powered recommendations
- **Cross-Platform**: Potential macOS and Linux support

---

**Technical Documentation for i3mr0 MusicRP v1.0**  
*Comprehensive guide for developers and contributors*
