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

    public void Init(int currentIn, Vector3 startPos, float speedIn, bool realIn, Spawner scriptIn) {
        game = Manager.Instance.game;

        transform.position = startPos;

        float randomFactor = Random.Range(0.8f, 1.2f);
        spawner = scriptIn;

        value = currentIn;
        real = realIn;

        int offset = 0;
        if (!real)
            offset = (int)Random.Range(-0.1f, 0.1f) * game.GetNumber(value, Manager.mode, Manager.subValue);

        text.text = (game.GetNumber(value, Manager.mode, Manager.subValue) + offset).ToString();
        speed = speedIn/randomFactor;
        transform.localScale = randomFactor * Vector3.one;

        // Modifying particle trail
        sh = trail.shape;
        sh.radius = 0.01f * speed;
        em = trail.emission;
        em.rate = new ParticleSystem.MinMaxCurve(speed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        switch (game.state)
        {
            case Game.State.ATTRACT:
            case Game.State.PLAY:
            case Game.State.CRITICAL:
                Color colour = game.ResolveNumber(value, true);
                DestroyThis(colour);
                break;
        }
    }

    void DestroyThis(Color colour)
    {
        GameObject expl = Manager.explosionPool.GetObject();

        if (expl != null)
            expl.GetComponent<Explosion>().Init(transform.position, speed, colour);
        else
            Debug.Log("No explosions left, returning null");

        Manager.numberPool.ReturnObject(gameObject);
    }

    void Update() {
        transform.Translate(speed * Vector3.down * Time.deltaTime);

        if (transform.position.y < spawner.gameCam.ViewportToWorldPoint(Vector3.zero).y)
        {
            Color colour = game.ResolveNumber(value);
            DestroyThis(colour);
        }
    }
}
