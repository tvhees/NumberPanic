using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Manager : Singleton<Manager> {

    
    [HideInInspector] public UIManager ui;
    [HideInInspector] public Game game;
    [HideInInspector] public Spawner spawner;
    [HideInInspector] public Advertising adverts;
    [HideInInspector] public MainMode modes;
    [HideInInspector] public Text highScore, title;
    [HideInInspector] public Animator titleAnimator;

    // Variables used to control pace of the game - set in inspector while testing
    [SerializeField]
    private float setPadding = 25;
    public static float padding;

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

    void Awake() {
        padding = setPadding;

        data.AssignArrays();

        Preferences.Instance.Load();
        StartCoroutine(LoadGame());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void Restart() {
        StartCoroutine(DestroyGame());
    }

    IEnumerator LoadGame() {
        current = 0;

        game = ScriptableObject.CreateInstance("Game") as Game;

        if (!SceneManager.GetSceneByName("Game").isLoaded)
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.01f);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
    }

    IEnumerator DestroyGame() {
        yield return new WaitForSeconds(1.2f);
        Time.timeScale = 1f;
        SceneManager.UnloadScene("Game");
        //titleAnimator.SetTrigger("fade");
        Destroy(game);

        // Show an ad at regular intervals
        adverts.RegularAd();
        StartCoroutine(LoadGame());
    }
}
