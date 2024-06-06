using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TriangleBox : MonoBehaviour
    {
        [SerializeField] private GameObject triangle0;
        [SerializeField] private GameObject triangle1;
        [SerializeField] private GameObject triangle2;

        public void SetTriangle(int triangleId)
        {
            switch (triangleId)
            {
                case 0:
                    triangle0.SetActive(true);
                    break;
                case 1:
                    triangle1.SetActive(true);
                    break;
                case 2:
                    triangle2.SetActive(true);
                    break;
            }
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }

}