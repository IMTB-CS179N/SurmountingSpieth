using Project.Input;

using System;

using UnityEngine;

namespace Project.Overworld
{
    public class OverworldInfo : IDisposable
    {
        [Order(0)]
        public int numEnemies { get; set; }

        [Order(1)]
        public float baseModifier { get; set; }

        [Order(2)]
        public float eliteModifier { get; set; }

        [Order(3)]
        public int Rewards { get; set; }

        public void Dispose() { }
    }
}
