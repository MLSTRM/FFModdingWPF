﻿using Bartz24.Data;
using Bartz24.RandoWPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FF13Rando;

public class MusicRando : Randomizer
{
    private readonly List<string> soundFiles = new();
    private List<string> newSoundFiles = new();
    private readonly Dictionary<string, string> names = new();

    public MusicRando(RandomizerManager randomizers) : base(randomizers) { }

    public override void Load()
    {
        Randomizers.SetUIProgress("Loading Music Data...", -1, 100);
        soundFiles.AddRange(File.ReadAllLines("data\\music13.csv"));
    }
    public override void Randomize()
    {
        Randomizers.SetUIProgress("Randomizing Music Data...", -1, 100);
        if (FF13Flags.Other.Music.FlagEnabled)
        {
            FF13Flags.Other.Music.SetRand();
            newSoundFiles = soundFiles.Shuffle().ToList();
            RandomNum.ClearRand();
        }
    }
    /*
    public override HTMLPage GetDocumentation()
    {
        HTMLPage page = new HTMLPage("Music", "template/documentation.html");
        page.HTMLElements.Add(new Table("Music", new string[] { "Original Track", "New Track" }.ToList(), new int[] { 50, 50 }.ToList(), Enumerable.Range(0, soundFiles.Count).Select(i => new string[] { names[soundFiles[i]], names[newSoundFiles[i]] }.ToList()).ToList())); ;
        return page;
    }*/

    public override void Save()
    {
        Randomizers.SetUIProgress("Saving Music Data...", -1, 100);
        for (int i = 0; i < Math.Min(soundFiles.Count, newSoundFiles.Count); i++)
        {
            Directory.CreateDirectory(Path.GetDirectoryName($"{SetupData.OutputFolder}\\{newSoundFiles[i]}"));
            File.Copy($"{Nova.GetNovaFile("13", soundFiles[i], SetupData.Paths["Nova"], SetupData.Paths["13"])}", $"{SetupData.OutputFolder}\\{newSoundFiles[i]}", true);
        }
    }
}
