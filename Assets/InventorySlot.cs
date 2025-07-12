using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Item heldItem = dropped.GetComponent<Item>();
        if (transform.childCount == 0)
        {
            DragItem dragItem = dropped.GetComponent<DragItem>();
            dragItem.parentAfterDrag = transform;
            Debug.Log("Item added to empty slot");
        }
        else if (heldItem.itemName == GetComponentInChildren<Item>().itemName)
        {
            Item slotItem = GetComponentInChildren<Item>();

            if (heldItem.count + slotItem.count <= slotItem.stackCount)
            {
                Debug.Log("stack not filled");
                slotItem.AddCount(heldItem.count);
                Destroy(heldItem.gameObject);
            }
            else
            {
                Debug.Log("stack filled");
                heldItem.SetCount(heldItem.count - slotItem.stackCount + slotItem.count);
                slotItem.SetCount(slotItem.stackCount);

            }
        }
        //Debug.Log("Held item: " + heldItem.itemName + " current item " + GetComponentInChildren<Item>().itemName);
    }
}
