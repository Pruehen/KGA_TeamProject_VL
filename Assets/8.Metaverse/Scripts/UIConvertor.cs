using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConvertor : MonoBehaviour
{
    [SerializeField] private Canvas _UI;
    void Start()
    {
        if(GameManager.Instance.IsXREnabled())
        {
            _UI.renderMode = RenderMode.WorldSpace;
        }
        else
        {
            _UI.renderMode = RenderMode.ScreenSpaceOverlay;
            _UI.transform.SetParent(null);
        }
    }
}
