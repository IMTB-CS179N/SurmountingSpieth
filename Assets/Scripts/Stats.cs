using Project.Input;
using System;

//using Project.Input;

// to create a usable copy, right click the script inside the file inspector
// and click "Unit Stats"

//[CreateAssetMenu(menuName = "Unit Stats")]


namespace Project{
    

public class Stats : IDisposable
{
    //[Order(0)] // need to add the column numbers
    public string EntityName { get; set; }
    [Order(0)]
    public string Race { get; set; }
    [Order(1)]
    public string CharacterClass { get; set; }
    [Order(2)]
    public int Health { get; set; } //HP
    [Order(3)]
    public int BaseDamage { get; set; } //attack points
    [Order(4)]
    public int BaseMana { get; set; } 
    [Order(5)]
    public double BaseDodge { get; set; } 
    [Order(6)]
    public double BasePrecision { get; set; }
    [Order(7)]
    public double BaseCritChance { get; set; }
    [Order(8)]
    public double BaseCritMult { get; set; }
    [Order(9)]
    public string Model { get; set; } //temporary until we have sprite incorporation
    [Order(10)]
    public bool Alive { get; set; }


    public Stats(){

        this.EntityName = "";
        this.Race = "";
        this.CharacterClass = "";
        this.Health = 0;
        this.BaseDamage = 0;
        this.BaseMana = 0;
        this.BaseDodge = 0.0;
        this.BasePrecision = 0.0;
        this.BaseCritChance = 0.0;
        this.BaseCritMult = 0.0;
        this.Model = "";
        this.Alive = true;

    }

    public Stats(string Race, string Class, int Health, int baseDamage, int baseMana, double baseDodge, double basePrecision, double baseCritChance, double baseCritMult, string spriteLoc){

        this.EntityName = "";
        this.Race = Race;
        this.CharacterClass = Class;
        this.Health = Health;
        this.BaseDamage = baseDamage;
        this.BaseMana = baseMana;
        this.BaseDodge = baseDodge;
        this.BasePrecision = basePrecision;
        this.BaseCritChance = baseCritChance;
        this.BaseCritMult = baseCritMult;
        this.Model = spriteLoc;
        this.Alive = true;

    }


    public void UpdateName(string name){

        this.EntityName = name;

    }

    public void changeHealth(int amount){ //increase or decrease health, meant for items or effects that modify health

        Health  = Health + amount;
        if(Health <= 0){ //
            Health = 1;
        }

    }

    public void changeAP(int amount){ //increase or decrease AP, meant for items or effects that modify AP

        BaseDamage = BaseDamage + amount;
        if(BaseDamage <= 0){
            BaseDamage = 1;
        }

    }

    public void takeDamage(int amount){ //used for combat 

        Health = Health - amount;
        if(Health <= 0){
            Alive = false;
        }

    }

    public void revive(){
        Alive = true;
    }

    public void kill(){
        Alive = false;
    }

    public bool giveAlive(){ //just wrote this to get rid of a warning message
        return Alive;
    }

    public void Dispose(){}

    

}
}

//Notes 
/*

Game object are empty containers
    right click -> create empty

Add component ->  Rendering -> Sprite Renderer




*/
