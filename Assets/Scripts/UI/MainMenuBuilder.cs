using Project.Game;
using Project.Input;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class MainMenuBuilder : UIBuilder
    {
        private const string kNewGameButton = "newgame-button";
        private const string kContinueButton = "continue-button";
        private const string kSettingsButton = "settings-button";
        private const string kQuestionButton = "question-button";
        private const string kCreditsButton = "credits-button";

        private static readonly Color ms_hoverTint = new Color32(200, 200, 200, 255);
        private static readonly Color ms_pressTint = new Color32(170, 170, 170, 255);

        protected override void BindEvents()
        {
            this.OnUIEnabled += this.OnEnableEvent;
            this.OnUIDisabled += this.OnDisableEvent;
        }

        private void OnEnableEvent()
        {
            this.SetupNewGameButton();
            this.SetupContinueButton();
            this.SetupSettingsButton();
            this.SetupQuestionButton();
            this.SetupCreditsButton();
        }

        private void OnDisableEvent()
        {
        }

        private void SetupNewGameButton()
        {
            var element = this.UI.rootVisualElement.Q<VisualElement>(kNewGameButton);

            if (element is not null)
            {
                element.RegisterCallback<MouseLeaveEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = Color.white;
                });

                element.RegisterCallback<MouseOverEvent>(e =>
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

                    this.OnNewGameEvent();
                });

                this.BindKeyAction(Key.N, this.OnNewGameEvent);
            }
        }

        private void SetupContinueButton()
        {
            var element = this.UI.rootVisualElement.Q<VisualElement>(kContinueButton);

            if (element is not null)
            {
                element.RegisterCallback<MouseLeaveEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = Color.white;
                });

                element.RegisterCallback<MouseOverEvent>(e =>
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

                    this.OnContinueEvent();
                });

                this.BindKeyAction(Key.C, this.OnContinueEvent);
            }
        }

        private void SetupSettingsButton()
        {
            var element = this.UI.rootVisualElement.Q<VisualElement>(kSettingsButton);

            if (element is not null)
            {
                element.RegisterCallback<MouseLeaveEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = Color.white;
                });

                element.RegisterCallback<MouseOverEvent>(e =>
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

                    this.OnSettingsEvent();
                });

                this.BindKeyAction(Key.S, this.OnSettingsEvent);
            }
        }

        private void SetupQuestionButton()
        {
            var element = this.UI.rootVisualElement.Q<VisualElement>(kQuestionButton);

            if (element is not null)
            {
                element.RegisterCallback<MouseLeaveEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = Color.white;
                });

                element.RegisterCallback<MouseOverEvent>(e =>
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

                    this.OnQuestionEvent();
                });

                this.BindKeyAction(Key.Q, this.OnQuestionEvent);
            }
        }

        private void SetupCreditsButton()
        {
            var element = this.UI.rootVisualElement.Q<VisualElement>(kCreditsButton);

            if (element is not null)
            {
                element.RegisterCallback<MouseLeaveEvent>(e =>
                {
                    element.style.unityBackgroundImageTintColor = Color.white;
                });

                element.RegisterCallback<MouseOverEvent>(e =>
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

                    this.OnCreditsEvent();
                });

                this.BindKeyAction(Key.K, this.OnCreditsEvent);
            }
        }

        private void OnNewGameEvent()
        {
            UIManager.Instance.PerformScreenChange(UIManager.ScreenType.Creation);
        }

        private void OnContinueEvent()
        {
            // #TODO we have to show message box if there is no save game data

            Player.InitializeFromSaveData();

            UIManager.Instance.PerformScreenChange(UIManager.ScreenType.InGame);
        }

        private void OnSettingsEvent()
        {

        }

        private void OnQuestionEvent()
        {

        }

        private void OnCreditsEvent()
        {

        }
    }
}
