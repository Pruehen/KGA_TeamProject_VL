 using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using EnumTypes;

public static class JsonDataManager
{
    
    private static string GetFilePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
    
    public static JsonCache jsonCache = new JsonCache();
    public static T DataTableListLoad<T>(string saveDataFileName) where T : class, new()
    {
        return LoadJson<T>(saveDataFileName);

        // string filePath = Application.dataPath + saveDataFileName;

        // if (!File.Exists(filePath))
        //     return new T();

        // string fileData = File.ReadAllText(filePath);
        // T data;

        // try
        // {
        //     data = JsonConvert.DeserializeObject<T>(fileData);
        //     if (data == null)
        //     {
        //         data = new T();
        //         Debug.Log("<color=#00FF00>새 저장 데이터 생성</color>");
        //     }

        //     Debug.Log($"<color=#00FF00>데이터 불러오기 완료</color> : {typeof(T).Name}");
        //     return data;
        // }
        // catch (Exception e)
        // {
        //     Debug.LogError($"<color=#FF0000>데이터 불러오기 실패</color> : {e.Message}");
        //     return new T();
        // }
    }
    public static void DataSaveCommand<T>(T jsonCacheData, string saveDataFileName)
    {
        DataSave(jsonCacheData, saveDataFileName);
    }
    static void DataSave<T>(T jsonCacheData, string saveDataFileName)
    {
        string data = JsonConvert.SerializeObject(jsonCacheData, Formatting.Indented);

        PlayerPrefs.SetString(saveDataFileName, data);
        PlayerPrefs.Save();

        Debug.Log($"<color=#FFFF00>데이터 저장 완료</color> : {typeof(T).Name}");
    }

    public static BlueChip GetBlueChipData(BlueChipID id)
    {
        return jsonCache.BlueChipTableCache.dic[id];
    }
    public static PassiveData GetPassive(PassiveID id)
    {
        return jsonCache.PassiveTableCache.dic[id];
    }
    public static UserData GetUserData()
    {        
        return jsonCache.UserDataCache.GetUserData();
    }
    public static bool TryGetUserData(int index, out UserData userData)
    {
        userData = jsonCache.UserDataCache.GetUserData(index);
        if (userData == null)
            return false;
        else
            return true;
    }
    public static void DeleteUserData(int index)
    {
        jsonCache.UserDataCache.DeleteUserData(index);
    }
    public static void SetUserDataIndex(int index)
    {
        jsonCache.UserDataCache.SetUserDataIndex(index);        
    }
    public static void SetupInitialData(string fileName, string defaultData)
    {
        try
        {
            PlayerPrefs.SetString(fileName, defaultData);
            PlayerPrefs.Save();
            Debug.Log($"Data saved successfully to {fileName}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save data to {fileName}. Error: {e.Message}");
        }
    }

    public static T LoadJson<T>(string fileName) where T : new()
    {
        try
        {
            string json = PlayerPrefs.GetString(fileName);
            Debug.Log($"File data: {json}");
            if(!PlayerPrefs.HasKey(fileName) || string.IsNullOrWhiteSpace(json))
            {
                Debug.LogWarning($"File is empty at {fileName}. Returning default value.");
                GameManager.Instance.SetupInitialUserData();
                return new T();
            }
            T data = JsonConvert.DeserializeObject<T>(json);
            if(data == null)
            {
                Debug.LogWarning($"File is empty at {fileName}. Returning default value.");
                GameManager.Instance.SetupInitialUserData();
                return new T();
            }
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load data from {fileName}. Error: {e.Message}");
            return new T();
        }
    }

    public static bool DeleteJson(string fileName)
    {
        if (PlayerPrefs.HasKey(fileName))
        {
            try
            {
                PlayerPrefs.DeleteKey(fileName);
                Debug.Log($"File deleted successfully: {fileName}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to delete file at {fileName}. Error: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"File not found at {fileName}. Nothing to delete.");
        }

        return false;
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
                _blueChipTableCache = JsonDataManager.DataTableListLoad<BlueChipTable>(GameManager.BLUECHIP_DATA_FILE);
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
                _passiveTableCache = JsonDataManager.DataTableListLoad<PassiveTable>(GameManager.PASSIVE_DATA_FILE);
            }
            foreach(var passive in _passiveTableCache.dic)
            {
                Debug.Log($"Passive: {passive.Key} {passive.Value.Name}");
                Debug.Log($"Desc: {passive.Value.Desc}");
                Debug.Log($"IconPath: {passive.Value.IconPath}");
                Debug.Log($"IconPath_Dis: {passive.Value.IconPath_Dis}");
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
                _userDataCache = JsonDataManager.DataTableListLoad<UserDataList>(GameManager.USER_DATA_FILE);
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
        JsonDataManager.DataSaveCommand(_blueChipTableCache, GameManager.BLUECHIP_DATA_FILE);
        JsonDataManager.DataSaveCommand(_passiveTableCache, GameManager.PASSIVE_DATA_FILE);
        JsonDataManager.DataSaveCommand(_userDataCache, GameManager.USER_DATA_FILE);
    }
}
