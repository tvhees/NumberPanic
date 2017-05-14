using System;
using Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View
{
    public class SubMode : Dropdown, IModeList {

        public static UnityEvent OnClicked = new UnityEvent();

        protected override void Awake() {
            base.Awake();
            if (!Application.isPlaying)
                return;
            onValueChanged.AddListener(SetSubValue);
        }

        // Called after the main modes are initially generated
        public void Init() {
            // Load any previously saved submode (default: 0) and use it
            value = Preferences.SubMode;
            SetSubValue(value);
        }

        // Called on initialisation and every time we choose a new submode
        // or new main mode
        private static void SetSubValue(int value) {
            // Tell player prefs and manager what submode we're on
            Preferences.SubMode = value;
            MainManager.SubMode = value;

            // Update the main score display to reflect the choice
            // mostly required for odd sequences like primes that don't start at 0
            if (UiManager.Instance.score != null)
                UiManager.Instance.score.UpdateDisplay();
        }

        // Get a new list of options any time we pick a different main mode
        public void GetOptionList()
        {
            ClearOptions();

            switch (MainManager.MainMode)
            {
                case MainManager.Mode.Linear:
                    // We just need numbers for these options
                    for (var i = 0; i < 10; i++)
                        options.Add(new OptionData { text = (i + 1).ToString() });
                    break;
                case MainManager.Mode.Power:
                    for (var i = 1; i < 3; i++)
                        options.Add(new OptionData { text = (i + 1).ToString() });
                    break;
                case MainManager.Mode.Sequence:
                    // Create an option for each sequence defined in the Manager
                    for (var i = 0; i < (int)MainManager.Sequence.NumberOfTypes; i++)
                        options.Add(new OptionData { text = ((MainManager.Sequence)i).ToString() });
                    break;
                case MainManager.Mode.English:
                    for (var i = 0; i < (int)MainManager.English.NumberOfTypes; i++)
                        options.Add(new OptionData { text = ((MainManager.English)i).ToString() });
                    break;
                case MainManager.Mode.NumberOfTypes:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }

            value = 0;
            RefreshShownValue();
            SetSubValue(value);
        }

        public void Fade(bool active)
        {
            interactable = active;
            animator.SetTrigger ("fade");
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            OnClicked.Invoke();
            base.OnPointerClick(eventData);
        }
    }
}
