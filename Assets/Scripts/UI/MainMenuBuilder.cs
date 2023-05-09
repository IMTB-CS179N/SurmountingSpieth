using Project.Input;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class MainMenuBuilder : UIBuilder
    {
        protected override void BindEvents()
        {
            this.OnUIEnabled += this.OnEnableEvent;
            this.OnUIDisabled += this.OnDisableEvent;

            this.BindButtonClick("newgame-button", this.OnNewGameButtonClicked);
            this.BindButtonClick("continue-button", this.OnContinueButtonClicked);
            this.BindButtonClick("settings-button", this.OnSettingsButtonClicked);
            this.BindButtonClick("question-button", this.OnQuestionButtonClicked);
            this.BindButtonClick("credits-button", this.OnCreditsButtonClicked);
        }

        private void OnEnableEvent()
        {

        }

        private void OnDisableEvent()
        {

        }

        private void OnNewGameButtonClicked()
        {

        }

        private void OnContinueButtonClicked()
        {

        }

        private void OnSettingsButtonClicked()
        {

        }

        private void OnQuestionButtonClicked()
        {

        }

        private void OnCreditsButtonClicked()
        {

        }
    }
}
