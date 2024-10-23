using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCameraCollision : MonoBehaviour
{
    [SerializeField] Transform _cameraParent;
    [SerializeField] Transform _camera;
    [SerializeField] LayerMask _collisionLayers;
    [SerializeField] float _cameraDistance = 3f;
    [SerializeField] float _offsetFromWall = 0.3f;
    [SerializeField] Transform _repositionCameraTarget;

    void Update()
    {
        if(Physics.Raycast(_cameraParent.position, -_camera.forward, out RaycastHit hit, _cameraDistance, _collisionLayers))
        {
            _repositionCameraTarget.position = _cameraParent.position - (_cameraParent.forward * (hit.distance - _offsetFromWall));
            Debug.Log("Hit");
        }
        else
        {
            _repositionCameraTarget.localPosition = Vector3.zero;
        }
    }
}
