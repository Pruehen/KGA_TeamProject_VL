using System;
using UnityEngine;

public class Player : SceneSingleton<Player>
{
    public Vector2 inputVector_Move { get; private set; }
    public Vector3 lookTargetPosVector { get; private set; }

    Action<Vector2> onInput_Move;
    public Action<Vector3> OnLookTargetPosSet;
    public Action<string> OnMouseObjectNameChanged;

    [SerializeField] Charactor controlledCharactor;

    IInteractable onMouseObject;
    bool _onInteration;

    private void Start()
    {
        onInput_Move += Command_CharactorMove;
        OnLookTargetPosSet += Command_SetCharactorLookPos;
    }
    // Update is called once per frame
    void Update()
    {
        PlayerInput_OnUpdate();
        MousePosCheck_OnUpdate();
        MouseClickCheck_OnUpdate();
    }    

    void PlayerInput_OnUpdate()
    {
        inputVector_Move = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputVector_Move += new Vector2(0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector_Move += new Vector2(0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector_Move += new Vector2(-1, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector_Move += new Vector2(1, 0);
        }

        inputVector_Move = inputVector_Move.normalized;
        onInput_Move?.Invoke(inputVector_Move);
    }
    
    void MousePosCheck_OnUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            string onMouseObjectName = null;

            if (hit.collider.TryGetComponent(out onMouseObject))
            {
                lookTargetPosVector = onMouseObject.GetPos();
                onMouseObjectName = onMouseObject.GetName();                
            }
            else
            {
                onMouseObject = null;                
                lookTargetPosVector = hit.point;
            }
            OnMouseObjectNameChanged?.Invoke(onMouseObjectName);
            OnLookTargetPosSet?.Invoke(lookTargetPosVector);
        }
    }

    void MouseClickCheck_OnUpdate()
    {
        if(Input.GetMouseButton(1))
        {
            _onInteration = true;
            Command_TryInteract();
        }
        if(Input.GetMouseButtonUp(1))
        {
            _onInteration = false;
            Command_EndInteract();
        }
    }

    void Command_CharactorMove(Vector2 inputVector)
    {
        if(controlledCharactor != null)
        {
            controlledCharactor.SetMoveVector(new Vector3(inputVector.x, 0, inputVector.y));
        }
    }
    void Command_SetCharactorLookPos(Vector3 pos)
    {
        if (controlledCharactor != null)
        {
            controlledCharactor.SetLookPosVector(pos);
        }
    }
    void Command_TryInteract()
    {
        if (controlledCharactor != null)
        {
            controlledCharactor.TryInteract(onMouseObject);
        }
    }
    void Command_EndInteract()
    {
        if (controlledCharactor != null)
        {
            controlledCharactor.EndInteract();
        }
    }
}
