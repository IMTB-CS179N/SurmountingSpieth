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
}
