﻿using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class HighScore : MonoBehaviour {

        private Text text;
        private Animator animator;

        void Awake()
        {
            text = GetComponent<Text>();
            animator = GetComponent<Animator>();
        }

        public void Fade()
        {
            animator.SetTrigger("fade");
        }

        public void ChangeText(FaceValue fV)
        {
            text.text = fV.Text;
        }
    }
}
