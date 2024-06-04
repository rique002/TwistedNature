using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace UI
{
    public class InteractionBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactText;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image interactImage;

        [SerializeField] private GameObject imageContainer;
        [SerializeField] private Animator animator;

        [SerializeField] public GameObject answer1;

        [SerializeField] public GameObject answer2;

        [SerializeField] private TypewriterEffect typewriterEffect;
        private Button buttonAnswer1;
        private Button buttonAnswer2;

        private bool isNPC = false;

        public void SetNPC(bool isNPC){
            this.isNPC = isNPC;
        }
        private void Awake()
        {
            answer1.SetActive(false);
            answer2.SetActive(false);
            imageContainer.SetActive(false);
        }

        private void Start()
        {
            buttonAnswer1 = answer1.GetComponentInChildren<Button>();
            buttonAnswer2 = answer2.GetComponentInChildren<Button>();
            buttonAnswer1.onClick.AddListener(OnAnswer1Clicked);
            buttonAnswer2.onClick.AddListener(OnAnswer2Clicked);

        }

        private void Update(){
            print("Typing: " + typewriterEffect.IsTyping);
            print("isNPC: " + isNPC);
            if(!isNPC){
                answer1.SetActive(false);
                answer2.SetActive(false);
            }
            else{
                imageContainer.SetActive(true);
                
                if(typewriterEffect.IsTyping){
                    answer1.SetActive(false);
                    answer2.SetActive(false);
                }
                else{
                    answer1.SetActive(true);
                    answer2.SetActive(true);
                }
            }
        }

        public void SetText(string text)
        {
            interactText.text = text;
        }

        public void SetName(string name)
        {
            nameText.text = name;
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
        public void SetAnswerText(string answer1Text, string answer2Text)
        {
            print("Setting answer text: " + answer1Text + " " + answer2Text);
            answer1.GetComponentInChildren<TextMeshProUGUI>().text = answer1Text;
            answer2.GetComponentInChildren<TextMeshProUGUI>().text = answer2Text;
        }

        public void OnAnswer1Clicked()
        {
            print("Answer1 clicked");
        }

        public void OnAnswer2Clicked()
        {
            print("Answer2 clicked");
        }
    }
}