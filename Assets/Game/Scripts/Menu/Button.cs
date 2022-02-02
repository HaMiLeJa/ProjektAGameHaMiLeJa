using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private GameSceneManager _gameSceneManager;
    [SerializeField] AudioSource MenuBaseLineAudioSource;

    void Awake()
    {
            _gameSceneManager = FindObjectOfType<GameSceneManager>();
    }
    public void ButtonLoadGame()
    {
        
        //_gameSceneManager.LoadGame();
        StartCoroutine(DecreaseMusicVolume());
    
    }

    IEnumerator DecreaseMusicVolume()
    {
        float startVolume = MenuBaseLineAudioSource.volume;
        float timer = 0;

        while(timer <= 0.7f)
        {
            timer += Time.deltaTime;
           

            MenuBaseLineAudioSource.volume = MenuBaseLineAudioSource.volume * 0.8f;
            Debug.Log("Decreasing");
            yield return new WaitForFixedUpdate();
            
        }

        _gameSceneManager.LoadGame();
        yield return null;
    }
      
}
