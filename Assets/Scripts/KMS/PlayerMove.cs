using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Vector2 moveInput = Vector2.zero;
    private Vector2 moveVector = Vector2.zero;
    public float moveSpeed = 2f;
    public float Speed = 2f;
    private CharacterController controller;
    public Animator animator;
    public Camera Cam;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    public void Update()
    {
        MoveOrder();
    }
    void OnMove(InputValue inputValue) // 이동(WASD)
    {
        moveInput = inputValue.Get<Vector2>();
        
    }

    void OnAtk(InputValue value)
    {
        // 공격 로직
        Debug.Log("Attack triggered");
    }

     void OnSkill(InputValue value)
    {
        // 스킬 로직
        Debug.Log("Skill triggered");
    }

    void OnBacuum(InputValue value)
    {
        // 백업 로직
        Debug.Log("Bacuum triggered");
    }

    void OnInventory(InputValue value)
    {
        // 인벤토리 로직
        Debug.Log("Inventory triggered");
    }

    void OnRotate(InputValue value)
    {
        // 회전 로직
        Debug.Log("Rotate triggered");
    }

    private void MoveOrder()
    {
        moveVector = Vector2.Lerp(moveVector, moveInput * moveSpeed * Speed, Time.deltaTime * 5);

        Vector3 moveVector3 = transform.right * moveVector.x + transform.forward * moveVector.y;
        Vector3 moveVector4 = transform.right * moveVector.x + Cam.transform.forward * moveVector.y;
        controller.Move(moveVector4 * Time.deltaTime);
        transform.rotation = Cam.transform.rotation;

        //animator.SetFloat("XSpeed", moveVector.x);
        //animator.SetFloat("ZSpeed", moveVector.y);
    }
}
