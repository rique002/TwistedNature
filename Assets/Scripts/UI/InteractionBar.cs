using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace UI
{
    public class InteractionBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private Image interactImage;
        [SerializeField] private Animator animator;

        public void SetText(string text)
        {
            interactText.text = text;
        }
        public void SetImage(Sprite sprite)
        {
            interactImage.sprite = sprite;
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                animator.SetTrigger("Open");
                animator.ResetTrigger("Close");
            }
            else
            {
                animator.SetTrigger("Close");
                animator.ResetTrigger("Open");
            }
        }
    }
}