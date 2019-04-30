using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Serializers;
using RedRunner.Characters;
using RedRunner.TerrainGeneration;

namespace RedRunner
{
    public sealed class GameManager : MonoBehaviour
    {
        public delegate void AudioEnabledHandler(bool active);

        public delegate void ScoreHandler(float newScore, float highScore, float lastScore);

        public delegate void ResetHandler();

        public static event ResetHandler OnReset;
        public static event ScoreHandler OnScoreChanged;
        public static event AudioEnabledHandler OnAudioEnabled;

        public static GameManager Singleton { get; private set; }

        [SerializeField] private Character m_MainCharacter;
        [SerializeField] [TextArea(3, 30)] private string m_ShareText;
        [SerializeField] private string m_ShareUrl;
        private float m_StartScoreX = 0f;
        private float m_HighScore = 0f;
        private float m_LastScore = 0f;
        private float m_Score = 0f;

        /// <summary>
        /// This is my developed callbacks compoents, because callbacks are so dangerous to use we need something that automate the sub/unsub to functions
        /// with this in-house developed callbacks feature, we garantee that the callback will be removed when we don't need it.
        /// </summary>
        public Property<int> m_Coin = new Property<int>(0);

        #region Getters

        public bool gameStarted { get; private set; } = false;

        public bool gameRunning { get; private set; } = false;

        public bool audioEnabled { get; private set; } = true;

        #endregion

        void Awake()
        {
            if (Singleton != null)
            {
                Destroy(gameObject);
                return;
            }

            SaveGame.Serializer = new SaveGameBinarySerializer();
            Singleton = this;
            m_Score = 0f;

            m_Coin.Value = SaveGame.Exists("coin") ? SaveGame.Load<int>("coin") : 0;
            SetAudioEnabled(!SaveGame.Exists("audioEnabled") || SaveGame.Load<bool>("audioEnabled"));
            m_LastScore = SaveGame.Exists("lastScore") ? SaveGame.Load<float>("lastScore") : 0f;
            m_HighScore = SaveGame.Exists("highScore") ? SaveGame.Load<float>("highScore") : 0f;
        }

        void UpdateDeathEvent(bool isDead)
        {
            if (isDead)
            {
                StartCoroutine(DeathCrt());
            }
            else
            {
                StopCoroutine("DeathCrt");
            }
        }

        IEnumerator DeathCrt()
        {
            m_LastScore = m_Score;
            if (m_Score > m_HighScore)
            {
                m_HighScore = m_Score;
            }

            OnScoreChanged?.Invoke(m_Score, m_HighScore, m_LastScore);

            yield return new WaitForSecondsRealtime(1.5f);

            EndGame();
            var endScreen = UIManager.Singleton.UISCREENS.Find(el => el.ScreenInfo == UIScreenInfo.END_SCREEN);
            UIManager.Singleton.OpenScreen(endScreen);
        }

        private void Start()
        {
            m_MainCharacter.IsDead.AddEventAndFire(UpdateDeathEvent, this);
            m_StartScoreX = m_MainCharacter.transform.position.x;
            Init();
        }

        public void Init()
        {
            EndGame();
            UIManager.Singleton.Init();
            StartCoroutine(Load());
        }

        void Update()
        {
            if (gameRunning)
            {
                if (m_MainCharacter.transform.position.x > m_StartScoreX &&
                    m_MainCharacter.transform.position.x > m_Score)
                {
                    m_Score = m_MainCharacter.transform.position.x;
                    OnScoreChanged?.Invoke(m_Score, m_HighScore, m_LastScore);
                }
            }
        }

        IEnumerator Load()
        {
            var startScreen = UIManager.Singleton.UISCREENS.Find(el => el.ScreenInfo == UIScreenInfo.START_SCREEN);
            yield return new WaitForSecondsRealtime(3f);
            UIManager.Singleton.OpenScreen(startScreen);
        }

        void OnApplicationQuit()
        {
            if (m_Score > m_HighScore)
            {
                m_HighScore = m_Score;
            }

            SaveGame.Save<int>("coin", m_Coin.Value);
            SaveGame.Save<float>("lastScore", m_Score);
            SaveGame.Save<float>("highScore", m_HighScore);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void ToggleAudioEnabled()
        {
            SetAudioEnabled(!audioEnabled);
        }

        public void SetAudioEnabled(bool active)
        {
            audioEnabled = active;
            AudioListener.volume = active ? 1f : 0f;
            OnAudioEnabled?.Invoke(active);
        }

        public void StartGame()
        {
            gameStarted = true;
            ResumeGame();
        }

        public void StopGame()
        {
            gameRunning = false;
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            gameRunning = true;
            Time.timeScale = 1f;
        }

        public void EndGame()
        {
            gameStarted = false;
            StopGame();
        }

        public void RespawnMainCharacter()
        {
            RespawnCharacter(m_MainCharacter);
        }

        public void RespawnCharacter(Character character)
        {
            Block block = TerrainGenerator.Singleton.GetCharacterBlock();
            if (block != null)
            {
                Vector3 position = block.transform.position;
                position.y += 2.56f;
                position.x += 1.28f;
                character.transform.position = position;
                character.Reset();
            }
        }

        public void Reset()
        {
            m_Score = 0f;
            OnReset?.Invoke();
        }

        public void ShareOnTwitter()
        {
            Share("https://twitter.com/intent/tweet?text={0}&url={1}");
        }

        public void ShareOnGooglePlus()
        {
            Share("https://plus.google.com/share?text={0}&href={1}");
        }

        public void ShareOnFacebook()
        {
            Share("https://www.facebook.com/sharer/sharer.php?u={1}");
        }

        public void Share(string url)
        {
            Application.OpenURL(string.Format(url, m_ShareText, m_ShareUrl));
        }

        [System.Serializable]
        public class LoadEvent : UnityEvent
        {
        }
    }
}
