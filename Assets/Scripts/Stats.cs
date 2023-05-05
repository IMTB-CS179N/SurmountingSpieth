using Project.Input;
using System;

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
    public string C_Class { get; set; }
    [Order(2)]
    public int Health { get; set; }
    [Order(3)]
    public int BaseDamage { get; set; } //attack points
    [Order(4)]
    public int BaseMana { get; set; }
    [Order(5)]
    public int BaseSpeed { get; set; }
    [Order(6)]
    public string Model { get; set; } //temporary until we have sprite incorporation
    [Order(7)]
    public bool Alive { get; set; }

    


    public Stats(){

        //EntityName = "";
        Race = "";
        C_Class = "";
        Health = -1;
        BaseDamage = -1;
        BaseMana = -1;
        BaseSpeed = -1;
        Model = "";
        Alive = false;


    }

    //deprecated
    public Stats(string EN, string Race, string Class, int Health, int BD, int BM, int BS, string Sprites){

        //this.EntityName = EN;
        this.Race = Race;
        this.C_Class = Class;
        this.Health = Health;
        this.BaseDamage = BD;
        this.BaseMana = BM;
        this.BaseSpeed = BS;
        this.Model = Sprites;
        this.Alive = true;

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
