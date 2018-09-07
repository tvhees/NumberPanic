using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class TutorialPanel : MonoBehaviour
    {

        public Text words;

        public void Display(string textIn)
        {
            gameObject.SetActive(true);
            words.text = textIn;
        }
    }
}
