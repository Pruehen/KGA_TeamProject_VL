using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveFillSlot : MonoBehaviour
{
    [SerializeField] Text Text_CreateTime;
    [SerializeField] Text Text_TryCount;
    [SerializeField] Text Text_ClearCount;
    [SerializeField] Text Text_PlayTime;
    [SerializeField] Text Text_Emerald;

    [SerializeField] Button Btn_Back;
    [SerializeField] Button Btn_Lode;
    [SerializeField] Button Btn_Delete;

    int _slotIndex;
    public void SetData(int index)
    {
        _slotIndex = index;
        if (JsonDataManager.TryGetUserData(_slotIndex, out UserData data))
        {
            Text_TryCount.gameObject.SetActive(true);
            Text_ClearCount.gameObject.SetActive(true);
            Text_PlayTime.gameObject.SetActive(true);
            Text_Emerald.gameObject.SetActive(true);

            Text_CreateTime.text = data.SaveTime.ToString();
            Text_TryCount.text = data.Count_Try.ToString();
            Text_ClearCount.text = data.Count_Clear.ToString();
            Text_PlayTime.text = data.PlayTime.ToString();
            Text_Emerald.text = data.Gold.ToString();
        }
        else
        {
            Text_CreateTime.text = "EMPTY";
            Text_TryCount.gameObject.SetActive(false);
            Text_ClearCount.gameObject.SetActive(false);
            Text_PlayTime.gameObject.SetActive(false);
            Text_Emerald.gameObject.SetActive(false);
        }

        Btn_Back.gameObject.SetActive(false);
        Btn_Lode.gameObject.SetActive(false);
        Btn_Delete.gameObject.SetActive(false);
    }

    public void SlotSelect_OnClick()
    {
        if(GameManager.Instance.IsLoading)
        {
            return;
        }
        Btn_Back.gameObject.SetActive(true);
        Btn_Lode.gameObject.SetActive(true);
        Btn_Delete.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Btn_Lode.gameObject);
    }
    public void SlotDeSelect_OnClick()
    {
        Btn_Back.gameObject.SetActive(false);
        Btn_Lode.gameObject.SetActive(false);
        Btn_Delete.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
    public void LodeData()
    {
        MainSceneUIManager.Instance.OnClick_NewGameButton(_slotIndex);
    }

    public void DeleteDate_OnClick()
    {
        CheckUIManager.Instance.CheckUiActive_OnClick(DeleteData, "세이브파일을 삭제하시겠습니까?");
    }
    void DeleteData()
    {
        JsonDataManager.DeleteUserData(_slotIndex);
        SetData(_slotIndex);
        JsonDataManager.DataSaveCommand(JsonDataManager.jsonCache.UserDataCache, UserDataList.FilePath());
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}
