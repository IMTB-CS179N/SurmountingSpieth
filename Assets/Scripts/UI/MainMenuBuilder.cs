using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class MainMenuBuilder : MonoBehaviour
    {
        private UIDocument m_ui;

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

            var fontSize = new StyleLength(Mathf.Ceil((Screen.width / 640) * 0.4f + 14.0f));

            {
                var button = this.m_ui.rootVisualElement.Q<Button>("button_play");

                button.style.fontSize = fontSize;

                button.clicked += () =>
                {
                    Debug.Log("Play Clicked");
                };
            }

            {
                var button = this.m_ui.rootVisualElement.Q<Button>("button_rules");

                button.style.fontSize = fontSize;

                button.clicked += () =>
                {
                    Debug.Log("Rules Clicked");
                };
            }

            {
                var button = this.m_ui.rootVisualElement.Q<Button>("button_settings");

                button.style.fontSize = fontSize;

                button.clicked += () =>
                {
                    Debug.Log("Settings Clicked");
                };
            }

            {
                var button = this.m_ui.rootVisualElement.Q<Button>("button_quit");

                button.style.fontSize = fontSize;

                button.clicked += () =>
                {
                    Debug.Log("Quit Clicked");
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
    }
}
