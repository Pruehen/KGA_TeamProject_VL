using EnumTypes;
using UnityEngine;
using UnityEngine.UI;

public class BlueChipIcon : MonoBehaviour
{
    static BlueChipID selectTempId;

    [SerializeField] Image iconImage;
    [SerializeField] Text iconName;
    [SerializeField] Text iconInfo;
    [SerializeField] Text iconLevel;

    BlueChipID _id;

    public void SetChipData(BlueChipSlot slot)
    {
        if (slot == null || slot.Level == 0)
        {            
            iconName.text = "¾øÀ½";
            iconLevel.text = "";
            iconInfo.text = "";
        }
        else
        {            
            BlueChip data = JsonDataManager.GetBlueChipData(slot.Id);
            _id = slot.Id;
            iconName.text = data.PrintName();
            iconLevel.text = data.PrintLevel(slot.Level);
            iconInfo.text = data.PrintInfo(slot.Level);
        }
    } 
    
    public void PickBtn_OnClick_TryAddBlueChip()
    {
        selectTempId = _id;
        if (PlayerMaster.Instance._PlayerEquipBlueChip.TryAddBlueChip(_id))
        {
            UIManager.Instance.BkBlueChipUi();
            return;
        }
        UIManager.Instance.PickBUtton();
    }

    public void PickBtn_OnClick_SwapBlueChip()
    {
        PlayerMaster.Instance._PlayerEquipBlueChip.SwapBlueChip(_id, selectTempId);
        UIManager.Instance.BkBlueChipUi();
    }
}
