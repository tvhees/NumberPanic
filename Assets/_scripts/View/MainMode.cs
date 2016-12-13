using System;
using Assets._scripts.Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using _scripts.Controller;
using _scripts.View;

namespace Assets._scripts.View
{
    public class MainMode : Dropdown, IModeList
    {
        public static UnityEvent OnClicked = new UnityEvent();

        protected override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying)
                return;

            GetOptionList();
            onValueChanged.AddListener(GetSubList);
            LoadSavedModes();
            UiManager.Instance.subModes.Init();
        }

        private void LoadSavedModes()
        {
            Preferences.Instance.Load();
            value = Preferences.MainMode;
            RefreshShownValue();
            onValueChanged.Invoke(value);
        }

        /// <summary>
        /// Create a new entry in the dropdown list for each mode specified in the game manager
        /// </summary>
        public void GetOptionList()
        {
            ClearOptions();
            for (var i = 0; i < (int)Manager.Mode.NumberOfTypes; i++)
                options.Add(new OptionData { text = ((Manager.Mode)i).ToString() });
        }

        /// <summary>
        /// Save mode choice and generate a new list of options in the SubMode menu.
        /// Called whenever a new MainMode is selected
        /// </summary>
        private static void GetSubList(int value)
        {
            Preferences.MainMode = value;
            Manager.MainMode = (Manager.Mode)value;
            UiManager.Instance.subModes.GetOptionList();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            OnClicked.Invoke();
            base.OnPointerClick(eventData);
        }
    }
}
