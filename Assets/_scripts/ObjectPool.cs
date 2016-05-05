using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ObjectPool : MonoBehaviour {

    public GameObject prefab;
    protected int poolSize;
	protected Vector3 homePosition;
	protected List<GameObject> pool, checkedOut;

    void Awake() {
        Init();
    }

    protected virtual void Init() {

    }

	public void CreatePool(){
		pool = new List<GameObject>();
        checkedOut = new List<GameObject>();

        for (int i = 0; i < poolSize; i++){
			GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(transform);
			obj.transform.position = homePosition;
			obj.SetActive (false);
			pool.Add(obj);
		}
    }

	public GameObject GetObject()
    {
        if(pool.Count > 0){
			GameObject obj = pool [0];
            pool.Remove(obj);
            checkedOut.Add(obj);

            poolSize = pool.Count;
            obj.SetActive(true);

            return obj;
		}

		return null;
	}

	public void ReturnObject(GameObject obj)
    {
		pool.Add(obj);
        checkedOut.Remove(obj);
        obj.transform.SetParent(transform);
        obj.transform.position = homePosition;
        obj.transform.localScale = Vector3.one;
        obj.SetActive(false);
	}

    public void Reset()
    {
        while(checkedOut.Count > 0)
        {
            ReturnObject(checkedOut[0]);
        }

        if (Manager.Instance.debugMode)
            Debug.Log(this.name + " reset");
    }
}
