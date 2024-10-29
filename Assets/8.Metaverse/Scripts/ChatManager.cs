using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;

public class MetaNetworkManager : NetworkBehaviour
{
    private Dictionary<uint, List<string>> _msgList = new Dictionary<uint, List<string>>();

    private uint _localPlayerNetId;
    private Action<uint, string> _recvMsgCallback;
    private Action<uint, string, bool> _rpcAnimStateChange;

    [Header("InteractionFieldObject")]
    [SerializeField] GameObject Prefab_SpawnInteractFieldObj;
    [SerializeField] ChatUI ChatUi;

    //임시
    private NetPlayer _localPlayer = null;

    private void OnDestroy()
    {
        if(_rpcAnimStateChange != null)
        {
            _rpcAnimStateChange = null;
        }
    }

    #region FieldObject
    public void BindLocalPlayer(NetPlayer player)
    {
        _localPlayer = player;
    }

    public void RequestSpawnFieldObject()
    {
        if(_localPlayer == null)
        {
            return;
        }

        var transform = _localPlayer.GetSpawnObjTransform();
        if(transform == null)
        {
            return;
        }

        CommandSpawnFieldObject(transform.position, transform.rotation);
    }

    [Command(requiresAuthority = false)]
    public void CommandSpawnFieldObject(Vector3 pos, Quaternion rotate)
    {
        GameObject spawnedObj = Instantiate(Prefab_SpawnInteractFieldObj, pos, rotate);
        NetworkServer.Spawn(spawnedObj);
    }
    #endregion

    #region Interact
    public void BindLocalPlayerNetId(uint netId)
    {
        _localPlayerNetId = netId;
    }

    public void BindRpcAnimStateChangedCallback(Action<uint, string, bool> onRpcAnimStateChange)
    {
        _rpcAnimStateChange += onRpcAnimStateChange;
    }

    public void RequestChangeAnimStateByRemoteId(uint remoteNetId, string animStateKey, bool isActive)
    {
        ReqChangeAnimStateBool(remoteNetId, animStateKey, isActive);
    }

    public void RequestChangeAnimState(string animStateKey, bool isActive)
    {
        ReqChangeAnimStateBool(_localPlayerNetId, animStateKey, isActive);
    }

    [Command(requiresAuthority=false)]
    void ReqChangeAnimStateBool(uint netId, string animStateKey, bool isActive)
    {
        RpcOnAnimStateChange(netId, animStateKey, isActive);
    }

    [ClientRpc]
    void RpcOnAnimStateChange(uint netId, string animStateKey, bool isActive)
    {
        if(_rpcAnimStateChange != null)
        {
            _rpcAnimStateChange.Invoke(netId, animStateKey, isActive);
        }
    }

    #endregion

    #region Chat
    private void AddMsgList(uint id, string msg)
    {
        if (_msgList.ContainsKey(id))
        {
            _msgList[id].Add(msg);
        }
        else
        {
            var msgList = new List<string>();
            _msgList.Add(id, msgList);
        }
    }

    public void BindRecvMsgCallback(Action<uint, string> onRecvMsg)
    {
        _recvMsgCallback += onRecvMsg;
    }

    public void SendMsg(string msg)
    {
        SendMsgCommand(_localPlayerNetId, GameManager.Instance.MetaVersePlayerName, msg);
    }

    [Command(requiresAuthority = false)]
    public void SendMsgCommand(uint id, string playerName, string msg)
    {
        if(string.IsNullOrEmpty(playerName))
        {
            playerName = $"Unknown{id}";
        }
        string hexColor = PlayerColorGenerator.GetHexColorFromPlayerName(playerName);
        msg = msg.Insert(0, $"<color={hexColor}>{playerName}</color> ");
        AddMsgList(id, msg);
        RecvMsg(id, msg);
    }

    [ClientRpc]
    public void RecvMsg(uint id, string msg)
    {
        AddMsgList(id, msg);

        if(_recvMsgCallback != null)
        {
            _recvMsgCallback.Invoke(id, msg);
        }
    }

    public void OnClick_ChatInputField()
    {
        ChatUi.OnClick_ChatInputField();
    }

    
public static class PlayerColorGenerator
{
    public static Color GetColorFromPlayerName(string playerName)
    {
        // 이름을 해시값으로 변환
        int hash = playerName.GetHashCode();
        
        // 해시값을 사용하여 랜덤 시드 생성
        System.Random random = new System.Random(hash);
        
        // 0-255 사이의 RGB 값 생성
        int r = random.Next(256);
        int g = random.Next(256);
        int b = random.Next(256);
        
        // RGB 값을 0-1 범위의 float로 변환
        Color color = new Color(r / 255f, g / 255f, b / 255f);
        
        return color;
    }

    public static string GetHexColorFromPlayerName(string playerName)
    {
        Color color = GetColorFromPlayerName(playerName);
        
        // Color를 hex 문자열로 변환
        string hex = ColorUtility.ToHtmlStringRGB(color);
        
        return "#" + hex;
    }
}
#endregion
}
