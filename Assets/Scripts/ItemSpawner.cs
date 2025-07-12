using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemSpawner : MonoBehaviour
{
    private int minSpawnCount, maxSpawnCount;
    public List<Item> itemsToSpawn;

    GameObject spawnedItem;

    void Start()
    {
        SpawnItem();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player" && spawnedItem)
        {
            //collider.GetComponent<Inventory>().AddItem(PickUpItem());
        }
    }

    public void SpawnItem()
    {
        int randomIndex = UnityEngine.Random.Range(0, itemsToSpawn.Count);
        spawnedItem = Instantiate(itemsToSpawn[randomIndex].transform, transform).gameObject;
        SetSpawnCounts(spawnedItem.GetComponent<Item>());
        spawnedItem.GetComponent<Item>().SetCount(UnityEngine.Random.Range(minSpawnCount, maxSpawnCount));
        spawnedItem.GetComponent<Item>().itemPrefab = spawnedItem;
    }

    void SetSpawnCounts(Item item)
    {
        int rarity = item.rarity;

        if (rarity == 1)
        {
            minSpawnCount = 5;
            maxSpawnCount = 20;
        }
        if (rarity == 2)
        {
            minSpawnCount = 3;
            maxSpawnCount = 10;
        }
        if (rarity == 3)
        {
            minSpawnCount = 1;
            maxSpawnCount = 3;
        }
    }

    public Item PickUpItem()
    {
        Item item = spawnedItem.GetComponent<Item>();
        Destroy(spawnedItem);
        return item;
    }

}