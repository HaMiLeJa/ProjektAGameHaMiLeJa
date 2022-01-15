using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
        private GameSceneManager _gameSceneManager;
        void Awake()
        {
                _gameSceneManager = FindObjectOfType<GameSceneManager>();
        }
        public void ButtonLoadGame()
        {
                _gameSceneManager.LoadGame();
        }
      
}
