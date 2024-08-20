using EnumTypes;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveUIManager : SceneSingleton<PassiveUIManager>
{
    [SerializeField] Button FalstButton;

    [SerializeField] List<Sprite> Icon_Image = new List<Sprite>();
    [SerializeField] List<PassiveUI> PassiveUI = new List<PassiveUI>();

    public Dictionary<PassiveID, PassiveUI> ID_PassiveUI_Dic = new Dictionary<PassiveID, PassiveUI>();

    public List<PassiveUI> PassiveUIGroup_Offensive = new List<PassiveUI>();
    public List<PassiveUI> PassiveUIGroup_Deffensive = new List<PassiveUI>();
    public List<PassiveUI> PassiveUIGroup_Utility = new List<PassiveUI>();

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(FalstButton.gameObject);
        InputManager.Instance.PropertyChanged += OnInputPropertyChanged;

        PassiveUI.AddRange(FindObjectsOfType<PassiveUI>());
        LoadIconImages();
    }

    private void Start()
    {
        IconImage();
    }



    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(InputManager.Instance.IsInteractiveBtnClick):
                if (InputManager.Instance.IsInteractiveBtnClick == true)
                {                  
                    if (gameObject.activeSelf == true)
                    {
                        Button selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
                        selectedButton.onClick.Invoke();                        

                    }
                    else
                    {
                        return;
                    }
                }
                break;
        }
    }

    public void IconImage()
    {
        for (int i = 0; i < PassiveUI.Count; i++)
        {
            if (i < Icon_Image.Count)
            {
                PassiveUI[i].ImageChange(Icon_Image);
            }
            else
            {
                Debug.LogWarning($"Icon_Image 리스트에 충분한 이미지가 없습니다. 인덱스 {i}에 대한 이미지를 찾을 수 없습니다.");
                return;
            }
        }
    }
    public void Command_IconImage(PassiveUI targetUI)
    {
        targetUI.ImageChange(Icon_Image);
    }    
    public void Try_EquipPassive(PassiveUI targetUI)
    {
        switch (targetUI.passiveID)
        {
            case PassiveID.Offensive1:
                break;
            case PassiveID.Offensive2:
                break;
            case PassiveID.Offensive3:
                break;
            case PassiveID.Offensive4:
                break;
            case PassiveID.Offensive5:
                break;
            case PassiveID.Defensive1:
                break;
            case PassiveID.Defensive2:
                break;
            case PassiveID.Defensive3:
                break;
            case PassiveID.Defensive4:
                break;
            case PassiveID.Defensive5:
                break;
            case PassiveID.Utility1:
                break;
            case PassiveID.Utility2:
                break;
            case PassiveID.Utility3:
                break;
            case PassiveID.Utility4:
                break;
            case PassiveID.Utility5:
                break;
            case PassiveID.None:
                break;
            default:
                break;
        }
    }

    private void LoadIconImages()
    {
        Icon_Image.Clear();
        string path = "Assets/NewAssets/아웃게임";
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { path });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite != null)
            {
                Icon_Image.Add(sprite);
            }
        }

        Debug.Log($"{Icon_Image.Count}개의 이미지를 불러왔습니다.");
    }

}