using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    GameState gameState = GameState.Play;

    enum GameState
    {
        Play,
        Pause
    }


    void Start()
    {
        
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



    void EndOfGame()
    {
        Time.timeScale = 0;
    }
}
