using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{

    InputManager _InputManager;
    Animator _animator;

    int _animTriggerSkill;
    int _animFloatSkillGauge;
    int _animTriggerSkillEnd;
    bool _isAttacking;
    [SerializeField] DamageBox _damageBox;
    [SerializeField] PlayerMaster _master;

    [SerializeField] public float _rangedSkill1 = 5f;
     [SerializeField] public float _rangedSkill2 = 7f;
     [SerializeField] public float _rangedSkill3 = 1f;
     [SerializeField] public float _rangedSkill4 = 10f;
     [SerializeField] public float _meleeSkill1 = 10f;
     [SerializeField] public float _meleeSkill2 = 10f;
     [SerializeField] public float _meleeSkill3_1 = 0.25f;
     [SerializeField] public float _meleeSkill3_2 = 0.25f;
     [SerializeField] public float _meleeSkill3_3 = 0.5f;
     [SerializeField] public float _meleeSkill3_4 = 10f;
     [SerializeField] public float _meleeSkill4 = 15f;

    [SerializeField] public float _rangedSkill1Range =1f;
    [SerializeField] public float _rangedSkill2Range =1f;
    [SerializeField] public float _rangedSkill3Range =1f;
    [SerializeField] public float _rangedSkill4Range =1f;
    [SerializeField] public float _meleeSkill1Range =1f;
    [SerializeField] public float _meleeSkill2Range =1f;
    [SerializeField] public float _meleeSkill3Range =1f;
    [SerializeField] public float _meleeSkill4Range =1f;

    [SerializeField] public float _rangedSkill1Distance = 1f;
    [SerializeField] public float _rangedSkill2Distance = 1f;
    [SerializeField] public float _rangedSkill3Distance = 1f;
    [SerializeField] public float _rangedSkill4Distance = 1f;
    [SerializeField] public float _meleeSkill4Distance = 20f;

    [SerializeField] private LayerMask enemyLayerMask;
    public float SkillPower
    {
        get { return _master._PlayerInstanteState.GetSkillDmg(); }
    }

    [SerializeField] SO_Skill so_Skill;

    public GameObject player;
    [SerializeField] private float moveDuration = 0.3f; // 이동할 때 걸리는 시간
    public Vector3 target;
    private Coroutine moveCoroutine;
    private void Awake()
    {
        _master = GetComponent<PlayerMaster>();
        _animator = GetComponent<Animator>();
        Init(_animator);
        InitSkillData(so_Skill);
    }
    public void Init(Animator animator)
    {
        _animator = animator;
        _animTriggerSkill = Animator.StringToHash("Skill");
        _animFloatSkillGauge = Animator.StringToHash("SkillGauge");
        _animTriggerSkillEnd = Animator.StringToHash("SkillEnd");
    }
    public void InitSkillData(SO_Skill sO_Skill)
    {
        if(so_Skill!=null)
        {
            _rangedSkill1 = so_Skill._rangedSkill1;
            _rangedSkill2 = so_Skill._rangedSkill2;
            _rangedSkill3 = so_Skill._rangedSkill3;
            _rangedSkill4 = so_Skill._rangedSkill4;
            _meleeSkill1 = so_Skill._meleeSkill1;
            _meleeSkill2 = so_Skill._meleeSkill2;
            _meleeSkill3_1 = so_Skill._meleeSkill3_1;
            _meleeSkill3_2 = so_Skill._meleeSkill3_2;
            _meleeSkill3_3 = so_Skill._meleeSkill3_3;
            _meleeSkill3_4 = so_Skill._meleeSkill3_4;
            _meleeSkill4 = so_Skill._meleeSkill4;

            _rangedSkill1Range =so_Skill._rangedSkill1; 
            _rangedSkill2Range =so_Skill._rangedSkill2; 
            _rangedSkill3Range =so_Skill._rangedSkill3;
            _rangedSkill4Range = so_Skill._rangedSkill4;
            _meleeSkill1Range = so_Skill._meleeSkill1Range;
            _meleeSkill2Range = so_Skill._meleeSkill2Range;
            _meleeSkill3Range = so_Skill._meleeSkill3Range;
            _meleeSkill4Range = so_Skill._meleeSkill4Range;

            _rangedSkill1Distance = so_Skill._rangedSkill1Distance;
            _rangedSkill2Distance = so_Skill._rangedSkill2Distance;
            _rangedSkill3Distance = so_Skill._rangedSkill3Distance;
            _rangedSkill4Distance = so_Skill._rangedSkill4Distance;
            _meleeSkill4Distance =  so_Skill._meleeSkill4Distance;
        }
    }
    public void EndSkill()
    {
        _animator.SetTrigger(_animTriggerSkillEnd);
    }

    public void SkillEnd()
    {
        if (!_animator.GetBool(_animTriggerSkill))
        {
            _animator.SetTrigger(_animTriggerSkillEnd);
            Debug.Log("ATKEnd");
        }
    }
    public void MellSkill4()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        // 카메라의 위치에서 forward 방향으로 레이를 쏩니다.
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        float damage = 0f;
        float range = 1f;
        float distance = 1f;


        damage = so_Skill._rangedSkill4 * SkillPower;
        range = so_Skill._rangedSkill4Range;
        distance = so_Skill._meleeSkill4Distance;


        if (Physics.Raycast(ray, out hit, distance, enemyLayerMask))
        {
            Vector3 hitPosition = hit.point;
            target = hit.point;
            _damageBox.transform.position = target;
            TargettoRun();
            //데미지 박스의 위치를 미리 이전
        }
        else
        {
            target = player.transform.localPosition;
            _damageBox.transform.localPosition = Vector3.zero;
            Debug.Log("레이 맞춘 게 없음");
        }
    }
    public void InvokeSkillDamage(string skillName)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        // 카메라의 위치에서 forward 방향으로 레이를 쏩니다.
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        float damage = 0f;
        float range = 1f;
        float distance = 1f;

        switch (skillName)
        {
            case "Skill1":
                damage = so_Skill._rangedSkill1 * SkillPower;
                range = so_Skill._rangedSkill1Range;
                distance = so_Skill._rangedSkill1Distance;
                break;

            case "Skill2":
                damage = so_Skill._rangedSkill2 * SkillPower;
                range = so_Skill._rangedSkill2Range;
                distance = so_Skill._rangedSkill2Distance;
                break;

            case "Skill3":
                damage = so_Skill._rangedSkill3 * SkillPower;
                range = so_Skill._rangedSkill3Range;
                distance = so_Skill._rangedSkill3Distance;
                break;

            case "Skill4":
                damage = so_Skill._rangedSkill4 * SkillPower;
                range = so_Skill._rangedSkill4Range;
                distance = so_Skill._meleeSkill4Distance;
                break;

            default:
                Debug.LogWarning($"Unrecognized skill: {skillName}");
                return;
        }

        if (Physics.Raycast(ray, out hit, distance, enemyLayerMask))
        {
            Vector3 hitPosition = hit.point;
            _damageBox.transform.position = hitPosition;
            _damageBox.EnableDamageBox(damage, range, null, 0f);
        }
        else
        {
            _damageBox.transform.localPosition = Vector3.zero;
            _damageBox.EnableDamageBox(damage, range, null, 0f);
            Debug.Log("레이 맞춘 게 없음");
        }
    }
    public void TargettoRun()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
    }
    public void SkillDamege(string skillName)
    {
       
        float damage = 0f;
        float range = 1f;
        switch (skillName)
        {
            case "Skill1":
                damage = so_Skill._rangedSkill1 * SkillPower;
                range = so_Skill._rangedSkill1Range;
                break;
            case "Skill2":
                damage = so_Skill._rangedSkill2 * SkillPower;
                range = so_Skill._rangedSkill2Range;
                break;
            case "Skill3":
                damage = so_Skill._rangedSkill3 * SkillPower;
                range = so_Skill._rangedSkill3Range;
                break;
            case "Skill4":
                damage = so_Skill._rangedSkill4 * SkillPower;
                range = so_Skill._rangedSkill4Range;
                break;
            default:
                Debug.LogWarning($"Unrecognized skill: {skillName}");
                return;
        }
        _damageBox.EnableDamageBox(damage, range, null, 0f);
    }
    private IEnumerator MoveToTargetCoroutine()
    {
        Vector3 startPosition = player.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            player.transform.position = Vector3.Lerp(startPosition, target, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }
        // 최종 위치 설정
        player.transform.position = target;
    }

    private void OnDrawGizmos()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Ray를 카메라의 위치에서 forward 방향으로 그립니다.
            Gizmos.color = Color.green;
            Vector3 start = mainCamera.transform.position;
            Vector3 direction = mainCamera.transform.forward * _meleeSkill4Distance;
            Gizmos.DrawLine(start, start + direction);
        }
    }

}
