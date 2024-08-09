using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject Prefab_Projectile;

    [SerializeField] Transform projectile_InitPos;    
    [SerializeField] float projectionSpeed_Forward = 15;
    [SerializeField] float projectionSpeed_Up = 3;
    [SerializeField] float attack_CoolTime = 0.7f;
    [SerializeField] float attack_Delay = 0.5f;

    InputManager _InputManager;
    PlayerCameraMove _PlayerCameraMove;
    PlayerMaster _PlayerMaster;

    float delayTime = 0;
    bool attackTrigger = false;

    private void Awake()
    {
        _InputManager = InputManager.Instance;
        _InputManager.PropertyChanged += OnInputPropertyChanged;        

        _PlayerCameraMove = PlayerCameraMove.Instance;

        _PlayerMaster = GetComponent<PlayerMaster>();
    }

    private void Update()
    {
        delayTime += Time.deltaTime;

        if(attackTrigger && delayTime >= attack_CoolTime + attack_Delay && !_PlayerMaster.IsMeleeMode && !_PlayerMaster.IsAbsorptState)
        {
            delayTime = 0;
            _PlayerMaster.OnAttackState(_PlayerCameraMove.CamAxisTransform().forward);
            //Attack();
            StartCoroutine(Attack_Delayed(attack_Delay));
        }
    }

    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_InputManager.IsLMouseBtnClick):
                attackTrigger = _InputManager.IsLMouseBtnClick;
                break;
        }
    }

    void Attack()
    {
        Projectile projectile = ObjectPoolManager.Instance.DequeueObject(Prefab_Projectile).GetComponent<Projectile>();

        Vector3 projectionVector = _PlayerCameraMove.CamAxisTransform().forward * projectionSpeed_Forward + Vector3.up * projectionSpeed_Up;

        projectile.Init(_PlayerMaster._PlayerInstanteState.GetAttackPower(), projectile_InitPos.position, projectionVector, OnProjectileHit);

        _PlayerMaster._PlayerInstanteState.BulletConsumption();
    }

    void OnProjectileHit()
    {
        _PlayerMaster._PlayerInstanteState.SkillGaugeRecovery(10);
        Debug.Log("공격 성공");
    }

    IEnumerator Attack_Delayed(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Attack();
    }
}
