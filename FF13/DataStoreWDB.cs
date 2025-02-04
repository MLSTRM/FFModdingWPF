﻿using Bartz24.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bartz24.FF13;

public class DataStoreWDB<T> where T : DataStoreWDBEntry, new()
{
    private readonly Dictionary<string, T> Data = new();
    private DataStoreStringPointerList StringList;
    private Dictionary<string, string> StringPointerMapping;
    private Dictionary<string, string> StringPointerEndingMapping;

    public T this[string id] => Data[id];

    public List<string> Keys => Data.Keys.ToList();
    public List<T> Values => Data.Values.ToList();

    public void Add(string id, T data)
    {
        Data.Add(id, data);
    }
    public void Add(T data)
    {
        Data.Add(data.ID, data);
    }
    public T Copy(string original, string newName)
    {
        T newData = new();
        newData.LoadData(new byte[Data[original].Data.Length]);
        Data[original].CopyPropertiesTo(newData);
        newData.LoadData(Data[original].Data.ToArray());
        newData.ID = newName;
        Add(newData);

        return newData;
    }

    public void Swap(string name1, string name2)
    {
        Data.Swap(name1, name2);
        Data[name1].ID = name2;
        Data[name2].ID = name1;
    }

    public void Rename(string name, string newName)
    {
        Copy(name, newName);
        Data.Remove(name);
    }

    public void Load(string game, string path, string novaPath)
    {
        Nova.UnpackWPD(path, novaPath);

        string folderPath = Path.Combine(Path.GetDirectoryName(path), "_" + Path.GetFileName(path));

        FindStringMappings();

        LoadStringList(folderPath);
        LoadData(folderPath);
    }

    private void LoadData(string path)
    {
        foreach (string filePath in Directory.GetFiles(path).Where(s => !Path.GetFileName(s).StartsWith("!!")))
        {
            T newEntry = new()
            {
                ID = Path.GetFileName(filePath)
            };
            newEntry.LoadData(File.ReadAllBytes(filePath));
            Add(newEntry);
        }

        UpdateStringValues();
    }
    private void LoadStringList(string path)
    {
        string filePath = path + "\\!!string";

        StringList = new DataStoreStringPointerList(new DataStoreString() { Value = "" });
        StringList.LoadData(File.ReadAllBytes(filePath));
    }

    private void FindStringMappings()
    {
        StringPointerMapping = new Dictionary<string, string>();
        StringPointerEndingMapping = new Dictionary<string, string>();
        foreach (PropertyInfo p in typeof(T).GetProperties())
        {
            if (p.Name.EndsWith("_pointer"))
            {
                string prefix = p.Name.Substring(0, p.Name.LastIndexOf("_pointer"));
                StringPointerMapping.Add(prefix + "_pointer", prefix + "_string");
            }

            if (p.Name.EndsWith("_pointer_end"))
            {
                string prefix = p.Name.Substring(0, p.Name.LastIndexOf("_pointer_end"));
                StringPointerEndingMapping.Add(prefix + "_pointer", prefix + "_pointer_end");
            }
        }
    }

    public void Save(string path, string novaPath)
    {
        UpdateStringPointers();

        string folderPath = Path.Combine(Path.GetDirectoryName(path), "_" + Path.GetFileName(path));
        SaveData(folderPath);
        SaveStringList(folderPath);

        List<string> sortOrder = new();
        PreWorkaround(folderPath, sortOrder);
        Nova.RepackWPD(path, novaPath);
        PostWorkaround(path, sortOrder);
    }

    private void UpdateStringPointers()
    {
        Values.ForEach(entry =>
        {
            StringPointerMapping.ForEach(p =>
            {
                string value = entry.GetPropValue<string>(p.Value);
                DataStoreString s = new() { Value = value };
                if (value != "" && !StringList.Contains(s))
                {
                    StringList.Add(s, StringList.Length);
                }
            });
        });

        StringList.UpdatePointers();

        Values.ForEach(entry =>
        {
            StringPointerMapping.ForEach(p =>
            {
                string value = entry.GetPropValue<string>(p.Value);
                uint pointer = entry.GetPropValue<uint>(p.Key);
                if (value != "")
                {
                    DataStoreString match = StringList.ToList().FirstOrDefault(s => s.Value == value);
                    entry.SetPropValue(p.Key, (uint)StringList.IndexOf(match));
                }
                else
                {
                    DataStoreString match = StringList.ToList().Skip(1).FirstOrDefault();
                    entry.SetPropValue(p.Key, (uint)StringList.IndexOf(match) - 1);
                }
            });
            StringPointerEndingMapping.ForEach(p =>
            {
                string value = entry.GetPropValue<string>(StringPointerMapping[p.Key]);
                uint pointer = entry.GetPropValue<uint>(p.Key);
                entry.SetPropValue(p.Value, (uint)(pointer + value.Length));
            });
        });
    }

    private void UpdateStringValues()
    {
        Values.ForEach(entry =>
        {
            StringPointerMapping.ForEach(p =>
            {
                string value = entry.GetPropValue<string>(p.Value);
                uint pointer = entry.GetPropValue<uint>(p.Key);
                entry.SetPropValue(p.Value, StringList[(int)pointer].Value);
            });
        });
    }

    private void SaveData(string path)
    {
        Directory.GetFiles(path).Where(s => !Path.GetFileName(s).StartsWith("!!")).ForEach(s => File.Delete(s));
        foreach (string id in Data.Keys)
        {
            File.WriteAllBytes(path + "\\" + id, Data[id].Data);
        }
    }
    private void SaveStringList(string path)
    {
        string filePath = path + "\\!!string";
        File.WriteAllBytes(filePath, StringList.Data);
    }

    private void PreWorkaround(string path, List<string> sortOrder)
    {

        Directory.GetFiles(path).Where(s => !Path.GetFileName(s).StartsWith("!!")).OrderBy(s => Path.GetFileName(s), StringComparer.Ordinal).ForEach(s =>
        {
            string fixPath = Path.Combine(Path.GetDirectoryName(s), "_" + sortOrder.Count.ToString("000000"));
            File.Move(s, fixPath);
            sortOrder.Add(Path.GetFileName(s));
        });
    }
    private void PostWorkaround(string path, List<string> sortOrder)
    {
        byte[] bytes = File.ReadAllBytes(path);
        for (int i = 0x90; i < bytes.ReadUInt(0x20); i += 0x20)
        {
            bytes.SetString(i, sortOrder[0], 16);
            sortOrder.RemoveAt(0);
        }

        File.WriteAllBytes(path, bytes);
    }
}
