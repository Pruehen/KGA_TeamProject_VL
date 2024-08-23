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
    [JsonProperty] public string Desc { get; private set; }
    [JsonProperty] public string IconName { get; private set; }
    [JsonProperty] public Dictionary<int, List<float>> Level_VelueList { get; private set; }

    [JsonConstructor]
    public BlueChip(string name, string desc, string iconName, Dictionary<int, List<float>> level_VelueList)
    {        
        Name = name;
        Desc = desc;
        IconName = iconName;
        Level_VelueList = level_VelueList;
    }
    public BlueChip(int id)
    {        
        Name = "���Ĩ �̸�";
        Desc = "���Ĩ ����";
        IconName = "������ �̸�";
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
        return string.Format(Desc, Level_VelueList[level].Cast<object>().ToArray());
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

public class PassiveData
{
    [JsonProperty] public string Name { get; private set; }
    [JsonProperty] public string Desc { get; private set; }
    [JsonProperty] public string IconPath { get; private set; }
    [JsonProperty] public string IconPath_Dis { get; private set; }
    [JsonProperty] public int Cost { get; private set; }
    [JsonProperty] public List<float> VelueList { get; private set; }

    [JsonConstructor]
    public PassiveData(string name, string desc, string iconPath, string iconPath_Dis, int cost, List<float> velueList)
    {
        Name = name;
        Desc = desc;
        IconPath = iconPath;
        IconPath_Dis = iconPath_Dis;
        Cost = cost;
        VelueList = velueList;
    }
    public PassiveData(PassiveID iD)
    {
        Name = iD.ToString();
        Desc = "�ش� �нú꿡 ���� ����. �� {0}";
        IconPath = "������ ���";
        IconPath_Dis = "��Ȱ�� ������ ���";
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
        return string.Format(Desc, VelueList.Cast<object>().ToArray());
    }
}
public class PassiveTable
{
    public Dictionary<PassiveID, PassiveData> dic;
    [JsonConstructor]
    public PassiveTable(Dictionary<PassiveID, PassiveData> dic)
    {
        this.dic = dic;
    }
    public PassiveTable()
    {
        dic = new Dictionary<PassiveID, PassiveData>();
        foreach (PassiveID passiveType in Enum.GetValues(typeof(PassiveID)))
        {            
            dic.Add(passiveType, new PassiveData(passiveType));
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
    [JsonProperty] public DateTime SaveTime { get; private set; }
    [JsonProperty] public int PlayTime { get; private set; }
    [JsonProperty] public int Gold { get; private set; }
    [JsonProperty] public int Count_Try { get; private set; }
    [JsonProperty] public int Count_Clear { get; private set; }
    [JsonProperty] public HashSet<PassiveID> UnlockPassiveHashSet { get; private set; }
    [JsonProperty] public HashSet<PassiveID> UsePassiveHashSet { get; private set; }
    [JsonProperty] public HashSet<string> IsClearAchievementsKey { get; private set; }
    [JsonProperty] public PlayData PlayData { get; private set; }

    [JsonConstructor]   
    public UserData(
        int saveDataIndex,
        DateTime saveTime,
        int playTime,
        int gold,
        int countTry,
        int countClear,
        HashSet<PassiveID> unlockPassiveHashSet,
        HashSet<PassiveID> usePassiveHashSet,
        HashSet<string> isClearAchievementsKey,
        PlayData playData)
    {
        SaveDataIndex = saveDataIndex;
        SaveTime = saveTime;
        PlayTime = playTime;
        Gold = gold;
        Count_Try = countTry;
        Count_Clear = countClear;
        UnlockPassiveHashSet = unlockPassiveHashSet ?? new HashSet<PassiveID>();
        UsePassiveHashSet = usePassiveHashSet ?? new HashSet<PassiveID>();
        IsClearAchievementsKey = isClearAchievementsKey ?? new HashSet<string>();
        PlayData = playData ?? new PlayData();
    }

    public UserData(int saveDataIndex)
    {
        SaveDataIndex = saveDataIndex;
        SaveTime = DateTime.Now;
        PlayTime = 0;
        Gold = 0;
        Count_Try = 0;
        Count_Clear = 0;
        UnlockPassiveHashSet = new HashSet<PassiveID>();
        UsePassiveHashSet = new HashSet<PassiveID>();
        IsClearAchievementsKey = new HashSet<string>();
        PlayData = new PlayData();
    }

    public static void Save()
    {
        JsonDataManager.DataSaveCommand(JsonDataManager.jsonCache.UserDataCache, UserDataList.FilePath());
    }

    public void TryAddPassive(PassiveID id)
    {
        if(UsePassiveHashSet.Add(id))
        {
            Save();
            Debug.Log($"�нú� �߰� : {id}");
        }
        else
        {
            Debug.LogWarning($"�̹� �����ϴ� �нú� : {id}");
        }
    }

    public void TryRemovePassive(PassiveID id)
    {
        if (UsePassiveHashSet.Remove(id))
        {
            Save();
            Debug.Log($"�нú� ���� : {id}");            
        }
        else
        {
            Debug.LogWarning($"�������� �ʴ� �нú� : {id}");
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public bool TryUseGold(int amount)
    {
        if(Gold - amount < 0)
        {
            return false;
        }
        Gold -= amount;
        return true;
    }
}

public class PlayData
{
    [JsonProperty] public int InGame_Gold { get; private set; }
    [JsonProperty] public Dictionary<BlueChipID, int> InGame_BlueChip_Level { get; private set; }
    [JsonProperty] public int InGame_Stage { get; private set; }

    [JsonConstructor]
    public PlayData(int InGame_Gold, Dictionary<BlueChipID, int> InGame_BlueChip_Level, int InGame_Stage)
    {
        this.InGame_Gold = InGame_Gold;
        this.InGame_BlueChip_Level = InGame_BlueChip_Level;
        this.InGame_Stage = InGame_Stage;
    }

    public PlayData()
    {
        this.InGame_Gold = 0;
        this.InGame_BlueChip_Level = new Dictionary<BlueChipID, int>();
        this.InGame_Stage = 0;
    }

    internal void AddGold(int amount)
    {
        InGame_Gold += amount;
    }
}

public class UserDataList
{
    public Dictionary<int, UserData> dic;
    public int UseIndex { get; private set; }
    
    public UserDataList(Dictionary<int, UserData> dic)
    {
        this.dic = dic;
    }
    public UserDataList()
    {
        dic = new Dictionary<int, UserData>();        
    }
    public UserData GetUserData()
    {
        if(dic.ContainsKey(UseIndex))
        {
            //Debug.Log($"{UseIndex} ���̺����� �ε�");
            return dic[UseIndex];
        }
        else
        {
            Debug.Log("�ش��ϴ� Ű�� �����ϴ�. ���̺������� �����մϴ�.");
            dic.Add(UseIndex, new UserData(UseIndex));
            return dic[UseIndex];
        }
    }
    public UserData GetUserData(int index)
    {
        if (dic.ContainsKey(index))
        {
            //Debug.Log($"{UseIndex} ���̺����� �ε�");
            return dic[index];
        }
        else
        {
            return null;
        }
    }
    public void DeleteUserData(int index)
    {
        dic.Remove(index);        
    }
    public void SetUserDataIndex(int index)
    {
        if (dic.ContainsKey(index))
        {            
            UseIndex = index;
        }
        else
        {
            Debug.Log("���̺����� ����");
            UseIndex = index;
            dic.Add(index, new UserData(index));
        }
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