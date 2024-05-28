using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class KeyInventory : MonoBehaviour
    {
        [SerializeField] private Image key1;
        public void SetKey1(bool active)
        {
            key1.gameObject.SetActive(active);
        }

    }

}