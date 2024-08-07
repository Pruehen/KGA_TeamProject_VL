using UnityEngine;

public class PlayerCameraMove : SceneSingleton<PlayerCameraMove>
{
    [Range(1f, 1000f)] public float mouseSpeed = 200f;
    [Range(1f, 50f)] public float camRange = 20f;
    float xRotation = 15f;

    public Transform camAxis;
    public Transform player;

    public LayerMask camraCollition;


    public void Update()
    {
        //transform.rotation = Quaternion.identity;
        this.transform.localPosition = new Vector3(0, transform.localPosition.y, -camRange);

        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        camAxis.Rotate(Vector3.up * mouseX);
        camAxis.position = player.position;

        //플레이어부터 카메라까지의 방향
        Vector3 rayDir = transform.position - player.position;

        //플레이어에서 부터의 Ray발사
        if (Physics.Raycast(player.position, rayDir, out RaycastHit hit, float.MaxValue, camraCollition))
        {
            //맞은 부위보다 더 안쪽으로 
            Vector3 point = hit.point - rayDir.normalized;
            transform.position = new Vector3(point.x, transform.position.y, point.z);
        }
    }

    public Transform CamAxisTransform()
    {
        return camAxis;
    }

    public Quaternion CamRotation()
    {
        return camAxis.rotation;
    }

}