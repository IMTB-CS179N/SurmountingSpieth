using System;

namespace Project.UI
{
    public class BattleBuilder : UIBuilder
    {
        protected override void BindEvents()
        {
            this.OnUIEnabled += this.OnEnableEvent;
            this.OnUIDisabled += this.OnDisableEvent;
        }

        private void OnEnableEvent()
        {

        }

        private void OnDisableEvent()
        {

        }
    }
}
