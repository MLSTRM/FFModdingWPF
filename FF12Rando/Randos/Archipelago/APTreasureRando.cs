using Bartz24.Data;
using Bartz24.RandoWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF12Rando;
class APTreasureRando : TreasureRando
{
    public APTreasureRando(SeedGenerator randomizers) : base(randomizers)
    {
    }

    public override void Randomize()
    {
        ItemLocations.Values.ForEach(l => l.SetItem("80E6", 1));
        ItemLocations.Values.Where(l=>l is RewardLocation r && r.Index != 1).ForEach(l => l.SetItem(null, 0));
        ItemLocations.Values.Where(l => l is StartingInvLocation s && s.Index > 0).ForEach(l => l.SetItem(null, 0));

        var treasures = RandoFlags.GetArchipelagoData<FF12ArchipelagoData>().Treasures.Select(data => ItemLocations.Values.First(l => l is TreasureLocation t && t.MapID == data.MapName && t.Index == data.Index)).Select(l => (TreasureLocation)l).ToList();

        SetTreasureRespawns(treasures);

        if (!FF12Flags.Items.Shops.FlagEnabled)
        {
            List<ItemLocation> initialAbilities = ItemLocations.Values.Where(l =>
            l is StartingInvLocation && l.GetItem(true) != null &&
            (l.GetItem(true).Value.Item.StartsWith("30") || l.GetItem(true).Value.Item.StartsWith("40"))).ToList();

            initialAbilities.ForEach(l =>
            {
                // Find the next empty spot for that character and add there
                var nextEmpty = ItemLocations.Values.Where(other => other is StartingInvLocation s && s.IntID == ((StartingInvLocation)l).IntID && s.GetItem(false) == null).First();
                nextEmpty.SetItem(l.GetItem(true).Value.Item, l.GetItem(true).Value.Amount);
            });
        }
    }
}
