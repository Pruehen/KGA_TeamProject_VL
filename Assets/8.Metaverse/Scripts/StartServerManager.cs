using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartServerManager : MonoBehaviour
{
    [SerializeField] MetaNetManager NetManager;

    [SerializeField] GameObject Obj_LoadingPopup;

    [SerializeField] bool IsStartAsServer;

    public void Start()
    {
        if(NetManager == null)
        {
            return;
        }

        IsStartAsServer = GameManager.Instance.IsMetaVerseServer;

        if (IsStartAsServer)
        {
            TryStartServer();
        }
        else
        {
            TryConnectToServer();
        }
    }

    private void TryStartServer()
    {
        NetManager.StartHost();
    }

    private void LateUpdate()
    {
        TryConnectToServer();
    }

    private void TryConnectToServer()
    {
        if (NetManager.GetNetworkClientConnected())
        {
            if (Obj_LoadingPopup.activeSelf)
            {
                StartCoroutine(DelayedSetActive(Obj_LoadingPopup, false));
            }

            return;
        }

        if(NetManager.OnMetaStartClientCallback != null)
        {
            NetManager.OnMetaStartClientCallback -= OnMetaStartClient;
        }

        NetManager.OnMetaStartClientCallback += OnMetaStartClient;
        NetManager.StartClient();
    }

    private void OnMetaStartClient()
    {
        Obj_LoadingPopup.gameObject.SetActive(!NetManager.GetNetworkClientConnected());
    }

    private IEnumerator DelayedSetActive(GameObject obj, bool active)
    {
        yield return new WaitForSeconds(2f);
        obj.gameObject.SetActive(active);
    }

}
