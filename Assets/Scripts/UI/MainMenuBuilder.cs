using Project.Input;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class MainMenuBuilder : MonoBehaviour
    {
        private UIDocument m_ui;

        private void Awake()
        {
            this.m_ui = this.GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            if (this.m_ui == null)
            {
                this.m_ui = this.GetComponent<UIDocument>();

                if (this.m_ui == null)
                {
                    return;
                }
            }

            {
                var button = this.m_ui.rootVisualElement.Q<Button>("newgame-button");

                button.clicked += () =>
                {
                    Debug.Log("NewGame Clicked");
                };
            }

            {
                var button = this.m_ui.rootVisualElement.Q<Button>("continue-button");

                button.clicked += () =>
                {
                    Debug.Log("Continue Clicked");
                };
            }

            {
                var button = this.m_ui.rootVisualElement.Q<Button>("settings-button");

                button.clicked += () =>
                {
                    Debug.Log("Settings Clicked");


                };
            }

            {
                var button = this.m_ui.rootVisualElement.Q<Button>("question-button");

                button.clicked += () =>
                {
                    Debug.Log("Question Clicked");
                };
            }

            {
                var button = this.m_ui.rootVisualElement.Q<Button>("credits-button");

                button.clicked += () =>
                {
                    Debug.Log("Credits Clicked");
                };
            }
        }

        private void OnDisable()
        {
            if (this.m_ui != null && this.m_ui.rootVisualElement != null)
            {
                //
            }
        }

        private void Update()
        {
            if (InputProcessor.Instance.IsButtonPressed(UnityEngine.InputSystem.Key.S))
            {
                var target = this.m_ui.rootVisualElement.Q<Button>("newgame-button");

                using (var e = new NavigationSubmitEvent()
                {
                    target = target,
                })
                {
                    target.SendEvent(e);
                }
            }
        }
    }
}
