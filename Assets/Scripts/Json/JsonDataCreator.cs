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
        Name = "블루칩 이름";
        Desc = "블루칩 설명";
        IconName = "아이콘 이름";
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
        Desc = "해당 패시브에 대한 설명. 값 {0}";
        IconPath = "아이콘 경로";
        IconPath_Dis = "비활성 아이콘 경로";
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
    [JsonProperty] public HashSet<string> IsClearAchievementsKey { get; private set; }//클리어한 업적 목록
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

    public void SavePlayData_OnSceneExit(PlayerInstanteState state, PlayerEquipBlueChip equipBlueChip)//씬 변환 시 호출
    {
        if (PlayData == null)
        {
            PlayData = new PlayData();
        }
        PlayData.SavePlayData_OnSceneExit(state, equipBlueChip);
        Save();
    }
    public void SavePlayData_OnSceneEnter(string newStage)//씬 입장 시 호출
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
            Debug.Log($"패시브 추가 : {id}");
        }
        else
        {
            Debug.LogWarning($"이미 존재하는 패시브 : {id}");
        }
    }

    public void TryRemovePassive(PassiveID id)
    {
        if (UsePassiveHashSet.Remove(id))
        {
            Save();
            Debug.Log($"패시브 제거 : {id}");            
        }
        else
        {
            Debug.LogWarning($"존재하지 않는 패시브 : {id}");
        }
    }
    public void TryUnLockPassive(PassiveID id)
    {
        if (UnlockPassiveHashSet.Add(id))
        {
            Save();
            Debug.Log($"패시브 해금 : {id}");
        }
        else
        {
            Debug.LogWarning($"이미 해금된 패시브 : {id}");
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
            Debug.LogError("슬롯 인덱스의 범위를 벗어났습니다.");
        }
    }
    public bool AvailableSlot(int slotIndex)
    {
        if (SlotIndex_SlotUnlock_Dic.ContainsKey(slotIndex) == false)
        {
            Debug.LogError("슬롯 인덱스의 범위를 벗어났습니다.");
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
    
    public void SavePlayData_OnSceneExit(PlayerInstanteState state, PlayerEquipBlueChip equipBlueChip)//씬 변환 시 호출
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
    public void SavePlayData_OnSceneEnter(string newStage)//씬 입장 시 호출
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
            Debug.Log($"{UseIndex} 세이브파일 로드");
            return dic[UseIndex];
        }
        return null;
    }
    public UserData GetUserData(int index)
    {
        if (dic.ContainsKey(index))
        {
            Debug.Log($"{UseIndex} 세이브파일 로드");
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
            Debug.Log("세이브파일 생성");
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