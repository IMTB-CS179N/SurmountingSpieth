using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Game;

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
        //List<EncounterData> enemies;

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }

        public void SetBattle(BattleInfo newBattle)
        {
            currentBattle = newBattle;
            //enemies = currentBattle.Enemies;
        }
    }
}
