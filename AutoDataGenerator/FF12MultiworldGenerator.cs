using FF12Rando;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Bartz24.Data;
using Bartz24.FF12;
using Bartz24.RandoWPF;
using static System.Windows.Forms.AxHost;
using System.Xml.Linq;

namespace AutoDataGenerator;
internal class FF12MultiworldGenerator
{
    public string OutputDir { get; }

    TreasureRando TreasureRando { get; }
    EquipRando EquipRando { get; }
    PartyRando PartyRando { get; }
    ShopRando ShopRando { get; }

    const long BASE_ID = 760701597784;

    Dictionary<ItemLocation, string> locations = new();

    public FF12MultiworldGenerator(string inputDir, string outputDir)
    {
        SetupData.Paths["12"] = "G:\\SteamLibrary\\steamapps\\common\\FINAL FANTASY XII THE ZODIAC AGE\\x64\\FFXII_TZA.exe";
        DataExtensions.Mode = ByteMode.LittleEndian;
        FF12Flags.Init();

        OutputDir = outputDir;
        var seedGenerator = new FF12SeedGenerator();
        TreasureRando = seedGenerator.Get<TreasureRando>();
        EquipRando = seedGenerator.Get<EquipRando>();
        PartyRando = seedGenerator.Get<PartyRando>();
        ShopRando = seedGenerator.Get<ShopRando>();

        // Set working directory to the input directory
        Directory.SetCurrentDirectory(inputDir);

        // Enable starting inv flag
        FF12Flags.Items.KeyStartingInv.Enabled = true;
        PartyRando.Load();
        EquipRando.Load();
        TreasureRando.Load();
        ShopRando.Load();
    }

    public void Generate()
    {
        GenerateItemsScript();
        GenerateLocationsScript();
        GenerateEventsScript();
        GenerateRulesScript();
    }

    private void GenerateItemsScript()
    {        
        // Auto generate the Items.py script
        string script =
            "from typing import Dict, NamedTuple, Optional\n" +
            "from BaseClasses import Item, ItemClassification\n" +
            "\n" +
            "\n" +
            $"FF12OW_BASE_ID = {BASE_ID}\n" +
            "\n" +
            "\n" +
            "class FF12OpenWorldItem(Item):\n" +
            "    game: str = \"Final Fantasy 12 Open World\"\n" +
            "\n" +
            "\n" +
            "class FF12OpenWorldItemData(NamedTuple):\n" +
            "    code: Optional[int] = None\n" +
            "    classification: ItemClassification = ItemClassification.filler\n" +
            "    weight: int = 0\n" +
            "    amount: int = 1\n" +
            "    duplicateAmount: int = 1\n" +
            "\n" +
            "\n" +
            "item_data_table: Dict[str, FF12OpenWorldItemData] = {\n";

        EquipRando.itemData.Values.ForEach(i =>
        {
            if (!i.Traits.Contains("Ignore"))
            {
                string type = "filler";
                int weight = 0;
                int duplicates = 1;
                if ((i.Category == "Key" || i.Category == "Esper" || i.Category == "Board") && !i.Traits.Contains("Trophy"))
                {
                    type = "progression";
                    int count = TreasureRando.ItemLocations.Values.Where(l => l.GetItem(true) != null && l.GetItem(true)?.Item == i.ID).Count();
                    // Allow duplicates except for writ of transit
                    if (count > 1 && i.IntID != 0x8070)
                    {
                        duplicates = count;
                        type = "progression_skip_balancing";
                    }
                }
                else if (i.Category == "Ability")
                {
                    type = "useful";
                }
                else if (i.Category == "Loot")
                {
                    weight = 10;
                }
                else if (i.Traits.Contains("Trophy"))
                {
                    weight = 0;
                }
                else
                {
                    // Weight is a value based on rank using exponential decay from 20 to 1
                    // Rank goes up to 10 using this formula
                    weight = i.Rank > 10 ? 1 : (int)Math.Ceiling(20 * Math.Pow(0.7, i.Rank) * 10);

                    if (i.Category == "Item")
                    {
                        weight *= 2;
                    }
                }

                script = AddItemToItemsScript(script, i.Name, i.IntID, type, weight, 1, duplicates);
            }
        });

        int[] gilAmounts = new[] { 1, 500, 1000, 5000, 10000, 25000 };
        int[] gilWeights = new[] { 250, 900, 1150, 800, 500, 200 };
        for (int i = 0; i < gilAmounts.Length; i++)
        {
            script = AddItemToItemsScript(script, $"{gilAmounts[i]} Gil", 0x18000 + i, "filler", gilWeights[i], gilAmounts[i], 0);
        }

        script += "}\n";

        script += "\n" +
            "item_table = {name: data.code for name, data in item_data_table.items()}\n" +
            "inv_item_table = {data.code: name for name, data in item_data_table.items()}\n" +
            "\n" +
            "filler_items = [name for name, data in item_data_table.items()\n" +
            "                if data.classification == ItemClassification.filler and data.weight > 0]\n" +
            "filler_weights = [item_data_table[name].weight for name in filler_items]\n";

        File.WriteAllText(Path.Combine(OutputDir, "Items.py"), script);
    }

    private static string AddItemToItemsScript(string script, string name, int id, string type, int weight, int amount, int duplicates)
    {
        script += 
            $"    \"{name}\": FF12OpenWorldItemData(\n" +
            $"        code=FF12OW_BASE_ID + {id},\n" +
            $"        classification=ItemClassification.{type}";
        if (weight > 0)
        {
            script += $",\n" +
                $"        weight={weight}";
        }


        if (amount != 1)
        {
            script += $",\n" +
                $"        amount={amount}";
        }

        if (duplicates != 1)
        {
            script += $",\n" +
                $"        duplicateAmount={duplicates}";
        }

        script += "\n    ),\n";
        return script;
    }

    private void GenerateLocationsScript()
    {
        string script =
            "from typing import Dict, NamedTuple, Optional\n" +
            "from BaseClasses import Location, LocationProgressType\n" +
            "from .Items import FF12OW_BASE_ID\n" +
            "\n" +
            "\n" +
            "class FF12OpenWorldLocation(Location):\n" +
            "    game: str = \"Final Fantasy 12 Open World\"\n" +
            "\n" +
            "\n" +
            "class FF12OpenWorldLocationData(NamedTuple):\n" +
            "    region: str\n" +
            "    type: str\n" +
            "    str_id: str\n" +
            "    address: Optional[int] = None\n" +
            "    classification: LocationProgressType = LocationProgressType.DEFAULT\n" +
            "    secondary_index: int = 0\n" +
            "    difficulty: int = 0" +
            "\n" +
            "\n" +
            "location_data_table: Dict[str, FF12OpenWorldLocationData] = {\n";

        locations.Clear();

        int nextIndex = 0;
        TreasureRando.ItemLocations.Values.Where(l => (!l.Traits.Contains("Missable") || l is not TreasureLocation) && l is not FakeLocation).ToList().ForEach(l =>
        {
            string classification = "DEFAULT";
            if (l.Traits.Contains("Missable"))
            {
                classification = "EXCLUDED";
            }

            string name;
            switch (l)
            {
                case RewardLocation r:
                    name = $"{r.Name} ({r.Index + 1})";
                    script = AddLocationToLocationsScript(script, name, nextIndex, classification, "reward", r.IntID.ToString("X4"), r.Index, l.BaseDifficulty);
                    break;
                case TreasureLocation t:
                    name = $"{t.Name} {t.Index + 1}";
                    script = AddLocationToLocationsScript(script, name, nextIndex, classification, "treasure", t.MapID, t.Index, l.BaseDifficulty);
                    break;
                case StartingInvLocation s:
                    name = $"{s.Name} ({s.Index + 1})";
                    script = AddLocationToLocationsScript(script, name, nextIndex, classification, "inventory", s.IntID.ToString(), s.Index, l.BaseDifficulty);
                    break;
                default:
                    throw new Exception("Unknown location type");
            }

            locations.Add(l, name);

            nextIndex++;
        });

        script += "}\n";

        script += "\nlocation_table = {location_name: location_data.address for location_name, location_data in location_data_table.items()}";

        File.WriteAllText(Path.Combine(OutputDir, "Locations.py"), script);
    }

    private string AddLocationToLocationsScript(string script, string name, int id, string classification, string type, string strId, int index, int difficulty)
    {
        // Map ID and secondary index are optional
        script +=
            $"    \"{name}\": FF12OpenWorldLocationData(\n" +
            $"        region=\"Ivalice\",\n" +
            $"        address=FF12OW_BASE_ID + {id},\n" +
            $"        classification=LocationProgressType.{classification},\n" +
            $"        type=\"{type}\"";
        if (!string.IsNullOrEmpty(strId))
        {
            script += $",\n" +
                $"        str_id=\"{strId}\"";
        }

        if (index > 0)
        {
            script += $",\n" +
                $"        secondary_index={index}";
        }

        if (difficulty > 0)
        {
            script += $",\n" +
                $"        difficulty={difficulty}";
        }

        script += "\n    ),\n";

        return script;
    }

    private void GenerateEventsScript()
    {
        string script =
            "from typing import Dict, NamedTuple\n" +
            "\n" +
            "\n" +
            "class FF12OpenWorldEventData(NamedTuple):\n" +
            "    item: str\n" +
            "    difficulty: int = 0\n" +
            "\n" +
            "\n" +
            "event_data_table: Dict[str, FF12OpenWorldEventData] = {\n";

        Dictionary<string, int> usedNames = new();
        TreasureRando.ItemLocations.Values.Where(l => l is FakeLocation).ForEach(l =>
        {
            FakeLocation fake = (FakeLocation)l;
            if (usedNames.ContainsKey(fake.Name))
            {
                usedNames[fake.Name]++;
            }
            else
            {
                usedNames.Add(fake.Name, 1);
            }
            string newName = $"{fake.Name} Event ({usedNames[fake.Name]})";

            script += $"    \"{newName}\": FF12OpenWorldEventData(\n" +
            $"        item=\"{fake.FakeItem}\",\n" +
            $"        difficulty={l.BaseDifficulty}\n" +
            $"    ),\n";

            locations.Add(l, newName);
        });

        script += "}\n";
        File.WriteAllText(Path.Combine(OutputDir, "Events.py"), script);
    }

    private void GenerateRulesScript()
    {
        string script =
            "from typing import Callable, Dict, List\n" +
            "from BaseClasses import CollectionState\n" +
            "from .RuleLogic import state_has_aerodromes, state_has_at_least" +
            "\n" +
            "\n" +
            "rule_data_list: List[Callable[[CollectionState, int], bool]] = [\n";

        Dictionary<string, string> locationToRules = new();
        List<string> rules = new();

        locations.Keys.ForEach(l =>
        {
            string ruleStr = l.GetArchipelagoRule(TreasureRando.GetItemName);
            if (!rules.Contains(ruleStr))
            {
                rules.Add(ruleStr);
            }

            locationToRules.Add(locations[l], ruleStr);
        });

        for (int i = 0; i < rules.Count; i++)
        {
            script += $"    {rules[i]},\n";
        }

        script += "]\n\n";

        script += "rule_data_table: Dict[str, Callable[[CollectionState, int], bool]] = {\n";
        locationToRules.Keys.ForEach(l =>
        {
            script += $"    \"{l}\": rule_data_list[{rules.IndexOf(locationToRules[l])}],\n";
        });

        script += "}\n";

        File.WriteAllText(Path.Combine(OutputDir, "Rules.py"), script);

    }
}
