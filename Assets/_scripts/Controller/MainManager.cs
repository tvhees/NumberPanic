using System.Collections;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;
using View;

namespace Controller
{
    public class MainManager : Singleton<MainManager> {

        public bool TimeAttackMode;
        public GameTimer GameTimer { get; private set; }

        [HideInInspector] public UiManager ui;
        [HideInInspector] public AudioManager audioManager;
        [HideInInspector] public Game game;
        [HideInInspector] public Spawner spawner;
        [HideInInspector] public Advertising adverts;
        [HideInInspector] public MainMode modes;
        [HideInInspector] public Text highScore, title;
        public NumberPool[] numberPools;
        [HideInInspector] public static ExplosionPool explosionPool;

        // Data holders
        public readonly Data data = new Data();

        // Game state variables
        public static Mode MainMode;
        public static int Current;
        public bool GameStarted { get { return game != null && game.IsInPlayState; } }
        public bool GameEnded { get { return game != null && !game.IsInPlayState; } }

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

        private void Start() {
            foreach(var pool in numberPools)
                pool.CreatePool();
            explosionPool.CreatePool();
            Preferences.Instance.Load();
            if(Preferences.ShowTutorial)
                Tutorial.Instance.RunMenuTutorial();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            if(Input.GetKeyDown(KeyCode.T))
                GameTimer.AddTimePenalty();

            if (game != null)
                game.ProcessState();
        }

        public void Restart() {
            if(SceneManager.GetSceneByName("Game").isLoaded)
                DestroyGame();
            StartCoroutine(LoadGame());
        }

        private IEnumerator LoadGame(bool initializing = false) {
            Current = 0;
            audioManager.StopTitleMusic();
            game = new Game(data, MainMode, SubMode);

            if (!SceneManager.GetSceneByName("Game").isLoaded)
            {
                var async = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
                while (!async.isDone)
                {
                    yield return null;
                }
            }

            if (TimeAttackMode && !initializing)
                NewTimer();
        }

        public void NewTimer()
        {
            GameTimer = new GameTimer();
        }

        private void DestroyGame() {
            Time.timeScale = 1f;
            GameTimer = null;
            foreach(var pool in numberPools)
                pool.Reset();
            explosionPool.Reset();
            game.End();
            SceneManager.UnloadSceneAsync("Game");
        }

        public NumberPool GetPool(int n)
        {
            var index = (int)Mathf.Repeat(n, numberPools.Length);
            return numberPools[index];
        }
    }
}
