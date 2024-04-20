using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bartz24.RandoWPF;
public abstract class ArchipelagoData
{
    public abstract void Parse(IDictionary<string, object> data);

    public abstract IDictionary<string, object> ToJsonObj();
}
