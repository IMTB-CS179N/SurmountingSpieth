using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Input;
using UnityEngine.SceneManagement;

namespace Project
{
    public class FinishCharacter : MonoBehaviour
    {
        [SerializeField]
        public CharacterSelection data; //used to tie button input to values
        public Stats[] Templates; //csv information
        public Stats UserCharacter; //Final decision

        // Start is called before the first frame update
        void Start()
        {
            Templates = AssetParser.ParseFromCSV<Stats>("CharacterTemplatesV1.csv");

            Debug.Log("Successful Parse");
            CompleteCharacter();
        }

        public void CompleteCharacter()
        {
            data.ClassChoice = "Barbarian";
            data.RaceChoice = "Orc";

            for (int i = 0; i < 30; ++i)
            {
                string CsvRace = Templates[i].Race;
                string CsvClass = Templates[i].CharacterClass;

                // Debug.Log(CsvClass);
                // Debug.Log(CsvRace);

                if (CsvClass == data.ClassChoice && CsvRace == data.RaceChoice)
                {
                    UserCharacter = Templates[i];

                    Debug.Log("Template Saved");
                    //SceneManager.LoadScene (sceneName:"Map");
                    break;
                }
            }

            // Debug.Log("Class Choice: " + UserCharacter.C_Class);
            // Debug.Log("Race Choice: " + UserCharacter.Race);
        }
    }
}
