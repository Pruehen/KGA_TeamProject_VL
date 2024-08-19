using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System;
using System.Threading.Tasks;
using EnumTypes;

public static class JsonDataManager
{
    public static JsonCache jsonCache = new JsonCache();
    public static T DataTableListLoad<T>(string saveDataFileName) where T : class, new()
    {
        string filePath = Application.dataPath + saveDataFileName;

        if (!File.Exists(filePath))
            return new T();

        string fileData = File.ReadAllText(filePath);
        T data;

        try
        {
            data = JsonConvert.DeserializeObject<T>(fileData);
            if (data == null)
            {
                data = new T();
                Debug.Log("<color=#00FF00>새 저장 데이터 생성</color>");
            }

            Debug.Log($"<color=#00FF00>데이터 불러오기 완료</color> : {typeof(T).Name}");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"<color=#FF0000>데이터 불러오기 실패</color> : {e.Message}");
            return new T();
        }
    }
    public static void DataSaveCommand<T>(T jsonCacheData, string saveDataFileName)
    {
        Task task = DataSaveAsync(jsonCacheData, saveDataFileName);

        task.ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Debug.LogError($"<color=#FF0000>데이터 저장 중 오류 발생</color>: {t.Exception}");
            }
        });
    }
    static async Task DataSaveAsync<T>(T jsonCacheData, string saveDataFileName)
    {
        string filePath = Application.dataPath + saveDataFileName;

        string data = JsonConvert.SerializeObject(jsonCacheData, Formatting.Indented);

        await File.WriteAllTextAsync(filePath, data);

        Debug.Log($"<color=#FFFF00>데이터 저장 완료</color> : {typeof(T).Name}");
    }

    public static BlueChip GetBlueChipData(BlueChipID id)
    {
        return jsonCache.BlueChipTableCache.dic[id];
    }
    public static Passive GetPassive(PassiveID id)
    {
        return jsonCache.PassiveTableCache.dic[id];
    }
    public static UserData GetUserData()
    {
        return jsonCache.UserDataCache.GetUserData();
    }
}

public class JsonCache
{
    BlueChipTable _blueChipTableCache;
    public BlueChipTable BlueChipTableCache
    {
        get
        {
            if (_blueChipTableCache == null)
            {
                _blueChipTableCache = JsonDataManager.DataTableListLoad<BlueChipTable>(BlueChipTable.FilePath());
            }
            return _blueChipTableCache;
        }
    }

    PassiveTable _passiveTableCache;
    public PassiveTable PassiveTableCache
    {
        get
        {
            if (_passiveTableCache == null)
            {
                _passiveTableCache = JsonDataManager.DataTableListLoad<PassiveTable>(PassiveTable.FilePath());
            }
            return _passiveTableCache;
        }
    }

    UserDataList _userDataCache;
    public UserDataList UserDataCache
    {
        get
        {
            if (_userDataCache == null)
            {
                _userDataCache = JsonDataManager.DataTableListLoad<UserDataList>(UserDataList.FilePath());
            }
            return _userDataCache;
        }
    }

    public void Lode()
    {
        _blueChipTableCache = BlueChipTableCache;
        _passiveTableCache = PassiveTableCache;
        _userDataCache = UserDataCache;
    }

    public void Save()
    {
        JsonDataManager.DataSaveCommand(_blueChipTableCache, BlueChipTable.FilePath());
        JsonDataManager.DataSaveCommand(_passiveTableCache, PassiveTable.FilePath());
        JsonDataManager.DataSaveCommand(_userDataCache, UserDataList.FilePath());
    }
}