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
                Debug.Log("�� ���� ������ ����");
            }

            Debug.Log($"������ �ҷ����� �Ϸ� : {typeof(T).Name}");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"������ �ҷ����� ���� : {e.Message}");
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
                Debug.LogError($"������ ���� �� ���� �߻�: {t.Exception}");
            }
        });
    }
    static async Task DataSaveAsync<T>(T jsonCacheData, string saveDataFileName)
    {
        string filePath = Application.dataPath + saveDataFileName;

        string data = JsonConvert.SerializeObject(jsonCacheData, Formatting.Indented);

        await File.WriteAllTextAsync(filePath, data);

        Debug.Log($"<color=#FFFF00>������ ���� �Ϸ�</color> : {typeof(T).Name}");
    }

    public static BlueChip GetBlueChipData(BlueChipID id)
    {
        return jsonCache.BlueChipTableCache.dic[(int)id];
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

    public void Lode()
    {
        _blueChipTableCache = BlueChipTableCache;        
    }
}