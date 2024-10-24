using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SceneSingleton<InputManager>
{   
    Vector2 _moveVector2_Left_WASD;
    Vector2 _rotateVector2_Mouse;

    bool _isLMouseBtnClick;
    bool _isRMouseBtnClick;
    bool _isLControlBtnClick;
    bool _isChipUiToggleBtnClick;
    bool _isRControlBtnClick;
    bool _isDashBtnClick;
    bool _isInteractiveBtnClick;
    bool _isEnterBtnClick;
    bool _isEscapeBtnClick;

    public Vector2 MoveVector2_Left_WASD
    {
        get { return _moveVector2_Left_WASD; }
        set
        {
            if (_moveVector2_Left_WASD != value)
            {
                _moveVector2_Left_WASD = value;
                OnPropertyChanged(nameof(MoveVector2_Left_WASD));
            }
        }
    }

    public Vector2 RotateVector2_Rotate
    {
        get { return _rotateVector2_Mouse; }
        set
        {
            if (_rotateVector2_Mouse != value)
            {
                _rotateVector2_Mouse = value;
                OnPropertyChanged(nameof(RotateVector2_Rotate));
            }
        }
    }

    public bool IsLMouseBtnClick
    {
        get { return _isLMouseBtnClick; }
        set
        {
            if (_isLMouseBtnClick != value)
            {
                _isLMouseBtnClick = value;
                OnPropertyChanged(nameof(IsLMouseBtnClick));
            }
        }
    }

    public bool IsRMouseBtnClick
    {
        get { return _isRMouseBtnClick; }
        set
        {
            if (_isRMouseBtnClick != value)
            {
                _isRMouseBtnClick = value;
                OnPropertyChanged(nameof(IsRMouseBtnClick));
            }
        }
    }

    public bool IsLControlBtnClick
    {
        get { return _isLControlBtnClick; }
        set
        {
            if (_isLControlBtnClick != value)
            {
                _isLControlBtnClick = value;
                OnPropertyChanged(nameof(IsLControlBtnClick));
            }
        }
    }

    public bool IsChipUiToggleBtnClick
    {
        get { return _isChipUiToggleBtnClick; }
        set
        {
            if (_isChipUiToggleBtnClick != value)
            {
                _isChipUiToggleBtnClick = value;
                OnPropertyChanged(nameof(IsChipUiToggleBtnClick));
            }
        }
    }

    public bool IsDashBtnClick
    {
        get { return _isDashBtnClick; }
        set
        {
            if(_isDashBtnClick != value)
            {
                _isDashBtnClick = value;
                OnPropertyChanged(nameof(IsDashBtnClick));
            }
        }
    }

    float last_InteractiveBtnClickSetTime;
    public bool IsInteractiveBtnClick
    {
        get { return _isInteractiveBtnClick; }
        set
        {
            if(_isInteractiveBtnClick != value)
            {
                _isInteractiveBtnClick = value;
                OnPropertyChanged(nameof(IsInteractiveBtnClick));
            }
        }
    }

    public bool IsInteractiveBtnClick_CoolTime
    {
        get { 
            if(Time.time - last_InteractiveBtnClickSetTime > 0.5f && _isInteractiveBtnClick)
            {
                last_InteractiveBtnClickSetTime = Time.time;
                return true;
            }
            return false;
        }
    }

    public bool IsEnterBtnClick
    {
        get { return _isEnterBtnClick; }
        set
        {
            if(_isEnterBtnClick != value)
            {
                _isEnterBtnClick = value;
                OnPropertyChanged(nameof(IsEnterBtnClick));
            }
        }
    }
    public bool IsEscapeBtnClick
    {
        get { 
            return _isEscapeBtnClick; }
        set
        {
            if(_isEscapeBtnClick != value)
            {
                _isEscapeBtnClick = value;
                OnPropertyChanged(nameof(IsEscapeBtnClick));
            }
        }
    }    
    
    public bool IsTyping { get; set; }


    #region PropChanged
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string propertyName)//���� ����Ǿ��� �� �̺�Ʈ�� �߻���Ű�� ���� �뵵 (������ ���ε�)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    void OnMove(InputValue inputValue) // �̵�(WASD)
    {        
        if(IsTyping)
        {
            return;
        }
        MoveVector2_Left_WASD = inputValue.Get<Vector2>();
    }

    void OnAtk(InputValue value) // ���콺 ��Ŭ��
    {
        if(IsTyping)
        {
            return;
        }
        IsLMouseBtnClick = value.isPressed;
    }

    void OnSkill(InputValue value)//���콺 ��Ŭ��
    {
        if(IsTyping)
        {
            return;
        }
        IsRMouseBtnClick = value.isPressed;
    }

    void OnBacuum(InputValue value)//���� ��Ʈ�� Ŭ��
    {
        if(IsTyping)
        {
            return;
        }
        IsLControlBtnClick = value.isPressed;
    }

    void OnInventory(InputValue value)//Tab ��ư Ŭ��
    {
        if(IsTyping)
        {
            return;
        }
        IsChipUiToggleBtnClick = value.isPressed;
    }
    void OnInteractive(InputValue value)//F ��ư Ŭ��
    {
        if(IsTyping)
        {
            return;
        }
        IsInteractiveBtnClick = value.isPressed;
    }
    void OnEnter(InputValue value)//Enter ��ư Ŭ��
    {
        IsEnterBtnClick = value.isPressed;
    }

    void OnRotate(InputValue value)//���콺 ��ǥ
    {
        if(IsTyping)
        {
            return;
        }
        RotateVector2_Rotate = value.Get<Vector2>();
    }

    void OnDash(InputValue value)//�����̽� ��ư Ŭ��
    {
        if(IsTyping)
        {
            return;
        }
        IsDashBtnClick = value.isPressed;
    }
    void OnEscape(InputValue value)//Escape ��ư Ŭ��
    {
        IsEscapeBtnClick = value.isPressed;
    }
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }
}
