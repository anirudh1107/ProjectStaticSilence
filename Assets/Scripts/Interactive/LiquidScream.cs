using UnityEngine;

public class LiquidScream : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(PlayerManager player)
    {
        Debug.Log("Interacted with liquid scream!");
        // Implement the effect of the liquid scream on the player here
        player.FillStamina(50f); // Example: Fill the player's stamina by 50
    }
}
