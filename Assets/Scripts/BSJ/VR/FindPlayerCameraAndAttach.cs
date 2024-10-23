using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerCameraAndAttach : MonoBehaviour
{
    GameObject _playerCamera;

    private void Start() {
        StartCoroutine(CheckPlayerCamera());
    }

    private IEnumerator CheckPlayerCamera()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (_playerCamera != null)
            {
                continue;
            }
            StartCoroutine(FindPlayerCamera());
            break;
        }
    }
    private IEnumerator FindPlayerCamera()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            GameObject playerCamera = GameObject.FindGameObjectWithTag("PCPlayerCamera");
            if (playerCamera != null)
            {
                transform.SetParent(playerCamera.transform, false);
                _playerCamera = playerCamera;
                StartCoroutine(CheckPlayerCamera());
                break;
            }
        }
    }
}
