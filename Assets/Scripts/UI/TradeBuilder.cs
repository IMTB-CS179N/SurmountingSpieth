using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class TradeBuilder : UIBuilder
    {
        private enum Tab
        {
            Armor,
            Weapon,
            Consume,
            Trinket,
        }

        private const string kArmorButton = "armor-button";
        private const string kWeaponButton = "weapon-button";
        private const string kConsumeButton = "consume-button";
        private const string kTrinketButton = "trinket-button";
        private const string kActionButton = "action-button";

        private const string kPlayerInventory = "player-scrollview";
        private const string kSellerInventory = "seller-scrollview";

        private static readonly Color ms_inactiveTint = Color.white;
        private static readonly Color ms_activeTint = new Color32(170, 170, 170, 255);

        private readonly List<InventoryItem> m_playerInventory = new();
        private readonly List<InventoryItem> m_sellerInventory = new();

        private Tab m_currentTab = Tab.Armor;

        protected override void BindEvents()
        {
            this.OnUIEnabled += this.OnEnableEvent;
            this.OnUIDisabled += this.OnDisableEvent;

            this.BindButtonClick(kArmorButton, this.OnArmorButtonClicked);
            this.BindButtonClick(kWeaponButton, this.OnWeaponButtonClicked);
            this.BindButtonClick(kConsumeButton, this.OnConsumeButtonClicked);
            this.BindButtonClick(kTrinketButton, this.OnTrinketButtonClicked);
            this.BindButtonClick(kActionButton, this.OnActionButtonClicked);

            this.BindKeyAction(Key.Escape, this.OnEscapeKeyHit);
        }

        private void OnEnableEvent()
        {
            this.OnArmorButtonClicked(); // setup armor stuff
        }

        private void OnDisableEvent()
        {
            // nothing here, technically
        }

        private void OnEscapeKeyHit()
        {

        }

        private void OnArmorButtonClicked()
        {
            this.ReinitializeAll(Tab.Armor);
        }

        private void OnWeaponButtonClicked()
        {
            this.ReinitializeAll(Tab.Weapon);
        }

        private void OnConsumeButtonClicked()
        {
            this.ReinitializeAll(Tab.Consume);
        }

        private void OnTrinketButtonClicked()
        {
            this.ReinitializeAll(Tab.Trinket);
        }

        private void OnActionButtonClicked()
        {
            // perform selling:
            //   - get the currently selected item
            //   - remove it from the scrollview
        }

        private void ReinitializeAll(Tab newTab)
        {
            this.SetCurrentButtonTint(ms_inactiveTint);

            this.ResetInventories();

            this.m_currentTab = newTab;

            this.SetCurrentButtonTint(ms_activeTint);

            this.SetupInventories();
        }

        private void SetCurrentButtonTint(Color color)
        {
            var root = this.UI.rootVisualElement;

            var button = this.m_currentTab switch
            {
                Tab.Armor => root.Q<Button>(kArmorButton),
                Tab.Weapon => root.Q<Button>(kWeaponButton),
                Tab.Consume => root.Q<Button>(kConsumeButton),
                Tab.Trinket => root.Q<Button>(kTrinketButton),
                _ => throw new Exception("The current tab is invalid"),
            };

            button.style.unityBackgroundImageTintColor = new StyleColor(color);
        }

        private void ResetInventories()
        {
            this.m_playerInventory.Clear();
            this.m_sellerInventory.Clear();

            var root = this.UI.rootVisualElement;

            root.Q<ScrollView>(kPlayerInventory).Clear();
            root.Q<ScrollView>(kSellerInventory).Clear();
        }

        private void SetupInventories()
        {

        }
    }
}
