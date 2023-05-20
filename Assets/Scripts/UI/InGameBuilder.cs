using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.UI
{
    public class InGameBuilder : UIBuilder
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
