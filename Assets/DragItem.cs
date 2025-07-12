using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEngine.UI.Image image;
    [HideInInspector] public Transform parentAfterDrag;

    Item currentItem;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (GetComponent<Item>().count != 1)
            {
                currentItem = GetComponent<Item>();
                GameObject clone = Instantiate(gameObject, transform.parent);
                clone.GetComponent<Item>().SetCount((int)Math.Floor((double)clone.GetComponent<Item>().count / 2));
                currentItem.SetCount((int)Math.Ceiling((double)GetComponent<Item>().count / 2));
            }

            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;


        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag");
        transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        transform.parent.GetComponent<InventorySlot>().OnDrop(eventData);
    }

    private static DragItem hoveredItem;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredItem = this;
        //Debug.Log("hovered over " + this.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoveredItem == this)
            hoveredItem = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && hoveredItem == this)
        {
            Debug.Log("pressed q");
            DropItem(hoveredItem.GetComponent<Item>());
        }
    }

    void DropItem(Item item)
    {
        // Find the player GameObject by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Inventory>().DropItem(item);
    }
}
