using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine.InputSystem;

namespace Project.UI
{
    public class TradeBuilder : UIBuilder
    {
        private readonly List<InventoryItem> m_playerInventory = new();
        private readonly List<InventoryItem> m_sellerInventory = new();

        protected override void BindEvents()
        {
            this.OnUIEnabled += this.OnEnableEvent;
            this.OnUIDisabled += this.OnDisableEvent;

            this.BindButtonClick("armor-button", this.OnArmorButtonClicked);
            this.BindButtonClick("weapon-button", this.OnWeaponButtonClicked);
            this.BindButtonClick("consume-button", this.OnConsumeButtonClicked);
            this.BindButtonClick("trinket-button", this.OnTrinketButtonClicked);
            this.BindButtonClick("sell-button", this.OnSellButtonClicked);

            this.BindKeyAction(Key.Escape, this.OnEscapeKeyHit);
        }

        private void OnEnableEvent()
        {

        }

        private void OnDisableEvent()
        {

        }

        private void OnEscapeKeyHit()
        {

        }

        private void OnArmorButtonClicked()
        {

        }

        private void OnWeaponButtonClicked()
        {

        }

        private void OnConsumeButtonClicked()
        {

        }

        private void OnTrinketButtonClicked()
        {

        }

        private void OnSellButtonClicked()
        {

        }
    }
}
