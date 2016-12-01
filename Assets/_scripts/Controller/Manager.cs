using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Manager : Singleton<Manager> {

    public bool timeAttackMode;
    public GameTimer gameTimer { get; private set; }

    [HideInInspector] public UIManager ui;
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
    public static float padding;
    public static float waitFactor;
    public static float speedFactor;

    // Data holders
    public Data data = new Data();

    // Game state variables
    public static Mode mode;
    public static int current;

    // Enums
    // Main modes
    public enum Mode
    {
        linear,
        power,
        sequence,
        english,
        NumberOfTypes
    }

    // Sub modes
    public static int subValue;
    public enum Sequence
    {
        primes,
        fibbonaci,
        NumberOfTypes
    }

    public enum English
    {
        common,
        aus_anthem,
        NumberOfTypes
    }

    void Start() {
        padding = setPadding;
        waitFactor = setWait;
        speedFactor = setSpeed;
        numberPool.CreatePool();
        explosionPool.CreatePool();
        data.AssignArrays();
        StartCoroutine(LoadGame(true));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (game != null)
            game.ProcessState();
    }

    public void Restart() {
        DestroyGame();
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame(bool initialize = false) {
        current = 0;

        game = new Game(mode, subValue);

        AsyncOperation async = null;

        if (!SceneManager.GetSceneByName("Game").isLoaded)
            async = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);

        int breakNo = 0;
        while (!async.isDone)
        {
            yield return null;
            breakNo++;
            if (breakNo > 1000)
            {
                Debug.Log("Breaking load loop");
                break;
            }
        }

        if (timeAttackMode && initialize == false)
            NewTimer();

        spawner.Init();
    }

    public void NewTimer()
    {
        gameTimer = new GameTimer();
    }

    void DestroyGame() {
        Time.timeScale = 1f;
        gameTimer = null;
        numberPool.Reset();
        explosionPool.Reset();
        game.End();
        SceneManager.UnloadScene("Game");
    }
}
