# BBCAutoPatcher-2.0
Generates an auto patch for the Morrowind mod Better Balanced Combat.

# About
Created in Visual Studio Community 2019 using VB.NET Core 3.1.

# Build
dotnet publish -r win-x64

dotnet publish -r win-x86

dotnet publish -r linux-x64

dotnet publish -r linux-arm

# Install
Place executable in Data Files folder

Place BBCAutoPatcher.pl from dependencies in Data Files folder

Windows: Place tes3cmd.exe from dependencies in Data Files folder

Linux: Place tes3cmd from dependencies in Data Files folder

# Additional Credits
Greatness7 for writing the perl code.

johnnyhostile for helping with load order conversion and Linux support.

john.moonsugar for TES3cmd, licensed under MIT.