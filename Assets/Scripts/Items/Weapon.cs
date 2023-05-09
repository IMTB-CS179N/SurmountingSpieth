using Project.Input;
using System;



namespace Project{
    

public class Weapons : IDisposable
{
    [Order(0)] 
    public string WeaponName { get; set; } //weapon name
    [Order(1)]
    public double WeaponDMG { get; set; } //base weapon dmg
    [Order(2)]
    public double WCChance { get; set; } //crit chance
    [Order(3)]
    public double WCMod { get; set; } //crit modifier
    [Order(4)]
    public double WDMGmod { get; set; } //weapon dmg modifier
    [Order(5)]
    public double WSPDmod { get; set; } //weapon spd modifier
    [Order(6)]
    public string Model { get; set; } //temporary until we have sprite incorporation

    


    public Weapons(){

        WeaponName = "";
        WeaponDMG = 7.0;
        WCChance = 0.5;
        WCMod = 0.5;
        WDMGmod = 2.0;
        WSPDmod = 2.0;
        Model="";


    }

    //deprecated
    public Weapons(string WeaponName, double WeaponDMG, double WCChance, double WCMod, double WDMGmod, double WSPDmod,string Model){

        this.WeaponName = WeaponName;
        this.WeaponDMG = WeaponDMG;
        this.WCChance = WCChance;
        this.WCMod = WCMod;
        this.WDMGmod = WDMGmod;
        this.WSPDmod = WSPDmod;
        this.Model = Model;

    }

    public void Dispose(){}


    

}
}


