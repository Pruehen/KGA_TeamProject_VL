using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EnumTypes;
using System;
using Unity.VisualScripting;

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
        Name = "���Ĩ �̸�";
        Info = "���Ĩ ����";
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
        Info = "�ش� �нú꿡 ���� ����. �� {0}";
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
    [JsonProperty] public int Gold { get; private set; }
    [JsonProperty] public HashSet<PassiveID> UsePassiveHashSet { get; private set; }

    [JsonConstructor]
    public UserData(int saveDataIndex, int gold, HashSet<PassiveID> usePassiveHashSet)
    {
        SaveDataIndex = saveDataIndex;
        Gold = gold;
        UsePassiveHashSet = usePassiveHashSet;
        if(UsePassiveHashSet == null)
        {
            UsePassiveHashSet = new HashSet<PassiveID>();
        }
    }

    public UserData(int saveDataIndex)
    {
        SaveDataIndex = saveDataIndex;
        Gold = 0;
        UsePassiveHashSet = new HashSet<PassiveID>();
    }

    public void TryAddPassive(PassiveID id)
    {
        if(UsePassiveHashSet.Add(id))
        {
            Debug.Log($"�нú� �߰� : {id.DisplayName()}");
        }
        else
        {
            Debug.LogWarning($"�̹� �����ϴ� �нú� : {id.DisplayName()}");
        }
    }

    public void TryRemovePassive(PassiveID id)
    {
        if (UsePassiveHashSet.Remove(id))
        {
            Debug.Log($"�нú� ���� : {id.DisplayName()}");
        }
        else
        {
            Debug.LogWarning($"�������� �ʴ� �нú� : {id.DisplayName()}");
        }
    }
}

public class UserDataList
{
    public List<UserData> list;

    [JsonConstructor]
    public UserDataList(List<UserData> list)
    {
        this.list = list;
    }
    public UserDataList()
    {
        list = new List<UserData>();
        list.Add(new UserData(0));
    }
    public static string FilePath()
    {        
        return $"/Data/UserData/SaveFile.json";
    }
}

public class JsonDataCreator : MonoBehaviour
{
    public void Awake()
    {
        JsonDataManager.jsonCache.Lode();
        JsonDataManager.jsonCache.Save();
    }
}
