﻿using System.Collections;
using Model;
using UnityEngine;

namespace Controller
{
    public class Spawner : MonoBehaviour
    {
        public Camera GameCam;
        [SerializeField] private bool spawn;

        private float padding;
        private float timeBetweenNumbers;
        private float numberSpeed;
        private Game game;
        private Game.State state = Game.State.Attract;

        private void Awake() {
            Manager.Instance.spawner = this;
            EventManager.OnStateChanged.AddListener(OnStateChanged);
        }

        public void OnStateChanged(Game.State newState)
        {
            state = newState;
        }

        public void Start()
        {
            padding = Manager.Instance.Padding;
            timeBetweenNumbers = Manager.Instance.TimeBetweenNumbers;
            numberSpeed = Manager.Instance.NumberSpeed;
            StartCoroutine(RegularSpawn());
            if(Preferences.ShowTutorial)
                Tutorial.Instance.RunGameTutorial();
        }

        private IEnumerator RegularSpawn() {
            var counter = 0;
            var poolCounter = 0;
            while (true)
            {
                if (state != Game.State.Title)
                {
                    var waitTime = Mathf.Pow(padding / (Manager.Current + padding), 2f) * timeBetweenNumbers;

                    if (counter == 0)
                    {
                        // Get a new 'true' number from the pool. Reset the counter only if this works.
                        if (SpawnNumber(poolCounter))
                            counter = Mathf.Min(Random.Range(0, Manager.Current - 1), 8);
                    }
                    else
                    {
                        // Get a new 'false' number from the pool. Decrement the counter only if this works.
                        if (SpawnNumber(poolCounter, false))
                            counter--;
                    }

                    poolCounter++;
                    yield return new WaitForSeconds(waitTime);
                }
                yield return null;
            }

        }

        private bool SpawnNumber(int poolNum, bool real = true) {

            var pool = Manager.Instance.GetPool(poolNum);
            var obj = pool.GetObject();

            // Speed to travel downwards, damped increase as numbers increase
            var speed = (Manager.Current + Manager.Instance.Padding) * numberSpeed / Manager.Instance.Padding;

            // Default value is the next one player needs to collect
            var value = Manager.Current;

            // For 'fake' numbers we want to vary the value a little bit
            if (!real)
            {
                for (var i = 0; i < 20; i++)
                {
                    value = (int)Random.Range(0.3f * Manager.Current, 1.7f * Manager.Current);

                    if (Mathf.Abs(value - Manager.Current) > 1 && value > 0)
                        break;
                }
            }

            if (obj)
            {
                obj.GetComponent<Number>().Init(value, speed, this, pool);
                return true;
            }

            Debug.Log("No numbers left to use, returning false");
            return false;
        }
    }
}
