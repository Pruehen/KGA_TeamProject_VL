using UnityEngine;

public class PlayerCameraMove : SceneSingleton<PlayerCameraMove>
{
    [Range(1f, 1000f)] public float mouseSpeed = 200f;
    [Range(1f, 50f)] public float camRange = 20f;
    //float xRotation = 15f;

    [SerializeField] Transform camAxis;

    Vector3 startLocalPosition;


    public LayerMask camraCollition;

    private void Awake()
    {
        startLocalPosition = transform.localPosition; 
    }

    public void LateUpdate()
    {
        //transform.rotation = Quaternion.identity;
        //this.transform.localPosition = new Vector3(0, transform.localPosition.y, -camRange);

        //float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;

        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //camAxis.Rotate(Vector3.up * mouseX);

       // //플레이어부터 카메라까지의 방향
       //Vector3 rayDir = transform.position - camAxis.position;


       // Debug.DrawRay(camAxis.position, rayDir.normalized);

       // if (Physics.Raycast(camAxis.position, rayDir, out RaycastHit hit, camRange, camraCollition))
       // {
       //     Vector3 point = hit.point - rayDir.normalized;
       //     transform.position = point;
       //     Debug.Log(rayDir);
       // }
       // else
       // {
       //     transform.localPosition = Vector3.Lerp(transform.localPosition, startLocalPosition, Time.deltaTime * 10);
       // }
      
    }
      
    public Transform CamAxisTransform()
    {
        return camAxis;
    }

    public Quaternion CamRotation()
    {
        Vector3 flat = transform.forward;
        flat.y = 0f;
        
        return Quaternion.LookRotation(flat);
    }

}