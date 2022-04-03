using UnityEngine;
public class Collectable : MonoBehaviour
{
    #region Inspector
    [SerializeField] private float rotation = 400;
    [SerializeField] public int CollectableIndexID;
    public ScriptableLevelObject settings;
   // public CollectableReferences colRef;
   #endregion
   void Update() => transform.Rotate(new Vector3(0, rotation * Time.deltaTime,0));
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(ReferenceLibrary.PlayerTag))
        {
            ScoreManager.OnScoring?.Invoke(settings.value);
            ReferenceLibrary.ColMng.CollectableCollected(this.gameObject, settings.secondValue,CollectableIndexID); //hier drin sind auch sounds;
        }
    }
}