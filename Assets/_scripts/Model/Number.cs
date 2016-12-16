using System;
using Assets._scripts.Controller;
using Assets._scripts.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using _scripts.Controller;
using _scripts.View;
using Random = UnityEngine.Random;

namespace Assets._scripts.Model
{
    public class Number : MonoBehaviour, IPointerDownHandler {

        public static readonly UnityEvent OnCorrectNumberTouch = new UnityEvent();

        private float speed;
        private int value;
        [SerializeField] private TextMesh text;
        [SerializeField] private ParticleSystem trail;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private Flickering arrow;
        [SerializeField] private Flickering cross;

        private Game game;
        private ParticleSystem.EmissionModule em;
        private ParticleSystem.ShapeModule sh;
        private Spawner spawner;
        [SerializeField] private FaceValue fV;

        public void Init(int currentIn, Vector3 startPos, float speedIn, Spawner scriptIn) {
            game = Manager.Instance.game;
            transform.position = startPos;
            var randomFactor = Random.Range(0.8f, 1.2f);
            spawner = scriptIn;
            value = currentIn;
            OnCorrectNumberTouch.AddListener(DestroyIfCurrentValue);

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
            if (!game.IsInPlayState)
                return;

            var color = GetExplosionColor(true);
            if(IsCurrentValue)
                OnCorrectNumberTouch.Invoke();
            else
                DestroyThis(color);

            game.ProcessGameEvent(IsCurrentValue);
        }

        public Color GetExplosionColor(bool touched = false)
        {
            if (touched)
                return IsCurrentValue ? CorrectNumberColor : IncorrectNumberColor;

            if (IsCurrentValue && Manager.Current > 0) // if we should have touched this number
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
            var explosion = Manager.explosionPool.GetObject();

            if (explosion != null)
                explosion.GetComponent<Explosion>().Init(transform.position, speed, colour);
            else
                Debug.Log("No explosions left, returning null");

            Manager.numberPool.ReturnObject(gameObject);
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
            transform.Translate(speed * Vector3.down * Time.deltaTime);
            DestroyIfOutOfView();
            //ToggleTutorialSymbols();
        }

        private void DestroyIfOutOfView()
        {
            if (!spawner.GameCam)
                Manager.numberPool.ReturnObject(gameObject);
            else if (transform.position.y < spawner.GameCam.ViewportToWorldPoint(Vector3.zero).y)
            {
                var colour = GetExplosionColor();
                DestroyThis(colour);
                if(IsCurrentValue)
                    game.ProcessGameEvent(false);
            }
        }

        private void ToggleTutorialSymbols()
        {
            if(Preferences.ShowTutorial)
            {
                if (IsCurrentValue)
                {
                    arrow.gameObject.SetActive(true);
                    cross.gameObject.SetActive(false);
                }
                else
                {
                    arrow.gameObject.SetActive(false);
                    cross.gameObject.SetActive(true);
                }
            }
            else
            {
                arrow.gameObject.SetActive(false);
                cross.gameObject.SetActive(false);
            }
        }
    }

    #endregion Update
}
