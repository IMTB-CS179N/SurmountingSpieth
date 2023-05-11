using Project.Game;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class CreationBuilder : UIBuilder
    {
        private class IconElement : VisualElement
        {
            public readonly string Name;

            public readonly VisualElement Image;

            public IconElement(RaceInfo info) : this(info.Name, info.Sprite)
            {
            }

            public IconElement(ClassInfo info) : this(info.Name, info.Sprite)
            {
            }

            public IconElement(string name, Sprite sprite)
            {
                this.Name = name;
                this.name = $"class-{name}-back";
                this.pickingMode = PickingMode.Ignore;

                this.style.width = 80;
                this.style.height = 80;

                this.style.marginTop = 4;
                this.style.marginBottom = 4;
                this.style.marginLeft = 4;
                this.style.marginRight = 4;

                this.style.paddingTop = 4;
                this.style.paddingBottom = 4;
                this.style.paddingLeft = 4;
                this.style.paddingRight = 4;

                this.style.unityBackgroundImageTintColor = Color.clear;
                this.style.backgroundImage = new StyleBackground(ResourceManager.LoadSprite(kSelectedItem));

                this.Image = new VisualElement()
                {
                    name = $"class-{name}-image",
                    pickingMode = PickingMode.Position,
                };

                this.Image.style.flexShrink = 1.0f;
                this.Image.style.flexGrow = 1.0f;

                this.Image.style.unityBackgroundImageTintColor = Color.white;
                this.Image.style.backgroundImage = new StyleBackground(sprite);
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
        }

        private const string kSelectedItem = "UI/Shared/SelectedItemBackground";

        private const string kCharacterImage = "character-image";
        private const string kFinishButton = "finish-button";
        private const string kClassView = "class-scrollview";
        private const string kRaceView = "race-scrollview";
        private const string kHealthBar = "health-bar";
        private const string kManaBar = "mana-bar";
        private const string kDamageBar = "damage-bar";
        private const string kPrecisionBar = "precision-bar";
        private const string kEvasionBar = "evasion-bar";

        private static readonly Color ms_hoverTint = new Color32(200, 200, 200, 255);
        private static readonly Color ms_pressTint = new Color32(170, 170, 170, 255);

        private static readonly Color ms_selectTint = new Color32(60, 20, 185, 255);

        private readonly List<IconElement> m_classIcons = new();
        private readonly List<IconElement> m_raceIcons = new();

        private float m_minimumHealth;
        private float m_maximumHealth;

        private float m_minimumMana;
        private float m_maximumMana;

        private float m_minimumDamage;
        private float m_maximumDamage;

        private float m_minimumPrecision;
        private float m_maximumPrecision;

        private float m_minimumEvasion;
        private float m_maximumEvasion;

        private int m_selectedClass = -1;
        private int m_selectedRace = -1;

        protected override void BindEvents()
        {
            this.OnUIEnabled += this.OnEnableEvent;
            this.OnUIDisabled += this.OnDisableEvent;
        }

        private void OnEnableEvent()
        {
            this.SetupFinishButton();
            this.SetupClassIcons();
            this.SetupRaceIcons();
            this.SetupStatistics();
        }

        private void OnDisableEvent()
        {
            this.m_selectedClass = -1;
            this.m_selectedRace = -1;
            this.m_classIcons.Clear();
            this.m_raceIcons.Clear();
        }

        private void SetupFinishButton()
        {
            var element = this.UI.rootVisualElement.Q<VisualElement>(kFinishButton);

            if (element is not null)
            {
                element.pickingMode = PickingMode.Ignore;

                element.style.unityBackgroundImageTintColor = Color.clear;

                element.RegisterCallback<MouseLeaveEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = Color.white;
                });

                element.RegisterCallback<MouseEnterEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = ms_hoverTint;
                });

                element.RegisterCallback<MouseDownEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = ms_pressTint;
                });
                
                element.RegisterCallback<MouseUpEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = ms_hoverTint;

                    this.OnFinishEvent();
                });

                this.BindKeyAction(Key.F, this.OnFinishEvent);
            }
        }

        private void SetupClassIcons()
        {
            var container = this.UI.rootVisualElement.Q<ScrollView>(kClassView);

            if (container is not null)
            {
                var classes = ResourceManager.Classes;

                for (int i = 0; i < classes.Count; ++i)
                {
                    var icon = new IconElement(classes[i]);

                    icon.RegisterCallback<MouseLeaveEvent>(e =>
                    {
                        icon.Image.style.unityBackgroundImageTintColor = Color.white;
                    });

                    icon.RegisterCallback<MouseEnterEvent>(e =>
                    {
                        icon.Image.style.unityBackgroundImageTintColor = ms_hoverTint;
                    });

                    icon.RegisterCallback<MouseDownEvent>(e =>
                    {
                        icon.Image.style.unityBackgroundImageTintColor = ms_pressTint;
                    });

                    icon.RegisterCallback<MouseUpEvent>(e =>
                    {
                        icon.Image.style.unityBackgroundImageTintColor = ms_hoverTint;

                        int index = this.m_classIcons.IndexOf(icon);

                        if (index != this.m_selectedClass)
                        {
                            if (this.m_selectedClass >= 0)
                            {
                                var selected = this.m_classIcons[this.m_selectedClass];

                                selected.Deselect();
                            }

                            icon.Select();

                            this.m_selectedClass = index;

                            this.UpdateCharacterAndFinish();
                        }

                        Debug.Log($"Currently selected class is \"{icon.Name}\"!");
                    });

                    container.Add(icon);

                    this.m_classIcons.Add(icon);
                }
            }
        }

        private void SetupRaceIcons()
        {
            var container = this.UI.rootVisualElement.Q<ScrollView>(kRaceView);

            if (container is not null)
            {
                var races = ResourceManager.Races;

                for (int i = 0; i < races.Count; ++i)
                {
                    var icon = new IconElement(races[i]);

                    icon.RegisterCallback<MouseLeaveEvent>(e =>
                    {
                        icon.Image.style.unityBackgroundImageTintColor = Color.white;
                    });

                    icon.RegisterCallback<MouseEnterEvent>(e =>
                    {
                        icon.Image.style.unityBackgroundImageTintColor = ms_hoverTint;
                    });

                    icon.RegisterCallback<MouseDownEvent>(e =>
                    {
                        icon.Image.style.unityBackgroundImageTintColor = ms_pressTint;
                    });

                    icon.RegisterCallback<MouseUpEvent>(e =>
                    {
                        icon.Image.style.unityBackgroundImageTintColor = ms_hoverTint;

                        int index = this.m_raceIcons.IndexOf(icon);

                        if (index != this.m_selectedRace)
                        {
                            if (this.m_selectedRace >= 0)
                            {
                                var selected = this.m_raceIcons[this.m_selectedRace];

                                selected.Deselect();
                            }

                            icon.Select();

                            this.m_selectedRace = index;

                            this.UpdateCharacterAndFinish();
                        }

                        Debug.Log($"Currently selected race is \"{icon.Name}\"!");
                    });

                    container.Add(icon);

                    this.m_raceIcons.Add(icon);
                }
            }
        }

        private void SetupStatistics()
        {
            var root = this.UI.rootVisualElement;

            var stats = ResourceManager.Stats;
            int count = stats.Count;

            if (count != 0)
            {
                float min;
                float max;

                min = Single.MaxValue;
                max = Single.MinValue;

                for (int i = 0; i < count; ++i)
                {
                    var value = stats[i].BaseHealth;

                    if (value < min)
                    {
                        min = value;
                    }
                    else if (value > max)
                    {
                        max = value;
                    }
                }

                this.m_maximumHealth = max;
                this.m_minimumHealth = min;

                min = Single.MaxValue;
                max = Single.MinValue;

                for (int i = 0; i < count; ++i)
                {
                    var value = stats[i].BaseMana;

                    if (value < min)
                    {
                        min = value;
                    }
                    else if (value > max)
                    {
                        max = value;
                    }
                }

                this.m_maximumMana = max;
                this.m_minimumMana = min;

                min = Single.MaxValue;
                max = Single.MinValue;

                for (int i = 0; i < count; ++i)
                {
                    var value = stats[i].BaseDamage;

                    if (value < min)
                    {
                        min = value;
                    }
                    else if (value > max)
                    {
                        max = value;
                    }
                }

                this.m_maximumDamage = max;
                this.m_minimumDamage = min;

                min = Single.MaxValue;
                max = Single.MinValue;

                for (int i = 0; i < count; ++i)
                {
                    var value = stats[i].BasePrecision;

                    if (value < min)
                    {
                        min = value;
                    }
                    else if (value > max)
                    {
                        max = value;
                    }
                }

                this.m_maximumPrecision = max;
                this.m_minimumPrecision = min;

                min = Single.MaxValue;
                max = Single.MinValue;

                for (int i = 0; i < count; ++i)
                {
                    var value = stats[i].BaseEvasion;

                    if (value < min)
                    {
                        min = value;
                    }
                    else if (value > max)
                    {
                        max = value;
                    }
                }

                this.m_maximumEvasion = max;
                this.m_minimumEvasion = min;
            }

            this.UpdateStatistics(null);
        }

        private void UpdateStatistics(Stats stats)
        {
            var root = this.UI.rootVisualElement;

            var healthBar = root.Q<ProgressBar>(kHealthBar);
            var manaBar = root.Q<ProgressBar>(kManaBar);
            var damageBar = root.Q<ProgressBar>(kDamageBar);
            var precisionBar = root.Q<ProgressBar>(kPrecisionBar);
            var evasionBar = root.Q<ProgressBar>(kEvasionBar);

            if (stats is null)
            {
                if (healthBar is not null)
                {
                    healthBar.value = 0.0f;
                }

                if (manaBar is not null)
                {
                    manaBar.value = 0.0f;
                }

                if (damageBar is not null)
                {
                    damageBar.value = 0.0f;
                }

                if (precisionBar is not null)
                {
                    this.m_minimumPrecision = precisionBar.lowValue;
                }

                if (evasionBar is not null)
                {
                    evasionBar.value = 0.0f;
                }
            }
            else
            {
                if (healthBar is not null)
                {
                    healthBar.value = RemapToRange(stats.BaseHealth, this.m_minimumHealth, this.m_maximumHealth, 0.0f, 100.0f);
                }

                if (manaBar is not null)
                {
                    manaBar.value = RemapToRange(stats.BaseMana, this.m_minimumMana, this.m_maximumMana, 0.0f, 100.0f);
                }

                if (damageBar is not null)
                {
                    damageBar.value = RemapToRange(stats.BaseDamage, this.m_minimumDamage, this.m_maximumDamage, 0.0f, 100.0f);
                }

                if (precisionBar is not null)
                {
                    precisionBar.value = RemapToRange(stats.BasePrecision, this.m_minimumPrecision, this.m_maximumPrecision, 0.0f, 100.0f);
                }

                if (evasionBar is not null)
                {
                    evasionBar.value = RemapToRange(stats.BaseEvasion, this.m_minimumEvasion, this.m_maximumEvasion, 0.0f, 100.0f);
                }
            }
        }

        private void UpdateCharacterAndFinish()
        {
            var stats = default(Stats);

            if (this.IsFinishButtonInteractable())
            {
                var @class = this.m_classIcons[this.m_selectedClass].Name;
                var race = this.m_raceIcons[this.m_selectedRace].Name;

                stats = ResourceManager.Stats.Find(_ => _.Class == @class && _.Race == race);

                if (stats is null)
                {
                    Debug.Log($"Stats for character with class \"{@class}\" and race \"{race}\" does not exist!");
                }
            }

            var finish = this.UI.rootVisualElement.Q<VisualElement>(kFinishButton);

            if (finish is not null)
            {
                if (stats is null)
                {
                    finish.style.unityBackgroundImageTintColor = Color.clear;

                    finish.pickingMode = PickingMode.Ignore;
                }
                else
                {
                    finish.style.unityBackgroundImageTintColor = Color.white;

                    finish.pickingMode = PickingMode.Position;
                }
            }

            var character = this.UI.rootVisualElement.Q<VisualElement>(kCharacterImage);

            if (character is not null)
            {
                if (stats is null)
                {
                    character.style.backgroundImage = new StyleBackground(StyleKeyword.None);
                }
                else
                {
                    character.style.backgroundImage = new StyleBackground(stats.Sprite);
                }
            }

            this.UpdateStatistics(stats);
        }

        private bool IsFinishButtonInteractable()
        {
            return this.m_selectedClass >= 0 && this.m_selectedRace >= 0;
        }

        private void OnFinishEvent()
        {
            if (this.IsFinishButtonInteractable())
            {
                Debug.Log("Finish clicked!");
            }
        }

        private static float RemapToRange(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }
    }
}
