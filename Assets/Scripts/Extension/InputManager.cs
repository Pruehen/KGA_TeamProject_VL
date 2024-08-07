using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{   
    static Vector2 _moveVector2_Left;
    static float _rotateValue;

    static bool _isLMouseBtnClick;
    static bool _isRMouseBtnClick;
    static bool _isLControlBtnClick;
    static bool _isTapBtnClick;

    public static Vector2 MoveVector2_Left
    {
        get { return _moveVector2_Left; }
        set
        {
            if (_moveVector2_Left != value)
            {
                _moveVector2_Left = value;
                OnPropertyChanged(nameof(MoveVector2_Left));
            }
        }
    }

    public static float RotateValue
    {
        get { return _rotateValue; }
        set
        {
            if (_rotateValue != value)
            {
                _rotateValue = value;
                OnPropertyChanged(nameof(RotateValue));
            }
        }
    }

    public static bool IsLMouseBtnClick
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

    public static bool IsRMouseBtnClick
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

    public static bool IsLControlBtnClick
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

    public static bool IsTapBtnClick
    {
        get { return _isTapBtnClick; }
        set
        {
            if (_isTapBtnClick != value)
            {
                _isTapBtnClick = value;
                OnPropertyChanged(nameof(IsTapBtnClick));
            }
        }
    }

    #region PropChanged
    public static event PropertyChangedEventHandler PropertyChanged;

    public static void OnPropertyChanged(string propertyName)//값이 변경되었을 때 이벤트를 발생시키기 위한 용도 (데이터 바인딩)
    {
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    static void OnMove(InputValue inputValue) // 이동(WASD)
    {
        MoveVector2_Left = inputValue.Get<Vector2>();
    }

    static void OnAtk(InputValue value) // 마우스 좌클릭
    {
        IsLMouseBtnClick = value.Get<bool>();
    }

    static void OnSkill(InputValue value)//마우스 우클릭
    {
        IsRMouseBtnClick = value.Get<bool>();
    }

    static void OnBacuum(InputValue value)//좌측 컨트롤 클릭
    {
        IsLControlBtnClick = value.Get<bool>();
    }

    static void OnInventory(InputValue value)//탭 버튼 클릭
    {
        IsTapBtnClick = value.Get<bool>();
    }

    static void OnRotate(InputValue value)
    {
        RotateValue = value.Get<float>();
    }
}
