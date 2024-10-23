using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NetPlayer : NetworkBehaviour
{
    [Header("MetaNetworkManager")]
    public MetaNetworkManager MetaNetworkManager;

    [Header("Components")]
    public NavMeshAgent NavAgent_Player;
    public Animator Animator_Player;

    [Header("Movement")]
    public Transform Mesh;
    public float _rotationSpeed = 100.0f;

    [Header("Interaction")]
    public KeyCode _atkKey = KeyCode.F;

    [SerializeField] Rigidbody RigidBody_Player;
    [SerializeField] Transform Transform_CameraParent;
    [SerializeField] Transform Transform_Camera;
    [SerializeField] float _cameraDistance = 4.0f;

    [Header("ChatMesh")]
    [SerializeField] Transform Transform_ChatRoot;
    [SerializeField] GameObject Prefab_SpeechBallonSlotUI;

    [Header("SpawnFieldObj")]
    [SerializeField] Transform Transform_SpawnObj;
    [SerializeField] LayerMask collisionLayers;

    private float _moveSpeed = 5.0f;
    private float _mouseSensitivity = 100.0f;
    private float _cameraRotationX = 0.0f;
    private Transform _chatRoot;
    public bool IsNearNPC = false;
    public NPCDialogue NpcDialogue;

    public void SetNPC(NPCDialogue npcDialogue)
    {
        NpcDialogue = npcDialogue;
    }

    public Transform GetSpawnObjTransform() { return Transform_SpawnObj; }



    private void Start()
    {
        Transform_CameraParent.gameObject.SetActive(false);

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        var gObj = GameObject.Find("InputField_Chat");
        Debug.Log($"InputField null: {gObj == null}");
        if(gObj != null)
        {
            InputField_Chat = gObj.GetComponent<TMP_InputField>();
        }
        Debug.Log($"InputField: {InputField_Chat.name}");
        StartCoroutine(CoDelayBindRpc());
    }

    private IEnumerator CoDelayBindRpc()
    {
        yield return new WaitForSeconds(1.0f);

        var gObj = GameObject.Find("MetaNetworkManager");
        if(gObj != null)
        {
            var metaManager = gObj.GetComponent<MetaNetworkManager>();
            if(metaManager != null)
            {
                MetaNetworkManager = metaManager;
                if (this.isLocalPlayer)
                {
                    Transform_CameraParent.name = "LocalPlayerCamera";
                    MetaNetworkManager.BindLocalPlayerNetId(this.netId);
                    MetaNetworkManager.BindLocalPlayer(this);
                    Transform_CameraParent.gameObject.SetActive(true);
                }
                
                MetaNetworkManager.BindRpcAnimStateChangedCallback(OnRpcAnimStateChanged);
                MetaNetworkManager.BindRecvMsgCallback(OnRecvChatMsg);
            }
        }
    }

    private void OnRpcAnimStateChanged(uint netId, string animStateKey, bool isActive)
    {
        if(netId == this.netId)
        {
            if(IsAnimStateChanged(animStateKey, isActive))
            {
                Animator_Player.SetBool(animStateKey, isActive);
            }
        }
    }

    private void OnRecvChatMsg(uint netId, string msg)
    {
        if (this.isLocalPlayer)
        {
            return;
        }

        if(netId != this.netId)
        {
            return;
        }

        var gObj = GameObject.Find("ChatSlotUIRoot");
        if(gObj != null)
        {
            _chatRoot = gObj.transform;
            var slotUI = Instantiate(Prefab_SpeechBallonSlotUI, _chatRoot);
            if(slotUI != null)
            {
                var speechBallonUI = slotUI.GetComponent<SpeechBallonSlotUI>();
                if(speechBallonUI != null)
                {
                    speechBallonUI.SetSpeechText(Transform_ChatRoot, msg);
                }
            }
        }
    }

    [SerializeField] TMP_InputField InputField_Chat;
    private void Update()
    {
        if(CheckIsFocusedOnUpdate() == false)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            string curStateName = "Run";
            if(IsAnimStateChanged(curStateName, false))
            {
                CheckAndCancelAnimState("Sit");
                CheckAndCancelAnimState("InteractLoop");
                MetaNetworkManager?.RequestChangeAnimState(curStateName, false);
            }
            MetaNetworkManager.OnClick_ChatInputField();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.LoadMainScene();
        }
        if(InputField_Chat.isFocused)
        {
            
            return;
        }

        MoveOnUpdate();
    }

    private bool CheckIsFocusedOnUpdate()
    {
        return Application.isFocused;
    }

    private bool IsAnimStateChanged(string animState, bool targetState)
    {
        var curState = Animator_Player.GetBool(animState);
        return (curState != targetState);
    }

    private void CheckAndCancelAnimState(string animState)
    {
        if (Animator_Player.GetBool(animState))
        {
            MetaNetworkManager?.RequestChangeAnimState(animState, false);
        }
    }

    private void MoveOnUpdate()
    {
        if (isLocalPlayer == false)
            return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        string curStateName = "Run";
        bool isRunning = movement.magnitude > 0;
        if(IsAnimStateChanged(curStateName, isRunning))
        {
            CheckAndCancelAnimState("Sit");
            CheckAndCancelAnimState("InteractLoop");
            MetaNetworkManager?.RequestChangeAnimState(curStateName, isRunning);
        }

        // Rotate Camera
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _cameraRotationX -= mouseY;
        _cameraRotationX = Mathf.Clamp(_cameraRotationX, -15f, 60f);

        Transform_CameraParent.localRotation = Quaternion.Euler(_cameraRotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Camera Collision
        if(Physics.Raycast(Transform_CameraParent.position, -Transform_CameraParent.forward, out RaycastHit hit, _cameraDistance, collisionLayers))
        {
            Transform_Camera.position = Transform_CameraParent.position - (Transform_CameraParent.forward * (hit.distance - 0.1f));
        }
        else
        {
            Transform_Camera.position = Transform_CameraParent.position - (Transform_CameraParent.forward * _cameraDistance);
        }

        // Root Anim - false
        this.transform.Translate(Vector3.right * (moveHorizontal * _moveSpeed * Time.deltaTime));
        this.transform.Translate(Vector3.forward * (moveVertical * _moveSpeed * Time.deltaTime));

        // Rotate Player
        if(moveHorizontal != 0 || moveVertical != 0)
        {
            Vector3 localMovement = Transform_CameraParent.transform.TransformDirection(movement);
            localMovement.y = 0;
            localMovement = localMovement.normalized;

            Quaternion targetRotation = Quaternion.LookRotation(localMovement, Vector3.up);
            Mesh.transform.rotation = Quaternion.Slerp(Mesh.transform.rotation, targetRotation, 1f - Mathf.Exp(-_rotationSpeed * Time.deltaTime));
        }

        // Interact
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(IsNearNPC)
            {
                NpcDialogue.StartDialogue();
            }
            else
            {
                MetaNetworkManager.RequestSpawnFieldObject();
            }
        }
    }

}
