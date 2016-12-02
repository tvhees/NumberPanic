using UnityEngine;
using UnityEngine.EventSystems;
using _scripts.Controller;
using _scripts.View;

namespace _scripts.Model
{
    public class Number : MonoBehaviour, IPointerDownHandler {
        private float speed;
        private int value;
        [SerializeField] TextMesh text;
        [SerializeField] ParticleSystem trail;
        [SerializeField] BoxCollider2D boxCollider;

        private Game game;
        private ParticleSystem.EmissionModule em;
        private ParticleSystem.ShapeModule sh;
        private Spawner spawner;
        private FaceValue fV;

        public void Init(int currentIn, Vector3 startPos, float speedIn, Spawner scriptIn) {
            game = Manager.Instance.game;

            transform.position = startPos;

            float randomFactor = Random.Range(0.8f, 1.2f);
            spawner = scriptIn;
            value = currentIn;

            // Call the game function to create a FaceValue struct, get the appropriate return
            fV = game.GetFaceValue(value);
            if (fV.Text != null)
            {
                text.text = fV.Text;
                boxCollider.size = new Vector2(fV.Text.Length, boxCollider.size.y);
            }

            speed = speedIn/randomFactor;

            // Modifying particle trail
            sh = trail.shape;
            sh.radius = 0.01f * speed;
            em = trail.emission;
            em.rateOverDistance = new ParticleSystem.MinMaxCurve(speed);
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

            if(!spawner.gameCam)
                Manager.numberPool.ReturnObject(gameObject);
            else if (transform.position.y < spawner.gameCam.ViewportToWorldPoint(Vector3.zero).y)
            {
                var colour = game.ResolveNumber(value);
                DestroyThis(colour);
            }
        }
    }
}
