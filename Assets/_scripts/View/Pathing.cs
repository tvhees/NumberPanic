using System;
using System.Collections;
using UnityEngine;

namespace View
{
    public class Pathing : MonoBehaviour
    {
        [SerializeField] private Vector3[] localNodes;
        [SerializeField] private float speed;
        private Vector3 origin;
        private int i;

        private void Awake()
        {
            StartCoroutine(Movement());
        }

        private IEnumerator Movement()
        {
            while(true)
            {
                MoveToNode(i++);
                yield return new WaitForSeconds(1 / speed);
            }
        }

        public void SetOrigin(Vector3 origin)
        {
            this.origin = origin;
            i = 0;
        }

        private void MoveToNode(int i)
        {
            var index = (int)Mathf.Repeat(i, localNodes.Length);
            transform.position = origin + localNodes[index];
        }

    }
}