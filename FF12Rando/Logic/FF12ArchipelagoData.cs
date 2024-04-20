using Bartz24.RandoWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FF12Rando;
public class FF12ArchipelagoData : ArchipelagoData
{
    public string Version { get; set; }
    public HashSet<string> UsedItems { get; set; }
    public List<(string MapName, int Index)> Treasures { get; set; }
    public List<int> CharacterOrder { get; set; }
    public bool AllowSeitengrat { get; set; }

    public FF12ArchipelagoData()
    {
    }

    public override void Parse(IDictionary<string, object> data)
    {
        Version = (string)data["version"];
        UsedItems = ((List<object>)data["used_items"]).Select(o => (string)o).ToHashSet();
        Treasures = ((List<object>)data["treasures"]).Select(o => {
            var treasureData = (IDictionary<string, object>)o;
            return (MapName: (string)treasureData["map"], Index: (int)(long)treasureData["index"]);
            }).ToList();
        CharacterOrder = ((List<object>)data["character_order"]).Select(o => (int)(long)o).ToList();
        AllowSeitengrat = (long)data["allow_seitengrat"] == 1;
    }

    public override IDictionary<string, object> ToJsonObj()
    {
        var treasures = Treasures.Select(t => new Dictionary<string, object>
        {
            { "map", t.MapName },
            { "index", t.Index }
        }).ToList();

        return new Dictionary<string, object>
        {
            { "version", Version },
            { "used_items", UsedItems.ToList() },
            { "treasures", treasures },
            { "character_order", CharacterOrder },
            { "allow_seitengrat", AllowSeitengrat ? 1 : 0 }
        };
    }
}
