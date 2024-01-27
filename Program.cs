using Microsoft.Win32;
using System.Diagnostics;
using System;
using System.IO;

public class Program
{
    public static void Main()
    {
        int a;
        Console.WriteLine("0 - exit");
        Console.WriteLine("1 - userFolder");
        Console.WriteLine("2 - path");
        try
        {
            a = int.Parse(Console.ReadLine());
            switch (a)
            {
                case 0:
                    break;
                case 1:
                    UserFolder userFolder = new UserFolder();
                    userFolder.SetFolders();
                    break;
                case 2:
                    PathAdder pathAdder = new PathAdder();
                    pathAdder.AddPath();
                    break;
            }
        }
        catch
        {

        }

    }
}

public class PathAdder
{
    public void AddPath()
    {
        try
        {
            var lines = File.ReadAllLines("path.txt");
            string value = string.Join(";", lines);
            string path = Environment.GetEnvironmentVariable("Path") + ";" + value;
            Environment.SetEnvironmentVariable("Path", path, EnvironmentVariableTarget.Machine);
        }
        catch
        {

        }
    }
}

public class UserFolder
{
    public const string REGEDIT_PATH = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\User Shell Folders";
    public const string REGEDIT_PATH1 = "HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders";
    public string targetDrive = "E:/";
    public string[] musicRegedit = new string[] { "{A0C69A99-21C8-4671-8703-7934162FCF1D}", "Music", "My Music" };
    public string[] pictureRegedit = new string[] { "{0DDD015D-B06C-45D5-8C4C-F59713854639}", "My Pictures" };
    public string[] videoRegedit = new string[] { "{35286A68-3C57-41A1-BBB1-0EAE73D76C95}", "{EDC0FE71-98D8-4F4A-B920-C8DC133CB165}", "My Video" };
    public string[] downloadRegedit = new string[] { "{7D83EE9B-2244-4E70-B1F5-5393042AF1E4}", "{374DE290-123F-4565-9164-39C4925E467B}" };
    public string[] desktopRegedit = new string[] { "{754AC886-DF64-4CBA-86B5-F7FBF4FBCEF5}", "Desktop" };
    public string[] documentRegedit = new string[] { "{F42EE2D3-909F-4907-8871-4C22FC0BF756}", "Personal" };

    public void SetFolders()
    {
        Set("Music", musicRegedit);
        Set("Pictures", pictureRegedit);
        Set("Downloads", downloadRegedit);
        Set("Documents", documentRegedit);
        Set("Desktop", desktopRegedit);
        Set("Videos", videoRegedit);
        KillProcess("explorer");
        StartProcess("explorer.exe");
    }
    void Set(string target, string[] regs)
    {
        target = targetDrive + target;
        if (CheckFolder(target))
        {
            foreach (var item in regs)
            {
                Registry.SetValue(REGEDIT_PATH, item, target);
                Registry.SetValue(REGEDIT_PATH1, item, target);
            }
        }
        else
        {
        }
    }
    void KillProcess(string processName)
    {
        Process[] processes = Process.GetProcessesByName(processName);
        foreach (Process process in processes)
        {
            try
            {
                process.Kill();
            }
            catch
            {
            }
        }
    }

    void StartProcess(string processName)
    {
        try
        {
            Process.Start(processName);
        }
        catch
        {
        }
    }
    bool CheckFolder(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }
        else
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
