using System.Collections.Generic;
using UnityEngine;
using UI;

namespace PlayableCharacters
{
    [CreateAssetMenu]
    public class Inventory : MonoBehaviour
    {

        public static Inventory Instance { get; private set; }

        [Header("Items")]
        public List<ItemData> items = new();

        [SerializeField] private KeyInventory KeyInventory;

        private List<int> keys = new List<int>();

        private List<int> triangles = new List<int>();

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

        public void AddTriangle(int triangleId)
        {
            triangles.Add(triangleId);
            Debug.Log("Added triangle with ID " + triangleId + " to inventory.");
        }

        public bool HasKey(int keyId)
        {
            if (keys.Contains(keyId))
            {
                KeyInventory.SetKey1(false);
                return true;
            }
            return false;
        }

        public bool HasTriangle(int triangleId)
        {
            return triangles.Contains(triangleId);
        }
    }
}
