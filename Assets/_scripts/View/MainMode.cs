using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
{
    public class MainMode : MonoBehaviour {

        private Dropdown dropDown;
        private Animator animator;

        void Awake()
        {
            // Get this objects components
            dropDown = GetComponent<Dropdown>();
            animator = GetComponent<Animator> ();

            // Link to UI manager
            UiManager.Instance.modes = this;

            // Create the list of options for this dropdown
            // and tell it which function to call when an option is chosen
            GetOptionList();
            dropDown.onValueChanged.AddListener(GetSubList);

            // Load any saved mode choice (0 - "linear" is default)
            Preferences.Instance.Load();
            dropDown.value = Preferences.MainMode;
            dropDown.RefreshShownValue();

            // Initialise the submenu as though we just chose an option
            GetSubList(dropDown.value);
            UiManager.Instance.subModes.Init();
        }

        // This is called whenever we choose a new mode.
        void GetSubList(int value)
        {
            // Tell player preferences and game manager which mode to save/use
            Preferences.MainMode = value;
            Manager.MainMode = (Manager.Mode)value;

            // Tell the submode to generate a list of options for this mode
            UiManager.Instance.subModes.GetOptionList();
        }

        // Called at Awake as this list is static
        void GetOptionList()
        {
            dropDown.ClearOptions();
            // Make a new option for each enum value set in the manager.
            for (int i = 0; i < (int)Manager.Mode.NumberOfTypes; i++)
            {
                dropDown.options.Add(new Dropdown.OptionData() { text = ((Manager.Mode)i).ToString() });
            }
        }

        // Called by the UImanager when a game is started or ended
        public void Fade(bool active) {
            dropDown.interactable = active;
            animator.SetTrigger ("fade");
        }
    }
}
