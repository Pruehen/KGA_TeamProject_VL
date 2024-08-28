using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillEvent", order = 0)]

public class SO_SKillEvent : ScriptableObject
{
    public GameObject preFab;
    public float size = 1f;
    public enum PlayerPos
    {
        Hand_L,
        Hand_R,
        Foot
    }
    public PlayerPos playerPos;
    public Vector3 offSet= new Vector3(0f,0f,0f);
    public Vector3 rotation;
}
