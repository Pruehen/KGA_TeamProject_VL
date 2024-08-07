using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Vector2 moveInput = Vector2.zero;
    private Vector2 moveVector = Vector2.zero;
    public float moveSpeed = 2f;
    public float speed = 2f;
    public float rotSpeed = 2f;
    
    private CharacterController controller;
    public Animator animator;
    public Camera Cam;

    public Transform atkPoint;
    public GameObject Item;

    public float Rotate;
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
        RotateCharacter();
        //MoveOrder();
    }

    void Atk()
    {
        RotateCharacter();
         GameObject items = Instantiate(Item,atkPoint.position,transform.rotation);
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
        //인풋시스템활용 카메라 회전
        //Rotate = value.Get<Vector2>().x;
        //Debug.Log(value.Get<Vector2>());
        //Cam.transform.Rotate(Vector3.up * Rotate);
        Debug.Log("Rotate triggered");
    }

    private void RotateCharacter()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, Cam.transform.eulerAngles.y, 0f), rotSpeed );
    }

    private void MoveOrder()
    {
        moveVector = Vector2.Lerp(moveVector, moveInput * moveSpeed * speed, Time.deltaTime * 5);
        Vector3 moveVector3 = transform.right * moveVector.x + transform.forward * moveVector.y;
        Vector3 moveVector4 = transform.right * moveVector.x + Cam.transform.forward * moveVector.y;
        //transform.rotation = moveVector4;
        controller.Move(moveVector4 * Time.deltaTime);

        // 애니메이션 설정
        // animator.SetFloat("XSpeed", moveVector.x);
        // animator.SetFloat("ZSpeed", moveVector.y);
    }
}
