# How to Bypass Windows SmartScreen for i3mr0 MusicRP

Windows SmartScreen is blocking the app because it's not digitally signed by a trusted certificate authority. Here's how to run it safely:

## Method 1: Right-click and "Run as administrator"
1. Right-click on `i3mr0-MusicRP.exe`
2. Select "Run as administrator"
3. Click "Yes" when prompted by UAC
4. Click "More info" on the SmartScreen warning
5. Click "Run anyway"

## Method 2: Unblock the file
1. Right-click on `i3mr0-MusicRP.exe`
2. Select "Properties"
3. Check "Unblock" at the bottom
4. Click "OK"
5. Double-click to run normally

## Method 3: Disable SmartScreen temporarily
1. Open Windows Security
2. Go to "App & browser control"
3. Turn off "Check apps and files"
4. Run the app
5. Turn SmartScreen back on after

## Why this happens
- The app is not digitally signed by a trusted certificate authority
- Windows SmartScreen blocks unsigned executables by default
- This is a security feature to protect against malware
- The app is safe - it's just not signed with an expensive certificate

## Future Solution
For future releases, we'll work on getting a proper code signing certificate to eliminate this warning.
