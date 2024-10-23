using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

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
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                _playerCamera = playerCamera;
                transform.GetChild(0).localPosition = Vector3.zero;
                transform.GetChild(0).localRotation = Quaternion.identity;
                XROrigin xrOrigin = GetComponent<XROrigin>();
                xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
                xrOrigin.CameraYOffset = 0.0f;
                StartCoroutine(CheckPlayerCamera());
                break;
            }
        }
    }
}
