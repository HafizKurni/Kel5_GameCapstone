using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    private List<Item> itemList;

    public Inventory() {
        itemList = new List<Item>();

        AddItem(new Item {itemType = Item.ItemType.Coin, amount = 1});
        AddItem(new Item {itemType = Item.ItemType.HealthPotion, amount = 1});
        AddItem(new Item {itemType = Item.ItemType.ManaPotion, amount = 1});
        // Debug.Log(itemList.Count);
    }

    public void AddItem (Item Item) {
        itemList.Add(Item);
    }

    public List<Item> GetItemList() {
        return itemList;
    }
}
