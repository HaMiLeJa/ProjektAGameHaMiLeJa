using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameState gameState = GameState.Start;
    Rigidbody playerRb;
    List<GameObject> hasAlltheManagers = new List<GameObject>();
    [SerializeField] AudioSource myAudioSource;
    bool MainSceneLoadActive = false;
    public enum GameState
    {
        Start,
        Play,
        Pause,
        End
    }


    void Start()
    {
        MainSceneLoadActive = false;
        if (myAudioSource == null)
        {
            myAudioSource = this.GetComponent<AudioSource>();
        }
        GameOver = false;
        gameState = GameState.Start;
        playerRb = ReferenceLibary.RigidbodyPl;
        
    }

    void UpdateStartGame()
    {
        if (Input.GetButtonDown("A"))
        {
            myAudioSource.Play();
            gameState = GameState.Play;
            ReferenceLibary.UIMng.DeactivateStartOfGameUI();

            Vector3 random = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

            ReferenceLibary.RigidbodyPl.AddForce(random * 5);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (MainSceneLoadActive == true) return;

            switch (gameState)
            {
                case GameState.Play:
                    myAudioSource.Play();
                    PauseGame();
                    break;
                case GameState.Pause:
                    myAudioSource.Play();
                    ResumeGame();
                    break;
                default:
                    break;
            }
        }

        if(gameState == GameState.Start)
        {
            UpdateStartGame();
        }

        if(gameState == GameState.Pause )
        {
            if (MainSceneLoadActive == true) return;

            if (Input.GetButtonDown("B"))
            {
                myAudioSource.Play();
                MainSceneLoadActive = true;
                StartCoroutine(AdvancedDelayedSceneMenuSceneLoad());
                
            }
        }

        if(gameState == GameState.End)
        {
            if (MainSceneLoadActive == true) return;

            if (Input.GetButtonDown("B"))
            {
                MainSceneLoadActive = true;
                myAudioSource.Play();
                StartCoroutine(DelayedSceneMenuSceneLoad());
            }
        }

    }

    IEnumerator AdvancedDelayedSceneMenuSceneLoad()
    {
        Debug.Log("DelayedLoad1");
        Time.timeScale = 0.005f;
        yield return new WaitForSeconds(0.001f);
        
        Debug.Log("DelayedLoad2");
        loadMainScreen();
    }

    IEnumerator DelayedSceneMenuSceneLoad()
    {
        yield return new WaitForSeconds(0.3f);

        loadMainScreen();
    }

    void loadMainScreen()
    {
        // AddAllManagers();
        // DestroyAllTheManagers();
        // for (int j = 0; j < UnityEngine.SceneManagement.SceneManager.sceneCount; j++)
        // {
        //     foreach (GameObject allParentObjects in UnityEngine.SceneManagement.SceneManager.GetSceneAt(j)
        //         .GetRootGameObjects())
        //      
        //     {  if(allParentObjects != this.gameObject)
        //         Destroy(allParentObjects);
        //       
        //     }
        //     
        // }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // UnsubscribeEvents();

        SceneManager.UnloadSceneAsync((int)SceneIndexes.MAINGAME); 
      
        SceneManager.LoadScene((int)SceneIndexes.MANAGER); 
        
        Time.timeScale = 1;
        //Destroy(this);
    }

    void UnsubscribeEvents()
    {
        Debug.Log("Unsubscirbe Events");
        ScoreManager.OnScoring -= ReferenceLibary.ScoreMng.UpdateScore;
        ScoreManager.OnScoring -= ReferenceLibary.UIMng.UpdateUIScore;
        ScoreManager.OnScoring -= ReferenceLibary.UIMng.PointsStarter;


        ScoreManager.OnPermanentMultiplicatorUpdate -= ReferenceLibary.ScoreMng.UpdateMultiplicator;
        ScoreManager.OnPermanentMultiplicatorUpdate -= ReferenceLibary.UIMng.UpdateMultiplicatorUI;
        ScoreManager.OnPermanentMultiplicatorUpdate -= ReferenceLibary.UIMng.PermanentMulitplicatorStarter;

        ScoreManager.OnTemporaryMultiplicatorUpdate -= ReferenceLibary.ScoreMng.UpdateMultiplicator;
        ScoreManager.OnTemporaryMultiplicatorUpdate -= ReferenceLibary.UIMng.UpdateMultiplicatorUI;
        ScoreManager.OnTemporaryMultiplicatorUpdate -= ReferenceLibary.UIMng.UpdateTemporaryMultiplicator;

        CollectableManager.OnRespawnCollectables -= ReferenceLibary.ColMng.StartCollectableSpawn;
    }

    void AddAllManagers()
    {
     

      
        foreach (GameObject manager in GameObject.FindGameObjectsWithTag("Manager"))
        {
            hasAlltheManagers.Add(manager);
        }
    }

    void DestroyAllTheManagers()
    {
        foreach (GameObject hex in hasAlltheManagers)
        {
            Destroy(hex);
        }

       
    }

   void PauseGame()
   {
        ReferenceLibary.UIMng.IngameCanvas.SetActive(false);
        ReferenceLibary.UIMng.PauseCanvas.SetActive(true);

        Time.timeScale = 0;
        gameState = GameState.Pause;
   }

   void ResumeGame()
   {

        ReferenceLibary.UIMng.IngameCanvas.SetActive(true);
        ReferenceLibary.UIMng.PauseCanvas.SetActive(false);

        Time.timeScale = 1;

        gameState = GameState.Play;
    }



    #region EndOfGame

    Coroutine GameOverCoroutine;
    [SerializeField] Dissolve playerDissolve;
    public Coroutine EndGameSafetyCoroutine;
   // public bool EndGameSafetyStarted = false;
    public static bool GameOver = false;

    float velocityX = 0;
    float velocityZ = 0;
    float velocityY = 0;

    Vector3 velocityLastFrame;
    Vector3 velocitySecondToLastFrame;

    [Space]
    [SerializeField] AudioClip gameOverClip;
    [SerializeField] UnityEngine.Audio.AudioMixerGroup gameOverGroup;

    public void CheckForEndOfGame()
    {

        if (Mathf.Approximately(playerRb.velocity.x, 0) && Mathf.Approximately(playerRb.velocity.y, 0) && Mathf.Approximately(playerRb.velocity.z, 0))
        {
            if (GameOver == true) return;

            StopAllCoroutines();

            gameState = GameState.End;
            CalculateEndOfGame();


        }
        else // vergleichen der Velocity des vorherigen frames mit dem der aktuellen;
        {

            if (velocityLastFrame == playerRb.velocity)
            {
                if (velocityLastFrame == velocitySecondToLastFrame) return;
                
                StartCoroutine(EndGameSavety(velocityLastFrame));
            }
            else
            {
                StopAllCoroutines();
            }

            velocitySecondToLastFrame = velocityLastFrame;
            velocityLastFrame = playerRb.velocity;

            return;
        }

    }

    void CalculateEndOfGame()
    {
        Debug.Log("GameOver");
        ReferenceLibary.AudMng.PlayGameStateSound(gameOverClip, gameOverGroup);
        GameOver = true;

        if (ReferenceLibary.ScoreMng.CheckForNewHighscore() == true)
        {
            ReferenceLibary.ScoreMng.SetNewHighscore();

            if (GameOverCoroutine == null)
                GameOverCoroutine = StartCoroutine(ReferenceLibary.UIMng.GameOverNewHighscoreCoroutine());
            Debug.Log("new highscore");
            StartCoroutine(playerDissolve.Coroutine_DisolveShield(1.1f));
        }
        else
        {
            if (GameOverCoroutine == null)
                GameOverCoroutine = StartCoroutine(ReferenceLibary.UIMng.GameOverCoroutine());
            else Debug.Log("GameOverCoroutine not null, whyever");

            Debug.Log("no new highscore");
            StartCoroutine(playerDissolve.Coroutine_DisolveShield(1.1f));
        }
    }

    IEnumerator EndGameSavety(Vector3 lastVelocity)
    {

        yield return new WaitForSeconds(3f);
        if (lastVelocity == playerRb.velocity && GameOver == false)
        {
            GameOver = true;
        }
        else
            yield break;

        
        yield return new WaitForSeconds(2f);
        
        
        gameState = GameState.End;
        CalculateEndOfGame();
            
        

        yield return null;
    }
    #endregion
}
