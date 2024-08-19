using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SKillDamage", order = 0)]
public class SO_Skill : ScriptableObject
{
    [Header("스킬 데미지")]
    public float _rangedSkill1 = 5f;
    public float _rangedSkill2 = 7f;
    public float _rangedSkill3 = 1f;
    public float _rangedSkill4 = 10f;
    public float _meleeSkill1 = 10f;
    public float _meleeSkill2 = 10f;
    public float _meleeSkill3_1 = 0.25f;
    public float _meleeSkill3_2 = 0.25f;
    public float _meleeSkill3_3 = 0.5f;
    public float _meleeSkill3_4 = 10f;
    public float _meleeSkill4 = 15f;
    
    [Header("스킬 범위")]
    public float _rangedSkill1Range = 1f;
    public float _rangedSkill2Range = 1f;
    public float _rangedSkill3Range = 1f;
    public float _rangedSkill4Range = 1f;
    public float _meleeSkill1Range = 1f;
    public float _meleeSkill2Range = 1f;
    public float _meleeSkill3Range = 1f;
    public float _meleeSkill4Range = 20f;
    
    [Header("스킬 사거리")]
    public float _rangedSkill1Distance = 1f;
    public float _rangedSkill2Distance = 1f;
    public float _rangedSkill3Distance = 1f;
    public float _rangedSkill4Distance = 1f;
    public float _meleeSkill4Distance = 20f;
}