namespace OculusKillSwitch;

using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Net;
using IniParser;
using IniParser.Model;
using Ookii.Dialogs.Wpf;
using IWshRuntimeLibrary;

static class Program
{
    static string GetMD5Hash(string filePath)
    {
        using (var fileStream = new FileStream(filePath,
                                                   FileMode.Open,
                                                   FileAccess.Read,
                                                   FileShare.ReadWrite))
        {
            byte[] hash = System.IO.File.ReadAllBytes(filePath);
            byte[] hashValue = MD5.HashData(hash);
            return Convert.ToHexString(hashValue);

        }
    }

    static void MakeShortcut()
    {
        var parser = new FileIniDataParser();
        IniData configdata = parser.ReadFile("OculusKillSwitch.ini");
        var startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var shell = new WshShell();
        var shortCutLinkFilePath = startupFolderPath + "\\Oculus Kill Switch.lnk";


        string[] fileArray = Directory.GetFiles(startupFolderPath);
        List<string> fileHashList = new();
        try
        {
            if (configdata["OculusKillSwitch"]["DontShowShortcutDialog"] != "true")
            {
                using (TaskDialog dialog = new TaskDialog())
                {
                    TaskDialogButton butYes = new TaskDialogButton(ButtonType.Yes);
                    TaskDialogButton butNo = new TaskDialogButton(ButtonType.No);
                    dialog.WindowTitle = "Oculus Kill Switch";
                    dialog.Content = "Do you want me to make a shortcut?";
                    dialog.MainIcon = TaskDialogIcon.Information;
                    dialog.VerificationText = "Don't Show Again";
                    dialog.Buttons.Add(butYes);
                    dialog.Buttons.Add(butNo);
                    dialog.CenterParent = false;
                    TaskDialogButton result = dialog.ShowDialog();
                    if (dialog.IsVerificationChecked && result == butNo)
                    {
                        configdata["OculusKillSwitch"]["DontShowShortcutDialog"] = "true";
                        parser.WriteFile("OculusKillSwitch.ini", configdata);
                    }
                    if (result == butYes)
                    {
                        configdata["OculusKillSwitch"]["DontShowShortcutDialog"] = "true";
                        var windowsApplicationShortcut = (IWshShortcut)shell.CreateShortcut(shortCutLinkFilePath);
                        windowsApplicationShortcut.Description = "Toggle Oculus Killer and play your Oculus games";
                        windowsApplicationShortcut.WorkingDirectory = Application.StartupPath;
                        windowsApplicationShortcut.TargetPath = Application.ExecutablePath;
                        windowsApplicationShortcut.Save();
                        parser.WriteFile("OculusKillSwitch.ini", configdata);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Application.Exit();
        }

    }

    static void MakeConfig()
    {
        if (System.IO.File.Exists("OculusKillSwitch.ini") != true)
        {
            System.IO.File.Create("OculusKillSwitch.ini").Close();
        }
        var parser = new FileIniDataParser();
        IniData configdata = parser.ReadFile("OculusKillSwitch.ini");
        if (new FileInfo("OculusKillSwitch.ini").Length == 0)
        {
            configdata["OculusKillSwitch"]["IgnoreOculusClient"] = "false";
            configdata["OculusKillSwitch"]["DontShowShortcutDialog"] = "false";
            parser.WriteFile("OculusKillSwitch.ini", configdata);
        }
    }

    static void Main()
    {
        MakeConfig();
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Process[] DashCheck = Process.GetProcessesByName("OculusDash");
        Process[] SteamVRCheck = Process.GetProcessesByName("vrmonitor");
        Process[] OculusAppCheck = Process.GetProcessesByName("OculusClient");

        var parser = new FileIniDataParser();
        IniData configdata = parser.ReadFile("OculusKillSwitch.ini");

        if (DashCheck.Length > 0 != true && SteamVRCheck.Length > 0 != true)
        {
            string activeFileHash;
            string backupFileHash;
            bool whoops = false;
            try
            {
                activeFileHash = GetMD5Hash("OculusDash.exe");
                MakeShortcut();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                whoops = true;
                activeFileHash = "null";
                Process.Start("explorer.exe", @"C:\Program Files\Oculus\Support\oculus-dash\dash\bin");
                DialogResult nuhuhbox = MessageBox.Show("I'm not in the right directory, I go here.\nAfter I close, would you please move me?", "Oculus Kill Switch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (nuhuhbox == DialogResult.OK)
                {
                    whoops = true;
                    Application.Exit();
                }
            }
            try
            {
                backupFileHash = GetMD5Hash("OculusDash.exe.bak");
            }
            catch (Exception)
            {
                whoops = true;
                if (activeFileHash == "9DB7CC8B646A01C60859B318F85E65D0")
                {
                    MessageBox.Show("Oculus Dash backup couldn't be found. If it's in a different directory, move it here. If you don't have it, you may need to reinstall the Oculus app to get it back.", "Oculus Kill Switch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (activeFileHash == "DD65C6B1E5578AE812966798101F1379")
                {
                    DialogResult Dialog1 = MessageBox.Show("The Oculus Killer isn't installed, or the backup file has been renamed. Would you like for me to download it for you?", "Oculus Kill Switch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Dialog1 == DialogResult.Yes)
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile("https://github.com/LibreQuest/OculusKiller/releases/latest/download/OculusDash.exe", "OculusDash.exe.bak");
                        }

                    }
                }
            }
            if (OculusAppCheck.Length > 0 == true && configdata["OculusKillSwitch"]["IgnoreOculusClient"] != "true" && whoops == false)
            {
                using (TaskDialog dialog = new TaskDialog())
                {
                    TaskDialogButton butOK = new TaskDialogButton(ButtonType.Ok);
                    TaskDialogButton butCancel = new TaskDialogButton(ButtonType.Cancel);
                    dialog.VerificationText = "Don't Show Again";
                    dialog.WindowTitle = "Oculus Kill Switch";
                    dialog.Content = "For safety reasons, I'm here to warn you of switching while in VR because I can't detect anything but the Oculus Client being open.\n\nIf you just took off your headset to switch, please make sure to save your game before either closing or restarting (Recommended) the Oculus app.\nIf all you did was open the Oculus app, you may continue, otherwise, click 'Cancel' to close this prompt and come back after you saved your progress.";
                    dialog.MainIcon = TaskDialogIcon.Warning;
                    dialog.Buttons.Add(butOK);
                    dialog.Buttons.Add(butCancel);
                    dialog.CenterParent = false;
                    TaskDialogButton result = dialog.ShowDialog();
                    if (dialog.IsVerificationChecked)
                    {
                        configdata["OculusKillSwitch"]["IgnoreOculusClient"] = "true";
                        parser.WriteFile("OculusKillSwitch.ini", configdata);
                    }
                    if (result == butCancel)
                    {
                        whoops = true;
                        Application.Exit();
                    }
                }

            }
            string killerEnabled;

            if (activeFileHash == "9DB7CC8B646A01C60859B318F85E65D0")
            {
                killerEnabled = "enabled.";
            }
            else
            {
                killerEnabled = "disabled.";
            };

            DialogResult Dialog0;

            if (whoops != true)
            {
                Dialog0 = MessageBox.Show("Toggle Oculus Killer?\n" + "It's currently " + killerEnabled, "Oculus Kill Switch", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }
            else
            {
                Dialog0 = DialogResult.None;
            }

            // It'll be fiiiiiine
            if (Dialog0 == DialogResult.OK)
            {

                System.IO.File.Move("OculusDash.exe.bak", "tempkill.exe");
                System.IO.File.Move("OculusDash.exe", "OculusDash.exe.bak");
                System.IO.File.Move("tempkill.exe", "OculusDash.exe");

                if (activeFileHash != "9DB7CC8B646A01C60859B318F85E65D0")
                {
                    MessageBox.Show("Successfully enabled the Oculus Killer!", "Oculus Kill Switch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Successfully disabled the Oculus Killer!", "Oculus Kill Switch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        else
        {
            MessageBox.Show("Please exit VR before switching your dash.\nIf this issue persists, try restarting the Oculus app.\nSettings > Beta > Restart Oculus", "Oculus Kill Switch", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}