namespace OculusKillSwitch;

using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Net;

static class Program
{
    static string GetMD5Hash(string filePath)
    {
        using (var fileStream = new FileStream(filePath,
                                                   FileMode.Open,
                                                   FileAccess.Read,
                                                   FileShare.ReadWrite))
        {
            byte[] hash = File.ReadAllBytes(filePath);
            byte[] hashValue = MD5.HashData(hash);
            return Convert.ToHexString(hashValue);

        }
    }

    static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Process[] DashCheck = Process.GetProcessesByName("OculusDash");
        if (DashCheck.Length > 0 != true)
        {
            string activeFileHash;
            string backupFileHash;
            bool whoops = false;
            try
            {
                activeFileHash = GetMD5Hash("OculusDash.exe");
            }
            catch
            {
                whoops = true;
                activeFileHash = "null";
                Process.Start("explorer.exe", @"C:\Program Files\Oculus\Support\oculus-dash\dash\bin");
                MessageBox.Show("I'm not in the right directory, I go here.\nAfter I close, would you please move me?", "Oculus Kill Switch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (activeFileHash != "9DB7CC8B646A01C60859B318F85E65D0")
                {
                    File.Move("OculusDash.exe.bak", "tempkill.exe");
                    File.Move("OculusDash.exe", "OculusDash.exe.bak");
                    File.Move("tempkill.exe", "OculusDash.exe");
                    MessageBox.Show("Successfully enabled the Oculus Killer!", "Oculus Kill Switch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    File.Move("OculusDash.exe.bak", "tempkill.exe");
                    File.Move("OculusDash.exe", "OculusDash.exe.bak");
                    File.Move("tempkill.exe", "OculusDash.exe");
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