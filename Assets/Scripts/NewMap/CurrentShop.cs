using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class CurrentShop : MonoBehaviour
    {
        private static CurrentShop ms_instance;
        public static CurrentShop Instance =>
            ms_instance == null
                ? (ms_instance = FindFirstObjectByType<CurrentShop>())
                : ms_instance;
        ShopInfo currentShop;

        void Start() { }

        void Update() { }
    }
}
