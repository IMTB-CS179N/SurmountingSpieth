using UnityEngine;

using Object = UnityEngine.Object;

namespace Project.UI
{
    public class UIManager : MonoBehaviour
    {
        public enum ScreenType
        {
            Invalid = -1,
            Main,
            InGame,
            Battle,
            Creation,
            Inventory,
            Trade,
        }

        private static UIManager ms_instance;

        public static UIManager Instance => UIManager.ms_instance == null ? (UIManager.ms_instance = Object.FindFirstObjectByType<UIManager>()) : UIManager.ms_instance;

        private ScreenType m_currType;
        private ScreenType m_nextType;

        [SerializeField]
        private UIBuilder MainUI;

        [SerializeField]
        private UIBuilder InGameUI;

        [SerializeField]
        private UIBuilder BattleUI;

        [SerializeField]
        private UIBuilder CreationUI;

        [SerializeField]
        private UIBuilder InventoryUI;

        [SerializeField]
        private UIBuilder TradeUI;

        private void Awake()
        {
            this.m_currType = ScreenType.Main;
            this.m_nextType = ScreenType.Invalid;

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

            if (this.BattleUI != null)
            {
                this.BattleUI.enabled = false;
            }
            else
            {
                Debug.LogWarning("Battle UI is not attached to the UI Manager!");
            }

            if (this.CreationUI != null)
            {
                this.CreationUI.enabled = false;
            }
            else
            {
                Debug.LogWarning("Creation UI is not attached to the UI Manager!");
            }

            if (this.InventoryUI != null)
            {
                this.InventoryUI.enabled = false;
            }
            else
            {
                Debug.LogWarning("Inventory UI is not attached to the UI Manager!");
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

        private void Update()
        {
            if (this.m_nextType != ScreenType.Invalid)
            {
                var prev = this.GetCurrentlyEnabledUI();

                if (prev != null)
                {
                    prev.enabled = false;
                }

                this.m_currType = this.m_nextType;
                this.m_nextType = ScreenType.Invalid;

                var next = this.GetCurrentlyEnabledUI();

                if (next != null)
                {
                    next.enabled = true;
                }
            }
        }

        public void PerformScreenChange(ScreenType type)
        {
            this.m_nextType = type;
        }

        public UIBuilder GetUI(ScreenType type)
        {
            return type switch
            {
                ScreenType.Main => this.MainUI,
                ScreenType.InGame => this.InGameUI,
                ScreenType.Battle => this.BattleUI,
                ScreenType.Creation => this.CreationUI,
                ScreenType.Inventory => this.InventoryUI,
                ScreenType.Trade => this.TradeUI,
                _ => null,
            };
        }

        public UIBuilder GetCurrentlyEnabledUI()
        {
            return this.m_currType switch
            {
                ScreenType.Main => this.MainUI,
                ScreenType.InGame => this.InGameUI,
                ScreenType.Battle => this.BattleUI,
                ScreenType.Creation => this.CreationUI,
                ScreenType.Inventory => this.InventoryUI,
                ScreenType.Trade => this.TradeUI,
                _ => null,
            };
        }
    }
}
