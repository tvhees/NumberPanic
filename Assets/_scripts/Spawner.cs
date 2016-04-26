using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Spawner : MonoBehaviour
{

    public int current;
    public float padding, waitFactor, speedFactor;
    public GameObject number;
    public Camera gameCam;
    public Score score;
    public bool spawn;
    public float leftBound, rightBound;

    void Awake() {
        Manager.Instance.game.spawner = this;
        leftBound = 0.2f;
        rightBound = 0.95f;
        padding = Manager.Instance.padding;
        spawn = true;
        StartCoroutine(RegularSpawn());
    }

    IEnumerator RegularSpawn() {
        int counter = 0;

        while (spawn)
        {
            float waitTime = Mathf.Pow(padding/(current + padding), 2f) * waitFactor;

            if (counter == 0)
            {
                counter = Mathf.Min(Random.Range(0, current - 1), 8);
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
        float speed = (current + padding) * speedFactor / padding;

        int value = current;

        if (!real)
        {
            while (Mathf.Abs(value - current) < 3)
                value = Random.Range(Mathf.Max(current - 50, 0), current + 50);
        }
//        else
  //          current++;

        GameObject obj = Instantiate(number, position, Quaternion.identity) as GameObject;
        obj.GetComponent<Number>().Init(value, speed, this);
    }
}
