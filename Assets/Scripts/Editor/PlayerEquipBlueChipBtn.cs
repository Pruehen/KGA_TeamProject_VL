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
        if (GUILayout.Button("�ٰŸ�1 Ĩ �߰�"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.�ٰŸ�1);
        }
        if (GUILayout.Button("�ٰŸ�2 Ĩ �߰�"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.�ٰŸ�2);
        }
        if (GUILayout.Button("���Ÿ�1 Ĩ �߰�"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.���Ÿ�1);
        }
        if (GUILayout.Button("���Ÿ�2 Ĩ �߰�"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.���Ÿ�2);
        }
        if (GUILayout.Button("���̺긮��1 Ĩ �߰�"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.���̺긮��1);
        }
        if (GUILayout.Button("���̺긮��2 Ĩ �߰�"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.���̺긮��2);
        }
        if (GUILayout.Button("����1 Ĩ �߰�"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.����1);
        }
        if (GUILayout.Button("����2 Ĩ �߰�"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.����2);
        }
        if (GUILayout.Button("����3 Ĩ �߰�"))
        {
            playerEquipBlueChip.TryAddBlueChip(BlueChipID.����3);
        }
        if (GUILayout.Button("��� Ĩ ����"))
        {
            playerEquipBlueChip.GetBlueChipDic().Clear();
        }
    }
}
