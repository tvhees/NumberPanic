using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace View
{
  public class Pause : BaseMonoBehaviour
  {
		[SerializeField] private GameObject _pauseMenu;
		[SerializeField] private Text _pauseText;

    public void HandlePauseClick()
    {
      GetManager<StateManager>().MoveTo(States.Pause);
			_pauseMenu.SetActive(true);
			_pauseText.gameObject.SetActive(false);
    }

		public void HandlePlayClick() {
			GetManager<StateManager>().MoveTo(States.Play);
			_pauseMenu.SetActive(false);
			_pauseText.gameObject.SetActive(true);
		}

		public void HandleQuitClick() {
			GetManager<StateManager>().MoveTo(States.Score);
			_pauseMenu.SetActive(false);
		}
  }

}
