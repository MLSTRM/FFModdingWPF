using Bartz24.RandoWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LRRando;
public class APEquipRando : EquipRando
{
    public APEquipRando(SeedGenerator randomizers) : base(randomizers)
    {
    }

    public override void Load()
    {
        base.Load();

        var apItem = items.InsertCopyAlphabetical("key_b_20", "key_r_apitem");
        apItem.sItemNameStringId_string = "$item_apitem";
        apItem.sHelpStringId_string = "$item_apitem_desc";
        apItem.u16SortAllByKCategory = 101;
        apItem.u16SortCategoryByCategory = 151;
    }
}
