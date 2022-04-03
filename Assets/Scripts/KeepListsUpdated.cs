
using System.Collections.Generic;
using UnityEngine;
public class KeepListsUpdated : MonoBehaviour
{
    [SerializeField] private List<GameObject> SegmentsThatNeedRefresh;
 
    private void Awake()
    {
        foreach (GameObject Seg in SegmentsThatNeedRefresh)
        {
            Seg.SetActive(false);
            Seg.SetActive(true);
        }
    }
#if UNITY_EDITOR
    [NaughtyAttributes.Button()] private void UpdateAllPreCalculatedLists()
    {
     FindObjectOfType<Highlightmanager>().GetComponent<Highlightmanager>().UpdateAllMaterialIndexies();
     FindObjectOfType<HexAutoTiling>().GetComponent<HexAutoTiling>().FindAllTheHexesTransform();
     FindObjectOfType<ReferenceLibrary>().GetComponent<ReferenceLibrary>().FillNullHashsetRefLibary();
     FindObjectOfType<ReferenceLibrary>().GetComponent<ReferenceLibrary>().populateList();
     FindObjectOfType<HexUpdater>().GetComponent<HexUpdater>().updateParticle();
     FindObjectOfType<HexUpdater>().GetComponent<HexUpdater>().updateAudioClips();
     FindObjectOfType<HexEffectAudioManager>().GetComponent<HexEffectAudioManager>().getAllTheAudioSourcesBeforeAwake();
     FindObjectOfType<CollectableManager>().GetComponent<CollectableManager>().fillCollectableListsBeforeStart();
     UpdateCollider();
    }
    
    
    void UpdateCollider()
    {
        SegmentsThatNeedRefresh?.Clear();
        SegmentsThatNeedRefresh.AddRange(GameObject.FindGameObjectsWithTag("BrueckeParent"));
    }

#endif
  
}

