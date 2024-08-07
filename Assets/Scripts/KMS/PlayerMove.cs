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

    void OnMove(InputValue inputValue) // 이동(WASD)
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
        // 공격 로직
        Atk();
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
        Debug.Log("Rotate triggered");
    }

    private void MoveOrder()
    {
        moveVector = Vector2.Lerp(moveVector, moveInput * moveSpeed * speed, Time.deltaTime * 5);
        Vector3 moveDirection = new Vector3(moveVector.x, 0, moveVector.y);

        // 캐릭터를 이동 방향으로 회전
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }

        Vector3 moveVector3 = Cam.transform.right * moveVector.x + Cam.transform.forward * moveVector.y;
        controller.Move(moveVector3 * Time.deltaTime);

        // 애니메이션 설정
        // animator.SetFloat("XSpeed", moveVector.x);
        // animator.SetFloat("ZSpeed", moveVector.y);
    }
}
