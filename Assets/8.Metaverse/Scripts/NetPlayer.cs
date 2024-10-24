using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;


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

    [Header("PlayerScript")]
    [SerializeField] Behaviour[] PlayerScripts;
    [SerializeField] GameObject[] PlayerGameObjects;

    private float _moveSpeed = 5.0f;
    private float _mouseSensitivity = 100.0f;
    private float _cameraRotationX = 0.0f;
    private Transform _chatRoot;
    public bool IsNearNPC = false;
    public NPCDialogue NpcDialogue;

    public InputActionAsset InputAsset;

    public void SetNPC(NPCDialogue npcDialogue)
    {
        NpcDialogue = npcDialogue;
    }

    public Transform GetSpawnObjTransform() { return Transform_SpawnObj; }



    private MetaNetworkManager _chatManager;
    [SerializeField] ChatUI _chatUI;
    private void Start()
    {
        //Transform_CameraParent.gameObject.SetActive(false);

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        if(!isLocalPlayer)
        {
            foreach (var localComponents in PlayerScripts)
            {
                localComponents.enabled = false;
            }
            foreach (var localGameObject in PlayerGameObjects)
            {
                localGameObject.SetActive(false);
            }
        }

        InputAsset.Enable();
        if(!isLocalPlayer)
        {
            return;
        }
        _chatManager = FindObjectOfType<MetaNetworkManager>();
        _chatManager.ChatUi = _chatUI;
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
                    MetaNetworkManager.BindLocalPlayerNetId(this.netId);
                    MetaNetworkManager.BindLocalPlayer(this);
                }
                
                //MetaNetworkManager.BindRpcAnimStateChangedCallback(OnRpcAnimStateChanged);
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
        if(isLocalPlayer == false)
        {
            return;
        }
        if(InputManager.Instance.IsEnterBtnClick)
        {
            MetaNetworkManager.OnClick_ChatInputField();
        }
        if(InputManager.Instance.IsEscapeBtnClick)
        {
            GameManager.Instance.LoadMainScene();
        }
        if(InputManager.Instance.IsInteractiveBtnClick_CoolTime)
        {
            if(IsNearNPC)
            {
                OnNpcInteract();
            }
            else
            {
                MetaNetworkManager.RequestSpawnFieldObject();
            }
        }
        if(InputField_Chat.isFocused)
        {
            InputManager.Instance.IsTyping = true;
        }
        else
        {
            InputManager.Instance.IsTyping = false;
        }
        // MoveOnUpdate();
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

        // if (isLocalPlayer == false)
        //     return;

        // float moveHorizontal = Input.GetAxis("Horizontal");
        // float moveVertical = Input.GetAxis("Vertical");
        // Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // string curStateName = "Run";
        // bool isRunning = movement.magnitude > 0;
        // if(IsAnimStateChanged(curStateName, isRunning))
        // {
        //     CheckAndCancelAnimState("Sit");
        //     CheckAndCancelAnimState("InteractLoop");
        //     MetaNetworkManager?.RequestChangeAnimState(curStateName, isRunning);
        // }

        // // Rotate Camera
        // float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        // float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        // _cameraRotationX -= mouseY;
        // _cameraRotationX = Mathf.Clamp(_cameraRotationX, -15f, 60f);

        //Transform_CameraParent.localRotation = Quaternion.Euler(_cameraRotationX, 0f, 0f);
        // transform.Rotate(Vector3.up * mouseX);

        // // Camera Collision
        // if(Physics.Raycast(Transform_CameraParent.position, -Transform_CameraParent.forward, out RaycastHit hit, _cameraDistance, collisionLayers))
        // {
        //     Transform_Camera.position = Transform_CameraParent.position - (Transform_CameraParent.forward * (hit.distance - 0.1f));
        // }
        // else
        // {
        //     Transform_Camera.position = Transform_CameraParent.position - (Transform_CameraParent.forward * _cameraDistance);
        // }

        // Root Anim - false
        // this.transform.Translate(Vector3.right * (moveHorizontal * _moveSpeed * Time.deltaTime));
        // this.transform.Translate(Vector3.forward * (moveVertical * _moveSpeed * Time.deltaTime));

        // // Rotate Player
        // if(moveHorizontal != 0 || moveVertical != 0)
        // {
        //     Vector3 localMovement = Transform_CameraParent.transform.TransformDirection(movement);
        //     localMovement.y = 0;
        //     localMovement = localMovement.normalized;

        //     Quaternion targetRotation = Quaternion.LookRotation(localMovement, Vector3.up);
        //     Mesh.transform.rotation = Quaternion.Slerp(Mesh.transform.rotation, targetRotation, 1f - Mathf.Exp(-_rotationSpeed * Time.deltaTime));
        // }

        // // Interact
        // if(Input.GetKeyDown(KeyCode.F))
        // {
        //     if(IsNearNPC)
        //     {
        //         NpcDialogue.StartDialogue();
        //     }
        //     else
        //     {
        //         MetaNetworkManager.RequestSpawnFieldObject();
        //     }
        // }
    }



    
    public GameObject dialoguePanel;
    public Text dialogueText;
    public float delayBetweenDialogues = 1.5f; // 대화 사이의 지연 시간 (초)
    private bool IsDisplayingDialogue = false;
    private bool IsPrintingDialogue = false;

    
    private IEnumerator DisplayDialogueCoroutine()
    {
        IsPrintingDialogue = true;

        if(NpcDialogue.TryGetCurrentLine(out string line))
        {
            StartCoroutine(PrintCharacterCoroutine(line));
            yield return new WaitForSeconds(delayBetweenDialogues);
        }
        else
        {
            EndDialog();
        }
    }


    private void OnNpcInteract()
    {
        if(IsPrintingDialogue)
        {
            return;
        }
        if(IsDisplayingDialogue)
        {
            OnNext();
        }
        else
        {
            dialoguePanel.SetActive(true);
            NpcDialogue.StartDialogue();
            IsDisplayingDialogue = true;
            StartCoroutine(DisplayDialogueCoroutine());
        }
        
        Debug.Log("NpcDialogue.StartDialogue()");
    }
    private void OnNext()
    {
        if(!IsPrintingDialogue)
        {
            StartCoroutine(DisplayDialogueCoroutine());
        }
    }
    private IEnumerator PrintCharacterCoroutine(string text)
    {
        Debug.Log($"PrintCharacterCoroutine : {text}");
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        IsPrintingDialogue = false;
    }
    public void EndDialog()
    {
        IsNearNPC = false;
        dialoguePanel.SetActive(false);
        IsDisplayingDialogue = false;
    }

    public void SendChatMessage(string msg)
    {
        _chatUI.OnClick_ChatInputField();
    }
}
