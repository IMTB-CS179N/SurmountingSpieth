using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class CreationBuilder : UIBuilder
    {
        private const string kFinishButton = "finish-button";

        private static readonly Color ms_hoverTint = new Color32(200, 200, 200, 255);
        private static readonly Color ms_pressTint = new Color32(170, 170, 170, 255);

        protected override void BindEvents()
        {
            this.SetupFinishButton();

            // #TODO
        }


        private void SetupFinishButton()
        {
            var button = this.UI.rootVisualElement.Q<VisualElement>(kFinishButton);

            if (button is not null)
            {
                button.RegisterCallback<MouseLeaveEvent>(e =>
                {
                    button.style.unityBackgroundImageTintColor = Color.white;
                });

                button.RegisterCallback<MouseEnterEvent>(e =>
                {
                    button.style.unityBackgroundImageTintColor = ms_hoverTint;
                });

                button.RegisterCallback<MouseDownEvent>(e =>
                {
                    button.style.unityBackgroundImageTintColor = ms_pressTint;
                });
                
                button.RegisterCallback<MouseUpEvent>(e =>
                {
                    button.style.unityBackgroundImageTintColor = ms_hoverTint;

                    Debug.Log("Clicked and released!");
                });
            }
        }
    }
}
