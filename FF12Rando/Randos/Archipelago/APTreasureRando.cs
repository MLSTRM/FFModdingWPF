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
    }
}
