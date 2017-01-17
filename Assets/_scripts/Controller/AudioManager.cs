using DG.Tweening;
using UnityEngine;

namespace Controller
{
    public class AudioManager : MonoBehaviour
    {

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource effectsSource;
        private AudioClip titleMusic;
        private AudioClip[] correctClips;
        private AudioClip[] incorrectClips;

        public void Awake()
        {
            Manager.Instance.audioManager = this;
            titleMusic = Resources.Load<AudioClip>("audio/TitleMusic");
            correctClips = Resources.LoadAll<AudioClip>("audio/correct/");
            incorrectClips = Resources.LoadAll<AudioClip>("audio/incorrect/");
            PlayTitleMusic();
        }

        public void PlayTitleMusic()
        {
            musicSource.clip = titleMusic;
            musicSource.volume = 0;
            musicSource.Play();
            musicSource.DOFade(0.5f, 10f);
        }

        public void StopTitleMusic()
        {
            musicSource.DOFade(0f, 1f)
                .OnComplete(() => musicSource.Stop());
        }

        public void PlayPositiveSound()
        {
            var clip = correctClips.Random();
            effectsSource.PlayOneShot(clip);
        }

        public void PlayNegativeSound()
        {
            var clip = incorrectClips.Random();
            effectsSource.PlayOneShot(clip);
        }
    }

    public static class AudioExtension
    {
        public static AudioClip Random(this AudioClip[] clips)
        {
            var i = UnityEngine.Random.Range(0, clips.Length);
            return clips[i];
        }
    }
}
