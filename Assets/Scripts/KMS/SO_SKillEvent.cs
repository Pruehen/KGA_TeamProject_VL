using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillEvent", order = 0)]

public class SO_SKillEvent : ScriptableObject
{
    public GameObject preFab;
    public float size;
    public enum PlayerPos
    {
        Hand_L,
        Hand_R,
        Foot
    }
    public PlayerPos playerPos;
}
