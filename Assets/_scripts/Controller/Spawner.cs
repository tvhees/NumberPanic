using System;
using System.Collections;
using Controller;
using GameData;
using Managers;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameData
{
    public partial class Settings
    {
        [SerializeField] private Spawner.SpawnerSettings spawnerSettings;
        public static Spawner.SpawnerSettings Spawner {get { return instance.spawnerSettings; }}
    }
}

namespace Controller
{
    public class Spawner : BaseMonoBehaviour
    {
        public Camera GameCam;

        [Serializable]
        public class SpawnerSettings
        {
            public float DifficultyPadding;
            public float TimeBetweenNumbers;
            public float NumberSpeed;
        }

        private Game game;

        protected override void Awake() {
            base.Awake();
        }

        public void Start()
        {
            StartCoroutine(RegularSpawn());
            if(Preferences.ShowTutorial) { Tutorial.Instance.RunGameTutorial(); }
        }

        private IEnumerator RegularSpawn() {
            var counter = 0;
            var poolCounter = 0;
            while (true)
            {
                if (!GetManager<StateManager>().CurrentStateIs(States.Title))
                {
                    var waitTime =
                        Mathf.Pow(
                            Settings.Spawner.DifficultyPadding /
                            (MainManager.Current + Settings.Spawner.DifficultyPadding), 2f) *
                        Settings.Spawner.TimeBetweenNumbers;

                    if (counter == 0)
                    {
                        // Get a new 'true' number from the pool. Reset the counter only if this works.
                        if (SpawnNumber(poolCounter))
                            counter = Mathf.Min(Random.Range(0, MainManager.Current - 1), 8);
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

            var pool = MainManager.Instance.GetPool(poolNum);
            var obj = pool.GetObject();

            // Speed to travel downwards, damped increase as numbers increase
            var speed = (MainManager.Current + Settings.Spawner.DifficultyPadding) * Settings.Spawner.NumberSpeed /
                        Settings.Spawner.DifficultyPadding;

            // Default value is the next one player needs to collect
            var value = MainManager.Current;

            // For 'fake' numbers we want to vary the value a little bit
            if (!real)
            {
                for (var i = 0; i < 20; i++)
                {
                    value = (int)Random.Range(0.3f * MainManager.Current, 1.7f * MainManager.Current);

                    if (Mathf.Abs(value - MainManager.Current) > 1 && value > 0)
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
