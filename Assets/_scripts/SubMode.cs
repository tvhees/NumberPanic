using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubMode : MonoBehaviour {

    private Dropdown dropDown;
	private Animator animator;

    void Awake() {
        dropDown = GetComponent<Dropdown>();
		animator = GetComponent<Animator> ();
        dropDown.onValueChanged.AddListener(SetSubValue);
        UIManager.Instance.subModes = this;
    }

    // Called after the main modes are initially generated
    public void Init() {
        // Load any previously saved submode (default: 0) and use it
        dropDown.value = Preferences.subMode;
        SetSubValue(dropDown.value);
    }

    // Get a new list of options any time we pick a different main mode
    public void GetOptionList()
    {
        dropDown.ClearOptions();

        switch (Manager.mode)
        {
            case Manager.Mode.linear:
                // We just need numbers for these options
                for (int i = 0; i < 10; i++)
                    dropDown.options.Add(new Dropdown.OptionData() { text = (i + 1).ToString() });
                break;
            case Manager.Mode.power:
                for (int i = 1; i < 3; i++)
                    dropDown.options.Add(new Dropdown.OptionData() { text = (i + 1).ToString() });
                break;
            case Manager.Mode.sequence:
                // Create an option for each sequence defined in the Manager
                for (int i = 0; i < (int)Manager.Sequence.NumberOfTypes; i++)
                    dropDown.options.Add(new Dropdown.OptionData() { text = ((Manager.Sequence)i).ToString() });
                break;
        }

        dropDown.RefreshShownValue();
        SetSubValue(dropDown.value);
    }

    // Called on initialisation and every time we choose a new submode
    // or new main mode
    void SetSubValue(int value) {
        // Tell player prefs and manager what submode we're on
        Preferences.subMode = value;
        Manager.subValue = value;

        // Update the main score display to reflect the choice
        // mostly required for odd sequences like primes that don't start at 0
        if (UIManager.Instance.score != null)
            UIManager.Instance.score.UpdateDisplay();
    }

    public void Fade(bool active)
    {
        dropDown.interactable = active;
		animator.SetTrigger ("fade");
    }
}
