using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Number : MonoBehaviour, IPointerDownHandler {

    public float speed;
    public int value;
    public TextMesh text;
    public GameObject explosion;
    public ParticleSystem trail;

    private ParticleSystem.EmissionModule em;
    private ParticleSystem.ShapeModule sh;
    private Spawner spawner;

    public void Init(int valueIn, float speedIn, Spawner scriptIn) {
        float randomFactor = Random.Range(0.8f, 1.2f);
        spawner = scriptIn;

        value = valueIn;
        text.text = valueIn.ToString();
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
        Color colour = Manager.Instance.game.ResolveNumber(value, true);
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
            Color colour = Manager.Instance.game.ResolveNumber(value);
            DestroyThis(colour);
        }
    }
}
