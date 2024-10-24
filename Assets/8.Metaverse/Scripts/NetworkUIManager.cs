using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkUIManager : NetworkBehaviour
{
    [SerializeField] GameObject localUI;
    private void Start()
    {
        if(isLocalPlayer)
        {
            localUI.SetActive(false);
        }
    }
}
