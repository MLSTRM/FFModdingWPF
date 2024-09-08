using Bartz24.RandoWPF;
using System.Collections.Generic;

namespace FF12Rando;

public class AugmentData : CSVDataRow
{
    [RowIndex(0)]
    public string Name { get; set; }
    [RowIndex(1)]
    public int IntID { get; set; }
    public string ID { get; set; }
    [RowIndex(2)]
    public string Description { get; set; }
    [RowIndex(3)]
    public List<string> Traits { get; set; }
    [RowIndex(4)]
    public int Value { get; set; }
    public AugmentData(string[] row) : base(row)
    {
        ID = IntID.ToString("X4");
    }
}
