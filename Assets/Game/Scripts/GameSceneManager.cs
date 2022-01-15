using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameSceneManager : MonoBehaviour
{
   public static GameSceneManager instance;
   [SerializeField] private GameObject loadingScreen;
   public TextMeshProUGUI tipsText;
 public CanvasGroup alphaCanvas;
 public string[] tips;
 public TextMeshProUGUI funnyMessagesText;
 public string[] funnyMessages;
 public Image progressbar;
 public TextMeshProUGUI loadingText;
 private float target;
   private void Awake()
   {
      if (instance = null)
      {
         instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else if (instance !=null)
      {
         Destroy(gameObject);
      }
        
      
      UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
   }
   
   private List<AsyncOperation> hasAllTheScenesLoading = new List<AsyncOperation>();
   public void LoadGame()
   {
      target = 0;
      progressbar.fillAmount = 0;
      loadingScreen.gameObject.SetActive(true);
      StartCoroutine(GenerateTips());
      hasAllTheScenesLoading.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
      hasAllTheScenesLoading.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int) SceneIndexes.MAINGAME, LoadSceneMode.Additive));
      
      StartCoroutine(GetSceneLoadProgress());
      StartCoroutine (WaitForSceneLoad (SceneManager.GetSceneByBuildIndex((int)SceneIndexes.MAINGAME )));
   }

   public IEnumerator WaitForSceneLoad(Scene scene)
{
   while(!scene.isLoaded)
   {
      yield return null;  
   }
   Debug.Log("Setting active scene..");
   SceneManager.SetActiveScene (scene);
   //unloads the persistant scene
   //hasAllTheScenesLoading.Add(UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int)SceneIndexes.MANAGER));
}

private float totalSceneProgress;
   public IEnumerator GetSceneLoadProgress()
   {
      for (int i = 0; i < hasAllTheScenesLoading.Count; i++)
      {
         while (!hasAllTheScenesLoading[i].isDone)
         {
            totalSceneProgress = 0;
            foreach (AsyncOperation operation in hasAllTheScenesLoading)
            {
               totalSceneProgress += operation.progress;
            }
            // *99 instead of *100  because a loading bar at 99% looks better 
            totalSceneProgress = Mathf.Clamp01((totalSceneProgress / hasAllTheScenesLoading.Count) / 0.9f) * 99f;
          target = Mathf.RoundToInt(totalSceneProgress);
         loadingText.text = (Mathf.RoundToInt(totalSceneProgress)).ToString();
            yield return null;
         }
      }
      
      loadingScreen.gameObject.SetActive(false);
   }

   void Update()
   {
      progressbar.fillAmount = Mathf.MoveTowards(progressbar.fillAmount, target, 10 * Time.deltaTime);
   }
   [HideInInspector]public int tipCount;
  [HideInInspector] public int funnyMessagesCount;
   public IEnumerator GenerateTips()
   {
      tipCount = Random.Range(0, tips.Length);
      tipsText.text = tips[tipCount];
      funnyMessagesCount = Random.Range(0, funnyMessages.Length);
      funnyMessagesText.text = funnyMessages[funnyMessagesCount];
      
      while (loadingScreen.activeInHierarchy)
      {
         yield return new WaitForSeconds(0.5f);
         
         tipCount++;
         funnyMessagesCount++;
         
         if (funnyMessagesCount >= funnyMessages.Length)
         {
            funnyMessagesCount = 0;
         }
         
         if (tipCount >= tips.Length)
         {
            tipCount = 0;
         }

         tipsText.text = tips[tipCount];
         funnyMessagesText.text = funnyMessages[funnyMessagesCount];
      }
   }
}

