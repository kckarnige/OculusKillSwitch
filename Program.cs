namespace OculusKillSwitch;

using System.IO;
using System.Security.Cryptography;

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

        var fileHash = GetMD5Hash("OculusDash.exe");
        DialogResult Dialog0 = MessageBox.Show("Enable Oculus Killer?", "Oculus Kill Switch", MessageBoxButtons.OKCancel);
        if (Dialog0 == DialogResult.OK)
        {
            MessageBox.Show("YOU'RE A KILLER!", "Oculus Kill Switch", MessageBoxButtons.OK);
        }
        else if (Dialog0 == DialogResult.Cancel)
        {
            MessageBox.Show(fileHash, "Oculus Kill Switch", MessageBoxButtons.OK);
        }

    }    
}