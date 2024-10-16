using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Meta
{
    public class InteractUI : MonoBehaviour
    {
        [Header("MetatNetworkManager")]
        public MetaNetworkManager MetaNetworkManager;

        [SerializeField] private string Str_GameStartSceneName;

        public void OnClick_StartGame()
        {
            SceneManager.LoadScene(Str_GameStartSceneName);
        }

        public void OnClick_CloseGame()
        {
            Application.Quit();
        }

        public void OnClick_CreateInteractObj()
        {
            MetaNetworkManager.RequestSpawnFieldObject();
        }

        public void OnClick_InteractMotion()
        {
            MetaNetworkManager.RequestChangeAnimState("InteractLoop", true);
        }
    }

}
