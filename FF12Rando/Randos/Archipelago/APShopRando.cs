using Bartz24.RandoWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF12Rando;
public class APShopRando : ShopRando
{
    public APShopRando(SeedGenerator randomizers) : base(randomizers)
    {
    }

    protected override HashSet<string> GetUsedAbilities()
    {
        EquipRando equipRando = Generator.Get<EquipRando>();
        return RandoFlags.GetArchipelagoData<FF12ArchipelagoData>().UsedItems.Select(s => equipRando.itemData.Values.FirstOrDefault(i => i.Name == s)).Where(i => i != null).Select(i => i.ID).ToHashSet();
    }
}
