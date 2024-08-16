using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EnumTypes;
using System;

public class BlueChip
{    
    [JsonProperty] public string Name { get; private set; }
    [JsonProperty] public string Info { get; private set; }
    [JsonProperty] public Dictionary<int, List<float>> Level_VelueList { get; private set; }

    [JsonConstructor]
    public BlueChip(string name, string info, Dictionary<int, List<float>> level_VelueList)
    {        
        Name = name;
        Info = info;
        Level_VelueList = level_VelueList;
    }
    public BlueChip(int id)
    {        
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
    }
    public static string FilePath()
    {
        return "/Data/Table/BlueChipTable.json";
    }
}

public class CoefficientTable
{
    public Dictionary<PlayerAttackType, float> dic;
    [JsonConstructor]
    public CoefficientTable(Dictionary<PlayerAttackType, float> dic)
    {
        this.dic = dic;
    }
    public CoefficientTable()
    {
        dic = new Dictionary<PlayerAttackType, float>();

        foreach (PlayerAttackType attackType in Enum.GetValues(typeof(PlayerAttackType)))
        {
            // 각 attackType에 대해 dic에 항목을 추가합니다.
            dic.Add(attackType, 1);
        }
    }
    public static string FilePath()
    {
        return "/Data/Table/CoefficientTable.json";
    }
}


public class Passive
{
    [JsonProperty] public string Name { get; private set; }
    [JsonProperty] public string Info { get; private set; }
    [JsonProperty] public int Cost { get; private set; }
    [JsonProperty] public List<float> VelueList { get; private set; }

    [JsonConstructor]
    public Passive(string name, string info, int cost, List<float> velueList)
    {
        Name = name;
        Info = info;
        Cost = cost;
        VelueList = velueList;
    }
    public Passive(PassiveID iD)
    {
        Name = iD.ToString();
        Info = "해당 패시브에 대한 설명. 값 {0}";
        Cost = 100;
        VelueList = new List<float>();
        VelueList.Add(1);
    }

    public string PrintName()
    {
        return Name;
    }
    public string PrintInfo()
    {
        return string.Format(Info, VelueList.Cast<object>().ToArray());
    }
}
public class PassiveTable
{
    public Dictionary<PassiveID, Passive> dic;
    [JsonConstructor]
    public PassiveTable(Dictionary<PassiveID, Passive> dic)
    {
        this.dic = dic;
    }
    public PassiveTable()
    {
        dic = new Dictionary<PassiveID, Passive>();
        foreach (PassiveID passiveType in Enum.GetValues(typeof(PassiveID)))
        {            
            dic.Add(passiveType, new Passive(passiveType));
        }
    }
    public static string FilePath()
    {
        return "/Data/Table/PassiveTable.json";
    }
}

public class UserData
{
    [JsonProperty] public int SaveDataIndex { get; private set; }    

    [JsonConstructor]
    public UserData(Dictionary<PassiveID, Passive> dic)
    {
        //this.dic = dic;
    }
    public UserData()
    {

    }
    public static string FilePath(int index)
    {
        string fileName = $"SaveFile_{index}";
        return $"/Data/UserData/{fileName}.json";
    }
}

public class JsonDataCreator : MonoBehaviour
{
    public void Awake()
    {
        JsonDataManager.jsonCache.Lode();
        //JsonDataManager.jsonCache.Save();
    }
}
