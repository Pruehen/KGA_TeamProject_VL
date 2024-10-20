using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChatUI : MonoBehaviour
{
    [SerializeField] private MetaNetworkManager ChatManager;

    [SerializeField] private GameObject GObj_ChatExtended;
    [SerializeField] private GameObject GObj_ChatMinimized;

    [Header("Chat")]
    [SerializeField] private TMP_InputField Input_ChatMsg;
    [SerializeField] private Text Text_ChatList;
    [SerializeField] private ScrollRect ScrollRect_ChatList;

    private void OnEnable()
    {
        StartCoroutine(CoDelayRecvMsg());
    }

    private IEnumerator CoDelayRecvMsg()
    {
        yield return new WaitForSeconds(1.0f);
        ChatManager.BindRecvMsgCallback(OnChatMsgReceived);
    }

    private void OnChatMsgReceived(uint id, string msg)
    {
        Text_ChatList.text = $"{Text_ChatList.text}\n{msg}";
        Canvas.ForceUpdateCanvases();
        ScrollRect_ChatList.verticalNormalizedPosition = 0f;
    }

    private void SendChatMsg()
    {
        ChatManager.SendMsg(Input_ChatMsg.text);
    }

    public void OnClick_SendMsg()
    {
        SendChatMsg();
    }

    void Update()
    {
    }

    float lastFocusedTime = 0f;

    public void OnSubmit_ChatMsg()
    {
        SendChatMsg();
        Input_ChatMsg.text = "";
        lastFocusedTime = Time.time;
        Input_ChatMsg.DeactivateInputField();
    }

    public void OnClick_ExtendChatArea()
    {
        ToggleExtendChatArea(true);
    }

    public void OnClick_MinimizeChatArea()
    {
        ToggleExtendChatArea(false);
    }

    private void ToggleExtendChatArea(bool isExtend)
    {
        GObj_ChatExtended.SetActive(isExtend);
        GObj_ChatMinimized.SetActive(!isExtend);
    }
    public void OnClick_ChatInputField()
    {
        if(!Input_ChatMsg.isFocused && Time.time - lastFocusedTime > 0.1f)
        {
            Input_ChatMsg.ActivateInputField();
            Debug.Log($"Submit: {lastFocusedTime}");
        }
    }

}
