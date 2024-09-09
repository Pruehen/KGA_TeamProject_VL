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
    bool _isActive;

    public void SetChipData(BlueChipSlot slot)
    {
        _isActive = true;

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
        if (_isActive)
        {
            if (_id != BlueChipID.None)
            {
                selectTempId = _id;
                selecTempLevel = _level;

                if (PlayerMaster.Instance._PlayerEquipBlueChip.TryAddBlueChip(_id, _level))
                {
                    UIManager.Instance.BkBlueChipUi();
                    _isActive = false;
                    return;
                }
                UIManager.Instance.PickBUtton();
                _isActive = false;
            }
        }
    }

    public void PickBtn_OnClick_SwapBlueChip()
    {
        if (_isActive)
        {
            PlayerMaster.Instance._PlayerEquipBlueChip.SwapBlueChip(_id, selectTempId, selecTempLevel);
            UIManager.Instance.BkBlueChipUi();
            _isActive = false;
        }
    }
}
