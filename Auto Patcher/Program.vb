Option Infer On

Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.FileIO

Module Program

    'Path to openmw.cfg
    Public cfgPath As String
    'Path to config for this program (AutoPatcher.cfg) which stores the cfgPath.
    Public localCfgPath As String
    'Answer to Yes/No prompt
    Public answer As String

    Sub Main(args As String())
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")
        Console.WriteLine("BBCAutoPatcher V2.0 by SilentNightxxx, Greatness7, and *****")
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")

        localCfgPath = FileSystem.CurrentDirectory + "\AutoPatcher.cfg"

        If System.IO.File.Exists(localCfgPath) Then
            cfgPath = FileSystem.ReadAllText(localCfgPath)
            Console.WriteLine(localCfgPath)
        Else
            FirstRun()
        End If

        Console.WriteLine("This will create an auto patch for your current load order and Better Balanced Combat.")
        Console.WriteLine("1. Auto patches are not a replacement for hand crafted patches.")
        Console.WriteLine("2. Make sure to sort your load order before continuing.")
        Console.WriteLine("3. If the patcher isn't working for you then delete AutoPatcher.cfg and try again.")
        If cfgPath <> "No" Then
            Console.WriteLine("4. Be aware that continuing will copy your OpenMW load order to the vanilla game first because the patching program")
            Console.WriteLine("   can't read OpenMW load orders.")
        End If
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")
        Console.Write("Press any key to continue...")
        Console.ReadKey(True)
        Console.WriteLine("")

        If cfgPath <> "No" Then
            ConvertLoadOrder()
        End If

        GeneratePatch()

    End Sub

    Sub ConvertLoadOrder()

        Console.WriteLine("Converting load order...")

        Dim lines = File.ReadAllLines(cfgPath)
        For Each line In lines
            If line.StartsWith("content=") Then

                'Add load order conversion code here. The program first warns that it will copy OpenMW load order to vanilla if running OpenMW. It's safe to edit and overwrite the file here.

            End If
        Next

    End Sub

    Sub GeneratePatch()

        Console.WriteLine("Generating patch...")

        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            Process.Start("CMD", "/K MKDIR bbcbackups && tes3cmd modify -backup-dir bbcbackups -hide-backups -program BBCAutoPatcher.pl && tes3cmd header -backup-dir bbcbackups -hide-backups -synchronize BBC_Auto_Patch.esp && RMDIR bbcbackups /s /q && ECHO ------------------------------------------------------------------------------------------------------------------- && ECHO Done! && ECHO Your load order patch was saved as BBC_Auto_Patch.esp. && ECHO Activate and load it after everything else. && ECHO ------------------------------------------------------------------------------------------------------------------- && PAUSE && EXIT")
        End If

        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Then

            Process.Start("/bin/bash", "-c mkdir ""bbcbackups"" && tes3cmd modify -backup-dir bbcbackups -hide-backups -program BBCAutoPatcher.pl && tes3cmd header -backup-dir bbcbackups -hide-backups -synchronize BBC_Auto_Patch.esp && rm -rf bbcbackups && echo ------------------------------------------------------------------------------------------------------------------- && echo Done! && echo Your load order patch was saved as BBC_Auto_Patch.esp. && echo Activate and load it after everything else. && echo -------------------------------------------------------------------------------------------------------------------")

        End If

    End Sub

    Sub FirstRun()

        Console.WriteLine("Preforming initial setup...")

        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            cfgPath = SpecialDirectories.MyDocuments + "\My Games\OpenMW\openmw.cfg"
            If System.IO.File.Exists(cfgPath) Then
                Console.WriteLine("OpenMW configuration file detected.")
                File.Create(localCfgPath).Dispose()
                FileSystem.WriteAllText(localCfgPath, cfgPath, True)
            Else
                Console.WriteLine("OpenMW configuration file not detected. Do you use OpenMW? (Y/N)")
                answer = Console.ReadLine()
                If answer = "Y" Or answer = "y" Or answer = "Yes" Or answer = "yes" Or answer = "YES" Or answer = "YEs" Or answer = "yES" Or answer = "yeS" Then
                    Console.WriteLine("Enter the path to your openmw.cfg file:")
                    cfgPath = Console.ReadLine()
                    If System.IO.File.Exists(cfgPath) Then
                        Console.WriteLine("OpenMW configuration file detected.")
                        File.Create(localCfgPath).Dispose()
                        FileSystem.WriteAllText(localCfgPath, cfgPath, True)
                    Else
                        Console.WriteLine("Error, no openmw.cfg detected.")
                        Console.Write("Press any key to close...")
                        Console.ReadKey(True)
                        Environment.Exit(-1)
                    End If
                ElseIf answer = "N" Or answer = "n" Or answer = "No" Or answer = "NO" Or answer = "no" Or answer = "nO" Then
                    cfgPath = "No"
                    File.Create(localCfgPath).Dispose()
                    FileSystem.WriteAllText(localCfgPath, cfgPath, True)
                Else
                    Console.WriteLine("Error, invalid input.")
                    Console.Write("Press any key to close...")
                    Console.ReadKey(True)
                    Environment.Exit(-1)
                End If
            End If
        End If

        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Then

            'Below is the path to the Linux openmw.cfg. Unsure if entering it like that will work or if I need to call a SpecialDirectories like function for Linux.
            cfgPath = "$HOME/.config/openmw/openmw.cfg"

            If System.IO.File.Exists(cfgPath) Then
                Console.WriteLine("OpenMW configuration file detected.")
                File.Create(localCfgPath).Dispose()
                FileSystem.WriteAllText(localCfgPath, cfgPath, True)
            Else
                Console.WriteLine("OpenMW configuration file not detected. Do you use OpenMW? (Y/N)")
                answer = Console.ReadLine()
                If answer = "Y" Or answer = "y" Or answer = "Yes" Or answer = "yes" Or answer = "YES" Or answer = "YEs" Or answer = "yES" Or answer = "yeS" Then
                    Console.WriteLine("Enter the path to your openmw.cfg file:")
                    cfgPath = Console.ReadLine()
                    If System.IO.File.Exists(cfgPath) Then
                        Console.WriteLine("OpenMW configuration file detected.")
                        File.Create(localCfgPath).Dispose()
                        FileSystem.WriteAllText(localCfgPath, cfgPath, True)
                    Else
                        Console.WriteLine("Error, no openmw.cfg detected.")
                        Console.Write("Press any key to continue...")
                        Console.ReadKey(True)
                        Environment.Exit(-1)
                    End If
                ElseIf answer = "N" Or answer = "n" Or answer = "No" Or answer = "NO" Or answer = "no" Or answer = "nO" Then
                    cfgPath = "No"
                    File.Create(localCfgPath).Dispose()
                    FileSystem.WriteAllText(localCfgPath, cfgPath, True)
                Else
                    Console.WriteLine("Error, invalid input.")
                    Console.Write("Press any key to continue...")
                    Console.ReadKey(True)
                    Environment.Exit(-1)
                End If
            End If
        End If

        Console.WriteLine("Configuration successful.")
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")
    End Sub
End Module