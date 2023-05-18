using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class CurrentBattle : MonoBehaviour
    {
        private static CurrentBattle ms_instance;
        public static CurrentBattle Instance =>
            ms_instance == null
                ? (ms_instance = FindFirstObjectByType<CurrentBattle>())
                : ms_instance;
        BattleInfo currentBattle;

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }
    }
}
