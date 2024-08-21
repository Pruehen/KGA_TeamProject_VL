using UnityEngine;

public class CheckUIManager : GlobalSingleton<CheckUIManager>
{
    [SerializeField] CheckUI _checkUI;

    private void Awake()
    {
        Instance.Init();
    }

    void Init()
    {

    }
}
