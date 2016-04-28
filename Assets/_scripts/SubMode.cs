using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubMode : MonoBehaviour {

    private Dropdown dropDown;

    void Awake() {
        dropDown = GetComponent<Dropdown>();
        dropDown.onValueChanged.AddListener(SetSubValue);
        UIManager.Instance.subModes = this;
    }

    public void GetOptionList()
    {
        dropDown.ClearOptions();

        switch (Manager.mode)
        {
            case Manager.Mode.linear:
                for (int i = 0; i < 10; i++)
                    dropDown.options.Add(new Dropdown.OptionData() { text = (i + 1).ToString() });
                break;
            case Manager.Mode.power:
                for (int i = 0; i < 3; i++)
                    dropDown.options.Add(new Dropdown.OptionData() { text = (i + 1).ToString() });
                break;
            case Manager.Mode.sequence:
                for (int i = 0; i < (int)Manager.Sequence.NumberOfTypes; i++)
                    dropDown.options.Add(new Dropdown.OptionData() { text = ((Manager.Sequence)i).ToString() });
                break;
        }

        dropDown.RefreshShownValue();
    }

    void SetSubValue(int value) {
        Manager.subValue = value;
    }

    public void Fade()
    {
        dropDown.interactable = false;
    }
}
