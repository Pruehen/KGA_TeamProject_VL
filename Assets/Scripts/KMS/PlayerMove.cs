using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Vector2 moveInput = Vector2.zero;
    private Vector2 moveVector = Vector2.zero;
    public float moveSpeed = 5f;
    public float speed = 5f;
    public float rotSpeed = 40f;

    private CharacterController controller;
    public Animator animator;
    public Camera Cam;

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

        GameObject items = Instantiate(Item, atkPoint.position, transform.rotation);
        RotateCharacter();
    }
    void RotateCharacter()
    {
        if (moveInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }

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
        Vector3 moveDirection = new Vector3(moveVector.x, 0, moveVector.y);

        // ĳ���͸� �̵� �������� ȸ��
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }

        Vector3 moveVector3 = Cam.transform.right * moveVector.x + Cam.transform.forward * moveVector.y;
        controller.Move(moveVector3 * Time.deltaTime);

        // �ִϸ��̼� ����
        // animator.SetFloat("XSpeed", moveVector.x);
        // animator.SetFloat("ZSpeed", moveVector.y);
    }
}
