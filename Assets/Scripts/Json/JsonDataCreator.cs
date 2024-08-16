using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EnumTypes;
using System;

public class BlueChip
{
    [JsonProperty] public int Id { get; private set; }
    [JsonProperty] public string Name { get; private set; }
    [JsonProperty] public string Info { get; private set; }
    [JsonProperty] public Dictionary<int, List<float>> Level_VelueList { get; private set; }

    [JsonConstructor]
    public BlueChip(int id, string name, string info, Dictionary<int, List<float>> level_VelueList)
    {
        Id = id;
        Name = name;
        Info = info;
        Level_VelueList = level_VelueList;
    }
    public BlueChip(int id)
    {
        Id = id;
        Name = "블루칩 이름";
        Info = "블루칩 설명";
        Level_VelueList = new Dictionary<int, List<float>>();
        Level_VelueList.Add(1, new List<float>());
        Level_VelueList.Add(2, new List<float>());
        Level_VelueList.Add(3, new List<float>());
        Level_VelueList.Add(4, new List<float>());
        Level_VelueList.Add(5, new List<float>());
    }

    public string PrintName()
    {
        return Name;
    }
    public string PrintLevel(int level)
    {
        return $"{level} / {Level_VelueList.Count}";
    }
    public string PrintInfo(int level)
    {        
        return string.Format(Info, Level_VelueList[level].Cast<object>().ToArray());
    }
}

public class CoefficientType
{
    [JsonProperty] public string Name { get; private set; }
    [JsonProperty] public float Coefficient { get; private set; }

    [JsonConstructor]
    public CoefficientType(string name, float coefficient)
    {
        Name = name;
        Coefficient = coefficient;
    }
    public CoefficientType(PlayerAttackType type, float coefficient)
    {
        Name = type.ToString();
        Coefficient = coefficient;
    }
}

public class CoefficientTable
{
    public Dictionary<PlayerAttackType, CoefficientType> dic;
    [JsonConstructor]
    public CoefficientTable(Dictionary<PlayerAttackType, CoefficientType> dic)
    {
        this.dic = dic;
    }
    public CoefficientTable()
    {
        dic = new Dictionary<PlayerAttackType, CoefficientType>();

        foreach (PlayerAttackType attackType in Enum.GetValues(typeof(PlayerAttackType)))
        {
            // 각 attackType에 대해 dic에 항목을 추가합니다.
            dic.Add(attackType, new CoefficientType(attackType, 1));
        }
    }
    public static string FilePath()
    {
        return "/Data/Table/CoefficientTable.json";
    }
}
public class BlueChipTable
{
    public Dictionary<BlueChipID, BlueChip> dic;
    [JsonConstructor]
    public BlueChipTable(Dictionary<BlueChipID, BlueChip> dic)
    {
        this.dic = dic;
    }
    public BlueChipTable()
    {
        dic = new Dictionary<BlueChipID, BlueChip>();
        for (int i = 0; i < 9; i++)
        {
            dic.Add((BlueChipID)i, new BlueChip(i));
        }
    }
    public static string FilePath()
    {
        return "/Data/Table/BlueChipTable.json";
    }
}

public class JsonDataCreator : MonoBehaviour
{
    public void Awake()
    {
        JsonDataManager.jsonCache.Lode();
        JsonDataManager.DataSaveCommand(JsonDataManager.jsonCache.CoefficientTableCache, CoefficientTable.FilePath());
    }
}
