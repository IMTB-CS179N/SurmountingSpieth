using Project.UI;

using UnityEngine;

namespace Project.Battle
{
    public class MapManager : MonoBehaviour
    {
        private static MapManager ms_instance;

        private GameObject m_overworld;

        public static MapManager Instance => ms_instance == null ? (ms_instance = FindFirstObjectByType<MapManager>()) : ms_instance;

        [SerializeField]
        private InGameBuilder InGameUI;

        [SerializeField]
        private GameObject OverworldPrefab;

        private void Awake()
        {
            if (this.InGameUI == null)
            {
                Debug.Log("Warning: InGameUI attached is null");
            }

            if (this.OverworldPrefab == null)
            {
                Debug.Log("Warning: Overworld prefab is null");
            }
        }

        public void LoadInGame()
        {
            UIManager.Instance.TransitionWithDelay(() =>
            {
                if (this.m_overworld == null)
                {
                    this.m_overworld = GameObject.Instantiate(this.OverworldPrefab);

                    this.m_overworld.name = "Overworld";
                }

                this.m_overworld.SetActive(true);

                UIManager.Instance.PerformScreenChange(UIManager.ScreenType.InGame);
            }, null, 2.0f);
        }

        public void ReturnToMain()
        {
            UIManager.Instance.TransitionWithDelay(() =>
            {
                if (this.m_overworld != null)
                {
                    this.m_overworld.SetActive(false);
                }

                UIManager.Instance.PerformScreenChange(UIManager.ScreenType.Main);
            }, null, 2.0f);
        }

        public void StartBattle()
        {

        }

        public void UpdateAction(InGameBuilder.ActionType action)
        {
            this.InGameUI.UpdateAction(action);
        }
    }
}
