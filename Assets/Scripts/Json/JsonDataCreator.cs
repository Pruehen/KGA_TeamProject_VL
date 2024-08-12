using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

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
        //this.Level_VelueList = Level_VelueList;
        Level_VelueList = new Dictionary<int, List<float>>();
        Level_VelueList.Add(1, new List<float>());
        Level_VelueList.Add(2, new List<float>());
        Level_VelueList.Add(3, new List<float>());
        Level_VelueList.Add(4, new List<float>());
        Level_VelueList.Add(5, new List<float>());
    }
    public BlueChip()
    {
        Id = -1;
        Name = "블루칩 이름";
        Info = "블루칩 설명";
        Level_VelueList = new Dictionary<int, List<float>>();
        Level_VelueList.Add(1, new List<float>());
        Level_VelueList.Add(2, new List<float>());
        Level_VelueList.Add(3, new List<float>());
        Level_VelueList.Add(4, new List<float>());
        Level_VelueList.Add(5, new List<float>());
    }
}
public class BlueChipTable
{
    public List<BlueChip> list = new List<BlueChip>();
    [JsonConstructor]
    public BlueChipTable(List<BlueChip> list)
    {
        this.list = list;
    }
    public BlueChipTable()
    {
        list = new List<BlueChip>();
        for (int i = 0; i < 9; i++)
        {
            list.Add(new BlueChip());
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
    }
}
