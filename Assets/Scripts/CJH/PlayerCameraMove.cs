using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMove : MonoBehaviour
{
    [Range(1f, 1000f)] public float mouseSpeed = 200f;
    [Range(1f, 50f)] public float camRange = 20f;
    float xRotation = 15f;

    public Transform camAxis;

    public void Update()
    {

        //transform.rotation = Quaternion.identity;
        this.transform.localPosition = new Vector3(0, 0, -camRange);

        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        camAxis.Rotate(Vector3.up * mouseX);


    }

    public Vector3 CamForward()
    {
        return camAxis.forward;
    }
}