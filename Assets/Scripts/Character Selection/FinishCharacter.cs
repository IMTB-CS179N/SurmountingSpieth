using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Input;



namespace Project{
    public class FinishCharacter : MonoBehaviour
    {
        [SerializeField] public CharacterSelection data; //used to tie button input to values
        Stats[] Templates; //csv information
        Stats UserCharacter; //Final decision

        // Start is called before the first frame update
        void Start(){

            Templates = AssetParser.ParseFromCSV<Stats>("CharacterTemplates.csv");

            Debug.Log("Successful Parse");
            
        
        }

        public void CompleteCharacter(){

            for(int i = 0; i < 30; ++i){

                string CsvRace = Templates[i].EntityName;
                string CsvClass = Templates[i].Race;
                
                //Debug.Log(CsvClass);
                //Debug.Log(CsvRace);
                

                if(CsvClass == data.ClassChoice && CsvRace == data.RaceChoice){
                    UserCharacter = Templates[i];
                    
                    Debug.Log("Template Saved");
                    break;
                }

            }

            //Debug.Log("Class Choice: "+UserCharacter.C_Class);
            //Debug.Log("Race Choice: "+UserCharacter.Race);

        }

        
    }
}
