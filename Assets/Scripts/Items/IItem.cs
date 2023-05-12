using UnityEngine;

namespace Project.Items
{
    public interface IItem
    {
        string Name { get; }

        int Price { get; }

        string Description { get; }

        Sprite Sprite { get; }
    }
}
