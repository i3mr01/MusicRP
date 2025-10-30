# Contributing to i3mr01 MusicRP

Thank you for your interest in contributing to i3mr0 MusicRP! This document provides guidelines and information for contributors.

## 🚀 **Getting Started**

### **Prerequisites**
- Windows 10/11 with Visual Studio 2022 or later
- .NET 8.0 SDK
- Git
- Basic knowledge of C# and WPF

### **Development Setup**
1. Fork the repository
2. Clone your fork: `git clone https://github.com/your-username/MusicRP.git`
3. Open the solution in Visual Studio
4. Restore NuGet packages
5. Build the solution

## 📋 **Contribution Guidelines**

### **Code Style**
- Follow C# naming conventions
- Use meaningful variable and method names
- Add XML documentation for public methods
- Keep methods focused and single-purpose
- Use async/await for I/O operations

### **Commit Messages**
Use clear, descriptive commit messages:
```
feat: add custom format support for Discord Rich Presence
fix: resolve memory leak in performance monitor
docs: update README with new features
refactor: improve theme management system
```

### **Pull Request Process**
1. Create a feature branch from `master`
2. Make your changes
3. Test thoroughly
4. Update documentation if needed
5. Submit a pull request with a clear description

## 🐛 **Bug Reports**

When reporting bugs, please include:
- **System Information**: OS version, .NET version, app version
- **Steps to Reproduce**: Clear, numbered steps
- **Expected vs Actual Behavior**: What should happen vs what happens
- **Log Files**: Attach relevant log files from `%localappdata%\i3mr0-MusicRP\`
- **Screenshots**: If applicable

## ✨ **Feature Requests**

When suggesting features:
- **Clear Description**: What you want and why
- **Use Case**: How it would be used
- **Alternatives**: Other solutions you've considered
- **Additional Context**: Any relevant information

## 🏗️ **Architecture Overview**

### **Project Structure**
```
i3mr0-MusicRP/
├── i3mr0-MusicRP/          # Main application code
│   ├── MusicRPApp.xaml/cs      # Main application
│   ├── MusicRPSettings.xaml/cs # Settings window
│   ├── MusicClientScraper.cs   # Apple Music integration
│   ├── MusicDiscordClient.cs   # Discord Rich Presence
│   ├── EnhancedFeatures.cs    # AI-powered features
│   ├── ThemeManager.cs         # Theme management
│   ├── PerformanceMonitor.cs   # Performance tracking
│   └── Properties/             # Settings and resources
├── .github/                    # GitHub workflows and templates
├── Documentation/             # Additional documentation
└── Tests/                      # Unit tests (future)
```

### **Key Components**
- **MusicRPApp**: Main application entry point
- **MusicClientScraper**: Apple Music detection and data extraction
- **MusicDiscordClient**: Discord Rich Presence integration
- **EnhancedFeatures**: AI-powered analytics and recommendations
- **ThemeManager**: Theme management system
- **PerformanceMonitor**: Performance tracking and optimization

## 🧪 **Testing**

### **Manual Testing**
- Test on different Windows versions
- Test with various Apple Music scenarios
- Test Discord Rich Presence display
- Test theme switching
- Test performance monitoring

### **Automated Testing** (Future)
- Unit tests for core functionality
- Integration tests for API interactions
- Performance tests for optimization

## 📚 **Documentation**

### **Code Documentation**
- Use XML documentation for public methods
- Include parameter descriptions
- Document return values and exceptions
- Add usage examples where helpful

### **User Documentation**
- Update README.md for new features
- Add to FEATURES.md for new capabilities
- Update TECHNICAL_DOCS.md for architectural changes
- Create/update QUICKSTART.md for new user workflows

## 🔧 **Development Workflow**

### **Branching Strategy**
- `master`: Stable, production-ready code
- `develop`: Integration branch for features
- `feature/*`: Feature development branches
- `hotfix/*`: Critical bug fixes

### **Release Process**
1. Update version numbers
2. Update CHANGELOG.md
3. Create release tag
4. GitHub Actions builds and releases automatically

## 🎨 **UI/UX Guidelines**

### **Design Principles**
- **Consistency**: Follow Windows design guidelines
- **Accessibility**: Support high contrast, screen readers
- **Performance**: Smooth animations, responsive UI
- **User-Friendly**: Intuitive navigation, clear feedback

### **Theme Development**
- Follow the existing theme structure
- Test with different Windows themes
- Ensure accessibility compliance
- Document theme-specific features

## 🔒 **Security Guidelines**

### **Data Protection**
- No personal data collection
- Secure credential storage
- Local processing only
- Privacy-first approach

### **Code Security**
- Validate all inputs
- Sanitize user data
- Use secure APIs
- Regular security reviews

## 📞 **Getting Help**

### **Communication Channels**
- **GitHub Issues**: Bug reports and feature requests
- **GitHub Discussions**: General questions and ideas
- **Email**: i3mr01@outlook.com for direct contact

### **Resources**
- **Documentation**: [Full Documentation](https://github.com/i3mr0/MusicRP/wiki)
- **Technical Docs**: [TECHNICAL_DOCS.md](TECHNICAL_DOCS.md)
- **Features**: [FEATURES.md](FEATURES.md)
- **Quick Start**: [QUICKSTART.md](QUICKSTART.md)

## 🙏 **Recognition**

Contributors will be recognized in:
- CONTRIBUTORS.md file
- Release notes
- Project documentation
- GitHub contributors page

## 📄 **License**

By contributing to i3mr0 MusicRP, you agree that your contributions will be licensed under the same GPL-3.0 license that covers the project.

---

**Thank you for contributing to i3mr0 MusicRP!** 🎵✨

Your contributions help make this project better for everyone. Whether you're fixing bugs, adding features, improving documentation, or just testing the app, every contribution is valuable and appreciated.
