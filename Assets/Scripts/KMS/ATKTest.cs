using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class ATKTest : MonoBehaviour
{
    
    InputManager _InputManager;
    Animator _animator;

    int _startAttackTrigger;
    int _endAttackTrigger;
    bool _isAttacking;
    
    private void Awake()
    {
        //_InputManager = InputManager.Instance;
        //_InputManager.PropertyChanged += OnInputPropertyChanged;
        _animator= GetComponent<Animator>();        
    }

    

    public void Init(Animator animator)
    {
        _animator = animator;
        _startAttackTrigger = Animator.StringToHash("StartCloseAttack");
        _endAttackTrigger = Animator.StringToHash("EndCloseAttack");
    }

    public void StartAttack()
    {
        _animator.SetTrigger("Attack");
    }

    public void EndAttack()
    {
        _animator.SetTrigger("ChargingEnd");
    }
    public void Click()
    {
        _animator.SetTrigger("Attack");
        _animator.SetTrigger("ChargingEnd");
    }
    public void Hold()
    {

        _animator.SetTrigger("Attack");
    }
    
    public void Release()
    {
        _animator.SetTrigger("ChargingEnd");
    }

    public void ATKEnd()
    {
        if (!_animator.GetBool("Attack"))
        {
            _animator.SetTrigger("AttackEnd");
            Debug.Log("ATKEnd");
        }
    }

    void Update()
    {
        
    }
    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_InputManager.IsLMouseBtnClick):
                _isAttacking = _InputManager.IsLMouseBtnClick;
                break;
        }
        //인풋매니저를 통해서 좌클릭시에 메세지를 send
    }

  
}
