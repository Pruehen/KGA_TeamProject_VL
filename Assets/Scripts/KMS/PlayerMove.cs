using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Vector2 moveInput = Vector2.zero;
    private Vector2 moveVector = Vector2.zero;
    public float moveSpeed = 10f;
    public float speed = 10f;
    public float rotSpeed = 50f;
    private Vector3 velocity;
    private CharacterController controller;
    public Animator animator;
    public Camera Cam;
    private bool isGravityInverted = false;
    public float gravity = -9.81f;

    public Transform atkPoint;
    public GameObject Item;

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
        Debug.Log(moveInput);
    }

    void Atk()
    {
        //RotateCharacter();
        GameObject items = Instantiate(Item, atkPoint.position, transform.rotation);
    }

    void OnAtk(InputValue value)
    {
        // ���� ����
        Atk();
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
        Debug.Log("Rotate triggered");
    }

    private void MoveOrder()
    {
        moveVector = Vector2.Lerp(moveVector, moveInput * moveSpeed * speed, Time.deltaTime * 5);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // ī�޶��� ȸ�� ������ �������� �̵� ���� ���
        Vector3 moveDirection = Cam.transform.right * moveVector.x + Cam.transform.forward * moveVector.y;
        moveDirection.y = 0; // ���� ���� ����

        // ĳ���͸� �̵� �������� ȸ��
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }

        float currentGravity = isGravityInverted ? -gravity : gravity;
        velocity.y += currentGravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);

        // �ִϸ��̼�
        // animator.SetFloat("XSpeed", moveVector.x);
        // animator.SetFloat("ZSpeed", moveVector.y);
    }
}
