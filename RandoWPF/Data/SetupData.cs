using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bartz24.RandoWPF;

public class SetupData
{
    public static Dictionary<string, string> Paths = new();
    public static Dictionary<string, string> PathRegistrySearch = new();
    public static Dictionary<string, List<string>> WPDTracking = new();

    public static string PathFileName;
    public static string OutputFolder;

    public static string Seed { get; set; }
    public static string Version { get; set; } = "0.7.5.28";

    public static string SearchSteamRegistry(string path)
    {
        object returnVal = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Valve\\Steam", "SteamPath", "c:/program files (x86)/steam");
        returnVal = returnVal.ToString().Replace("/", "\\");

        string steamPath = GetSteamPath(path, returnVal.ToString());
        if (steamPath == null)
        {
            string text = File.ReadAllText(returnVal.ToString() + "/steamapps/libraryfolders.vdf");
            Regex regex = new("\"\\d+\"\\s+\"(.*)\"");
            foreach (Match match in regex.Matches(text))
            {
                if (match.Success)
                {
                    steamPath = GetSteamPath(path, match.Groups[1].Value);
                    return steamPath;
                }
            }
        }

        return steamPath;
    }
    private static string GetSteamPath(string pathCheck, string directoryCheck = null)
    {
        if (directoryCheck != null && Directory.Exists(directoryCheck))
        {
            string[] paths = Directory.GetFiles(directoryCheck, pathCheck.Substring(pathCheck.LastIndexOf("\\") + 1), SearchOption.AllDirectories);
            if (paths.Length > 0)
            {
                return paths[0].Replace("/", "\\");
            }
        }

        return null;
    }

    private static string GetFile(string key, string file, bool checkSteam = true)
    {
        Dictionary<string, string> lines = new();
        if (File.Exists(file))
        {
            lines = File.ReadAllLines(file).Select(s => s.Replace("/", "\\").Split(";")).ToDictionary(a => a[0], a => a[1]);
            if (lines.ContainsKey(key))
            {
                return lines[key];
            }
        }

        if (checkSteam)
        {
            string path = SearchSteamRegistry(PathRegistrySearch[key]);
            if (path != null)
            {
                lines.Add(key, path);
                File.WriteAllLines(file, lines.Select(p => $"{p.Key};{p.Value}"));
            }

            return path;
        }

        return null;
    }

    public static string GetSteamPath(string key, bool checkSteam = true)
    {
        string path = null;
        try
        {
            path = GetFile(key, PathFileName, checkSteam);
        }
        catch (Exception)
        {
            // Do nothing
        }

        return path == null ? null : PathRegistrySearch.ContainsKey(key) ? path.Substring(0, path.IndexOf(PathRegistrySearch[key])) : path;
    }
}
