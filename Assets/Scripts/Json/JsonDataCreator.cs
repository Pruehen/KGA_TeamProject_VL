using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UI.Extension;
using System.Linq;

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

    public void Print(int level)
    {
        Debug.Log($"{level}레벨 : {Name}");
        int[] arr = new int[Level_VelueList[level].Count];

        for (int i = 0; i < Level_VelueList[level].Count; i++)
        {
            arr[i] = (int)(Level_VelueList[level][i]);
        }
        //Info.FormatStringWithArray(arr);
        Debug.Log(string.Format(Info, Level_VelueList[level].Cast<object>().ToArray()));
    }
}
public class BlueChipTable
{
    public Dictionary<int, BlueChip> dic;
    [JsonConstructor]
    public BlueChipTable(Dictionary<int, BlueChip> dic)
    {
        this.dic = dic;
    }
    public BlueChipTable()
    {
        dic = new Dictionary<int, BlueChip>();
        for (int i = 0; i < 9; i++)
        {
            dic.Add(i, new BlueChip(i));
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
        JsonDataManager.jsonCache.BlueChipTableCache.dic[4].Print(1);
    }
}
