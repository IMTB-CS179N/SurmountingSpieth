﻿using Project.Game;
using Project.Items;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class TradeBuilder : UIBuilder
    {
        private class IconElement : VisualElement
        {
            private static int ms_uniqueId;

            private bool m_hovered;
            private bool m_pressed;
            private bool m_locked;

            public readonly IItem Item;

            public readonly bool IsPlayers;

            public bool Locked => this.m_locked;

            public readonly VisualElement Image;

            public IconElement(IItem item, bool isPlayers)
            {
                this.Item = item;
                this.IsPlayers = isPlayers;

                this.name = $"item-slot-{ms_uniqueId}-back";
                this.pickingMode = PickingMode.Ignore;

                this.m_locked = false;
                this.m_hovered = false;
                this.m_pressed = false;

                this.style.width = 48;
                this.style.height = 48;

                this.style.marginTop = 5;
                this.style.marginBottom = 5;
                this.style.marginLeft = 5;
                this.style.marginRight = 5;

                this.style.paddingTop = 5;
                this.style.paddingBottom = 5;
                this.style.paddingLeft = 5;
                this.style.paddingRight = 5;

                this.style.backgroundColor = ms_unlockedBackColor;
                this.style.unityBackgroundImageTintColor = Color.clear;
                this.style.backgroundImage = new StyleBackground(ResourceManager.LoadSprite(ResourceManager.SelectedItemPath));

                this.Image = new VisualElement()
                {
                    name = $"item-slot-{ms_uniqueId++}-image",
                    pickingMode = PickingMode.Position,
                };

                this.Image.style.flexShrink = 1;
                this.Image.style.flexGrow = 1;

                this.Image.style.unityBackgroundImageTintColor = Color.white;
                this.Image.style.backgroundImage = new StyleBackground(item.Sprite);
                this.Image.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;

                this.Add(this.Image);
            }

            public void Select()
            {
                this.style.unityBackgroundImageTintColor = ms_selectTint;
            }

            public void Deselect()
            {
                this.style.unityBackgroundImageTintColor = Color.clear;
            }

            public void Lock()
            {
                this.m_locked = true;

                this.style.backgroundColor = ms_lockedBackColor;

                if (this.m_hovered)
                {
                    this.Hover();
                }
                else if (this.m_pressed)
                {
                    this.Press();
                }
                else
                {
                    this.Idle();
                }
            }

            public void Unlock()
            {
                this.m_locked = false;

                this.style.backgroundColor = ms_unlockedBackColor;

                if (this.m_hovered)
                {
                    this.Hover();
                }
                else if (this.m_pressed)
                {
                    this.Press();
                }
                else
                {
                    this.Idle();
                }
            }

            public void Idle()
            {
                this.m_hovered = false;
                this.m_pressed = false;

                this.Image.style.unityBackgroundImageTintColor = this.m_locked ? ms_lockedIdledTint : ms_unlockedIdledTint;
            }

            public void Hover()
            {
                this.m_hovered = true;
                this.m_pressed = false;

                this.Image.style.unityBackgroundImageTintColor = this.m_locked ? ms_lockedHoverTint : ms_unlockedHoverTint;
            }

            public void Press()
            {
                this.m_hovered = false;
                this.m_pressed = true;

                this.Image.style.unityBackgroundImageTintColor = this.m_locked ? ms_lockedPressTint : ms_unlockedPressTint;
            }
        }

        private enum Tab
        {
            Armor,
            Weapon,
            Potion,
            Trinket,
        }

        private const string kArmorButton = "armor-button";
        private const string kWeaponButton = "weapon-button";
        private const string kPotionButton = "potion-button";
        private const string kTrinketButton = "trinket-button";
        private const string kActionButton = "action-button";

        private const string kPlayerInventory = "player-scrollview";
        private const string kSellerInventory = "seller-scrollview";

        private const string kPlayerMoney = "player-money-label";

        private const string kSelectedName = "selected-item-name";
        private const string kSelectedDesc = "selected-item-desc";
        private const string kSelectedPrice = "selected-item-price";
        private const string kSelectedImage = "selected-item-image";

        private static readonly Color ms_unlockedIdledTint = new Color32(255, 255, 255, 255);
        private static readonly Color ms_unlockedHoverTint = new Color32(200, 200, 200, 255);
        private static readonly Color ms_unlockedPressTint = new Color32(170, 170, 170, 255);
        private static readonly Color ms_unlockedBackColor = new Color32(101, 59, 28, 255);

        private static readonly Color ms_lockedIdledTint = new Color32(120, 120, 120, 255);
        private static readonly Color ms_lockedHoverTint = new Color32(95, 95, 95, 255);
        private static readonly Color ms_lockedPressTint = new Color32(70, 70, 70, 255);
        private static readonly Color ms_lockedBackColor = new Color32(61, 29, 14, 255);

        private static readonly Color ms_activeTint = new Color32(150, 150, 150, 255);
        private static readonly Color ms_selectTint = new Color32(60, 20, 185, 255);

        private readonly List<IconElement> m_playerInventory = new();
        private readonly List<IconElement> m_sellerInventory = new();

        private ScrollView m_playerView;
        private ScrollView m_sellerView;

        private bool m_isFocusedOnSeller = false;
        private int m_selectedIndex = -1;
        private Tab m_currentTab = Tab.Armor;

        protected override void BindEvents()
        {
            this.OnUIEnabled += this.OnEnableEvent;
            this.OnUIDisabled += this.OnDisableEvent;
        }

        private void OnEnableEvent()
        {
            this.m_playerView = this.UI.rootVisualElement.Q<ScrollView>(kPlayerInventory);
            this.m_sellerView = this.UI.rootVisualElement.Q<ScrollView>(kSellerInventory);

            this.SetupArmorButton();
            this.SetupWeaponButton();
            this.SetupPotionButton();
            this.SetupTrinketButton();
            this.SetupActionButton();
            this.UpdateMoneyLabel();
            this.UpdateDisplayedItem(null);

            this.BindKeyAction(Key.Escape, this.OnEscapeKeyHit);

            this.ReinitializeAll(this.m_currentTab);
        }

        private void OnDisableEvent()
        {
            this.ResetInventories();

            this.m_playerView = null;
            this.m_sellerView = null;

            this.m_currentTab = Tab.Armor;
        }

        private void OnEscapeKeyHit()
        {
            // #TODO perform save

            UIManager.Instance.PerformScreenChange(UIManager.ScreenType.InGame);
        }

        private void SetupArmorButton()
        {
            this.SetupCallbacksInternal(kArmorButton, Key.A, () => this.ReinitializeAll(Tab.Armor));
        }

        private void SetupWeaponButton()
        {
            this.SetupCallbacksInternal(kWeaponButton, Key.W, () => this.ReinitializeAll(Tab.Weapon));
        }

        private void SetupPotionButton()
        {
            this.SetupCallbacksInternal(kPotionButton, Key.P, () => this.ReinitializeAll(Tab.Potion));
        }

        private void SetupTrinketButton()
        {
            this.SetupCallbacksInternal(kTrinketButton, Key.T, () => this.ReinitializeAll(Tab.Trinket));
        }

        private void SetupActionButton()
        {
            this.SetupCallbacksInternal(kActionButton, Key.S, () => this.PerformInventoryAction());
        }

        private void SetupCallbacksInternal(string name, Key key, Action onMouseUp)
        {
            var element = this.UI.rootVisualElement.Q<VisualElement>(name);

            if (element is not null)
            {
                element.RegisterCallback<MouseLeaveEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = ms_unlockedIdledTint;
                });

                element.RegisterCallback<MouseEnterEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = ms_unlockedHoverTint;
                });

                element.RegisterCallback<MouseDownEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = ms_unlockedPressTint;
                });

                element.RegisterCallback<MouseUpEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = ms_unlockedHoverTint;

                    onMouseUp?.Invoke();
                });

                this.BindKeyAction(key, onMouseUp);
            }
        }

        private void PerformInventoryAction()
        {
            if (this.IsActionInteractable())
            {
                if (this.m_isFocusedOnSeller) // buy from store
                {
                    var item = this.m_sellerInventory[this.m_selectedIndex].Item;

                    this.CreateIconElement(this.m_playerView, item, this.m_playerInventory, true, this.m_currentTab switch
                    {
                        Tab.Armor => Player.Instance.PurchaseArmor(Unsafe.As<Armor>(item)),
                        Tab.Weapon => Player.Instance.PurchaseWeapon(Unsafe.As<Weapon>(item)),
                        Tab.Potion => Player.Instance.PurchasePotion(Unsafe.As<Potion>(item)),
                        Tab.Trinket => Player.Instance.PurchaseTrinket(Unsafe.As<Trinket>(item)),
                        _ => throw new Exception("The current tab is invalid"),
                    });

                    this.UpdateMoneyLabel();

                    this.RecalculatePurchasableItems();

                    this.TryMakeActionInteractable();
                }
                else // sell to store
                {
                    switch (this.m_currentTab)
                    {
                        case Tab.Armor:
                            Player.Instance.SellArmor(this.m_selectedIndex);
                            break;

                        case Tab.Weapon:
                            Player.Instance.SellWeapon(this.m_selectedIndex);
                            break;

                        case Tab.Potion:
                            Player.Instance.SellPotion(this.m_selectedIndex);
                            break;

                        case Tab.Trinket:
                            Player.Instance.SellTrinket(this.m_selectedIndex);
                            break;
                    }

                    this.m_playerView.RemoveAt(this.m_selectedIndex);

                    this.m_playerInventory.RemoveAt(this.m_selectedIndex);

                    this.UpdateMoneyLabel();

                    this.RecalculatePurchasableItems();

                    this.ResetFocusedElement();
                }
            }
        }

        private void ReinitializeAll(Tab newTab)
        {
            this.SetCurrentButtonTint(Color.white);

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
                Tab.Armor => root.Q<VisualElement>(kArmorButton),
                Tab.Weapon => root.Q<VisualElement>(kWeaponButton),
                Tab.Potion => root.Q<VisualElement>(kPotionButton),
                Tab.Trinket => root.Q<VisualElement>(kTrinketButton),
                _ => throw new Exception("The current tab is invalid"),
            };

            if (button is not null)
            {
                button.style.unityBackgroundImageTintColor = color;
            }
        }

        private void ResetInventories()
        {
            this.ResetFocusedElement();

            this.m_playerInventory.Clear();
            this.m_sellerInventory.Clear();

            this.m_playerView?.Clear();
            this.m_sellerView?.Clear();
        }

        private void ResetFocusedElement()
        {
            this.m_selectedIndex = -1;
            this.m_isFocusedOnSeller = false;

            this.UpdateDisplayedItem(null);
        }

        private void SetupInventories()
        {
            this.SetupPlayerInventory();
            this.SetupSellerInventory();
        }

        private void SetupPlayerInventory()
        {
            var view = this.m_playerView;

            if (view is not null)
            {
                switch (this.m_currentTab)
                {
                    case Tab.Armor:
                        this.SetupViewFromItemList(view, Player.Instance.Armors, this.m_playerInventory, true);
                        break;

                    case Tab.Weapon:
                        this.SetupViewFromItemList(view, Player.Instance.Weapons, this.m_playerInventory, true);
                        break;

                    case Tab.Potion:
                        this.SetupViewFromItemList(view, Player.Instance.Potions, this.m_playerInventory, true);
                        break;

                    case Tab.Trinket:
                        this.SetupViewFromItemList(view, Player.Instance.Trinkets, this.m_playerInventory, true);
                        break;
                }
            }
        }

        private void SetupSellerInventory()
        {
            var view = this.m_sellerView;

            if (view is not null)
            {
                switch (this.m_currentTab)
                {
                    case Tab.Armor:
                        this.SetupViewFromItemList(view, ResourceManager.Armors, this.m_sellerInventory, false);
                        break;

                    case Tab.Weapon:
                        this.SetupViewFromItemList(view, ResourceManager.Weapons, this.m_sellerInventory, false);
                        break;

                    case Tab.Potion:
                        this.SetupViewFromItemList(view, ResourceManager.Potions, this.m_sellerInventory, false);
                        break;

                    case Tab.Trinket:
                        this.SetupViewFromItemList(view, ResourceManager.Trinkets, this.m_sellerInventory, false);
                        break;
                }

                this.RecalculatePurchasableItems();
            }
        }

        private void SetupViewFromItemList(ScrollView view, IReadOnlyList<IItem> items, List<IconElement> elements, bool isPlayers)
        {
#if DEBUG
            Debug.Assert(view is not null);
            Debug.Assert(items is not null);
            Debug.Assert(elements is not null);
#endif
            for (int i = 0; i < items.Count; ++i)
            {
                this.CreateIconElement(view, items[i], elements, isPlayers);
            }
        }

        private void CreateIconElement(ScrollView view, IItem item, List<IconElement> elements, bool isPlayers, int index = -1)
        {
            var icon = new IconElement(item, isPlayers);

            icon.RegisterCallback<MouseLeaveEvent>(e =>
            {
                icon.Idle();
            });

            icon.RegisterCallback<MouseEnterEvent>(e =>
            {
                icon.Hover();
            });

            icon.RegisterCallback<MouseDownEvent>(e =>
            {
                icon.Press();
            });

            icon.RegisterCallback<MouseUpEvent>(e =>
            {
                icon.Hover();

                int index = elements.IndexOf(icon);

                if (index != this.m_selectedIndex)
                {
                    if (this.m_selectedIndex >= 0)
                    {
                        var selected = this.m_isFocusedOnSeller
                            ? this.m_sellerInventory[this.m_selectedIndex]
                            : this.m_playerInventory[this.m_selectedIndex];

                        selected.Deselect();
                    }

                    icon.Select();

                    this.m_selectedIndex = index;

                    this.m_isFocusedOnSeller = !icon.IsPlayers;

                    this.UpdateDisplayedItem(icon.Item);
                }

                Debug.Log($"Currently selected item is \"{icon.Item.Name}\"!");
            });

            if (index < 0 || index >= elements.Count)
            {
                view.Add(icon);

                elements.Add(icon);
            }
            else
            {
                view.Insert(index, icon);

                elements.Insert(index, icon);
            }
        }

        private void RecalculatePurchasableItems()
        {
            int money = Player.Instance.Money;

            for (int i = 0; i < this.m_sellerInventory.Count; ++i)
            {
                var icon = this.m_sellerInventory[i];

                if (icon.Item.Price > money)
                {
                    icon.Lock();
                }
                else
                {
                    icon.Unlock();
                }
            }
        }

        private void UpdateMoneyLabel()
        {
            var money = this.UI.rootVisualElement.Q<Label>(kPlayerMoney);

            if (money is not null)
            {
                money.text = "$" + Player.Instance.Money.ToString();
            }
        }

        private void UpdateDisplayedItem(IItem item)
        {
#if DEBUG
            Debug.Assert((item is null) == (this.m_selectedIndex < 0));
#endif
            var root = this.UI.rootVisualElement;
            var name = root.Q<Label>(kSelectedName);
            var desc = root.Q<Label>(kSelectedDesc);
            var price = root.Q<Label>(kSelectedPrice);
            var image = root.Q<VisualElement>(kSelectedImage);

            if (name is not null)
            {
                name.text = item is null ? String.Empty : item.Name;
            }

            if (desc is not null)
            {
                desc.text = item is null ? String.Empty : item.Description;
            }

            if (price is not null)
            {
                price.text = item is null ? String.Empty : "$" + item.Price.ToString();
            }

            if (image is not null)
            {
                image.style.backgroundImage = item is null ? new StyleBackground(StyleKeyword.None) : new StyleBackground(item.Sprite);
            }

            this.TryMakeActionInteractable();
        }

        private bool IsActionInteractable()
        {
            return this.m_selectedIndex >= 0 && (!this.m_isFocusedOnSeller || !this.m_sellerInventory[this.m_selectedIndex].Locked);
        }

        private void TryMakeActionInteractable()
        {
            var action = this.UI.rootVisualElement.Q<VisualElement>(kActionButton);

            if (this.IsActionInteractable())
            {
                action.style.unityBackgroundImageTintColor = ms_unlockedIdledTint;

                action.pickingMode = PickingMode.Position;
            }
            else
            {
                action.style.unityBackgroundImageTintColor = ms_lockedIdledTint;

                action.pickingMode = PickingMode.Ignore;
            }
        }
    }
}