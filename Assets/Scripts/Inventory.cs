using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class Inventory : MonoBehaviour
{
    public List<Item> inventoryItems = new List<Item>();
    UIController uIController;
    public GameObject mainInventory, itemPrefabUI, itemPrefabPhysical;

    void Start()
    {
        uIController = GetComponent<UIController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (mainInventory.activeSelf)
            {
                mainInventory.SetActive(false);
                GetComponent<Weapon>().enabled = true;
            }
            else
            {
                mainInventory.SetActive(true);
                GetComponent<Weapon>().enabled = false;
            }
        }
    }

    public void AddItem(Item newItem)
    {
        Debug.Log("Item name: " + newItem.itemName + " count: " + newItem.count);
        /*
        int countLeft = newItem.count;

        foreach (Item item in inventoryItems)
        {
            if (item.itemName == newItem.itemName)
            {
                if (item.count + countLeft > item.stackCount)
                {
                    countLeft -= (item.stackCount - item.count);
                    item.count = item.stackCount;
                }
                else
                {
                    item.count += countLeft;
                    uIController.RefreshInventory();
                    return;
                }
            }
        }

        while (countLeft > newItem.stackCount)
        {
            Item temp = new Item(newItem);
            temp.count = temp.stackCount;
            inventoryItems.Add(temp);
            countLeft -= temp.count;
        }

        newItem.count = countLeft;
        inventoryItems.Add(newItem);
        uIController.RefreshInventory();

        foreach (Item item in inventoryItems)
            Debug.Log(item.count);
        */


        int countLeft = newItem.count;

        foreach (Transform slot in mainInventory.transform)
        {
            if (slot.GetComponentInChildren<Item>())
            {
                Item item = slot.GetComponentInChildren<Item>();
                if (item.itemName == newItem.itemName)
                {
                    if (item.count + countLeft > item.stackCount)
                    {
                        countLeft -= (item.stackCount - item.count);
                        item.SetCount(item.stackCount);
                    }
                    else
                    {
                        item.AddCount(countLeft);
                        return;
                    }
                }
            }
        }


        foreach (Transform slot in mainInventory.transform)
        {
            if (!slot.GetComponentInChildren<Item>())
            {
                if (countLeft - newItem.stackCount > 0)
                {
                    GameObject clone = Instantiate(itemPrefabUI, slot);
                    Item cloneItem = clone.GetComponent<Item>();
                    cloneItem.SetItem(newItem);
                    cloneItem.SetCount(newItem.stackCount);
                    countLeft -= newItem.stackCount;
                }
                else
                {
                    GameObject clone = Instantiate(itemPrefabUI, slot);
                    Item cloneItem = clone.GetComponent<Item>();
                    newItem.count = countLeft;
                    cloneItem.SetItem(newItem);
                    return;
                }
            }
        }

    }

    public void DropItem(Item item)
    {
        GameObject clone = Instantiate(itemPrefabPhysical, transform.position, quaternion.identity);
        clone.GetComponent<Item>().SetItem(item);
        Destroy(item.gameObject);
    }
}