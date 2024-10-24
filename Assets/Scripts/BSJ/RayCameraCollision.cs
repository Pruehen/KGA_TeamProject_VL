using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCameraCollision : MonoBehaviour
{
    [SerializeField] Transform _cameraParent;
    [SerializeField] Transform _camera;
    [SerializeField] LayerMask _collisionLayers;
    [SerializeField] Transform _originalCameraTarget;
    [SerializeField] float _offsetFromWall = 0.3f;
    [SerializeField] Transform _repositionCameraTarget;

    void FixedUpdate()
    {
        float _cameraDistance = Vector3.Distance(_cameraParent.position, _originalCameraTarget.position);
        Debug.DrawRay(_cameraParent.position, -_camera.forward * _cameraDistance, Color.red);
        if(Physics.Raycast(_cameraParent.position, -_camera.forward, out RaycastHit hit, _cameraDistance, _collisionLayers))
        {
            _repositionCameraTarget.position =
             -_camera.forward * (hit.distance - _offsetFromWall) + _cameraParent.position;
            Debug.DrawRay(_repositionCameraTarget.position, Vector3.forward, Color.green);
            // Debug.Log($"Hit1 : {_camera.forward * (hit.distance - _offsetFromWall)}");
            // Debug.Log($"Hit2 : {_repositionCameraTarget.position}");
            // Debug.Log($"Hit3 : {_repositionCameraTarget.localPosition}");
        } 
        else
        {
            _repositionCameraTarget.localPosition = Vector3.zero;
        }
    }
}
