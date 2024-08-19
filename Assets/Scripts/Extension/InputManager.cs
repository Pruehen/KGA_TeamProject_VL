using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SceneSingleton<InputManager>
{   
    Vector2 _moveVector2_Left_WASD;
    Vector2 _moveVector2_Right_Mouse;

    bool _isLMouseBtnClick;
    bool _isRMouseBtnClick;
    bool _isLControlBtnClick;
    bool _isChipUiToggleBtnClick;
    bool _isRControlBtnClick;
    bool _isDashBtnClick;
    bool _isInteractiveBtnClick;

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

    public Vector2 MoveVector2_Right_Mouse
    {
        get { return _moveVector2_Right_Mouse; }
        set
        {
            if (_moveVector2_Right_Mouse != value)
            {
                _moveVector2_Right_Mouse = value;
                OnPropertyChanged(nameof(MoveVector2_Right_Mouse));
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

    #region PropChanged
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string propertyName)//값이 변경되었을 때 이벤트를 발생시키기 위한 용도 (데이터 바인딩)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    void OnMove(InputValue inputValue) // 이동(WASD)
    {        
        MoveVector2_Left_WASD = inputValue.Get<Vector2>();
    }

    void OnAtk(InputValue value) // 마우스 좌클릭
    {
        IsLMouseBtnClick = value.isPressed;
    }

    void OnSkill(InputValue value)//마우스 우클릭
    {
        IsRMouseBtnClick = value.isPressed;
    }

    void OnBacuum(InputValue value)//좌측 컨트롤 클릭
    {
        IsLControlBtnClick = value.isPressed;
    }

    void OnInventory(InputValue value)//T 버튼 클릭
    {
        IsChipUiToggleBtnClick = value.isPressed;
    }
    void OnInteractive(InputValue value)//F 버튼 클릭
    {
        IsInteractiveBtnClick = value.isPressed;
    }

    void OnRotate(InputValue value)//마우스 좌표
    {
        MoveVector2_Right_Mouse = value.Get<Vector2>();
    }

    void OnDash(InputValue value)//스페이스 버튼 클릭
    {
        IsDashBtnClick = value.isPressed;
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
