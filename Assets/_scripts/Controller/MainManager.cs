using System.Collections;
using System.Linq;
using Managers;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;
using View;

namespace Controller
{
    public class MainManager : Singleton<MainManager>
    {

        public GameTimer GameTimer;

        [HideInInspector] public AudioManager audioManager;
        [HideInInspector] public Game game;

        public StateManager StateManager;

        private IObjectPool[] numberPools;
        public static IObjectPool ExplosionPool;

        // Game state variables
        public static Mode MainMode;
        public static int Current;

        // Enums
        // Main modes
        public enum Mode
        {
            Linear,
            Power,
            Sequence,
            English,
            NumberOfTypes
        }

        // Sub modes
        public static int SubMode;
        public enum Sequence
        {
            Primes,
            Fibbonaci,
            Pi,
            NumberOfTypes
        }

        public enum English
        {
            Alphabet,
            CommonWords,
            AusAnthem,
            NumberOfTypes
        }

        private void Start()
        {
            numberPools = GetComponents<NumberPool>();
            ExplosionPool = GetComponent<ExplosionPool>();

            foreach (var pool in GetComponents<ObjectPool>())
            {
                pool.CreatePool();
            }
            Preferences.Instance.Load();
            if(Preferences.ShowTutorial)
                Tutorial.Instance.RunMenuTutorial();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (game != null)
            {
                game.ProcessState();
            }
        }

        public void Restart() {
            if (SceneManager.GetSceneByName("Game").isLoaded)
            {
                DestroyGame();
            }

            StartCoroutine(LoadGame());
        }

        private IEnumerator LoadGame() {
            Current = 0;
            audioManager.StopTitleMusic();
            game = new Game(MainMode, SubMode, StateManager);

            if (!SceneManager.GetSceneByName("Game").isLoaded)
            {
                var async = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
                while (!async.isDone)
                {
                    yield return null;
                }
            }

            GameTimer = new GameTimer();
        }

        private void DestroyGame() {
            Time.timeScale = 1f;
            GameTimer = null;
            foreach (var pool in numberPools)
            {
                pool.Reset();
            }
            ExplosionPool.Reset();
            StateManager.MoveTo(States.End);
            SceneManager.UnloadSceneAsync("Game");
        }

        public IObjectPool GetPool(int n)
        {
            var index = (int)Mathf.Repeat(n, numberPools.Length);
            return numberPools[index];
        }
    }
}
