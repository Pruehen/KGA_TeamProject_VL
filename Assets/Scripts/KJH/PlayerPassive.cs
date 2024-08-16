using EnumTypes;
using UnityEngine;

public class PlayerPassive : MonoBehaviour
{
    [Header("���� ī�װ� �нú� ����")]
    [SerializeField] PassiveID PassiveID_Offensive1 = PassiveID.None;
    [SerializeField] PassiveID PassiveID_Offensive2 = PassiveID.None;

    [Header("ü�� ī�װ� �нú� ����")]
    [SerializeField] PassiveID PassiveID_Defensive1 = PassiveID.None;
    [SerializeField] PassiveID PassiveID_Defensive2 = PassiveID.None;

    [Header("��ƿ ī�װ� �нú� ����")]
    [SerializeField] PassiveID PassiveID_Utility1 = PassiveID.None;
    [SerializeField] PassiveID PassiveID_Utility2 = PassiveID.None;

    [Header("����� ��� ��� : üũ ��, �����ͻ󿡼� ������ �нú긦 ������.")]
    [SerializeField] bool IsDebugMode = false;

    public void Init(PlayerInstanteState targetState)
    {
        if(IsDebugMode)
        {

        }
    }
}
