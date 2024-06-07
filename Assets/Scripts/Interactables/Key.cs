using PlayableCharacters;

namespace Interactables
{
    public class Key : Interactable
    {
        public int keyId;

        public override void Interact()
        {
            Inventory.Instance.AddKey(keyId);
            Destroy(gameObject);
        }
    }
}