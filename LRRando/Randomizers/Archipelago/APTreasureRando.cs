using Bartz24.Data;
using Bartz24.RandoWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRRando;
public class APTreasureRando : TreasureRando
{
    public APTreasureRando(SeedGenerator randomizers) : base(randomizers)
    {
    }

    public override void Randomize()
    {
        ItemLocations.Values.ForEach(l => l.SetItem("key_r_apitem", 1));
    }
}
