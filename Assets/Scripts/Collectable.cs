using UnityEngine;
using PlayableCharacters;

public abstract class Collectable : MonoBehaviour
{

    [SerializeField]
    protected float collectDistance = 2f; 


    [SerializeField]
    protected PlayableCharacter playableCharacter;

    protected virtual void Update()
    {
        
        if (Vector3.Distance(transform.position, playableCharacter.gameObject.transform.position) <= collectDistance)
        {

            
            Collect();
        }
    }

    protected abstract void Collect(); 
}

