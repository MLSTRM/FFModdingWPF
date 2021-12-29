﻿using Bartz24.RandoWPF;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FF13_2Rando
{
    public class FF13_2Flags
    {
        public enum FlagType
        {
            All = -1,
            Enemies,
            Other
        }
        public class Enemies
        {
            public static Flag EnemyLocations;

            internal static void Init()
            {
                EnemyLocations = new Flag()
                {
                    Text = "Randomize Enemy Locations",
                    FlagID = "RandEne",
                    DescriptionFormat = "Randomize normal enemies between each other."
                }.Register(FlagType.Enemies);
            }
        }
        public class Other
        {
            public static Flag HistoriaCrux;
            public static Flag Music;
            public static ToggleFlagProperty BodhumStart;

            internal static void Init()
            {
                HistoriaCrux = new Flag()
                {
                    Text = "Randomize Historia Crux",
                    FlagID = "HistCrux",
                    DescriptionFormat = "Randomizes the Historia Crux map.",
                    Aesthetic = true
                }.Register(FlagType.Other);

                BodhumStart = (ToggleFlagProperty)new ToggleFlagProperty()
                {
                    Text = "Force New Bodhum 3 AF Start",
                    ID = "BodhumStart",
                    Description = "Force starting in New Bodhum 3 AF. (Recommended to avoid softlocks and resets)."
                }.Register(HistoriaCrux);

                Music = new Flag()
                {
                    Text = "Shuffle Music",
                    FlagID = "Music",
                    DescriptionFormat = "Shuffle music around.",
                    Aesthetic = true
                }.Register(FlagType.Other);
            }
        }

        public static void Init()
        {
            Flags.FlagsList.Clear();
            Enemies.Init();
            Other.Init();
            Flags.CategoryMap = ((FlagType[])Enum.GetValues(typeof(FlagType))).ToDictionary(f => (int)f, f => string.Join("/", Regex.Split(f.ToString(), @"(?<!^)(?=[A-Z])")));
            Flags.SelectedCategory = "All";
        }
    }
}

