﻿using System.Collections;
using UnityEngine;
using _scripts.Model;

namespace _scripts.Controller
{
    public class Spawner : MonoBehaviour
    {

        public GameObject number;
        public Camera gameCam;
        [HideInInspector] public bool spawn;
        [HideInInspector] public float leftBound, rightBound;

        private float padding, waitFactor, speedFactor;

        void Awake() {
            Manager.Instance.spawner = this;
        }

        public void Init()
        {
            leftBound = 0.2f;
            rightBound = 0.95f;
            padding = Manager.Padding;
            waitFactor = Manager.WaitFactor;
            speedFactor = Manager.SpeedFactor;
            spawn = true;
            StartCoroutine(RegularSpawn());
        }

        IEnumerator RegularSpawn() {
            int counter = 0;

            while (spawn)
            {
                switch (Manager.Instance.game.state)
                {
                    case Game.State.ATTRACT:
                    case Game.State.PLAY:
                    case Game.State.CRITICAL:
                    case Game.State.END:
                    case Game.State.SCORE:
                        float waitTime = Mathf.Pow(padding / (Manager.Current + padding), 2f) * waitFactor;

                        if (counter == 0)
                        {
                            // Get a new 'true' number from the pool. Reset the counter only if this works.
                            if (SpawnNumber())
                                counter = Mathf.Min(Random.Range(0, Manager.Current - 1), 8);
                        }
                        else
                        {
                            // Get a new 'false' number from the pool. Decrement the counter only if this works.
                            if (SpawnNumber(false))
                                counter--;
                        }

                        yield return new WaitForSeconds(waitTime);

                        break;
                }

                yield return null;

            }

        }

        private bool SpawnNumber(bool real = true) {
            // Spawn from the top of the screen - recalculate every time in case orientation changes
            Vector3 position = gameCam.ViewportToWorldPoint(new Vector3(Random.Range(leftBound, rightBound), 1f, 20f));

            // Speed to travel downwards, damped increase as numbers increase
            float speed = (Manager.Current + Manager.Padding) * speedFactor / Manager.Padding;

            // Default value is the next one player needs to collect
            int value = Manager.Current;

            // For 'fake' numbers we want to vary the value a little bit
            if (!real)
            {
                for (int i = 0; i < 20; i++)
                {
                    value = (int)Random.Range(0.3f * Manager.Current, 1.7f * Manager.Current);

                    if (Mathf.Abs(value - Manager.Current) > 1 && value > 0)
                        break;
                }
            }

            var obj = Manager.numberPool.GetObject();
            if (obj)
            {
                obj.GetComponent<Number>().Init(value, position, speed, this);
                return true;
            }

            Debug.Log("No numbers left to use, returning false");
            return false;
        }
    }
}