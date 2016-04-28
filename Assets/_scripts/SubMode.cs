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

    public void Init() {
        SetSubValue(dropDown.value);
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
        SetSubValue(dropDown.value);
    }

    void SetSubValue(int value) {
        Manager.subValue = value;
        if (UIManager.Instance.score != null)
            UIManager.Instance.score.UpdateDisplay();
    }

    public void Fade(bool active)
    {
        dropDown.interactable = active;
    }
}
