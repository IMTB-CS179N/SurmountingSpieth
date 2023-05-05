﻿using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

using InputProcessor = Project.Input.InputProcessor;

namespace Project.UI
{
    public class UIBuilder : MonoBehaviour
    {
        private readonly List<(Key key, Action action)> m_keyEvents = new();
        private UIDocument m_ui;

        public UIDocument UI => this.m_ui == null ? (this.m_ui = this.gameObject.GetComponent<UIDocument>()) : this.m_ui;

        public event Action OnUIEnabled;

        public event Action OnUIDisabled;

        private void Awake()
        {
            this.OnUIEnabled += () => this.UI.enabled = true;
            this.OnUIDisabled += () => this.UI.enabled = false;
        }

        private void OnEnable()
        {
            this.OnUIEnabled?.Invoke();
        }

        private void OnDisable()
        {
            this.OnUIDisabled?.Invoke();
        }

        private void Update()
        {
            var processor = InputProcessor.Instance;

            if (processor != null)
            {
                for (int i = 0; i < this.m_keyEvents.Count; ++i)
                {
                    var keyEvent = this.m_keyEvents[i];

                    if (processor.IsButtonPressed(keyEvent.key))
                    {
                        keyEvent.action();
                    }
                }
            }
        }

        public void BindButtonClick(string name, Action action)
        {
            var button = this.UI.rootVisualElement.Q<Button>(name);

            if (button is not null)
            {
                button.clicked += action;
            }
        }

        public void BindKeyAction(Key key, Action action)
        {
            if (action is not null)
            {
                this.m_keyEvents.Add((key, action));
            }
        }

        protected virtual void BindEvents()
        {
        }
    }
}
