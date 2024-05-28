using System.Collections.Generic;
using UnityEngine;
using UI;

namespace PlayableCharacters
{
    public class Inventory : MonoBehaviour
    {

        public static Inventory Instance { get; private set; }

        [SerializeField] private KeyInventory KeyInventory;

        private List<int> keys = new List<int>(); 

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddKey(int keyId)
        {
            keys.Add(keyId);
            Debug.Log("Added key with ID " + keyId + " to inventory.");
            KeyInventory.SetKey1(true);
        }

        public bool HasKey(int keyId)
        {
            if(keys.Contains(keyId))
            {
                KeyInventory.SetKey1(false);
                return true;
            }
            return false;
        }
    }
}