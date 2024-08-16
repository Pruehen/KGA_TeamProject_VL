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
        if (GUILayout.Button("근거리1 칩 추가"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Melee1);
        }
        if (GUILayout.Button("근거리2 칩 추가"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Melee2);
        }
        if (GUILayout.Button("원거리1 칩 추가"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Range1);
        }
        if (GUILayout.Button("원거리2 칩 추가"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Range2);
        }
        if (GUILayout.Button("하이브리드1 칩 추가"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Hybrid1);
        }
        if (GUILayout.Button("하이브리드2 칩 추가"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Hybrid2);
        }
        if (GUILayout.Button("범용1 칩 추가"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Generic1);
        }
        if (GUILayout.Button("범용2 칩 추가"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Generic2);
        }
        if (GUILayout.Button("범용3 칩 추가"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.Generic3);
        }
        if (GUILayout.Button("모든 칩 제거"))
        {
            playerEquipBlueChip.GetBlueChipDic().Clear();
        }
    }
}
