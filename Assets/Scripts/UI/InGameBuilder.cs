using Project.Battle;
using Project.Game;

using System;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class InGameBuilder : UIBuilder
    {
        private class HoverableButton
        {
            private bool m_hovered;
            private bool m_pressed;

            public readonly VisualElement Image;

            public readonly VisualElement Layout;

            public HoverableButton(VisualElement layout, VisualElement image, Action onPressed)
            {
                this.Image = image;
                this.Layout = layout;

                this.SetupCallbacks(onPressed);
            }

            private void SetupCallbacks(Action onPressed)
            {
                this.Layout.RegisterCallback<PointerLeaveEvent>(e =>
                {
                    this.m_hovered = false;
                    this.m_pressed = false;

                    if (this.Layout is not null)
                    {
                        this.Layout.style.backgroundColor = ms_backIdledColor;
                    }

                    if (this.Image is not null)
                    {
                        this.Image.style.unityBackgroundImageTintColor = ms_foreIdledColor;
                    }
                });

                this.Layout.RegisterCallback<PointerEnterEvent>(e =>
                {
                    this.m_hovered = true;
                    this.m_pressed = false;

                    if (this.Layout is not null)
                    {
                        this.Layout.style.backgroundColor = ms_backHoverColor;
                    }

                    if (this.Image is not null)
                    {
                        this.Image.style.unityBackgroundImageTintColor = ms_foreHoverColor;
                    }
                });

                this.Layout.RegisterCallback<PointerDownEvent>(e =>
                {
                    if (e.button == 0)
                    {
                        this.m_hovered = false;
                        this.m_pressed = true;

                        if (this.Layout is not null)
                        {
                            this.Layout.style.backgroundColor = ms_backPressColor;
                        }

                        if (this.Image is not null)
                        {
                            this.Image.style.unityBackgroundImageTintColor = ms_forePressColor;
                        }
                    }
                });

                this.Layout.RegisterCallback<PointerUpEvent>(e =>
                {
                    if (e.button == 0)
                    {
                        if (this.m_pressed)
                        {
                            this.m_hovered = true;
                            this.m_pressed = false;

                            if (this.Layout is not null)
                            {
                                this.Layout.style.backgroundColor = ms_backHoverColor;
                            }

                            if (this.Image is not null)
                            {
                                this.Image.style.unityBackgroundImageTintColor = ms_foreHoverColor;
                            }

                            onPressed?.Invoke();
                        }
                    }
                });
            }
        }

        public enum ActionType
        {
            None, // no action, button invisible
            Battle, // start battle
            Enter, // enter shop
        }

        private static readonly Color ms_backIdledColor = new Color32(189, 146, 82, 255);
        private static readonly Color ms_backHoverColor = new Color32(150, 115, 60, 255);
        private static readonly Color ms_backPressColor = new Color32(130, 100, 50, 255);

        private static readonly Color ms_foreIdledColor = new Color32(255, 255, 255, 255);
        private static readonly Color ms_foreHoverColor = new Color32(215, 215, 215, 255);
        private static readonly Color ms_forePressColor = new Color32(175, 175, 175, 255);

        private const string kBackButton = "back-button";

        private const string kSoundLayout = "sound-layout";
        private const string kSoundButton = "sound-button";

        private const string kSaveLayout = "save-layout";
        private const string kSaveButton = "save-button";

        private const string kDebugShopLayout = "debugshop-layout";
        private const string kDebugShopButton = "debugshop-button";

        private const string kInventoryLayout = "inventory-layout";
        private const string kInventoryButton = "inventory-button";

        private const string kActionLayout = "action-layout";
        private const string kActionButton = "action-button";

        private const string kMoneyLabel = "money-label";

        private HoverableButton m_debugshopButton;
        private HoverableButton m_inventoryButton;
        private HoverableButton m_actionButton;
        private HoverableButton m_soundButton;
        private HoverableButton m_saveButton;
        private ActionType m_currentAction;

        protected override void BindEvents()
        {
            this.OnUIEnabled += this.OnEnableEvent;
            this.OnUIDisabled += this.OnDisableEvent;
        }

        private void OnEnableEvent()
        {
            Main.Instance.ShowAllTradeElements = false;

            this.SetupSaveButton();
            this.SetupSoundButton();
            this.SetupInventoryButton();
            this.SetupDebugShopButton();
            this.SetupActionButton();
            this.SetupBackButton();
            this.SetupMoneyLabel();
        }

        private void OnDisableEvent()
        {
            this.m_saveButton = null;
            this.m_soundButton = null;
            this.m_actionButton = null;
            this.m_inventoryButton = null;
            this.m_debugshopButton = null;
        }

        private void SetupSaveButton()
        {
            var root = this.UI.rootVisualElement;

            this.m_saveButton = new HoverableButton(root.Q<VisualElement>(kSaveLayout), root.Q<VisualElement>(kSaveButton), () =>
            {
                // #TODO PERFORM SAVE
            });
        }

        private void SetupSoundButton()
        {
            var root = this.UI.rootVisualElement;

            this.m_soundButton = new HoverableButton(root.Q<VisualElement>(kSoundLayout), root.Q<VisualElement>(kSoundButton), () =>
            {
                // #TODO DISABLE / ENABLE BUTTON
            });
        }

        private void SetupActionButton()
        {
            var root = this.UI.rootVisualElement;

            this.m_actionButton = new HoverableButton(root.Q<VisualElement>(kActionLayout), root.Q<VisualElement>(kActionButton), () =>
            {
                if (this.m_currentAction == ActionType.Enter)
                {
                    UIManager.Instance.PerformScreenChange(UIManager.ScreenType.Trade);
                }

                if (this.m_currentAction == ActionType.Battle)
                {
                    TheBattle.Instance.StartBattle();
                }
            });

            this.UpdateAction(ActionType.None);
        }

        private void SetupInventoryButton()
        {
            var root = this.UI.rootVisualElement;

            this.m_inventoryButton = new HoverableButton(root.Q<VisualElement>(kInventoryLayout), root.Q<VisualElement>(kInventoryButton), () =>
            {
                UIManager.Instance.PerformScreenChange(UIManager.ScreenType.Inventory);
            });
        }

        private void SetupDebugShopButton()
        {
            var root = this.UI.rootVisualElement;

            this.m_debugshopButton = new HoverableButton(root.Q<VisualElement>(kDebugShopLayout), root.Q<VisualElement>(kDebugShopButton), () =>
            {
                Main.Instance.ShowAllTradeElements = true;

                UIManager.Instance.PerformScreenChange(UIManager.ScreenType.Trade);
            });
#if !DEBUG && !DEVELOPMENT_BUILD
            if (this.m_debugshopButton.Layout is not null)
            {
                this.m_debugshopButton.Layout.visible = false;
                this.m_debugshopButton.Layout.pickingMode = PickingMode.Ignore;
            }

            if (this.m_debugshopButton.Image is not null)
            {
                this.m_debugshopButton.Image.visible = false;
                this.m_debugshopButton.Image.pickingMode = PickingMode.Ignore;
            }
#endif
        }

        private void SetupBackButton()
        {
            var button = this.UI.rootVisualElement.Q<VisualElement>(kBackButton);

            var pressed = false;

            if (button is not null)
            {
                button.RegisterCallback<PointerLeaveEvent>(e =>
                {
                    pressed = false;

                    button.style.unityBackgroundImageTintColor = ms_foreIdledColor;
                });

                button.RegisterCallback<PointerEnterEvent>(e =>
                {
                    pressed = false;

                    button.style.unityBackgroundImageTintColor = ms_foreHoverColor;
                });

                button.RegisterCallback<PointerDownEvent>(e =>
                {
                    if (e.button == 0)
                    {
                        pressed = true;

                        button.style.unityBackgroundImageTintColor = ms_forePressColor;
                    }
                });

                button.RegisterCallback<PointerUpEvent>(e =>
                {
                    if (e.button == 0)
                    {
                        if (pressed)
                        {
                            pressed = false;

                            button.style.unityBackgroundImageTintColor = ms_foreHoverColor;

                            UIManager.Instance.PerformScreenChange(UIManager.ScreenType.Main);
                        }
                    }
                });
            }
        }

        private void SetupMoneyLabel()
        {
            var label = this.UI.rootVisualElement.Q<Label>(kMoneyLabel);

            if (label is not null)
            {
                label.text = "$" + Player.Instance.Money.ToString();
            }
        }

        public void UpdateAction(ActionType type)
        {
            this.m_currentAction = type;

            if (type == ActionType.None)
            {
                this.m_actionButton.Layout.visible = false;
                this.m_actionButton.Layout.pickingMode = PickingMode.Ignore;
                this.m_actionButton.Image.visible = false;
                this.m_actionButton.Image.pickingMode = PickingMode.Ignore;

                Unsafe.As<Label>(this.m_actionButton.Image).text = String.Empty;
            }
            else if (type == ActionType.Battle)
            {
                this.m_actionButton.Layout.visible = true;
                this.m_actionButton.Layout.pickingMode = PickingMode.Position;
                this.m_actionButton.Image.visible = true;
                this.m_actionButton.Image.pickingMode = PickingMode.Position;

                Unsafe.As<Label>(this.m_actionButton.Image).text = "BATTLE";
            }
            else if (type == ActionType.Enter)
            {
                this.m_actionButton.Layout.visible = true;
                this.m_actionButton.Layout.pickingMode = PickingMode.Position;
                this.m_actionButton.Image.visible = true;
                this.m_actionButton.Image.pickingMode = PickingMode.Position;

                Unsafe.As<Label>(this.m_actionButton.Image).text = "ENTER";
            }
        }
    }
}
