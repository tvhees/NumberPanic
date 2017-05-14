using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public interface IObjectPool
    {
        void CreatePool();
        GameObject GetObject();
        void ReturnObject(GameObject obj);
        void Reset();
    }

    public abstract class ObjectPool : MonoBehaviour, IObjectPool
    {

        public GameObject ObjectPrefab;
        protected int PoolSize;
        protected Vector3 HomePosition;
        protected List<GameObject> Available, InUse;

        protected virtual void Awake() {}

        public void CreatePool() {
            Available = new List<GameObject>();
            InUse = new List<GameObject>();

            for (var i = 0; i < PoolSize; i++){
                var obj = Instantiate(ObjectPrefab);
                obj.transform.SetParent(transform);
                obj.transform.position = HomePosition;
                obj.SetActive (false);
                Available.Add(obj);
            }
        }

        public GameObject GetObject()
        {
            if (Available.Count <= 0) return null;

            var obj = Available [0];
            Available.Remove(obj);
            InUse.Add(obj);
            PoolSize = Available.Count;
            obj.SetActive(true);
            return obj;
        }

        public void ReturnObject(GameObject obj)
        {
            Available.Add(obj);
            InUse.Remove(obj);
            obj.transform.SetParent(transform);
            obj.transform.position = HomePosition;
            obj.transform.localScale = Vector3.one;
            obj.SetActive(false);
        }

        public void Reset()
        {
            while(InUse.Count > 0)
                ReturnObject(InUse[0]);
        }
    }
}
