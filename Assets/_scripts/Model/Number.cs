using System;
using Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using View;
using Random = UnityEngine.Random;

namespace Model
{
    public class Number : MonoBehaviour, IPointerDownHandler {

        public static readonly UnityEvent OnCorrectNumberTouch = new UnityEvent();

        private const float BaseCharacterSize = 0.1f;
        private int value;
        [SerializeField] private TextMesh text;
        [SerializeField] private ParticleSystem trail;
        [SerializeField] private BoxCollider2D boxCollider;

        private Game game;
        private MovingObject movementModule;
        private ParticleSystem.EmissionModule em;
        private ParticleSystem.ShapeModule sh;
        private Spawner spawner;
        private NumberPool homePool;
        [SerializeField] private FaceValue fV;

        public void Init(int currentIn, float speedIn, Spawner scriptIn, NumberPool homePool)
        {
            movementModule = GetComponent<MovingObject>();
            game = MainManager.Instance.game;
            var randomFactor = Random.Range(0.8f, 1.2f);
            spawner = scriptIn;
            value = currentIn;
            this.homePool = homePool;
            OnCorrectNumberTouch.AddListener(DestroyIfCurrentValue);

            // Call the game function to create a FaceValue struct, get the appropriate return
            fV = game.GetFaceValue(value);
            if (fV.Text != null)
            {
                text.text = fV.Text;
                text.characterSize = Number.BaseCharacterSize * randomFactor;
                text.color = homePool.colour;
                boxCollider.size = new Vector2(fV.Text.Length * 0.8f, boxCollider.size.y);
            }

            // Spawn from the top of the screen
            // Calculate horizontal bounds for which collider won't be outside camera view.
            var halfWidth = new Vector3(boxCollider.bounds.extents.x, 0f, 0f);
            var leftBound = spawner.GameCam.ViewportToWorldPoint(new Vector3(0f, 1f, homePool.transform.position.z));
            var rightBound = spawner.GameCam.ViewportToWorldPoint(new Vector3(1f, 1f, homePool.transform.position.z));
            var x = Random.value;
            transform.position = (1 - x) * (leftBound + halfWidth) + x * (rightBound - halfWidth);

            movementModule.Speed = speedIn/randomFactor;

            // Modifying particle trail
            sh = trail.shape;
            sh.radius = 0.01f * movementModule.Speed;
            em = trail.emission;
            em.rateOverDistance = new ParticleSystem.MinMaxCurve(movementModule.Speed);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!game.IsInPlayState)
                return;

            var color = GetExplosionColor(true);
            if(IsCurrentValue)
                OnCorrectNumberTouch.Invoke();
            else
                DestroyThis(color);

            game.ProcessGameEvent(IsCurrentValue, spawner.GameCam);
        }

        public Color GetExplosionColor(bool touched = false)
        {
            if (touched)
                return IsCurrentValue ? CorrectNumberColor : IncorrectNumberColor;

            if (IsCurrentValue && MainManager.Current > 0) // if we should have touched this number
                return IncorrectNumberColor;

            return Color.white;
        }

        public bool IsCurrentValue
        {
            get { return fV == game.GetFaceValue(); }
        }

        private Color IncorrectNumberColor
        {
            get { return Color.red; }
        }

        private Color CorrectNumberColor
        {
            get { return fV.Value > game.HighScore ? Color.green : Color.yellow; }
        }

        private void DestroyIfCurrentValue()
        {
            if(IsCurrentValue)
                DestroyThis(CorrectNumberColor);
        }

        private void DestroyThis(Color colour)
        {
            var explosion = MainManager.explosionPool.GetObject();

            if (explosion != null)
                explosion.GetComponent<Explosion>().Init(transform.position, movementModule.Speed, colour);
            else
                Debug.Log("No explosions left, returning null");

            homePool.ReturnObject(gameObject);
        }

        private void OnDisable()
        {
            OnCorrectNumberTouch.RemoveListener(DestroyIfCurrentValue);
        }

        #region Update

        /// <summary>
        /// Move numbers downwards each frame, toggle tutorial symbols and destroy
        /// </summary>
        private void Update() {
            DestroyIfOutOfView();
        }

        private void DestroyIfOutOfView()
        {
            if (!spawner.GameCam)
                homePool.ReturnObject(gameObject);
            else if (transform.position.y < spawner.GameCam.ViewportToWorldPoint(Vector3.zero).y)
            {
                var colour = GetExplosionColor();
                DestroyThis(colour);
                if(IsCurrentValue)
                    game.ProcessGameEvent(false, spawner.GameCam);
            }
        }
    }

    #endregion Update
}
