using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        public void SetValue(float value)
        {
            slider.value = value;
        }
    }
}