using UnityEngine;

public class Coin : Collectable
{
    [SerializeField]
    private int value = 1; // Value of the coin

    protected override void Collect()
    {
        // Implement collectible behavior, such as adding score, currency, etc.
        Debug.Log("Collected a coin worth " + value + "!");
        // Add score, currency, etc. here
        Destroy(gameObject); // Destroy the collected object
    }
}

