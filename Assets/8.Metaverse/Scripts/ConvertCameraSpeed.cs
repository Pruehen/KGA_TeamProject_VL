using UnityEngine;
using Cinemachine;

public class ConvertCameraSpeed : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _freeLookCamera;
    void Start()
    {
        if(GameManager.Instance.IsXREnabled())
        {
            _freeLookCamera.m_XAxis.m_MaxSpeed = 2f;
        }
        else
        {
            _freeLookCamera.m_XAxis.m_MaxSpeed = .15f;
        }
    }
}
