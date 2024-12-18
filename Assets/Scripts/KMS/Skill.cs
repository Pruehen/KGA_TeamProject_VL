using EnumTypes;
using System;
using System.Collections;
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
    [SerializeField] public float _meleeSkill3_3 = 10f;
    [SerializeField] public float _meleeSkill4 = 15f;

    [SerializeField] public Vector3 _rangedSkill1Range = new Vector3(1f, 1f, 1f);
    [SerializeField] public Vector3 _rangedSkill2Range = new Vector3(1f, 1f, 1f);
    [SerializeField] public Vector3 _rangedSkill3Range = new Vector3(1f, 1f, 1f);
    [SerializeField] public Vector3 _rangedSkill4Range = new Vector3(1f, 1f, 1f);
    [SerializeField] public Vector3 _meleeSkill1Range = new Vector3(1f, 1f, 1f);
    [SerializeField] public Vector3 _meleeSkill2Range = new Vector3(1f, 1f, 1f);
    [SerializeField] public Vector3 _meleeSkill3Range = new Vector3(1f, 1f, 1f);
    [SerializeField] public Vector3 _meleeSkill4Range = new Vector3(1f, 1f, 1f);

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
    [SerializeField] public SO_SKillEvent RangeSkill4;
    [SerializeField] public SO_SKillEvent RangeSkill3;
    [SerializeField] public SO_SKillEvent RangeSkill2;
    [SerializeField] public SO_SKillEvent RangeSkill1;
    private void Awake()
    {
        so_Skill = _master.SkillData;
        player = this.gameObject;
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
        if (so_Skill != null)
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
            _meleeSkill4 = so_Skill._meleeSkill4;

            _rangedSkill1Range = so_Skill._rangedSkill1Range;
            _rangedSkill2Range = so_Skill._rangedSkill2Range;
            _rangedSkill3Range = so_Skill._rangedSkill3Range;
            _rangedSkill4Range = so_Skill._rangedSkill4Range;
            _meleeSkill1Range = so_Skill._meleeSkill1Range;
            _meleeSkill2Range = so_Skill._meleeSkill2Range;
            _meleeSkill3Range = so_Skill._meleeSkill3Range;
            _meleeSkill4Range = so_Skill._meleeSkill4Range;

            _rangedSkill1Distance = so_Skill._rangedSkill1Distance;
            _rangedSkill2Distance = so_Skill._rangedSkill2Distance;
            _rangedSkill3Distance = so_Skill._rangedSkill3Distance;
            _rangedSkill4Distance = so_Skill._rangedSkill4Distance;
            _meleeSkill4Distance = so_Skill._meleeSkill4Distance;
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
        Vector3 range = new Vector3(1f, 0, 1f);
        float distance = 1f;
        SM.Instance.PlaySound2("MeleeSkill4_1", transform.position);

        damage = so_Skill._rangedSkill4 * SkillPower;
        range = so_Skill._rangedSkill4Range;
        distance = so_Skill._meleeSkill4Distance;
        transform.rotation = mainCamera.transform.rotation;
        if (Physics.Raycast(ray, out hit, distance, enemyLayerMask))
        {
            Vector3 hitPosition = new Vector3(hit.point.x,transform.position.y, hit.point.z);
            target = hitPosition;
            //_damageBox.transform.position = target;
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

    public void StartRangeSkill3(PlayerSkill skillType)
    {
        StartCoroutine(RangeSkill3Coroutine(skillType));
    }

    private IEnumerator RangeSkill3Coroutine(PlayerSkill skillType)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) yield break;

        // 카메라의 위치에서 forward 방향으로 레이를 쏩니다.
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        transform.rotation = mainCamera.transform.rotation;
        float damage = 0f;
        Vector3 range = new Vector3(1f, 1f, 1f);
        float distance = 0f;
        Vector3 offset = new Vector3(0f, 0f, 0f);

        switch (skillType)
        {
            case PlayerSkill.RangeSkillAttack3:
                damage = so_Skill._rangedSkill3 * SkillPower;
                range = so_Skill._rangedSkill3Range;
                distance = so_Skill._rangedSkill3Distance;
                offset = so_Skill._rangedSkill3OffSet;
                break;
            default:
                Debug.LogWarning($"Unrecognized skill: {skillType}");
                yield break;
        }

        for (int i = 0; i < 10; i++)
        {
            if (Physics.Raycast(ray, out hit, distance, enemyLayerMask))
            {
                Vector3 hitPosition = hit.point;
                _damageBox.transform.position = hitPosition;
                _damageBox.EnableSkillDamageBox(damage, range, null, 0, offset);
            }
            else
            {
                _damageBox.transform.localPosition = Vector3.zero;
                _damageBox.EnableDamageBox(damage, range, null, 0f, offset);
                Debug.Log("레이 맞춘 게 없음");
            }

            Effect2(RangeSkill3);
            yield return new WaitForSeconds(0.04F);
        }

        Debug.Log("스킬실행 완료");
        Debug.Log(skillType);
    }

    public void StartRangeSkill3(PlayerSkill skillType,float Multi)
    {
        StartCoroutine(RangeSkill3Coroutine(skillType, Multi));
    }

    private IEnumerator RangeSkill3Coroutine(PlayerSkill skillType, float Multi)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) yield break;

        // 카메라의 위치에서 forward 방향으로 레이를 쏩니다.
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        transform.rotation = mainCamera.transform.rotation;
        float damage = 0f;
        Vector3 range = new Vector3(1f, 1f, 1f);
        float distance = 0f;
        Vector3 offset = new Vector3(0f, 0f, 0f);

        switch (skillType)
        {
            case PlayerSkill.RangeSkillAttack3:
                damage = (so_Skill._rangedSkill3 * SkillPower)*Multi;
                range = so_Skill._rangedSkill3Range;
                distance = so_Skill._rangedSkill3Distance;
                offset = so_Skill._rangedSkill3OffSet;
                break;
            default:
                Debug.LogWarning($"Unrecognized skill: {skillType}");
                yield break;
        }

        for (int i = 0; i < 10; i++)
        {
            if (Physics.Raycast(ray, out hit, distance, enemyLayerMask))
            {
                Vector3 hitPosition = hit.point;
                _damageBox.transform.position = hitPosition;
                _damageBox.EnableSkillDamageBox(damage, range, null, 0, offset);
            }
            else
            {
                _damageBox.transform.localPosition = Vector3.zero;
                _damageBox.EnableDamageBox(damage, range, null, 0f, offset);
                Debug.Log("레이 맞춘 게 없음");
            }

            Effect2(RangeSkill3);
            yield return new WaitForSeconds(0.04F);
        }

        Debug.Log("스킬실행 완료");
        Debug.Log(skillType);
    }



    public void InvokeSkillDamage(PlayerSkill skillType)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        // 카메라의 위치에서 forward 방향으로 레이를 쏩니다.
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        transform.rotation = mainCamera.transform.rotation;
        float damage = 0f;
        Vector3 range = new Vector3(1f, 1f, 1f);
        float distance = 0f;
        Vector3 offset = new Vector3(0f, 0f, 0f);

        switch (skillType)
        {
            case PlayerSkill.RangeSkillAttack1:
                damage = so_Skill._rangedSkill1 * SkillPower;
                range = so_Skill._rangedSkill1Range;
                distance = so_Skill._rangedSkill1Distance;
                offset = so_Skill._rangedSkill1OffSet;
                break;

            case PlayerSkill.RangeSkillAttack2:
                damage = so_Skill._rangedSkill2 * SkillPower;
                range = so_Skill._rangedSkill2Range;
                distance = so_Skill._rangedSkill2Distance;
                offset = so_Skill._rangedSkill2OffSet;
                break;

            case PlayerSkill.RangeSkillAttack3:
                damage = so_Skill._rangedSkill3 * SkillPower;
                range = so_Skill._rangedSkill3Range;
                distance = so_Skill._rangedSkill3Distance;
                offset = so_Skill._rangedSkill3OffSet;
                break;

            case PlayerSkill.RangeSkillAttack4:
                damage = so_Skill._rangedSkill4 * SkillPower;
                range = so_Skill._rangedSkill4Range;
                distance = so_Skill._rangedSkill4Distance; ;
                offset = so_Skill._rangedSkill4OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack1:
                damage = so_Skill._meleeSkill1 * SkillPower;
                range = so_Skill._meleeSkill1Range;
                offset = so_Skill._meleeSkill1OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack2:
                damage = so_Skill._meleeSkill2 * SkillPower;
                range = so_Skill._meleeSkill2Range;
                offset = so_Skill._meleeSkill2OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack3_1:
                damage = so_Skill._meleeSkill3_1 * SkillPower;
                range = so_Skill._meleeSkill3Range;
                offset = so_Skill._meleeSkill3OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack3_2:
                damage = so_Skill._meleeSkill3_2 * SkillPower;
                range = so_Skill._meleeSkill3Range;
                offset = so_Skill._meleeSkill3OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack3_3:
                damage = so_Skill._meleeSkill3_3 * SkillPower;
                range = so_Skill._meleeSkill3Range;
                offset = so_Skill._meleeSkill3OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack4:
                damage = so_Skill._meleeSkill4 * SkillPower;
                range = so_Skill._meleeSkill4Range;
                offset = so_Skill._meleeSkill4OffSet;
                break;

            default:
                Debug.LogWarning($"Unrecognized skill: {skillType}");
                return;
        }

        if (Physics.Raycast(ray, out hit, distance, enemyLayerMask))
        {
            Vector3 hitPosition = hit.point;
            _damageBox.transform.position = hitPosition;
            _damageBox.EnableSkillDamageBox(damage, range, null, 0, offset);
        }
        else
        {
            _damageBox.transform.localPosition = Vector3.zero;
            _damageBox.EnableDamageBox(damage, range, null, 0f, offset);
            Debug.Log("레이 맞춘 게 없음");
        }
        Debug.Log("스킬실행");
        Debug.Log(skillType);
    }
    public void InvokeSkillDamage(PlayerSkill skillType,float Multi)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        // 카메라의 위치에서 forward 방향으로 레이를 쏩니다.
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        transform.rotation = mainCamera.transform.rotation;
        float damage = 0f;
        Vector3 range = new Vector3(1f, 1f, 1f);
        float distance = 0f;
        Vector3 offset = new Vector3(0f, 0f, 0f);
        float skillPower = SkillPower * Multi;


        switch (skillType)
        {
            case PlayerSkill.RangeSkillAttack1:
                damage = so_Skill._rangedSkill1 * skillPower;
                range = so_Skill._rangedSkill1Range;
                distance = so_Skill._rangedSkill1Distance;
                offset = so_Skill._rangedSkill1OffSet;
                break;

            case PlayerSkill.RangeSkillAttack2:
                damage = so_Skill._rangedSkill2 * skillPower;
                range = so_Skill._rangedSkill2Range;
                distance = so_Skill._rangedSkill2Distance;
                offset = so_Skill._rangedSkill2OffSet;
                break;

            case PlayerSkill.RangeSkillAttack3:
                damage = so_Skill._rangedSkill3 * skillPower;
                range = so_Skill._rangedSkill3Range;
                distance = so_Skill._rangedSkill3Distance;
                offset = so_Skill._rangedSkill3OffSet;
                break;

            case PlayerSkill.RangeSkillAttack4:
                damage = so_Skill._rangedSkill4 * skillPower;
                range = so_Skill._rangedSkill4Range;
                distance = so_Skill._rangedSkill4Distance; ;
                offset = so_Skill._rangedSkill4OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack1:
                damage = so_Skill._meleeSkill1 * skillPower;
                range = so_Skill._meleeSkill1Range;
                offset = so_Skill._meleeSkill1OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack2:
                damage = so_Skill._meleeSkill2 * skillPower;
                range = so_Skill._meleeSkill2Range;
                offset = so_Skill._meleeSkill2OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack3_1:
                damage = so_Skill._meleeSkill3_1 * skillPower;
                range = so_Skill._meleeSkill3Range;
                offset = so_Skill._meleeSkill3OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack3_2:
                damage = so_Skill._meleeSkill3_2 * skillPower;
                range = so_Skill._meleeSkill3Range;
                offset = so_Skill._meleeSkill3OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack3_3:
                damage = so_Skill._meleeSkill3_3 * skillPower;
                range = so_Skill._meleeSkill3Range;
                offset = so_Skill._meleeSkill3OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack4:
                damage = so_Skill._meleeSkill4 * skillPower;
                range = so_Skill._meleeSkill4Range;
                offset = so_Skill._meleeSkill4OffSet;
                break;

            default:
                Debug.LogWarning($"Unrecognized skill: {skillType}");
                return;
        }

        if (Physics.Raycast(ray, out hit, distance, enemyLayerMask))
        {
            Vector3 hitPosition = hit.point;
            _damageBox.transform.position = hitPosition;
            _damageBox.EnableSkillDamageBox(damage, range, null, 0, offset);
        }
        else
        {
            _damageBox.transform.localPosition = Vector3.zero;
            _damageBox.EnableDamageBox(damage, range, null, 0f, offset);
            Debug.Log("레이 맞춘 게 없음");
        }
        Debug.Log("스킬실행");
        Debug.Log(skillType);
    }
    public void TargettoRun()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
    }
    public void SkillDamege(PlayerSkill skillType)
    {

        float damage = 0f;
        Vector3 range = new Vector3(1f, 1f, 1f);
        float distance = 1f;
        Vector3 offset = new Vector3(1f, 1f, 1f);
        switch (skillType)
        {
            case PlayerSkill.RangeSkillAttack1:
                damage = so_Skill._rangedSkill1 * SkillPower;
                range = so_Skill._rangedSkill1Range;
                distance = so_Skill._rangedSkill1Distance;
                offset = so_Skill._rangedSkill1OffSet;
                break;

            case PlayerSkill.RangeSkillAttack2:
                damage = so_Skill._rangedSkill2 * SkillPower;
                range = so_Skill._rangedSkill2Range;
                distance = so_Skill._rangedSkill2Distance;
                offset = so_Skill._rangedSkill2OffSet;
                break;

            case PlayerSkill.RangeSkillAttack3:
                damage = so_Skill._rangedSkill3 * SkillPower;
                range = so_Skill._rangedSkill3Range;
                distance = so_Skill._rangedSkill3Distance;
                offset = so_Skill._rangedSkill3OffSet;
                break;

            case PlayerSkill.RangeSkillAttack4:
                damage = so_Skill._rangedSkill4 * SkillPower;
                range = so_Skill._rangedSkill4Range;
                distance = so_Skill._rangedSkill4Distance; ;
                offset = so_Skill._rangedSkill4OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack1:
                damage = so_Skill._meleeSkill1 * SkillPower;
                range = so_Skill._meleeSkill1Range;
                offset = so_Skill._meleeSkill1OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack2:
                damage = so_Skill._meleeSkill2 * SkillPower;
                range = so_Skill._meleeSkill2Range;
                offset = so_Skill._meleeSkill2OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack3_1:
                damage = so_Skill._meleeSkill3_1 * SkillPower;
                range = so_Skill._meleeSkill3Range;
                offset = so_Skill._meleeSkill3OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack3_2:
                damage = so_Skill._meleeSkill3_2 * SkillPower;
                range = so_Skill._meleeSkill3Range;
                offset = so_Skill._meleeSkill3OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack3_3:
                damage = so_Skill._meleeSkill3_3 * SkillPower;
                range = so_Skill._meleeSkill3Range;
                offset = so_Skill._meleeSkill3OffSet;
                break;
            case PlayerSkill.MeleeSkillAttack4:
                damage = so_Skill._meleeSkill4 * SkillPower;
                range = so_Skill._meleeSkill4Range;
                offset = so_Skill._meleeSkill4OffSet;
                break;
            default:
                Debug.LogWarning($"Unrecognized skill: {skillType}");
                return;

        }
        _damageBox.transform.localPosition = Vector3.zero;
        _damageBox.EnableDamageBox(damage, range, null, 0f, offset);
        Debug.Log($"Unrecognized skill:{offset} {skillType}");
    }
    private IEnumerator MoveToTargetCoroutine()
    {
        Vector3 startPosition = player.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            //Vector3 newtarget = new Vector3(target.x, player.transform.position.y, target.z);
            Vector3 newtarget = new Vector3(target.x, 0, target.z);
            player.transform.position = Vector3.Lerp(startPosition, newtarget, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }
        // 최종 위치 설정
        player.transform.position = target;
    }

    public float RayDistance = 20;
    private void OnDrawGizmos()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Ray를 카메라의 위치에서 forward 방향으로 그립니다.
            Gizmos.color = Color.green;
            Vector3 start = mainCamera.transform.position;
            Vector3 direction = mainCamera.transform.forward * RayDistance;
            Gizmos.DrawLine(start, start + direction);
        }
    }
    public void Effect2(SO_SKillEvent skill)
    {
        GameObject VFX = ObjectPoolManager.Instance.DequeueObject(skill.preFab);

        Vector3 position = Vector3.zero;

        // 플레이어의 특정 위치를 가져옴
        switch (skill.playerPos)
        {
            case SO_SKillEvent.PlayerPos.Hand_L:
                position = hand_L.position;
                break;
            case SO_SKillEvent.PlayerPos.Hand_R:
                position = hand_R.position;
                break;
            case SO_SKillEvent.PlayerPos.Foot:
                position = Foot.position;
                break;
            case SO_SKillEvent.PlayerPos.Target:
                position = targetpos.position;
                break;
        }
        Vector3 finalPosition = position + transform.TransformDirection(skill.offSet);
        VFX.transform.position = finalPosition;
        Quaternion playerRotation = transform.rotation;
        Quaternion finalRotation = playerRotation * Quaternion.Euler(skill.rotation);
        VFX.transform.rotation = finalRotation;
        VFX.transform.localScale = Vector3.one * skill.size;
    }
    [SerializeField] Transform hand_L;
    [SerializeField] Transform hand_R;
    [SerializeField] Transform Foot;
    [SerializeField] Transform targetpos;
    public void Effect3(SO_SKillEvent skill)
    {
        GameObject VFX = ObjectPoolManager.Instance.DequeueObject(skill.preFab);

        Vector3 position = Vector3.zero;
        Transform targetTransform = null;

        // 플레이어의 특정 위치를 가져옴
        switch (skill.playerPos)
        {
            case SO_SKillEvent.PlayerPos.Hand_L:
                position = hand_L.position;
                targetTransform = hand_L;  // Transform 설정
                break;
            case SO_SKillEvent.PlayerPos.Hand_R:
                position = hand_R.position;
                targetTransform = hand_R;  // Transform 설정
                break;
            case SO_SKillEvent.PlayerPos.Foot:
                position = Foot.position;
                targetTransform = Foot;  // Transform 설정
                break;
            case SO_SKillEvent.PlayerPos.Target:
                position = targetpos.position;
                targetTransform = targetpos;  // Transform 설정
                break;
        }

        Vector3 finalPosition = position + transform.TransformDirection(skill.offSet);
        VFX.transform.position = finalPosition;
        Quaternion playerRotation = transform.rotation;
        Quaternion finalRotation = playerRotation * Quaternion.Euler(skill.rotation);
        VFX.transform.rotation = finalRotation;
        VFX.transform.localScale = Vector3.one * skill.size;

        PlayerTrackVFX track = VFX.GetComponent<PlayerTrackVFX>();
        track.SetTarget(targetTransform);  // Transform을 SetTarget으로 설정
    }
    public void StartSFX(string SFX)
    {
        SM.Instance.PlaySound2(SFX, transform.position);
    }

    public void StartSkill()
    {
        float skillGauge = _master._PlayerInstanteState.skillGauge;
        if (skillGauge < 100f)
        {
            return;
        }
        _animator.SetFloat(_animFloatSkillGauge, skillGauge);
        _animator.SetTrigger(_animTriggerSkill);
    }
}