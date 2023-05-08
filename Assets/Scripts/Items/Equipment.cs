using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Input;

namespace Project
{
    public interface IItem
    {
        int Price { get; set; }
    }

    public class Armor : IItem
    {
        [Order(0)]
        public string ArmorName { get; set; }

        [Order(1)]
        public int ArmorCost { get; set; }

        [Order(2)]
        public int ArmorValue { get; set; }

        [Order(3)]
        public int x { get; set; }

        [Order(4)]
        public string Model { get; set; }

        public int Price { get; set; }

        public Armor()
        {
            ArmorName = "";
            ArmorCost = -1;
            ArmorValue = 5;
            x = 0;
            Model = "";
            Price = (int)(ArmorCost * .75);
        }
    }

    public class Weapon : IItem
    {
        // int w_price = 100;
        public string WName { get; set; }
        public double WeaponDMG { get; set; }
        public double WCChance { get; set; }
        public double WCMod { get; set; }
        public double WDMGmod { get; set; } //attack points
        public double WSPDmod { get; set; } //attack points
        public string Model { get; set; }
        public int Price { get; set; }

        // Start is called before the first frame update
        public Weapon()
        {
            WName = "WeaponName";
            WeaponDMG = 10;
            WCChance = 2.0;
            WCMod = 2.0;
            WDMGmod = 2.0;
            WSPDmod = -1;
            Model = "";
            Price = (int)0;
        }
    }
}
