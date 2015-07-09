using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    class Storefront
    {
        public static List<Item> Inventory = new List<Item>(); //List of all items available in the store

        public static void AddItem(Item item)
        {
            bool itemExists = false;

            //Search inventory to see if item already exists
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].GetName() == item.GetName())
                {
                    itemExists = true;
                }
            }

            //Add item to inventory
            if (!itemExists) Inventory.Add(item);
        }

        public static void RemoveItem(Item item)
        {
            bool itemExists = false;
            //Remove item if it exists
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].GetName() == item.GetName())
                {
                    Inventory.Remove(Inventory[i]);
                    itemExists = true;
                    break;
                }
            }

            //If item is not found in the store
            if (itemExists == false)
            {
                Debug.Print("Item is not in the inventory!");
            }
        }

        public static Item SearchInventory(string item)
        {
            //Find item based on name
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].GetName() == item)
                {
                    return Inventory[i];
                }
            }

            return null;
        }

        public static void UpdateItemQuantity(string item, int quantity)
        {
            Trace.Write("QUAN TO UPDATE: " + quantity);
            //Find item based on name
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].GetName() == item)
                {
                    Inventory[i].SetQuantity(quantity); //Update quantity
                }
            }
        }

    }
}
