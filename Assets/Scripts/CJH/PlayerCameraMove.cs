using UnityEngine;
using Cinemachine;
public class PlayerCameraMove : SceneSingleton<PlayerCameraMove>
{
    [Range(1f, 1000f)] public float mouseSpeed = 200f;
    [Range(1f, 50f)] public float camRange = 20f;
    //float xRotation = 15f;

    [SerializeField] Transform camAxis;

    Vector3 startLocalPosition;

    public LayerMask camraCollition;

    public CinemachineFreeLook freeLookCamera;

    private void Awake()
    {
        startLocalPosition = transform.localPosition; 
    }
    private void Start()
    {
        if (freeLookCamera != null)
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
        }
    }

    private void Update()
    {
        UpdateCameraInput(InputManager.Instance.RotateVector2_Rotate.x, InputManager.Instance.RotateVector2_Rotate.y);
    }

    public void UpdateCameraInput(float xAxisValue, float yAxisValue)
    {
        if (freeLookCamera != null)
        {
            freeLookCamera.m_XAxis.m_InputAxisValue = xAxisValue;
            freeLookCamera.m_YAxis.m_InputAxisValue = yAxisValue;
        }
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