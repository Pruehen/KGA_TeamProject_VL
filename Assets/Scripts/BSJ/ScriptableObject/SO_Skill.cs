using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SKillDamage", order = 0)]
public class SO_Skill : ScriptableObject
{

    [Header("스킬 데미지")]
    public float MeleedefaultAttak;
    public float MeleeChargeAttak;
    public float _rangedSkill1 = 5f;
    public float _rangedSkill2 = 7f;
    public float _rangedSkill3 = 1f;
    public float _rangedSkill4 = 10f;
    public float _meleeSkill1 = 10f;
    public float _meleeSkill2 = 10f;
    public float _meleeSkill3_1 = 0.25f;
    public float _meleeSkill3_2 = 0.5f;
    public float _meleeSkill3_3 = 10f;
    public float _meleeSkill4 = 15f;
    
    [Header("스킬 범위")]

    public Vector3 _rangedSkill1Range = new Vector3 (1f,1f,1f);
    public Vector3 _rangedSkill2Range = new Vector3 (1f,1f,1f);
    public Vector3 _rangedSkill3Range = new Vector3 (1f,1f,1f);
    public Vector3 _rangedSkill4Range = new Vector3(20f, 20f, 20f);
    public Vector3 _meleeSkill1Range = new Vector3(1f, 1f, 1f);
    public Vector3 _meleeSkill2Range = new Vector3(1f,1f,1f);
    public Vector3 _meleeSkill3Range = new Vector3(2f,2f,2f);
    public Vector3 _meleeSkill4Range = new Vector3(20f,20f,20f);
    public Vector3 MeleedefaultAttackRange = new Vector3(1f, 1f, 1f);
    public Vector3 MeleeChargedAttackRange = new Vector3(500f, 500f, 500f);

    [Header("스킬 사거리")]
    public float _rangedSkill1Distance = 1f;
    public float _rangedSkill2Distance = 1f;
    public float _rangedSkill3Distance = 1f;
    public float _rangedSkill4Distance = 1f;
    public float _meleeSkill4Distance = 20f;

    [Header("스킬 시작위치")]
    public Vector3 _rangedSkill1OffSet = new Vector3(0f,0f,0f);
    public Vector3 _rangedSkill2OffSet = new Vector3(0f,0f,0f);
    public Vector3 _rangedSkill3OffSet = new Vector3(0f,0f,0f);
    public Vector3 _rangedSkill4OffSet = new Vector3(0f,0f,0f);
    public Vector3 _meleeSkill1OffSet = new Vector3(0f,0f,0f);
    public Vector3 _meleeSkill2OffSet = new Vector3(0f,0f,0f);
    public Vector3 _meleeSkill3OffSet = new Vector3(0f,0f,0f);
    public Vector3 _meleeSkill4OffSet = new Vector3(0f, 0f, 0f);
    public Vector3 MeleedefaultAttackOffset = new Vector3(1f, 1f, 1f);
    public Vector3 MeleeChargedAttackOffset = new Vector3(0f, 0f, 0f);

}