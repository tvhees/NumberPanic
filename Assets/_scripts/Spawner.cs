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
            float waitTime = Mathf.Pow(padding/(Manager.current + padding), 2f) * waitFactor;

            if (counter == 0)
            {
                counter = Mathf.Min(Random.Range(0, Manager.current - 1), 8);
                SpawnNumber();
            }
            else
            {
                counter--;
                SpawnNumber(false);
            }

            yield return new WaitForSeconds(waitTime);
        }

    }

    void SpawnNumber(bool real = true) {
        Vector3 position = gameCam.ViewportToWorldPoint(new Vector3(Random.Range(leftBound, rightBound), 1f, 20f));
        float speed = (Manager.current + Manager.padding) * speedFactor / Manager.padding;

        int value = Manager.current;

        if (!real)
        {
            for (int i = 0; i < 20; i++)
            {
                value = (int)Random.Range(0.3f * Manager.current, 1.7f * Manager.current);

                if (Mathf.Abs(value - Manager.current) > 1 && value > 0)
                    break;
            }
        }

        GameObject obj = Instantiate(number, position, Quaternion.identity) as GameObject;
        obj.GetComponent<Number>().Init(value, speed, real, this);
    }
}
