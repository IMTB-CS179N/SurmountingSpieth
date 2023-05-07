using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public interface IItem
    {
        int Price { get; }
    }

    public class Armor : IItem
    {
        int m_price = 100;
        public int ArmorValue { get; set; }

        // Start is called before the first frame update
        public Armor()
        {
            ArmorValue = 30;
        }

        public int Price => this.m_price;
    }

    public class Weapon : IItem
    {
        int w_price = 100;
        public string WName{ get; set; }
        public double WeaponDMG { get; set; }
        public double WCChance { get; set; }
        public double WCMod { get; set; }
        public double WDMGmod { get; set; } //attack points
        public double WSPDmod { get; set; } //attack points
        public string Model { get; set; } 
        // Start is called before the first frame update
        public Weapon()
        {
            WName="WeaponName";
            WeaponDMG=10;
            WCChance=2.0;
            WCMod=2.0;
            WDMGmod=2.0;
            WSPDmod=-1;
            Model="";
        }

        public int Price => this.w_price;
    }
    
}
