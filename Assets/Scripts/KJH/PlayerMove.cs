using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{    
    [SerializeField] float rotSpeed = 0.2f;

    Vector3 _moveVector3_Origin;
    Vector3 _moveVector3;
    Vector3 _lookTargetPos;

    public void OnAttackState(Vector3 attaciDir)
    {
        SetMoveLock(0.4f);
        _lookTargetPos = new Vector3(attaciDir.x, 0, attaciDir.z);
        StartCoroutine(Rotate_Coroutine(0.3f));
    }

    bool _isMoving = true;
    bool _isDash = false;
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
        Move_OnFixedUpdate();
        Rotate_OnFixedUpdate();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }
    }

    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_InputManager.MoveVector2_Left_WASD):
                float moveSpeed = _PlayerMaster._PlayerInstanteState.GetMoveSpeed();
                _moveVector3_Origin = new Vector3(_InputManager.MoveVector2_Left_WASD.x * moveSpeed, 0, _InputManager.MoveVector2_Left_WASD.y * moveSpeed);
                break;            
        }
    }

    void Move_OnFixedUpdate()
    {
        if (_isMoving && !_PlayerMaster.IsAbsorptState && !_isDash)
        {
            _moveVector3 = new Vector3(_moveVector3_Origin.x, 0, _moveVector3_Origin.z);

            // 카메라의 회전 방향을 기준으로 이동 방향 계산 
            if (_PlayerCameraMove != null)
            {
                _moveVector3 = _PlayerCameraMove.CamRotation() * _moveVector3_Origin;
            }

            //이동구현(에드포스 , 벨로시티)
            //_Rigidbody.AddForce(_moveVector3, ForceMode.VelocityCha);
            _moveVector3.y = _Rigidbody.velocity.y;
            _Rigidbody.velocity = _moveVector3;

           // this.transform.position += (_moveVector3 * Time.deltaTime);

            // 애니메이션
            // animator.SetFloat("XSpeed", moveVector.x);
            // animator.SetFloat("ZSpeed", moveVector.y);
        }
    }

    void Rotate_OnFixedUpdate()
    {
        // 캐릭터를 이동 방향으로 회전
        if (_moveVector3 != Vector3.zero && _isMoving && !_PlayerMaster.IsAbsorptState)
        {
            _lookTargetPos = _moveVector3;

            Quaternion targetRotation = Quaternion.LookRotation(_lookTargetPos, Vector3.up);

            targetRotation = FreezeXZ(targetRotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed);
        }
    }

    Quaternion FreezeXZ(Quaternion initialAngle)
    {
        Vector3 rotEuler = initialAngle.eulerAngles;
        rotEuler.x = 0;
        rotEuler.z = 0;
        initialAngle.eulerAngles = rotEuler;
        return initialAngle;
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
        StopCoroutine(Dash_Coroutine());
        StartCoroutine(Dash_Coroutine());
    }

    IEnumerator Dash_Coroutine()
    {
        if(!_isDash)
        {
            _Rigidbody.velocity *= 3;
            transform.position = new Vector3(0, 1, _Rigidbody.velocity.x);


        }
        _isDash = true;

        yield return new WaitForSeconds(0.5f);
        _isDash = false;
    }

}
