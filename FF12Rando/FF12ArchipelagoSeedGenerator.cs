﻿using Bartz24.Data;
using Bartz24.RandoWPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF12Rando;
public class FF12ArchipelagoSeedGenerator : FF12SeedGenerator
{
    public FF12ArchipelagoSeedGenerator() : base()
    {
    }

    protected override void SetRandomizers()
    {
        Randomizers = new()
        {
            new APPartyRando(this),
            new APTreasureRando(this),
            new EquipRando(this),
            new LicenseRando(this),
            new LicenseBoardRando(this),
            new EnemyRando(this),
            new APShopRando(this),
            new MusicRando(this),
            new APTextRando(this)
        };
    }

    protected override void CopyLuaScripts()
    {
        base.CopyLuaScripts();

        // No lua scripts for now...
        //string scriptsFolder = Path.Combine(SetupData.Paths["12"], "x64\\scripts");

        //FileHelpers.CopyFromFolder(scriptsFolder, "data\\scriptsAP");
    }
}
