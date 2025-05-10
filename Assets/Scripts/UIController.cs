using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Transform heartParent;
    public Sprite livingHeart, deadHeart;
    
    PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        UpdateHearts();
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
}
