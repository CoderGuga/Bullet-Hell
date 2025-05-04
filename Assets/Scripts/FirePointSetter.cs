using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointSetter : MonoBehaviour
{
    [SerializeField] float offset = 0.5f;
    Vector2 mousePosition, playerPosition, playerToMouse;
    Vector3 worldPosition;
    void Update()
    {
        mousePosition = Input.mousePosition;

        worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

        mousePosition = new Vector2(worldPosition.x, worldPosition.y);

        playerPosition = transform.parent.position;
        playerToMouse = (mousePosition - playerPosition).normalized;
        transform.position = (playerToMouse * offset) + playerPosition;

        //Debug.Log($"mouse pos x:{mousePosition.x} y:{mousePosition.y}");
        //Debug.Log($"player pos x:{playerPosition.x} y:{playerPosition.y}");
        //Debug.Log($"playertomouse pos x:{playerToMouse.x} y:{playerToMouse.y}");
    }
}