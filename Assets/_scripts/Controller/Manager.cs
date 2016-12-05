using System.Collections;
using Assets._scripts.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using _scripts.Model;
using _scripts.View;

namespace _scripts.Controller
{
    public class Manager : Singleton<Manager> {

        public bool timeAttackMode;
        public GameTimer gameTimer { get; private set; }

        [HideInInspector] public UiManager ui;
        [HideInInspector] public Game game;
        [HideInInspector] public Spawner spawner;
        [HideInInspector] public Advertising adverts;
        [HideInInspector] public MainMode modes;
        [HideInInspector] public Text highScore, title;
        [HideInInspector] public static NumberPool numberPool;
        [HideInInspector] public static ExplosionPool explosionPool;

        // Variables used to control pace of the game - set in inspector while testing
        [SerializeField] private float setPadding = 20f;
        [SerializeField] private float setWait = 1f;
        [SerializeField] private float setSpeed = 3f;
        public static float Padding;
        public static float WaitFactor;
        public static float SpeedFactor;

        // Data holders
        readonly Data data = new Data();

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
            NumberOfTypes
        }

        public enum English
        {
            Common,
            AusAnthem,
            NumberOfTypes
        }

        private void Start() {
            Padding = setPadding;
            WaitFactor = setWait;
            SpeedFactor = setSpeed;
            numberPool.CreatePool();
            explosionPool.CreatePool();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

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

            game = new Game(data, MainMode, SubMode);

            if (!SceneManager.GetSceneByName("Game").isLoaded)
            {
                var async = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
                var breakNo = 0;
                while (!async.isDone)
                {
                    yield return null;
                    breakNo++;
                    if (breakNo <= 1000) continue;
                    Debug.Log("Breaking load loop");
                    break;
                }
            }

            if (timeAttackMode && !initializing)
                NewTimer();

            spawner.Init();
        }

        public void NewTimer()
        {
            gameTimer = new GameTimer();
        }

        private void DestroyGame() {
            Time.timeScale = 1f;
            gameTimer = null;
            numberPool.Reset();
            explosionPool.Reset();
            game.End();
            SceneManager.UnloadSceneAsync("Game");
        }
    }
}
