using UnityEngine;

public class Coin : Collectable
{

    protected override void Collect()
    {
        // Add score, currency, etc. here
        Destroy(gameObject); // Destroy the collected object
    }
}

