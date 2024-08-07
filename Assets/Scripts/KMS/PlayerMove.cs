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

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    public void Update()
    {
        MoveOrder();
    }
    void OnMove(InputValue inputValue) // �̵�(WASD)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    void OnAtk(InputValue value)
    {
        // ���� ����
        Debug.Log("Attack triggered");
    }

     void OnSkill(InputValue value)
    {
        // ��ų ����
        Debug.Log("Skill triggered");
    }

    void OnBacuum(InputValue value)
    {
        // ��� ����
        Debug.Log("Bacuum triggered");
    }

    void OnInventory(InputValue value)
    {
        // �κ��丮 ����
        Debug.Log("Inventory triggered");
    }

    void OnRotate(InputValue value)
    {
        // ȸ�� ����
        Debug.Log("Rotate triggered");
    }

    private void MoveOrder()
    {
        moveVector = Vector2.Lerp(moveVector, moveInput * moveSpeed * Speed, Time.deltaTime * 5);

        Vector3 moveVector3 = transform.right * moveVector.x + transform.forward * moveVector.y;
        controller.Move(moveVector3 * Time.deltaTime);

        //animator.SetFloat("XSpeed", moveVector.x);
        //animator.SetFloat("ZSpeed", moveVector.y);
    }
}
