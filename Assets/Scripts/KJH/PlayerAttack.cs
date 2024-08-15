using EnumTypes;
using System.ComponentModel;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject Prefab_Projectile;

    [SerializeField] Transform projectile_InitPos;
    [SerializeField] float projectionSpeed_Forward = 15;
    [SerializeField] float projectionSpeed_Up = 3;
    [SerializeField] float attack_CoolTime = 0.7f;

    InputManager _InputManager;
    PlayerCameraMove _PlayerCameraMove;
    PlayerMaster _PlayerMaster;
    AttackSystem _AttackSystem;
    PlayerModChangeManager _PlayerMod;

    float delayTime = 0;
    bool attackTrigger = false;
    bool attackBool = false;

    bool skillTrigger = false;
    bool skillBool = false;

    [SerializeField] AttackType _currentAttackType = AttackType.CloseNormal; 
    [SerializeField] int _initialAttackComboIndex = 0; 
    [SerializeField] PlayerAttackType _currentAttackType = PlayerAttackType.CloseNormal;
    [SerializeField] int _initialAttackComboIndex = 0;
    int _currentAttackCount;

    private PlayerAttackType _currentPlayerAttackType;

    private void Awake()
    {
        _InputManager = InputManager.Instance;
        _InputManager.PropertyChanged += OnInputPropertyChanged;

        _PlayerCameraMove = PlayerCameraMove.Instance;

        _PlayerMaster = GetComponent<PlayerMaster>();
        _AttackSystem = GetComponent<AttackSystem>();
        _PlayerMod = GetComponent<PlayerModChangeManager>();

        _PlayerMod.OnSucceseAbsorpt += ChangeAttackState;
        _PlayerMod.OnEnterAbsorptState += ChangeAbsorbing;
        _PlayerMod.OnEndAbsorptState += AbsorbingFall;

    }

    private void OnDestroy()
    {
        _PlayerMod.OnSucceseAbsorpt -= ChangeAttackState;
        _InputManager.PropertyChanged -= OnInputPropertyChanged;
        _PlayerMod.OnEnterAbsorptState -= AbsorbingFall;
    }

    private void ChangeAbsorbing()
    {
        Debug.Log("��������");
        _AttackSystem.Absober();
    }
    private void AbsorbingFall()
    {
        Debug.Log("AbsorbingFall");
        ChangeAttackState(false);
        _AttackSystem.AbsoberEnd();
    }
    private void ChangeAttackState(bool isMelee)
    {
        if (isMelee)
        {
            _currentAttackType = PlayerAttackType.CloseNormal;
            _AttackSystem.ModTransform();
        }
        else
        {
            _currentAttackType = PlayerAttackType.RangeNormal;
            _AttackSystem.ModTransform();
        }
    }

    bool prevAttackTrigger = false;
    private void Update()
    {
        delayTime += Time.deltaTime;
        //if(attackTrigger && delayTime >= attack_CoolTime + attack_Delay && !_PlayerMaster.IsMeleeMode && !_PlayerMaster.IsAbsorptState)
        if (attackTrigger && !prevAttackTrigger)
        {
            delayTime = 0;
            _PlayerMaster.OnAttackState(_PlayerCameraMove.CamRotation() * Vector3.forward);

            _AttackSystem.StartAttack((int)_currentAttackType, _initialAttackComboIndex);
            //StartCoroutine(Attack_Delayed(attack_Delay));
        }
        if (!attackTrigger && prevAttackTrigger)
        {
            if (_currentAttackType == AttackType.CloseNormal)
            {
                _AttackSystem.OnRelease();
            }
        }
        //if (skillTrigger && !prevAttackTrigger)
        if (skillTrigger)
        {
            delayTime = 0;
            _PlayerMaster.OnAttackState(_PlayerCameraMove.CamRotation() * Vector3.forward);
            _AttackSystem.StartSkill((int)_currentAttackType, _PlayerMaster._PlayerInstanteState.skillGauge);
        }
        //if(!skillTrigger && prevAttackTrigger)
        if (!skillTrigger)
        {
            if (_currentAttackType == PlayerAttackType.CloseNormal)
            {
                _AttackSystem.OnRelease();
            }
        }
        prevAttackTrigger = attackTrigger;
    }

    public PlayerAttackType GetCurrentAttackType()
    {
        if (_currentAttackType == PlayerAttackType.CloseNormal)
        {
            //checkAttackCount
            return PlayerAttackType.CloseNormal;
        }
        return _currentPlayerAttackType;
    }
    public int GetCurrentAttackCount()
    {
        return _currentAttackCount;
    }
    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_InputManager.IsLMouseBtnClick):
                attackTrigger = _InputManager.IsLMouseBtnClick;
                attackBool = _InputManager.IsLMouseBtnClick;
                break;
            case nameof(_InputManager.IsRMouseBtnClick):
                skillTrigger = _InputManager.IsRMouseBtnClick;
                skillBool = _InputManager.IsRMouseBtnClick;
                Debug.Log("R �÷��̾ų");
                break;

        }
    }

    void ShootProjectile()
    {
        Projectile projectile = ObjectPoolManager.Instance.DequeueObject(Prefab_Projectile).GetComponent<Projectile>();

        Vector3 projectionVector = _PlayerCameraMove.CamRotation() * Vector3.forward * projectionSpeed_Forward + Vector3.up * projectionSpeed_Up;
        //���ýý��ۿ��� ���� ������ Ÿ���� �����´�
        projectile.Init(_PlayerMaster._PlayerInstanteState.GetDmg(_currentPlayerAttackType, GetCurrentAttackCount()), projectile_InitPos.position, projectionVector, OnProjectileHit);

        _PlayerMaster._PlayerInstanteState.BulletConsumption();
        IncreaseAttackCount();
    }

    void OnProjectileHit()
    {
        _PlayerMaster._PlayerInstanteState.SkillGaugeRecovery(10);
        Debug.Log("���� ����");
    }

    public void ResetAttackCount()
    {
        _currentAttackCount = 0;
    }
    public void IncreaseAttackCount()
    {
        _currentAttackCount++;
    }

    //IEnumerator Attack_Delayed(float delayTime)
    //{
    //    yield return new WaitForSeconds(delayTime);
    //    Attack();
    //}
}
