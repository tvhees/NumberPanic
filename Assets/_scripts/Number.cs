using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Number : MonoBehaviour, IPointerDownHandler {

    public float speed;
    public int value;
    public TextMesh text;
    public GameObject explosion;
    public ParticleSystem trail;

    bool real;

    private Game game;
    private ParticleSystem.EmissionModule em;
    private ParticleSystem.ShapeModule sh;
    private Spawner spawner;

    public void Init(int currentIn, float speedIn, bool realIn, Spawner scriptIn) {
        game = Manager.Instance.game;

        float randomFactor = Random.Range(0.8f, 1.2f);
        spawner = scriptIn;

        value = currentIn;
        text.text = game.GetNumber(value).ToString();
        speed = speedIn/randomFactor;
        real = realIn;
        transform.localScale = randomFactor * Vector3.one;

        // Modifying particle trail
        sh = trail.shape;
        sh.radius = 0.01f * speed;
        em = trail.emission;
        em.rate = new ParticleSystem.MinMaxCurve(speed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Color colour = game.ResolveNumber(value, true);
        DestroyThis(colour);
    }

    void DestroyThis(Color colour)
    {
        GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
        expl.GetComponent<Explosion>().Init(speed, colour);
        Destroy(gameObject);
    }

    void Update() {
        transform.Translate(speed * Vector3.down * Time.deltaTime);

        if (transform.position.y < spawner.gameCam.ViewportToWorldPoint(Vector3.zero).y)
        {
            Color colour = game.ResolveNumber(value);
            DestroyThis(colour);
        }

        if (!real && value == Manager.current)
            DestroyThis(Color.cyan);
    }
}
