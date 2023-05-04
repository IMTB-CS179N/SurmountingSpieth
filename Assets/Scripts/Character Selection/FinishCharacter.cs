using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Input;



namespace Project{
    public class FinishCharacter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start(){

            var items = AssetParser.ParseFromCSV<Stats>("CharacterTemplates.csv");

            Debug.Log("Hello Unity!");
        
        }

        
    }
}
