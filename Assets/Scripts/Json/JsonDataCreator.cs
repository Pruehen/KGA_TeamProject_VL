using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EnumTypes;
using System;

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
    [JsonProperty] public HashSet<string> IsClearAchievementsKey { get; private set; }//Ŭ������ ���� ���
    [JsonProperty] public Dictionary<int, bool> SlotIndex_SlotUnlock_Dic { get; private set; }
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
        Dictionary<int, bool> slotIndex_SlotUnlock_Dic,
        PlayData playData
        )
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
        SlotIndex_SlotUnlock_Dic = slotIndex_SlotUnlock_Dic;
        PlayData = playData;

        if (SlotIndex_SlotUnlock_Dic == null)
        {
            SlotIndex_SlotUnlock_Dic = new Dictionary<int, bool>();
            SlotIndex_SlotUnlock_Dic.Add(0, true);
            SlotIndex_SlotUnlock_Dic.Add(2, true);
            SlotIndex_SlotUnlock_Dic.Add(4, true);

            SlotIndex_SlotUnlock_Dic.Add(1, false);
            SlotIndex_SlotUnlock_Dic.Add(3, false);
            SlotIndex_SlotUnlock_Dic.Add(5, false);
        }
    }

    public UserData(int saveDataIndex)
    {
        SaveDataIndex = saveDataIndex;
        SaveTime = DateTime.Now;
        PlayTime = 0;
        Gold = 1000;
        Count_Try = 0;
        Count_Clear = 0;
        UnlockPassiveHashSet = new HashSet<PassiveID>();
        UsePassiveHashSet = new HashSet<PassiveID>();
        IsClearAchievementsKey = new HashSet<string>();
        PlayData = null;

        SlotIndex_SlotUnlock_Dic = new Dictionary<int, bool>();
        SlotIndex_SlotUnlock_Dic.Add(0, true);
        SlotIndex_SlotUnlock_Dic.Add(2, true);
        SlotIndex_SlotUnlock_Dic.Add(4, true);

        SlotIndex_SlotUnlock_Dic.Add(1, false);
        SlotIndex_SlotUnlock_Dic.Add(3, false);
        SlotIndex_SlotUnlock_Dic.Add(5, false);
    }

    public static void Save()
    {
        JsonDataManager.DataSaveCommand(JsonDataManager.jsonCache.UserDataCache, UserDataList.FilePath());
    }

    public void SavePlayData_OnSceneExit(PlayerInstanteState state, PlayerEquipBlueChip equipBlueChip)//�� ��ȯ �� ȣ��
    {
        if (PlayData == null)
        {
            PlayData = new PlayData();
        }
        PlayData.SavePlayData_OnSceneExit(state, equipBlueChip);
        Save();
    }
    public void SavePlayData_OnSceneEnter(string newStage)//�� ���� �� ȣ��
    {
        if(PlayData != null)
        {
            PlayData.SavePlayData_OnSceneEnter(newStage);
            Save();
        }
    }

    public bool TryGetPlayData(out PlayData playData)
    {
        if(PlayData == null)
        {
            playData = null;
            return false;
        }
        else
        {
            playData = PlayData;
            return true;
        }
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
    public void TryUnLockPassive(PassiveID id)
    {
        if (UnlockPassiveHashSet.Add(id))
        {
            Save();
            Debug.Log($"�нú� �ر� : {id}");
        }
        else
        {
            Debug.LogWarning($"�̹� �رݵ� �нú� : {id}");
        }
    }
    public void TryUnLockPassiveSlot(int slotIndex)
    {
        if(SlotIndex_SlotUnlock_Dic.ContainsKey(slotIndex))
        {
            SlotIndex_SlotUnlock_Dic[slotIndex] = true;
        }
        else
        {
            Debug.LogError("���� �ε����� ������ ������ϴ�.");
        }
    }
    public bool AvailableSlot(int slotIndex)
    {
        if (SlotIndex_SlotUnlock_Dic.ContainsKey(slotIndex) == false)
        {
            Debug.LogError("���� �ε����� ������ ������ϴ�.");
            return false;
        }
        return SlotIndex_SlotUnlock_Dic[slotIndex];
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

    public void ClearAndSaveUserData()
    {
        PlayData.Clear();
        Save();
    }

    public void InitPlayData(int gold)
    {
        PlayData = new PlayData();
        PlayData.AddGold_InGame(gold);
    }
}

public class PlayData
{
    [JsonProperty] public int InGame_Gold { get; private set; }
    [JsonProperty] public Dictionary<BlueChipID, int> InGame_BlueChip_Level { get; private set; }
    [JsonProperty] public string InGame_Stage { get; private set; }
    [JsonProperty] public float InGame_Hp { get; private set; }
    [JsonProperty] public float InGame_SkillGauge { get; private set; }
    [JsonProperty] public int InGame_Bullet { get; private set; }
    [JsonProperty] public int InGame_MeleeBullet { get; private set; }

    [JsonConstructor]
    public PlayData(int InGame_Gold, Dictionary<BlueChipID, int> InGame_BlueChip_Level, string InGame_Stage, float inGame_Hp, float inGame_SkillGauge, int inGame_Bullet, int inGame_MeleeBullet)
    {
        this.InGame_Gold = InGame_Gold;
        this.InGame_BlueChip_Level = InGame_BlueChip_Level;
        this.InGame_Stage = InGame_Stage;
        InGame_Hp = inGame_Hp;
        InGame_SkillGauge = inGame_SkillGauge;
        InGame_Bullet = inGame_Bullet;
        InGame_MeleeBullet = inGame_MeleeBullet;
    }

    public PlayData() 
    {
        InGame_BlueChip_Level = new Dictionary<BlueChipID, int>();
    }
    
    public void SavePlayData_OnSceneExit(PlayerInstanteState state, PlayerEquipBlueChip equipBlueChip)//�� ��ȯ �� ȣ��
    {        
        InGame_BlueChip_Level.Clear();
        foreach (var item in equipBlueChip.GetBlueChipDic())
        {
            InGame_BlueChip_Level.Add(item.Key, item.Value.Level);
        }
        InGame_Hp = state.hp;
        InGame_SkillGauge = state.skillGauge;
        InGame_Bullet = state.bullets;
        InGame_MeleeBullet = state.meleeBullets;
        JsonDataManager.GetUserData().AddGold(InGame_Gold - JsonDataManager.GetUserData().Gold);
    }    
    public void SavePlayData_OnSceneEnter(string newStage)//�� ���� �� ȣ��
    {
        InGame_Stage = newStage;
    }
    public void InitGold_InGame(int amount)
    {
        InGame_Gold = amount;
    }
    public void AddGold_InGame(int amount)
    {
        InGame_Gold += amount;
    }

    public void Clear()
    {
        PlayerInstanteState state = PlayerMaster.Instance._PlayerInstanteState;
        state.Restore();
        InGame_Hp = state.GetMaxHp();
        InGame_SkillGauge = 0f;
        InGame_MeleeBullet = state.meleeBullets;
        InGame_Bullet= state.bullets;

        InGame_Gold = 0;
        InGame_BlueChip_Level.Clear();
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
            Debug.Log($"{UseIndex} ���̺����� �ε�");
            return dic[UseIndex];
        }
        return null;
    }
    public UserData GetUserData(int index)
    {
        if (dic.ContainsKey(index))
        {
            Debug.Log($"{UseIndex} ���̺����� �ε�");
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