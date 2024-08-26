using EnumTypes;
using UnityEngine;
using UnityEngine.UI;

public class BlueChipIcon : MonoBehaviour
{
    static BlueChipID selectTempId;
    static int selecTempLevel;

    [SerializeField] Image iconImage;
    [SerializeField] Text iconName;
    [SerializeField] Text iconInfo;
    [SerializeField] Text iconLevel;

    BlueChipID _id;
    int _level;

    public void SetChipData(BlueChipSlot slot)
    {
        if (slot == null || slot.Level == 0)
        {            
            iconName.text = "¾øÀ½";
            iconLevel.text = "";
            iconInfo.text = "";
            iconImage.gameObject.SetActive(false);
            _id = BlueChipID.None;
            _level = 0;
        }
        else
        {            
            BlueChip data = JsonDataManager.GetBlueChipData(slot.Id);
            _id = slot.Id;
            _level = slot.Level;
            iconName.text = data.PrintName();
            iconLevel.text = data.PrintLevel(slot.Level);
            iconInfo.text = data.PrintInfo(slot.Level);
            iconImage.gameObject.SetActive(true);
            iconImage.sprite = Resources.Load<Sprite>($"Icon/bluechip/{data.IconName}");
        }
    } 
    
    public void PickBtn_OnClick_TryAddBlueChip()
    {
        if (_id != BlueChipID.None)
        {
            selectTempId = _id;
            selecTempLevel = _level;

            if (PlayerMaster.Instance._PlayerEquipBlueChip.TryAddBlueChip(_id, _level))
            {
                UIManager.Instance.BkBlueChipUi();
                return;
            }
            UIManager.Instance.PickBUtton();
        }
    }

    public void PickBtn_OnClick_SwapBlueChip()
    {
        PlayerMaster.Instance._PlayerEquipBlueChip.SwapBlueChip(_id, selectTempId, selecTempLevel);
        UIManager.Instance.BkBlueChipUi();
    }
}
