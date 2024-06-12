using UnityEngine;
using TMPro;
namespace UI
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tutorialText;
        [SerializeField] private TextMeshProUGUI skipText;
        private int status = 1;

        public void SetText(string text)
        {
            tutorialText.text = text;
        }

        public void skipTutorial()
        {
            if (status == 0)
            {
                status++;
                tutorialText.text = "use WASD to move yourself around";
            }
            else if (status == 1)
            {
                status++;
                tutorialText.text = "press F to interact with objects and creatures";
            }
            else if (status == 2)
            {
                status++;
                tutorialText.text = "press SPACE to jump and hold to fly";
            }
            else if (status == 3)
            {
                status++;
                tutorialText.text = "press P to attack";
            }
            else if (status == 4)
            {
                status++;
                tutorialText.text = "press E to change between characters";
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
