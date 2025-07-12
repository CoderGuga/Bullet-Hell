using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    public string itemName;
    public int stackCount,
    count,
    rarity;
    public Sprite sprite;
    public Collider2D currentCollider;
    public GameObject itemPrefab;
    bool inRange;

    void Start()
    {
        //itemName = name;
        if (GetComponent<SpriteRenderer>())
        {
            if (!sprite && GetComponent<SpriteRenderer>().sprite)
                sprite = GetComponent<SpriteRenderer>().sprite;
        }
        SetCountText(count);
    }

    public Item(Item item)
    {
        itemName = item.itemName;
        stackCount = item.stackCount;
        count = item.count;
        rarity = item.rarity;
        sprite = item.sprite;
    }

    public void SetItem(Item item)
    {
        if (item.itemName != null)
            itemName = item.itemName;
        else
            itemName = "empty";
        stackCount = item.stackCount;
        count = item.count;
        rarity = item.rarity;

        if (item.sprite)
            sprite = item.sprite;

        if (GetComponent<UnityEngine.UI.Image>())
            GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        SetCountText(count);

        if (GetComponent<SpriteRenderer>())
            GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetCount(int newCount)
    {
        count = newCount;
        SetCountText(count);
    }

    public void AddCount(int newCount)
    {
        count += newCount;
        SetCountText(count);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            currentCollider = collider;
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
            inRange = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("e") && inRange)
        {
            currentCollider.GetComponent<Inventory>().AddItem(this);
            Destroy(gameObject);
        }
    }

    void SetCountText(int count)
    {
        if (transform.Find("Count"))
        {
            Transform countT = transform.Find("Count");
            if (countT.GetComponent<TextMeshProUGUI>())
            {
                TextMeshProUGUI countText = countT.GetComponent<TextMeshProUGUI>();
                countText.text = count.ToString();
            }
            else if (countT.GetComponent<TextMeshPro>())
            {
                TextMeshPro countText = countT.GetComponent<TextMeshPro>();
                countText.text = count.ToString();
            }
        }
    }

    /*
    Rarity list:
    1 - common
    2 - uncommon
    3 - rare
    4 - epic
    5 - legendary
    */
}