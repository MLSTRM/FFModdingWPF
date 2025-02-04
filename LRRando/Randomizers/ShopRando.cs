﻿using Bartz24.Data;
using Bartz24.Docs;
using Bartz24.FF13_2_LR;
using Bartz24.LR;
using Bartz24.RandoWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using static Bartz24.FF13_2_LR.Enums;

namespace LRRando;

public class ShopRando : Randomizer
{
    private readonly DataStoreDB3<DataStoreShop> shopsOrig = new();
    private readonly DataStoreDB3<DataStoreShop> shops = new();
    private readonly Dictionary<string, ShopData> shopData = new();

    public ShopRando(RandomizerManager randomizers) : base(randomizers) { }

    public override void Load()
    {
        Randomizers.SetUIProgress("Loading Shop Data...", 0, 100);
        shopsOrig.LoadDB3("LR", @"\db\resident\shop.wdb");
        Randomizers.SetUIProgress("Loading Shop Data...", 50, 100);
        shops.LoadDB3("LR", @"\db\resident\shop.wdb");

        FileHelpers.ReadCSVFile(@"data\shops.csv", row =>
        {
            ShopData s = new(row);
            shopData.Add(s.ID, s);
        }, FileHelpers.CSVFileHeader.HasHeader);

        EquipRando equipRando = Randomizers.Get<EquipRando>();
        equipRando.items["mat_z_000"].uPurchasePrice = 770;
        equipRando.items["mat_z_001"].uPurchasePrice = 1900;
        equipRando.items["mat_z_002"].uPurchasePrice = 770;
        equipRando.items["mat_z_003"].uPurchasePrice = 840;
        equipRando.items["mat_z_004"].uPurchasePrice = 2100;
        equipRando.items["mat_z_007"].uPurchasePrice = 1500;
        equipRando.items["mat_z_008"].uPurchasePrice = 2500;
        equipRando.items["mat_z_009"].uPurchasePrice = 1300;
        equipRando.items["mat_z_010"].uPurchasePrice = 2900;
        equipRando.items["mat_z_011"].uPurchasePrice = 2700;
        equipRando.items["mat_z_012"].uPurchasePrice = 2100;
        equipRando.items["mat_z_013"].uPurchasePrice = 2700;
        equipRando.items["mat_z_014"].uPurchasePrice = 6600;
        equipRando.items["mat_z_015"].uPurchasePrice = 2100;
        equipRando.items["mat_z_016"].uPurchasePrice = 3500;
        equipRando.items["mat_z_017"].uPurchasePrice = 1600;
        equipRando.items["mat_z_018"].uPurchasePrice = 4100;
        equipRando.items["mat_z_019"].uPurchasePrice = 2700;
        equipRando.items["mat_z_020"].uPurchasePrice = 2700;
        equipRando.items["mat_z_021"].uPurchasePrice = 2900;
        equipRando.items["mat_z_022"].uPurchasePrice = 4100;
        equipRando.items["mat_z_024"].uPurchasePrice = 8800;
        equipRando.items["mat_z_028"].uPurchasePrice = 8800;
        equipRando.items["mat_z_029"].uPurchasePrice = 8800;
        equipRando.items["mat_z_030"].uPurchasePrice = 8800;
        equipRando.items["mat_z_031"].uPurchasePrice = 5800;
        equipRando.items["mat_z_032"].uPurchasePrice = 5300;
        equipRando.items["mat_z_033"].uPurchasePrice = 4800;
        equipRando.items["mat_z_035"].uPurchasePrice = 8800;
        equipRando.items["mat_z_036"].uPurchasePrice = 8800;
        equipRando.items["mat_z_044"].uPurchasePrice = 8800;
        equipRando.items["mat_z_045"].uPurchasePrice = 4100;

        equipRando.items["mat_abi_0_00"].uPurchasePrice = 400;
        equipRando.items["mat_abi_0_01"].uPurchasePrice = 700;
        equipRando.items["mat_abi_0_02"].uPurchasePrice = 1200;
        equipRando.items["mat_abi_0_03"].uPurchasePrice = 2000;
        equipRando.items["mat_abi_0_04"].uPurchasePrice = 4000;
        equipRando.items["mat_abi_0_05"].uPurchasePrice = 9000;
        equipRando.items["mat_abi_0_06"].uPurchasePrice = 15000;
        equipRando.items["mat_abi_0_07"].uPurchasePrice = 28000;
        equipRando.items["mat_abi_0_08"].uPurchasePrice = 54000;

        equipRando.items["mat_cus_0_00"].uPurchasePrice = 100;
        equipRando.items["mat_cus_0_01"].uPurchasePrice = 400;
        equipRando.items["mat_cus_0_02"].uPurchasePrice = 1000;
        equipRando.items["mat_cus_0_03"].uPurchasePrice = 2500;
        equipRando.items["mat_cus_0_04"].uPurchasePrice = 6000;
        equipRando.items["mat_cus_0_05"].uPurchasePrice = 10000;
        equipRando.items["mat_cus_0_06"].uPurchasePrice = 15000;
        equipRando.items["mat_cus_0_07"].uPurchasePrice = 22000;
        equipRando.items["mat_cus_0_08"].uPurchasePrice = 36000;

        equipRando.items["it_reraise"].uPurchasePrice = 2520;
        equipRando.items["it_hero"].uPurchasePrice = 4340;

    }
    public override void Randomize()
    {
        EquipRando equipRando = Randomizers.Get<EquipRando>();
        TreasureRando treasureRando = Randomizers.Get<TreasureRando>();

        Randomizers.SetUIProgress("Randomizing Shop Data...", 0, 100);
        equipRando.itemWeapons.Values.Where(i => equipRando.items.Keys.Contains(i.name) && ((i.u4WeaponKind == (int)WeaponKind.Weapon && i.u4AccessoryPos == 0) || i.u4WeaponKind == (int)WeaponKind.Shield)).ForEach(i =>
        {
            equipRando.items[i.name].uSellPrice = (int)(2 * equipRando.items[i.name].uSellPrice / Math.Log10(Math.Pow(equipRando.items[i.name].uSellPrice, 1.5) / 1.5));
            equipRando.items[i.name].uSellPrice = equipRando.items[i.name].uSellPrice.RoundToSignificantDigits((int)Math.Max(2, Math.Ceiling(Math.Log10(equipRando.items[i.name].uSellPrice) - 2)));
        });

        Randomizers.SetUIProgress("Randomizing Shop Data...", 20, 100);
        if (LRFlags.Items.Shops.FlagEnabled)
        {
            LRFlags.Items.Shops.SetRand();

            Dictionary<string, List<string>> uniqueShops = new()
            {
                { "shop_ptl", new List<string>() },
                { "shop_equ_wd00", new List<string>() }
            };
            shops.Values.Where(s => s.u3Category == (int)ShopCategory.Forge).Shuffle().Take(3).ForEach(s => uniqueShops.Add(s.name, new List<string>()));

            Dictionary<string, List<string>> shopsDict = new();

            shopData.Keys.Select(s => shopsOrig[s]).ForEach(s => shopsDict.Add(s.name, s.GetItems().Where(i =>
                  s.u3Category == (int)ShopCategory.Inn ||
                  (s.u3Category == (int)ShopCategory.Libra && (
                                !i.StartsWith("libra") ||
                                treasureRando.itemLocations.Values.Where(t => treasureRando.PlacementAlgo.Logic.GetLocationItem(t.ID, false).Value.Item1 == i).Count() == 0))
                  ).ToList())
            );

            Dictionary<string, int> maxSizes = shopsDict.Keys.ToDictionary(k => k, k =>
                shopsDict[k].Count + RandomNum.RandInt(
                    shopsOrig[k].u3Category is ((int)ShopCategory.Ark) or ((int)ShopCategory.Items) ? 1 : 3,
                    (shopsOrig[k].u3Category is ((int)ShopCategory.Ark) or ((int)ShopCategory.Items) ? 6 : 18) - shopsDict[k].Count)
            );
            List<string> libraMaterials = new();
            shopsOrig.Values.Where(s => s.u3Category == (int)ShopCategory.Libra).ForEach(_ => libraMaterials.AddRange(equipRando.items.Keys.Where(i => i.StartsWith("mat_z")).Shuffle()));

            for (int n = 4; n <= 8; n++)
            {
                shopsOrig.Values.Where(s => s.u3Category == (int)ShopCategory.Libra && shopsDict.ContainsKey(s.name)).ForEach(s => shopsDict[s.name].Add("mat_abi_0_0" + n));
            }

            for (int n = 3; n <= 8; n++)
            {
                shopsOrig.Values.Where(s => s.u3Category == (int)ShopCategory.Libra && shopsDict.ContainsKey(s.name)).ForEach(s => shopsDict[s.name].Add("mat_cus_0_0" + n));
            }

            foreach (string equip in treasureRando.RemainingEquip)
            {
                string next = shopsDict.Keys.Where(k => shopsDict[k].Count < 32 && IsValid(equip, (ShopCategory)shopsOrig[k].u3Category)).Shuffle().First();
                shopsDict[next].Add(equip);
                string unique = uniqueShops.Keys.Where(k => next.StartsWith(k)).FirstOrDefault();
                if (!string.IsNullOrEmpty(unique))
                {
                    uniqueShops[unique].Add(equip);
                }
            }

            foreach (string adorn in treasureRando.RemainingAdorn)
            {
                string next = shopsDict.Keys.Where(k => shopsDict[k].Count < 32 && IsValid(adorn, (ShopCategory)shopsOrig[k].u3Category)).Shuffle().First();
                shopsDict[next].Add(adorn);
                string unique = uniqueShops.Keys.Where(k => next.StartsWith(k)).FirstOrDefault();
                if (!string.IsNullOrEmpty(unique))
                {
                    uniqueShops[unique].Add(adorn);
                }
            }

            Randomizers.SetUIProgress("Randomizing Shop Data...", 70, 100);

            List<string> possibleItems = new();
            possibleItems.AddRange(treasureRando.RemainingEquip);
            possibleItems.AddRange(treasureRando.RemainingAdorn);
            possibleItems.AddRange(GetItems());
            foreach (string shop in shopsDict.Keys.Shuffle().OrderBy(s => s != "shop_ptl_pt00"))
            {
                string unique = uniqueShops.Keys.Where(k => shop.StartsWith(k)).FirstOrDefault();
                while (shopsDict[shop].Count < maxSizes[shop])
                {
                    IList<string> possible = possibleItems.Where(i =>
                        IsValid(i, (ShopCategory)shopsOrig[shop].u3Category)
                        && !shopsDict[shop].Contains(i)
                        && ((uniqueShops.Keys.Where(k => shop.StartsWith(k)).Count() == 0 && !uniqueShops.Values.SelectMany(l => l).Contains(i)) || uniqueShops.Keys.Where(k => shop.StartsWith(k)).Count() > 0)
                    ).Shuffle();
                    if (possible.Count == 0)
                    {
                        break;
                    }

                    string next = possible.First();
                    shopsDict[shop].Add(next);

                    if (!string.IsNullOrEmpty(unique) && !uniqueShops[unique].Contains(next))
                    {
                        uniqueShops[unique].Add(next);
                    }
                }

                if (shopsOrig[shop].u3Category == (int)ShopCategory.Libra)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < libraMaterials.Count; j++)
                        {
                            if (!shopsDict[shop].Contains(libraMaterials[j]))
                            {
                                shopsDict[shop].Add(libraMaterials[j]);
                                libraMaterials.RemoveAt(j);
                                break;
                            }
                        }
                    }
                }

                shops[shop].SetItems(shopsDict[shop].OrderBy(s => s.StartsWith("e")).ThenByDescending(s => equipRando.items[s].uItemNum).ToList());
                shops[shop].SetItemFlags(Enumerable.Range(0, 32).Select(_ => 0).ToList());
            }

            RandomNum.ClearRand();
        }
    }
    public List<string> GetRandomizableEquip()
    {
        if (!LRFlags.Items.Shops.FlagEnabled)
        {
            return new List<string>();
        }

        static bool isEquip(string s)
        {
            return s.StartsWith("cos") || s.StartsWith("wea") || s.StartsWith("shi");
        }

        List<string> list = new();
        list.AddRange(shopsOrig.Values.SelectMany(s => s.GetItems().Where(i => isEquip(i))));

        return list.Distinct().ToList();
    }
    public List<string> GetAdornments()
    {
        if (!LRFlags.Items.Shops.FlagEnabled)
        {
            return new List<string>();
        }

        static bool isAdorn(string s)
        {
            return s.StartsWith("e") && s.Length == 4;
        }

        List<string> list = new();
        list.AddRange(shopsOrig.Values.SelectMany(s => s.GetItems().Where(i => isAdorn(i))));

        return list.Distinct().ToList();
    }
    public List<string> GetItems()
    {
        if (!LRFlags.Items.Shops.FlagEnabled)
        {
            return new List<string>();
        }

        static bool isItem(string s)
        {
            return s.StartsWith("it");
        }

        List<string> list = new();
        list.AddRange(shopsOrig.Values.SelectMany(s => s.GetItems().Where(i => isItem(i))));

        return list.Distinct().ToList();
    }

    private bool IsValid(string item, ShopCategory shop)
    {
        return shop is ShopCategory.Ark or ShopCategory.Items
            ? item.StartsWith("it")
            : shop == ShopCategory.Forge
            ? (item.StartsWith("e") && item.Length == 4) || item.StartsWith("wea") || item.StartsWith("shi")
            : shop == ShopCategory.Outfitters
&& ((item.StartsWith("e") && item.Length == 4) || item.StartsWith("cos") || item.StartsWith("acc"));
    }

    public override void Save()
    {
        Randomizers.SetUIProgress("Saving Shop Data...", -1, 100);
        shops.SaveDB3(@"\db\resident\shop.wdb");
    }

    public override Dictionary<string, HTMLPage> GetDocumentation()
    {
        Dictionary<string, HTMLPage> pages = base.GetDocumentation();
        EquipRando equipRando = Randomizers.Get<EquipRando>();
        AbilityRando abilityRando = Randomizers.Get<AbilityRando>();
        TextRando textRando = Randomizers.Get<TextRando>();
        HTMLPage page = new("Shops", "template/documentation.html");

        shopData.Keys.Select(s => shops[s]).Where(s => s.u3Category is ((int)ShopCategory.Ark) or ((int)ShopCategory.Forge) or ((int)ShopCategory.Items) or ((int)ShopCategory.Libra) or ((int)ShopCategory.Outfitters)).ForEach(shop =>
           {
               ShopData shopInfo = shopData[shop.name];
               string name = $"{shopInfo.Area} {shopInfo.SubArea} - {textRando.mainSysUS[shop.sShopNameLabel_string]}";
               if (shopInfo.DayStart != -1)
               {
                   name += $" - Day {shopInfo.DayStart}";
                   if (shopInfo.DayEnd != -1)
                   {
                       name += $" - {shopInfo.DayEnd}";
                   }
                   else
                   {
                       name += "+";
                   }
               }

               if (!string.IsNullOrEmpty(shopInfo.AdditionalInfo))
               {
                   name += $" ({shopInfo.AdditionalInfo})";
               }

               page.HTMLElements.Add(new Table(name, (new string[] { "New Contents" }).ToList(), (new int[] { 100 }).ToList(), shop.GetItems().Select(itemID =>
               {
                   string name;
                   if (itemID == "")
                   {
                       name = "Gil";
                   }
                   else if (abilityRando.abilities.Keys.Contains(itemID))
                   {
                       name = textRando.mainSysUS[abilityRando.abilities[itemID].sStringResId_string];
                   }
                   else
                   {
                       name = textRando.mainSysUS[equipRando.items[itemID].sItemNameStringId_string].Replace("{Var83 182}", "Omega");
                       if (name.Contains("{End}"))
                       {
                           name = name.Substring(0, name.IndexOf("{End}"));
                       }
                   }

                   return (new string[] { name }).ToList();
               }).ToList()));
           });
        pages.Add("shops", page);
        return pages;
    }

    public class ShopData : CSVDataRow
    {
        [RowIndex(0)]
        public string ID { get; set; }
        [RowIndex(1)]
        public string Area { get; set; }
        [RowIndex(2)]
        public string SubArea { get; set; }
        [RowIndex(3)]
        public string AdditionalInfo { get; set; }
        [RowIndex(4)]
        public int DayStart { get; set; }
        [RowIndex(5)]
        public int DayEnd { get; set; }
        public ShopData(string[] row) : base(row)
        {
        }
    }
}
