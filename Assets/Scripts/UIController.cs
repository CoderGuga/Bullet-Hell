using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Transform heartParent, inventoryParent;
    public Sprite livingHeart, deadHeart;

    PlayerHealth playerHealth;
    Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    public float itemSlotCellSize = 50f;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        inventory = GetComponent<Inventory>();

        itemSlotContainer = inventoryParent.Find("ItemBackround");
        itemSlotTemplate = itemSlotContainer.Find("Item");

        UpdateHearts();
        RefreshInventory();
    }
    public void UpdateHearts()
    {
        for (int i = 0; i < playerHealth.playerMaxHealth; i++)
        {
            Transform child = heartParent.GetChild(i);
            if (i < playerHealth.playerCurrentHealth)
                child.GetComponent<Image>().sprite = livingHeart;
            else
                child.GetComponent<Image>().sprite = deadHeart;
        }
    }

    public void AddHearts()
    {
        for (int i = 0; i < playerHealth.playerMaxHealth; i++)
        {
            Transform child = heartParent.GetChild(i);
            child.gameObject.SetActive(true);
        }
        UpdateHearts();
    }

    public void RefreshInventory()
    {
        foreach (Transform child in inventoryParent)
            {
                if (child != itemSlotContainer && child.name != "backround")
                    Destroy(child.gameObject);

            }
        int x = 0, y = 0;
        foreach (Item item in inventory.inventoryItems)
        {
            Transform newSlot = Instantiate(itemSlotContainer, inventoryParent);
            newSlot.gameObject.SetActive(true);
            RectTransform itemSlotRectTransfrom = newSlot.GetComponent<RectTransform>();
            itemSlotRectTransfrom.anchoredPosition = new Vector2(itemSlotRectTransfrom.anchoredPosition.x + x * itemSlotCellSize, itemSlotRectTransfrom.anchoredPosition.y + y * itemSlotCellSize);

            Transform newItem = newSlot.Find("Item");  
            newItem.gameObject.SetActive(true);

            UnityEngine.UI.Image image = newItem.GetComponent<UnityEngine.UI.Image>();
            image.sprite = item.sprite;

            TextMeshProUGUI countText = newSlot.Find("Count").GetComponent<TextMeshProUGUI>();
            countText.text = item.count.ToString();

            x++;
        }
    }
}
