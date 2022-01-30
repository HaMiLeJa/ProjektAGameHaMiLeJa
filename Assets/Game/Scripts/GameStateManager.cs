using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    GameState gameState = GameState.Play;
    Rigidbody playerRb;

    enum GameState
    {
        Play,
        Pause,
        End
    }


    void Start()
    {
        playerRb = ReferenceLibary.RigidbodyPl;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            switch(gameState)
            {
                case GameState.Play:
                    PauseGame();
                    break;
                case GameState.Pause:
                    ResumeGame();
                    break;
                default:
                    break;
            }
                
        }

        if(gameState == GameState.Pause )
        {
            if(Input.GetButtonDown("X"))
            {

                SceneManager.LoadScene(1); 
                //timescale wieder auf 1?
            }
        }

        if(gameState == GameState.End)
        {
            if (Input.GetButtonDown("X"))
            {

                SceneManager.LoadScene(1); 
                //timescale wieder auf 1?
            }
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

    public void CheckForEndOfGame()
    {

        /*
        if (ReferenceLibary.PlayerMov.TotalVelocity < 10 && EndGameSafetyStarted == false)
        {
            EndGameSafetyStarted = true;
            EndGameSafetyCoroutine = StartCoroutine(EndGameSavety());
        }*/


        if (Mathf.Approximately(playerRb.velocity.x, 0) && Mathf.Approximately(playerRb.velocity.y, 0) && Mathf.Approximately(playerRb.velocity.z, 0))
        {

            gameState = GameState.End;
            CalculateEndOfGame();


        }
        else
        {
            // Debug.Log("CheckVelocity");
            return;
        }


    }

    void CalculateEndOfGame()
    {
        Debug.Log("GameOver");
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

            Debug.Log("no new highscore");
            StartCoroutine(playerDissolve.Coroutine_DisolveShield(1.1f));
        }
    }

    public IEnumerator EndGameSavety()
    {
        yield return new WaitForSeconds(10f);


        CalculateEndOfGame();

        yield return null;
    }
    #endregion
}
