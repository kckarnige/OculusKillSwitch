namespace OculusKillSwitch;

using System.IO;
using System.Security.Cryptography;
using System.Windows;

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
        string fileHash = GetMD5Hash("OculusDash.exe");
        string killerEnabled;

        if (fileHash == "9DB7CC8B646A01C60859B318F85E65D0") {
            killerEnabled = "enabled.";
        } else
        {
            killerEnabled = "disabled.";
        } ;
        DialogResult Dialog0 = MessageBox.Show("Toggle Oculus Killer?\n" + "It's currently " + killerEnabled, "Oculus Kill Switch", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        if (Dialog0 == DialogResult.OK)
        {
            if (fileHash != "9DB7CC8B646A01C60859B318F85E65D0")
            {
                File.Move("OculusDash.exe.bak", "tempkill.exe");
                File.Move("OculusDash.exe", "OculusDash.exe.bak");
                File.Move("tempkill.exe", "OculusDash.exe");
                MessageBox.Show("Successfully enabled the Oculus Killer!", "Oculus Kill Switch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
                File.Move("OculusDash.exe.bak", "tempkill.exe");
                File.Move("OculusDash.exe", "OculusDash.exe.bak");
                File.Move("tempkill.exe", "OculusDash.exe");
                MessageBox.Show("Successfully disabled the Oculus Killer!", "Oculus Kill Switch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }    
}