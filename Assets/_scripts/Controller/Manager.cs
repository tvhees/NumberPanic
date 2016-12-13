using System.Collections;
using Assets._scripts.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using _scripts.Controller;
using _scripts.Model;

namespace Assets._scripts.Controller
{
    public class Manager : Singleton<Manager> {

        public bool TimeAttackMode;
        public GameTimer GameTimer { get; private set; }

        [HideInInspector] public UiManager ui;
        [HideInInspector] public Game game;
        [HideInInspector] public Spawner spawner;
        [HideInInspector] public Advertising adverts;
        [HideInInspector] public MainMode modes;
        [HideInInspector] public Text highScore, title;
        [HideInInspector] public static NumberPool numberPool;
        [HideInInspector] public static ExplosionPool explosionPool;

        // Variables used to control pace of the game - set in inspector while testing
        [SerializeField] private float padding;
        [SerializeField] private float timeBetweenNumbers;
        [SerializeField] private float numberSpeed;

        public float Padding { get { return padding; } }
        public float TimeBetweenNumbers { get { return timeBetweenNumbers; } }
        public float NumberSpeed { get { return numberSpeed; } }

        // Data holders
        private readonly Data data = new Data();

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
            NumberOfTypes
        }

        public enum English
        {
            Common,
            AusAnthem,
            NumberOfTypes
        }

        private void Start() {
            numberPool.CreatePool();
            explosionPool.CreatePool();

            if(Preferences.ShowTutorial)
                Tutorial.Instance.RunMenuTutorial();
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
            numberPool.Reset();
            explosionPool.Reset();
            game.End();
            SceneManager.UnloadSceneAsync("Game");
        }
    }
}
