using System;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "RandomQuestsData", menuName = "Quests/RandomSets/RandomQuests", order = 1)]
public class SO_RandomQuestSetData : ScriptableObject
{
    public SO_Quest[] EasyQuests;
    public SO_Quest[] NormalQuests;
    public SO_Quest[] HardQuests;

    [Range (0f,100f)]
    public float QuestPosiblity = 30f;

    [Space (15)]
    [Range (0f,100f)]
    public float EasyPosiblity = 30f;
    [Range (0f,100f)]
    public float NormalPosiblity = 30f;
    [Range (0f,100f)]
    public float HardPosiblity = 30f;

    public SO_Quest TryGetRandomQuest()
    {

        float r = UnityEngine.Random.value * 100f;

        if(r >= (QuestPosiblity))
        {
            return null;
        }

        return GetRandomQuest();

    }

    public SO_Quest GetRandomQuest()
    {
        float sum = EasyPosiblity + NormalPosiblity + HardPosiblity;
        float r = UnityEngine.Random.value;

        float normalizedEasy = EasyPosiblity / sum;
        float normalizedNormal = normalizedEasy + NormalPosiblity / sum;
        float normalizedHard = normalizedNormal + HardPosiblity / sum;

        if (r <= normalizedEasy)
        {
            return EasyQuests[UnityEngine.Random.Range(0, EasyQuests.Length)];
        }
        else if (r <= normalizedNormal)
        {
            return NormalQuests[UnityEngine.Random.Range(0, NormalQuests.Length)];
        }
        else
        {
            return HardQuests[UnityEngine.Random.Range(0, HardQuests.Length)];
        }
    }
}
