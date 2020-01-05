Option Infer On

Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.FileIO

Module Program

    'Path to openmw.cfg
    Public cfgPath As String
    'Path to Morrowind.ini
    Public iniPath As String
    'Path to config for this program (BBCAutoPatcher.cfg) which stores the cfgPath.
    Public localCfgPath As String

    Sub Main(args As String())
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")
        Console.WriteLine("BBCAutoPatcher V2.0 by SilentNightxxx, Greatness7, and johnnyhostile")
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")

        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            localCfgPath = FileSystem.CurrentDirectory + "\BBCAutoPatcher.cfg"
        End If

        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Then
            localCfgPath = FileSystem.CurrentDirectory + "/BBCAutoPatcher.cfg"
        End If

        If System.IO.File.Exists(localCfgPath) Then
            cfgPath = FileSystem.ReadAllText(localCfgPath)
        Else
            FirstRun()
        End If

        Console.WriteLine("This will create an auto patch for your current load order and Better Balanced Combat.")
        Console.WriteLine("1. Auto patches are not a replacement for hand crafted patches.")
        Console.WriteLine("2. Make sure to sort your load order before continuing.")
        Console.WriteLine("3. If the patcher isn't working for you then delete BBCAutoPatcher.cfg and try again.")
        If cfgPath <> "No" Then
            Console.WriteLine("4. Be aware that continuing will copy your OpenMW load order to the vanilla game first because the patching program")
            Console.WriteLine("   can't read OpenMW load orders.")
        End If
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")
        Console.WriteLine("Press any key to continue...")
        Console.ReadKey(True)

        If cfgPath <> "No" Then
            ConvertLoadOrder()
        End If

        GeneratePatch()

    End Sub

    Sub ConvertLoadOrder()

        Console.WriteLine("Converting load order...")

        iniPath = FileSystem.CurrentDirectory
        iniPath = iniPath.Remove(iniPath.Length - 10)
        iniPath += "Morrowind.ini"

        If System.IO.File.Exists(iniPath) And System.IO.File.Exists(cfgPath) Then

            If File.ReadAllText(iniPath).Length = 0 Then

                Console.WriteLine("Error, Morrowind.ini is empty. Please run the game once.")
                Console.WriteLine("Press any key to continue...")
                Console.ReadKey(True)
                Environment.Exit(-1)

            ElseIf File.ReadAllText(cfgPath).Length = 0 Then

                Console.WriteLine("Error, openmw.cfg is empty. Please run OpenMW once.")
                Console.WriteLine("Press any key to continue...")
                Console.ReadKey(True)
                Environment.Exit(-1)


            Else

                Dim newLines = From line In File.ReadAllLines(iniPath) Where Not line.StartsWith("GameFile")
                File.WriteAllLines(iniPath, newLines)

                Dim newLines2 = From line In File.ReadAllLines(iniPath) Where Not line.StartsWith("[Game Files]")
                File.WriteAllLines(iniPath, newLines2)

                Using sw As New StreamWriter(File.Open(iniPath, FileMode.Append))
                    sw.WriteLine("[Game Files]")
                End Using

                Dim lines = File.ReadAllLines(cfgPath)
                Dim fileNo As Integer = -1
                For Each line In lines
                    If line.StartsWith("content=") Then
                        fileNo += 1
                        Dim splitLine As String() = line.Split("=")
                        Dim formattedLine = "GameFile" + fileNo.ToString + "=" + splitLine(1)

                        Using sw As New StreamWriter(File.Open(iniPath, FileMode.Append))
                            sw.WriteLine(formattedLine)
                        End Using

                    End If
                Next

                Console.WriteLine("Load order converted.")

            End If

        ElseIf System.IO.File.Exists(cfgPath) Then

            Console.WriteLine("Error, Morrowind.ini doesn't exist. Please run the game once and make sure you placed this program in your")
            Console.WriteLine("Data Files directory.")
            Console.WriteLine("Press any key to continue...")
            Console.ReadKey(True)
            Environment.Exit(-1)

        ElseIf System.IO.File.Exists(iniPath) Then

            Console.WriteLine("Error, saved openmw.cfg doesn't exist. Please delete BBCAutoPatcher.cfg and rerun to reconfigure the program.")
            Console.WriteLine("Press any key to continue...")
            Console.ReadKey(True)
            Environment.Exit(-1)

        Else

            Console.WriteLine("Error, Morrowind.ini and openmw.cfg don't exist. Please run the game once and make sure you placed this program")
            Console.WriteLine("in your Data Files directory. After that, delete BBCAutoPatcher.cfg and rerun to reconfigure the program.")
            Console.WriteLine("Press any key to continue...")
            Console.ReadKey(True)
            Environment.Exit(-1)

        End If

    End Sub

    Sub GeneratePatch()

        Console.WriteLine("Generating patch...")

        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            If System.IO.File.Exists(FileSystem.CurrentDirectory + "\BBCAutoPatcher.pl") And System.IO.File.Exists(FileSystem.CurrentDirectory + "\tes3cmd.exe") Then
                Process.Start("CMD", "/K MKDIR bbcbackups & tes3cmd modify -backup-dir bbcbackups -hide-backups -program BBCAutoPatcher.pl && tes3cmd header -backup-dir bbcbackups -hide-backups -synchronize BBC_Auto_Patch.esp && RMDIR bbcbackups /s /q & ECHO ------------------------------------------------------------------------------------------------------------------- && ECHO Done! && ECHO Your load order patch was saved as BBC_Auto_Patch.esp. && ECHO Activate and load it after everything else. && ECHO ------------------------------------------------------------------------------------------------------------------- && PAUSE && EXIT")
            ElseIf System.IO.File.Exists(FileSystem.CurrentDirectory + "\BBCAutoPatcher.pl") Then
                Console.WriteLine("Error, missing tes3cmd.exe in your Data Files directory.")
                Console.WriteLine("Press any key to continue...")
                Console.ReadKey(True)
                Environment.Exit(-1)
            ElseIf System.IO.File.Exists(FileSystem.CurrentDirectory + "\tes3cmd.exe") Then
                Console.WriteLine("Error, missing BBCAutoPatcher.pl in your Data Files directory.")
                Console.WriteLine("Press any key to continue...")
                Console.ReadKey(True)
                Environment.Exit(-1)
            Else
                Console.WriteLine("Error, missing tes3cmd.exe and BBCAutoPatcher.pl in your Data Files directory.")
                Console.WriteLine("Press any key to continue...")
                Console.ReadKey(True)
                Environment.Exit(-1)
            End If
        End If

        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Then
            If System.IO.File.Exists(FileSystem.CurrentDirectory + "/BBCAutoPatcher.pl") And System.IO.File.Exists(FileSystem.CurrentDirectory + "/tes3cmd.exe") Then
                Process.Start("/bin/bash", "-c ""mkdir bbcbackups || true && ./tes3cmd modify -backup-dir bbcbackups -hide-backups -program BBCAutoPatcher.pl && ./tes3cmd header -backup-dir bbcbackups -hide-backups -synchronize BBC_Auto_Patch.esp && rm -rf bbcbackups  || true && echo ------------------------------------------------------------------------------------------------------------------- && echo Done! && echo Your load order patch was saved as BBC_Auto_Patch.esp. && echo Activate and load it after everything else. && echo -------------------------------------------------------------------------------------------------------------------""")
            ElseIf System.IO.File.Exists(FileSystem.CurrentDirectory + "/BBCAutoPatcher.pl") Then
                Console.WriteLine("Error, missing tes3cmd.exe in your Data Files directory.")
                Console.WriteLine("Press any key to continue...")
                Console.ReadKey(True)
                Environment.Exit(-1)
            ElseIf System.IO.File.Exists(FileSystem.CurrentDirectory + "/tes3cmd.exe") Then
                Console.WriteLine("Error, missing BBCAutoPatcher.pl in your Data Files directory.")
                Console.WriteLine("Press any key to continue...")
                Console.ReadKey(True)
                Environment.Exit(-1)
            Else
                Console.WriteLine("Error, missing tes3cmd.exe and BBCAutoPatcher.pl in your Data Files directory.")
                Console.WriteLine("Press any key to continue...")
                Console.ReadKey(True)
                Environment.Exit(-1)
            End If
        End If
    End Sub

    Sub FirstRun()

        Console.WriteLine("Preforming initial setup...")

        Dim answer As String

        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            cfgPath = SpecialDirectories.MyDocuments + "\My Games\OpenMW\openmw.cfg"
            If System.IO.File.Exists(cfgPath) Then
                Console.WriteLine("OpenMW configuration file detected.")
                File.Create(localCfgPath).Dispose()
                FileSystem.WriteAllText(localCfgPath, cfgPath, True)
            Else
                Console.WriteLine("OpenMW configuration file not detected. Do you use OpenMW? (Y/N)")
                answer = Console.ReadLine()
                If LCase(answer) = "y" Or LCase(answer) = "yes" Then
                    Console.WriteLine("Enter the full path to your openmw.cfg file:")
                    cfgPath = Console.ReadLine()
                    If System.IO.File.Exists(cfgPath) Then
                        Console.WriteLine("OpenMW configuration file detected.")
                        File.Create(localCfgPath).Dispose()
                        FileSystem.WriteAllText(localCfgPath, cfgPath, True)
                    Else
                        Console.WriteLine("Error, no openmw.cfg detected.")
                        Console.WriteLine("Press any key to close...")
                        Console.ReadKey(True)
                        Environment.Exit(-1)
                    End If
                ElseIf LCase(answer) = "n" Or LCase(answer) = "no" Then
                    cfgPath = "No"
                    File.Create(localCfgPath).Dispose()
                    FileSystem.WriteAllText(localCfgPath, cfgPath, True)
                Else
                    Console.WriteLine("Error, invalid input.")
                    Console.WriteLine("Press any key to close...")
                    Console.ReadKey(True)
                    Environment.Exit(-1)
                End If
            End If

            Console.WriteLine("Configuration successful.")
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")

        End If

        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Then

            Process.Start("/bin/bash", "-c ""chmod 777 tes3cmd && echo tes3cmd execute permissions set.""")

            cfgPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/openmw/openmw.cfg"

            If System.IO.File.Exists(cfgPath) Then
                Console.WriteLine("OpenMW configuration file detected.")
                File.Create(localCfgPath).Dispose()
                FileSystem.WriteAllText(localCfgPath, cfgPath, True)
            Else
                Console.WriteLine("OpenMW configuration file not detected. Do you use OpenMW? (Y/N)")
                answer = Console.ReadLine()
                If LCase(answer) = "y" Or LCase(answer) = "yes" Then
                    Console.WriteLine("Enter the path to your openmw.cfg file:")
                    cfgPath = Console.ReadLine()
                    If System.IO.File.Exists(cfgPath) Then
                        Console.WriteLine("OpenMW configuration file detected.")
                        File.Create(localCfgPath).Dispose()
                        FileSystem.WriteAllText(localCfgPath, cfgPath, True)
                    Else
                        Console.WriteLine("Error, no openmw.cfg detected.")
                        Console.WriteLine("Press any key to continue...")
                        Console.ReadKey(True)
                        Environment.Exit(-1)
                    End If
                ElseIf LCase(answer) = "n" Or LCase(answer) = "no" Then
                    cfgPath = "No"
                    File.Create(localCfgPath).Dispose()
                    FileSystem.WriteAllText(localCfgPath, cfgPath, True)
                Else
                    Console.WriteLine("Error, invalid input.")
                    Console.WriteLine("Press any key to continue...")
                    Console.ReadKey(True)
                    Environment.Exit(-1)
                End If
            End If

            If (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/perl5/perlbrew/bin/perlbrew")) Then
                Console.WriteLine("Perl detected.")
                Console.WriteLine("Configuration finished, please rerun the program.")
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")
                Console.ReadKey(True)
                Environment.Exit(-1)
            Else
                Console.WriteLine("No Perl installation detected. This program needs Perl installed to run properly. Due to the many flavors of Linux")
                Console.WriteLine("detection could be wrong. If you believe you already have Perl you may close and rerun the program to skip this")
                Console.WriteLine("step, otherwise continue to install Perl.")
                Process.Start("/bin/bash", "-c ""read -p ""Press any key to continue..."" && echo Installing Perl... && curl -L http://xrl.us/installperlnix | bash && echo Configuration finished, please rerun the program. && echo -------------------------------------------------------------------------------------------------------------------""")
                Console.ReadKey(True)
                Environment.Exit(-1)
            End If

        End If

    End Sub
End Module