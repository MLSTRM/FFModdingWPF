using Bartz24.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bartz24.FF12;

public class DataStoreLicense : DataStore
{
    public enum LicenseType
    {
        Weapon = 1, // SwordsBowsSpearsAxesHammersKatanasGreatswords, Blue
        Weapon2 = 2, // RodsStavesMacesMeasures, Pink
        Weapon3 = 3, // DaggersGunsPolesCrossbowsHandbombsNinjaSwords, Purple
        Armor = 4, // Default armor, (heavy armor) Pale Blue
        MysticArmor = 5, // Pale Blue
        LightArmor = 6, // Pale Blue
        Shield = 7, // Pale Blue
        Accessory = 8, // Rust
        Magick = 9, // Default magick, (white magick) Pale Pink
        BlackMagick = 10, // Pale Pink
        TimeMagick = 11, // Pale Pink
        GreenMagick = 12, // Pale Pink
        ArcaneMagick = 13, // Pale Pink
        Technick = 14, // Orange
        Augment = 15, // Green
        Augment2 = 16, // Green
        Augment3 = 17, // Green
        EssentialGambit = 18, // Pale Olive
        Quickening = 19, // Neon Orange
        Esper = 20, // Neon Teal
        Unused = 21,
        Unused2 = 22,
        Unused3 = 23,
        Unused4 = 24,
        Unused5 = 25,
        Unused6 = 26,
        Unused7 = 27,
        Unused8 = 28,
        Unused9 = 29,
        SecondBoard = 30
    }

    public enum LicenseRestriction
    {
        None = 0,
        Unique = 1,
        Quickening = 2,
        Esper = 3,
        Inaccessible = 4
    }

    public string ID { get => IntID.ToString("X4"); }
    public int IntID { get; set; }
    public ushort NameAddress
    {
        get => Data.ReadUShort(0x00);
        set => Data.SetUShort(0x00, value);
    }
    public ushort DescriptionAddress
    {
        get => Data.ReadUShort(0x02);
        set => Data.SetUShort(0x02, value);
    }

    public byte LPCost
    {
        get => Data.ReadByte(0x04);
        set => Data.SetByte(0x04, value);
    }

    public LicenseType Type
    {
        get => (LicenseType)Data.ReadByte(0x05);
        set => Data.SetByte(0x05, (byte)value);
    }

    public LicenseRestriction Restriction
    {
        get => (LicenseRestriction)Data.ReadByte(0x06);
        set => Data.SetByte(0x06, (byte)value);
    }

    public bool[] DefaultCharUnlock
    {
        get
        {
            bool[] unlock = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                unlock[i] = Data.ReadBinary(0x07, i);
            }
            return unlock;
        }
        set
        {
            for (int i = 0; i < 8; i++)
            {
                Data.SetBinary(0x07, i, value[i]);
            }
        }
    }

    public List<string> ContentsStr
    {
        get
        {
            return Contents.Select(i => i.ToString("X4")).ToList();
        }
        set
        {
            Contents = value.Select(s => Convert.ToUInt16(s, 16)).ToList();
        }
    }

    public List<ushort> Contents
    {
        get
        {
            return Enumerable.Range(0, 8).Select(i => Data.ReadUShort(0x08 + i * 2)).Where(s => s != 0xFFFF).ToList();
        }
        set
        {
            for (int i = 0; i < 8; i++)
            {
                Data.SetUShort(0x08 + i * 2, value.Count > i ? value[i] : (ushort)0xFFFF);
            }
        }
    }

    public override int GetDefaultLength()
    {
        return 0x18;
    }
}
