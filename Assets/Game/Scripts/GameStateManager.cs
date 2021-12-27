using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    GameState gameState = GameState.Play;

    enum GameState
    {
        Play,
        Pause,
        End
    }


    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Debug.Log("PauseButton");
            switch(gameState)
            {
                case GameState.Play:
                    PauseGame();
                    break;
                case GameState.Pause:
                    ResumeGame();
                    break;
                case GameState.End:
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

}
