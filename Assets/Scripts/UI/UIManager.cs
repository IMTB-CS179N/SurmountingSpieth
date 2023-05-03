using System;

using UnityEngine;
using UnityEngine.UIElements;

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
            Character,
            Trade,
        }

        private static UIManager ms_instance;

        public static UIManager Instance => UIManager.ms_instance == null ? (UIManager.ms_instance = Object.FindFirstObjectByType<UIManager>()) : UIManager.ms_instance;

        private ScreenType m_currentType;

        [SerializeField]
        private UIDocument MainUI;

        [SerializeField]
        private UIDocument InGameUI;

        [SerializeField]
        private UIDocument SettingsUI;

        [SerializeField]
        private UIDocument CharacterCreationUI;

        [SerializeField]
        private UIDocument TradeUI;

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

            if (this.CharacterCreationUI != null)
            {
                this.CharacterCreationUI.enabled = false;
            }
            else
            {
                Debug.LogWarning("Character Creation UI is not attached to the UI Manager!");
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

        public UIDocument GetUI(ScreenType type)
        {
            return type switch
            {
                ScreenType.Main => this.MainUI,
                ScreenType.InGame => this.InGameUI,
                ScreenType.Settings => this.SettingsUI,
                ScreenType.Character => this.CharacterCreationUI,
                ScreenType.Trade => this.TradeUI,
                _ => null,
            };
        }

        public UIDocument GetCurrentlyEnabledUI()
        {
            return this.m_currentType switch
            {
                ScreenType.Main => this.MainUI,
                ScreenType.InGame => this.InGameUI,
                ScreenType.Settings => this.SettingsUI,
                ScreenType.Character => this.CharacterCreationUI,
                ScreenType.Trade => this.TradeUI,
                _ => null,
            };
        }

        public void BindEventToScreenButton(ScreenType type, string buttonName, Action action)
        {
            var screen = this.GetUI(type);

            if (screen != null)
            {
                var button = screen.rootVisualElement.Q<Button>(buttonName);

                if (button != null)
                {
                    button.clicked += action;
                }
            }
        }
    }
}
