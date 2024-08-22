using UnityEngine;
using UnityEditor;
using EnumTypes;

[CustomEditor(typeof(PlayerEquipBlueChip))]
[CanEditMultipleObjects]
public class CubeGenerateButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PlayerEquipBlueChip playerEquipBlueChip = (PlayerEquipBlueChip)target;
        if (GUILayout.Button("근거리1 칩 추가 (차징 체력쉴드 비례 추가뎀, 범위 상승)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Melee1);
        }
        if (GUILayout.Button("근거리2 칩 추가 (근접모드 돌입, 타격 시 쉴드 획득)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Melee2);
        }
        if (GUILayout.Button("원거리1 칩 추가 (일반공격 타수 감소)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Range1);
        }
        if (GUILayout.Button("원거리2 칩 추가 (원거리 막타 확률 무작위 스킬 발동)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Range2);
        }
        if (GUILayout.Button("하이브리드1 칩 추가 (모드 전환시 몇회 타격 데미지 증가)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Hybrid1);
        }
        if (GUILayout.Button("하이브리드2 칩 추가 (몇 초마다 자동으로 모드 변환, 근접 자원 소모 없음)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Hybrid2);
        }
        if (GUILayout.Button("범용1 칩 추가 (대시 장판 근거리 효과 추가)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Generic1);
        }
        if (GUILayout.Button("범용2 칩 추가 (소모량 및 피해량 증가)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Generic2);
        }
        if (GUILayout.Button("범용3 칩 추가 (스킬 게이지에 비례 공속 증가, 공격 안한 시간 초과시 스킬게이지 0)"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Generic3);
        }
        if (GUILayout.Button("모든 칩 제거"))
        {
            playerEquipBlueChip.GetBlueChipDic().Clear();
        }
    }
}
