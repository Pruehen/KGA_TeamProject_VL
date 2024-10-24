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
        string filePath = GetFilePath(saveDataFileName);

        string data = JsonConvert.SerializeObject(jsonCacheData, Formatting.Indented);

        await File.WriteAllTextAsync(filePath, data);

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
        string filePath = GetFilePath(fileName);

        try
        {
            File.WriteAllText(filePath, defaultData);
            Debug.Log($"Data saved successfully to {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save data to {filePath}. Error: {e.Message}");
        }
    }

    public static async Task SaveJsonAsync(string jsonData, string fileName)
    {
        string filePath = GetFilePath(fileName);

        try
        {
            await File.WriteAllTextAsync(filePath, jsonData);
            Debug.Log($"Data saved successfully to {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save data to {filePath}. Error: {e.Message}");
        }
    }

    public static T LoadJson<T>(string fileName) where T : new()
    {
        string filePath = GetFilePath(fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"File not found at {filePath}. Returning default value.");
            return new T();
        }

        try
        {
            string json = File.ReadAllText(filePath);
            Debug.Log($"File loaded successfully from {filePath}");
            Debug.Log($"File data: {json}");
            if(string.IsNullOrWhiteSpace(json))
            {
                Debug.LogWarning($"File is empty at {filePath}. Returning default value.");
                GameManager.Instance.SetupInitialUserData();
                return new T();
            }
            T data = JsonConvert.DeserializeObject<T>(json);
            if(data == null)
            {
                Debug.LogWarning($"File is empty at {filePath}. Returning default value.");
                GameManager.Instance.SetupInitialUserData();
                return new T();
            }
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load data from {filePath}. Error: {e.Message}");
            return new T();
        }
    }

    public static bool DeleteJson(string fileName)
    {
        string filePath = GetFilePath(fileName);

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log($"File deleted successfully: {filePath}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to delete file at {filePath}. Error: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"File not found at {filePath}. Nothing to delete.");
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
