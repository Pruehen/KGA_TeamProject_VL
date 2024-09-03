using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "RandomQuestsData", menuName = "Quests/RandomSets/RandomQuests", order = 1)]
public class SO_RandomQuestSetData : ScriptableObject
{
    public SO_Quest[] Quests;

    private List<SO_Quest> EasyQuests = new List<SO_Quest>();
    private List<SO_Quest> NormalQuests = new List<SO_Quest>();
    private List<SO_Quest> HardQuests = new List<SO_Quest>();

    [Range(0f, 100f)]
    public float QuestPosiblity = 30f;

    [Space(15)]
    [Range(0f, 100f)]
    public float EasyPosiblity = 30f;
    [Range(0f, 100f)]
    public float NormalPosiblity = 30f;
    [Range(0f, 100f)]
    public float HardPosiblity = 30f;

    public SO_Quest TryGetRandomQuest()
    {

        float r = UnityEngine.Random.value * 100f;

        if (r >= (QuestPosiblity))
        {
            return null;
        }

        return GetRandomQuest();

    }

    public SO_Quest GetRandomQuest()
    {
        foreach (SO_Quest s in Quests)
        {
            EasyQuests.Clear();
            NormalQuests.Clear();
            HardQuests.Clear();

            if (s.Difficurty == QuestDfficurty.Easy)
            {
                EasyQuests.Add(s);
            }
            if (s.Difficurty == QuestDfficurty.Normal)
            {
                NormalQuests.Add(s);
            }
            if (s.Difficurty == QuestDfficurty.Hard)
            {
                HardQuests.Add(s);
            }
        }

        float sum = EasyPosiblity + NormalPosiblity + HardPosiblity;
        float r = UnityEngine.Random.value;

        float normalizedEasy = EasyPosiblity / sum;
        float normalizedNormal = NormalPosiblity / sum;
        float normalizedHard = HardPosiblity / sum;

        if (r <= normalizedHard)
        {
            if (HardQuests.Count > 0)
            {
                return HardQuests[UnityEngine.Random.Range(0, HardQuests.Count)];
            }
        }
        if (r <= (normalizedHard + normalizedNormal))
        {
            if (NormalQuests.Count > 0)
            {
                return NormalQuests[UnityEngine.Random.Range(0, NormalQuests.Count)];
            }
        }
        else
        {
            if (EasyQuests.Count > 0)
            {
                return EasyQuests[UnityEngine.Random.Range(0, EasyQuests.Count)];
            }
        }
        return null;
    }
}
