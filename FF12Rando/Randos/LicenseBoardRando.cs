using Bartz24.Data;
using Bartz24.FF12;
using Bartz24.RandoWPF;
using FF12Rando;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FF12Rando;

public class LicenseBoardRando : Randomizer
{
    private DataStoreLicenseBoard[] boards = new DataStoreLicenseBoard[12];
    private Dictionary<string, DataStoreLicenseBoard> leftSplitBoards = new();
    private Dictionary<string, DataStoreLicenseBoard> rightSplitBoards = new();
    private int[] startingBoards = new int[6];

    private readonly string[] LeftBoardNames = { "Astrologer", "Dark Bishop", "Elementalist", "Enchanter", "Gambler", "Innkeeper", "Loremaster", "Nightshade", "Red Mage", "Shaman", "Sorceror Supreme", "White Mage" };
    private readonly string[] RightBoardNames = { "Black Belt", "Brawler", "Demolitionist", "Gladiator", "Hunter", "Ninja", "Ravager", "Rogue", "Samurai", "Valkyrie", "Viking", "Weaponmaster" };
    private readonly string[] LeftBoardShort = { "AST", "DBP", "ELE", "ENC", "GMB", "INN", "LOR", "NSH", "RDM", "SMN", "SRC", "WHM" };
    private readonly string[] RightBoardShort = { "BLT", "BWR", "DEM", "GLD", "HNT", "NIN", "RAV", "ROG", "SAM", "VAL", "VKG", "WPN" };

    private Dictionary<string, DataStoreLicenseBoard> vanillaBoards = new();
    private readonly string[] VanillaBoardIds = { "whitemage", "uhlan", "machinist", "redbattlemage", "knight", "monk", "timebattlemage", "foebreaker" };
    private readonly string[] VanillaBoardNames = { "White Mage", "Uhlan", "Machinist", "Red Battlemage", "Knight", "Monk", "Time Battlemage", "Foebreaker" };

    private readonly string[] VanillaBoardShort = { "WHM", "UHL", "MCH", "RDM", "KNT", "MNK", "TIM", "FBK" };

    public LicenseBoardRando(SeedGenerator randomizers) : base(randomizers) { }

    public override void Load()
    {
        RandoUI.SetUIProgressIndeterminate("Loading License Board Data...");
        for (int i = 0; i < 12; i++)
        {
            boards[i] = new();
        }

        /*
        boards = Enumerable.Range(0, 12).Select(i =>
        {
            DataStoreLicenseBoard board = new();
            board.LoadData(File.ReadAllBytes($"data\\boards\\split\\center.bin"));
            return board;
        }).ToArray();

        Directory.GetFiles("data\\boards\\split\\left", "*.bin").ForEach(s =>
        {
            string fileName = Path.GetFileName(s);
            string name = fileName.Substring(0, fileName.LastIndexOf("."));
            DataStoreLicenseBoard board = new();
            board.LoadData(File.ReadAllBytes(s));
            leftSplitBoards.Add(name, board);
        });
        Directory.GetFiles("data\\boards\\split\\right", "*.bin").ForEach(s =>
        {
            string fileName = Path.GetFileName(s);
            string name = fileName.Substring(0, fileName.LastIndexOf("."));
            DataStoreLicenseBoard board = new();
            board.LoadData(File.ReadAllBytes(s));
            rightSplitBoards.Add(name, board);
        });
        */

        LicenseRando licenseRando = Generator.Get<LicenseRando>();
        Directory.GetFiles("data\\licenses\\vanilla\\boards", "*.csv").ForEach(s =>
        {
            string fileName = Path.GetFileName(s);
            string name = fileName.Substring(0, fileName.LastIndexOf("."));
            DataStoreLicenseBoard board = new();
            board.LoadData(s, lName => licenseRando.licenseData.Values.First(l => l.Name == lName).IntID);
            vanillaBoards.Add(name, board);
        });

        TreasureRando treasureRando = Generator.Get<TreasureRando>();

        treasureRando.prices[0x77].Price = 0;
        startingBoards = MathHelpers.DecodeNaturalSequence(treasureRando.prices[0x77].Price, 6, 13).Select(l => (int)l).ToArray();
    }
    public override void Randomize()
    {
        RandoUI.SetUIProgressIndeterminate("Randomizing License Board Data...");
        TreasureRando treasureRando = Generator.Get<TreasureRando>();
        TextRando textRando = Generator.Get<TextRando>();

        if (FF12Flags.Other.LicenseBoards.FlagEnabled)
        {
            FF12Flags.Other.LicenseBoards.SetRand();
            /*
            int[] left = Enumerable.Range(0, 12).Shuffle().ToArray();
            int[] right = Enumerable.Range(0, 12).Shuffle().ToArray();
            for (int i = 0; i < 12; i++)
            {
                AddBoard(boards[i], leftSplitBoards.Values.ToList()[left[i]]);
                AddBoard(boards[i], rightSplitBoards.Values.ToList()[right[i]]);

                textRando.TextMenuMessage[104 + i].Text = LeftBoardNames[left[i]] + " - " + RightBoardNames[right[i]] + "\n {0x0f}où Preview License Board";
                textRando.TextMenuCommand[5 + i].Text = LeftBoardShort[left[i]] + "-" + RightBoardShort[right[i]];
            }

            */

            foreach(string id in VanillaBoardIds)
            {
                int i = VanillaBoardIds.ToList().IndexOf(id);
                AddBoard(boards[i], vanillaBoards[id]);
                textRando.TextMenuMessage[104 + i].Text = VanillaBoardNames[i] + "\n {btn:square} Preview License Board";
                textRando.TextMenuCommand[5 + i].Text = VanillaBoardShort[i];
            }

            CenterBoards();

            RandomNum.ClearRand();
        }

        if (FF12Flags.Other.StartingBoards.FlagEnabled)
        {
            FF12Flags.Other.StartingBoards.SetRand();
            startingBoards = Enumerable.Range(1, 12).Shuffle().Take(6).ToArray();
            treasureRando.prices[0x77].Price = (uint)MathHelpers.EncodeNaturalSequence(startingBoards.Select(i => (long)i).ToArray(), 13);
            RandomNum.ClearRand();
        }
    }

    private void AddBoard(DataStoreLicenseBoard main, DataStoreLicenseBoard add)
    {
        for (int x = 0; x < 24; x++)
        {
            for (int y = 0; y < 24; y++)
            {
                if (add.Board[y, x] != 0xFFFF)
                {
                    main.Board[y, x] = add.Board[y, x];
                }
            }
        }
    }

    private void CenterBoards()
    {
        // Center each board in the middle of the 24x24 grid
        for (int i = 0; i < 12; i++)
        {
            DataStoreLicenseBoard board = boards[i];
            int minX = 24;
            int minY = 24;
            int maxX = 0;
            int maxY = 0;
            for (int x = 0; x < 24; x++)
            {
                for (int y = 0; y < 24; y++)
                {
                    if (board.Board[y, x] != 0xFFFF)
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }

            ushort[,] newBoard = new ushort[24, 24];
            // Fill with 0xFFFF
            for (int x = 0; x < 24; x++)
            {
                for (int y = 0; y < 24; y++)
                {
                    newBoard[y, x] = 0xFFFF;
                }
            }

            int offsetX = (24 - (maxX - minX + 1)) / 2 - minX;
            int offsetY = (24 - (maxY - minY + 1)) / 2 - minY;
            for (int x = 0; x < 24; x++)
            {
                for (int y = 0; y < 24; y++)
                {
                    if (x + offsetX >= 0 && x + offsetX < 24 && y + offsetY >= 0 && y + offsetY < 24)
                    {
                        newBoard[y + offsetY, x + offsetX] = board.Board[y, x];
                    }
                }
            }

            board.Board = newBoard;
        }
    }

    public override void Save()
    {
        Generator.SetUIProgress("Saving License Board Data...", 0, -1);

        for (int i = 0; i < 12; i++)
        {
            string path = $"{Generator.DataOutFolder}\\image\\ff12\\test_battle\\in\\binaryfile\\board_{i + 1}.bin";
            Directory.CreateDirectory($"{Generator.DataOutFolder}\\image\\ff12\\test_battle\\in\\binaryfile");
            File.WriteAllBytes(path, boards[i].Data);
        }
    }
}
