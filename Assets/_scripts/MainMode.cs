using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;

public class MainMode : MonoBehaviour {

    private Dropdown dropDown;

    void Awake()
    {
        dropDown = GetComponent<Dropdown>();

        UIManager.Instance.modes = this;

        GetOptionList();
        dropDown.onValueChanged.AddListener(GetSubList);

        dropDown.value = Preferences.mainMode;
        dropDown.RefreshShownValue();
        GetSubList(dropDown.value);
        UIManager.Instance.subModes.Init();
    }


    void GetSubList(int value)
    {
        Manager.mode = (Manager.Mode)value;
        UIManager.Instance.subModes.GetOptionList();
    }


    void GetOptionList()
    {
        dropDown.ClearOptions();
        for (int i = 0; i < (int)Manager.Mode.NumberOfTypes; i++)
        {
            dropDown.options.Add(new Dropdown.OptionData() { text = ((Manager.Mode)i).ToString() });
        }
    }

    public void Fade(bool active) {
        dropDown.interactable = active;
    }
}
