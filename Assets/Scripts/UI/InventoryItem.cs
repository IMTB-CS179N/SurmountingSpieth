using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace Project.UI
{
    public class InventoryItem : VisualElement
    {
        [Preserve]
        public new class UxmlFactory : UxmlFactory<InventoryItem, UxmlTraits>
        {
        }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }

        public Image Image;
        public string Name;
        public Texture2D Texture;

        public InventoryItem()
        {
            this.Image = new Image();

            this.Add(this.Image);

            this.Image.AddToClassList("item-icon");
            this.AddToClassList("item-base");
        }
    }
}
