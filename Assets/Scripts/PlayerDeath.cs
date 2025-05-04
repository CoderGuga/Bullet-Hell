using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private Sprite newSprite; // Assign the new sprite in the Inspector

    Weapon weapon;
    PlayerMovement playerMovement;
    PlayerHealth playerHealth;
    GameObject firePoint;
    public bool Immortal;

    public void Die()
    {
        //Debug.Log("Player died");
        if (!Immortal)
        {
            // Get the SpriteRenderer component
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            weapon = GetComponent<Weapon>();
            playerMovement = GetComponent<PlayerMovement>();
            playerHealth = GetComponent<PlayerHealth>();

            firePoint = transform.GetChild(0).gameObject;

            SetStage(false);
            
            // Change the sprite
            if (spriteRenderer != null && newSprite != null)
            {
                spriteRenderer.sprite = newSprite;
            }
        }
    }

    void SetStage(bool stage)
    {
        weapon.enabled = stage;
        playerMovement.enabled = stage;
        playerHealth.enabled = stage;
        firePoint.SetActive(stage);
    }
}