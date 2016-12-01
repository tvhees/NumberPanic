using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Spawner : MonoBehaviour
{

    public GameObject number;
    public Camera gameCam;
    [HideInInspector] public bool spawn;
    [HideInInspector] public float leftBound, rightBound;

    private float padding, waitFactor, speedFactor;

    void Awake() {
        Manager.Instance.spawner = this;
    }

    public void Init()
    {
        leftBound = 0.2f;
        rightBound = 0.95f;
        padding = Manager.padding;
        waitFactor = Manager.waitFactor;
        speedFactor = Manager.speedFactor;
        spawn = true;
        StartCoroutine(RegularSpawn());
    }

    IEnumerator RegularSpawn() {
        int counter = 0;

        while (spawn)
        {
            switch (Manager.Instance.game.state)
            {
                case Game.State.ATTRACT:
                case Game.State.PLAY:
                case Game.State.CRITICAL:
                case Game.State.END:
                case Game.State.SCORE:
                    float waitTime = Mathf.Pow(padding / (Manager.current + padding), 2f) * waitFactor;

                    if (counter == 0)
                    {
                        // Get a new 'true' number from the pool. Reset the counter only if this works.
                        if (SpawnNumber())
                            counter = Mathf.Min(Random.Range(0, Manager.current - 1), 8);
                    }
                    else
                    {
                        // Get a new 'false' number from the pool. Decrement the counter only if this works.
                        if (SpawnNumber(false))
                            counter--;
                    }

                    yield return new WaitForSeconds(waitTime);

                    break;
            }

            yield return null;

        }

    }

    private bool SpawnNumber(bool real = true) {
        // Spawn from the top of the screen - recalculate every time in case orientation changes
        Vector3 position = gameCam.ViewportToWorldPoint(new Vector3(Random.Range(leftBound, rightBound), 1f, 20f));

        // Speed to travel downwards, damped increase as numbers increase
        float speed = (Manager.current + Manager.padding) * speedFactor / Manager.padding;

        // Default value is the next one player needs to collect
        int value = Manager.current;

        // For 'fake' numbers we want to vary the value a little bit
        if (!real)
        {
            for (int i = 0; i < 20; i++)
            {
                value = (int)Random.Range(0.3f * Manager.current, 1.7f * Manager.current);

                if (Mathf.Abs(value - Manager.current) > 1 && value > 0)
                    break;
            }
        }

        GameObject obj = Manager.numberPool.GetObject();
        if (obj != null)
        {
            obj.GetComponent<Number>().Init(value, position, speed, this);
            return true;
        }

        Debug.Log("No numbers left to use, returning false");
        return false;
    }
}
