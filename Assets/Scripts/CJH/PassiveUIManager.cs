using EnumTypes;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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

    int useOffensivePassiveCount = 0;
    int useDeffensivePassiveCount = 0;
    int useUtilityPassiveCount = 0;

    [SerializeField] int Max_Passive_Count = 2;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(FalstButton.gameObject);
        
        PassiveUI.AddRange(FindObjectsOfType<PassiveUI>());
        LoadIconImages();
    }

    private void Start()
    {
        IconImage();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            MainSceneUIManager.Instance.OnClick_NewGameButton();

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
                Debug.LogWarning($"Icon_Image ����Ʈ�� ����� �̹����� �����ϴ�. �ε��� {i}�� ���� �̹����� ã�� �� �����ϴ�.");
                return;
            }
        }
    }
    public void Command_IconImage(PassiveUI targetUI)
    {
        targetUI.ImageChange(Icon_Image);
        
    }    
    public bool Try_EquipPassive(PassiveUI targetUI)
    {
        switch (targetUI.passiveID)
        {
            case PassiveID.Offensive1:
            case PassiveID.Offensive2:
            case PassiveID.Offensive3:
            case PassiveID.Offensive4:
            case PassiveID.Offensive5:
                if (Max_Passive_Count > useOffensivePassiveCount)
                {
                    PassiveUIGroup_Offensive[useOffensivePassiveCount].passiveID = targetUI.passiveID;
                    PassiveUIGroup_Offensive[useOffensivePassiveCount].ImageChange(Icon_Image);
                    useOffensivePassiveCount++;
                    return true;
                }
                break;
            case PassiveID.Defensive1:
            case PassiveID.Defensive2:
            case PassiveID.Defensive3:
            case PassiveID.Defensive4:
            case PassiveID.Defensive5:
                if (Max_Passive_Count > useDeffensivePassiveCount)
                {
                    PassiveUIGroup_Deffensive[useDeffensivePassiveCount].passiveID = targetUI.passiveID;
                    PassiveUIGroup_Deffensive[useDeffensivePassiveCount].ImageChange(Icon_Image);
                    useDeffensivePassiveCount++;
                    return true;
                }
                break;
            case PassiveID.Utility1:
            case PassiveID.Utility2:
            case PassiveID.Utility3:
            case PassiveID.Utility4:
            case PassiveID.Utility5:
                if (Max_Passive_Count > useUtilityPassiveCount)
                {
                    PassiveUIGroup_Utility[useUtilityPassiveCount].passiveID = targetUI.passiveID;
                    PassiveUIGroup_Utility[useUtilityPassiveCount].ImageChange(Icon_Image);
                    useUtilityPassiveCount++;
                    return true;
                }
                break;
            case PassiveID.None:
                Debug.LogWarning("�нú� ID�� None�Դϴ�. �ƹ� �۾��� �������� �ʽ��ϴ�.");
                return false;                
        }
        return false;
    }
    public void Try_EquipUnPassive(PassiveUI targetUI)
    {
        PassiveID targetUiPassiveID = targetUI.passiveID;
        ID_PassiveUI_Dic[targetUiPassiveID].passiveID = targetUiPassiveID;
        ID_PassiveUI_Dic[targetUiPassiveID].ImageChange(Icon_Image);

        switch (targetUiPassiveID)
        {
            case PassiveID.Offensive1:
            case PassiveID.Offensive2:
            case PassiveID.Offensive3:
            case PassiveID.Offensive4:
            case PassiveID.Offensive5:
                useOffensivePassiveCount--;                
                break;
            case PassiveID.Defensive1:
            case PassiveID.Defensive2:
            case PassiveID.Defensive3:
            case PassiveID.Defensive4:
            case PassiveID.Defensive5:
                useDeffensivePassiveCount--;
                break;
            case PassiveID.Utility1:
            case PassiveID.Utility2:
            case PassiveID.Utility3:
            case PassiveID.Utility4:
            case PassiveID.Utility5:
                useUtilityPassiveCount--;
                break;
            case PassiveID.None:
                Debug.LogWarning("�нú� ID�� None�Դϴ�. �ƹ� �۾��� �������� �ʽ��ϴ�.");
                break;
        }
    }

    private void LoadIconImages()
    {
        Icon_Image.Clear();
        string path = "Assets/NewAssets/�ƿ�����";
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { path });

        foreach (string guid in guids)
        {
            //���� Gui�� ���� ���� ��η� ����
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            //��ȯ�� ��θ� ����Ͽ� �ش� ��ο� �ִ� Sprite������ �ε� �մϴ�.
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite != null)
            {
                Icon_Image.Add(sprite);
                //���Ƕ���ũ�� Null�� �ƴ� ��� , ����Ʈ�� �߰��մϴ�.
            }
        }

        Debug.Log($"{Icon_Image.Count}���� �̹����� �ҷ��Խ��ϴ�.");
    }


    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Jihe");
    }

    public void Reton()
    {
        EventSystem.current.SetSelectedGameObject(FalstButton.gameObject);
    }

}