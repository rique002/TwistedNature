using System.Collections.Generic;
using UnityEngine;

namespace PlayableCharacters
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance { get; private set; }

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
        }

        public bool HasKey(int keyId)
        {
            return keys.Contains(keyId);
        }
    }
}