using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{    
    [SerializeField] float rotSpeed = 0.2f;

    Vector3 _moveVector3_Origin;
    Vector3 _moveVector3;
    Vector3 _lookTargetPos;

    AttackSystem _attackSystem;

    Animator _animator;

 

    private void Start()
    {
        _attackSystem = GetComponent<AttackSystem>();
        _animator = GetComponent<Animator>();
    }

    public void OnAttackState(Vector3 attaciDir)
    {
        SetMoveLock(0.4f);
        _lookTargetPos = new Vector3(attaciDir.x, 0, attaciDir.z);
        StartCoroutine(Rotate_Coroutine(0.3f));
    }

    bool _isMoving = true;
    bool _isGrounded = true;
    bool _isDashing = false;
    public void SetMoveLock(float time)
    {
        //_Rigidbody.velocity = Vector3.zero;
        StartCoroutine(SetMoveLock_Coroutine(time));
    }
    IEnumerator SetMoveLock_Coroutine(float time)
    {
        _isMoving = false;
        yield return new WaitForSeconds(time);

        _isMoving = true;
    }
    public void SetDashLock(float time)
    {
        //_Rigidbody.velocity = Vector3.zero;
        StartCoroutine(SetDashLock_Coroutine(time));
    }
    IEnumerator SetDashLock_Coroutine(float time)
    {
        _isDashing = true;
        yield return new WaitForSeconds(time);

        _isDashing = false;
    }

    InputManager _InputManager;
    PlayerCameraMove _PlayerCameraMove;
    Rigidbody _Rigidbody;
    PlayerMaster _PlayerMaster;

    private void Awake()
    {
        _InputManager = InputManager.Instance;
        _InputManager.PropertyChanged += OnInputPropertyChanged;

        _Rigidbody = GetComponent<Rigidbody>();
        _PlayerMaster = GetComponent<PlayerMaster>();

        _PlayerCameraMove = PlayerCameraMove.Instance;
    }

    public void FixedUpdate()
    {
        CheckGounded_OnFixedUpdate();
        Move_OnFixedUpdate();
        Rotate_OnFixedUpdate();
    }
    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_InputManager.MoveVector2_Left_WASD):
                float moveSpeed = _PlayerMaster._PlayerInstanteState.GetMoveSpeed();
                _moveVector3_Origin = new Vector3(_InputManager.MoveVector2_Left_WASD.x * moveSpeed, 0, _InputManager.MoveVector2_Left_WASD.y * moveSpeed);
                break;
            case nameof(_InputManager.IsDashBtnClick):
                if(_InputManager.IsDashBtnClick)
                {
                    Dash();
                }
                break;
        }
    }
    void CheckGounded_OnFixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(transform.position, .5f, ~LayerMask.GetMask("Character_Collider"));
    }
    void Move_OnFixedUpdate()
    {
        if (_isMoving && !_PlayerMaster.IsAbsorptState && !_attackSystem.AttackLockMove && _isGrounded && !_isDashing)
        {
            _moveVector3 = new Vector3(_moveVector3_Origin.x, 0, _moveVector3_Origin.z);

            // 카메라의 회전 방향을 기준으로 이동 방향 계산 
            if (_PlayerCameraMove != null)
            {
                _moveVector3 = _PlayerCameraMove.CamRotation() * _moveVector3_Origin;
            }


            _Rigidbody.velocity = _moveVector3;

            _animator.SetBool("IsMoving", _moveVector3.x != 0f || _moveVector3.z != 0f);
        }
        else if (_isDashing)
        {
            //_animator.SetBool("IsDashing", true);
        }
        else if (_isGrounded)
        {
            _animator.SetBool("IsMoving", false);
            _Rigidbody.velocity = new Vector3(0f, _Rigidbody.velocity.y, 0f);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
            _animator.SetBool("IsFalling", true);
        }
        if (!_isDashing)
        {
            //_animator.SetBool("IsDashing", false);
        }
        _animator.SetFloat("XSpeed", _moveVector3.x);
        _animator.SetFloat("ZSpeed", _moveVector3.z);
    }

    void Rotate_OnFixedUpdate()
    {
        // 캐릭터를 이동 방향으로 회전
        if (_moveVector3 != Vector3.zero && _isMoving && !_PlayerMaster.IsAbsorptState && !_attackSystem.AttackLockMove)
        {
            _lookTargetPos = _moveVector3;

            Quaternion targetRotation = Quaternion.LookRotation(_lookTargetPos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed);
        }
    }

    IEnumerator Rotate_Coroutine(float time)
    {
        float onTime = 0;
        while (onTime < time)
        {
            yield return null;
            onTime += Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(_lookTargetPos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed);
        }
    }

    public void Dash()
    {

        if (_PlayerMaster._PlayerInstanteState.TryStaminaConsumption(30))
        {
            OnlyDash();
            _attackSystem.ReleaseLockMove();
            _attackSystem.ResetEndAttack();
            _animator.SetTrigger("Dash");
            if (_moveVector3 == Vector3.zero)
            {
                _PlayerMaster.OnAttackState(_PlayerCameraMove.CamRotation() * Vector3.forward);
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(_lookTargetPos, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed);
            }
        }
        else
        {
            return;
        }
    }
    //원거리4타시 호출
    public void OnlyDash()
    {
        Vector3 newPoint = _moveVector3_Origin;

        if (newPoint == Vector3.zero)
        {
            newPoint = new Vector3(0, 0, 14.26f);
        }
        _Rigidbody.AddForce(_PlayerCameraMove.CamRotation() * newPoint * 100f, ForceMode.Acceleration);
        SetDashLock(.2f);
    }
}
