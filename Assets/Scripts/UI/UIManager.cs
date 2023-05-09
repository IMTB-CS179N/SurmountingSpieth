using UnityEngine;

using Object = UnityEngine.Object;

namespace Project.UI
{
    public class UIManager : MonoBehaviour
    {
        public enum ScreenType
        {
            Main,
            InGame,
            Settings,
            Creation,
            Trade,
        }

        private static UIManager ms_instance;

        public static UIManager Instance => UIManager.ms_instance == null ? (UIManager.ms_instance = Object.FindFirstObjectByType<UIManager>()) : UIManager.ms_instance;

        private ScreenType m_currentType;

        [SerializeField]
        private UIBuilder MainUI;

        [SerializeField]
        private UIBuilder InGameUI;

        [SerializeField]
        private UIBuilder SettingsUI;

        [SerializeField]
        private UIBuilder CreationUI;

        [SerializeField]
        private UIBuilder TradeUI;

        private void Awake()
        {
            this.m_currentType = ScreenType.Main;

            if (this.MainUI != null)
            {
                this.MainUI.enabled = true;
            }
            else
            {
                Debug.LogWarning("Main UI is not attached to the UI Manager!");
            }

            if (this.InGameUI != null)
            {
                this.InGameUI.enabled = false;
            }
            else
            {
                Debug.LogWarning("InGame UI is not attached to the UI Manager!");
            }

            if (this.SettingsUI != null)
            {
                this.SettingsUI.enabled = false;
            }
            else
            {
                Debug.LogWarning("Settings UI is not attached to the UI Manager!");
            }

            if (this.CreationUI != null)
            {
                this.CreationUI.enabled = false;
            }
            else
            {
                Debug.LogWarning("Character Creation UI is not attached to the UI Manager!");
            }

            if (this.TradeUI != null)
            {
                this.TradeUI.UI.enabled = false;
            }
            else
            {
                Debug.LogWarning("Trade UI is not attached to the UI manager!");
            }
        }

        public void PerformScreenChange(ScreenType type)
        {
            var prev = this.GetCurrentlyEnabledUI();

            if (prev != null)
            {
                prev.enabled = false;
            }

            this.m_currentType = type;

            var next = this.GetCurrentlyEnabledUI();

            if (next != null)
            {
                next.enabled = true;
            }
        }

        public UIBuilder GetUI(ScreenType type)
        {
            return type switch
            {
                ScreenType.Main => this.MainUI,
                ScreenType.InGame => this.InGameUI,
                ScreenType.Settings => this.SettingsUI,
                ScreenType.Creation => this.CreationUI,
                ScreenType.Trade => this.TradeUI,
                _ => null,
            };
        }

        public UIBuilder GetCurrentlyEnabledUI()
        {
            return this.m_currentType switch
            {
                ScreenType.Main => this.MainUI,
                ScreenType.InGame => this.InGameUI,
                ScreenType.Settings => this.SettingsUI,
                ScreenType.Creation => this.CreationUI,
                ScreenType.Trade => this.TradeUI,
                _ => null,
            };
        }
    }
}
