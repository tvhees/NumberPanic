using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class TutorialPanel : MonoBehaviour
    {

        public Text words;

        public void Display(string textIn)
        {
            words.text = textIn;
        }
    }
}
