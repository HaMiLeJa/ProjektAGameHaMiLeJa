#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
public class KeepListsUpdated : MonoBehaviour
{
    [NaughtyAttributes.Button()]
    private void UpdateAllPreCalculatedLists()
    {
     FindObjectOfType<Highlightmanager>().GetComponent<Highlightmanager>().UpdateAllMaterialIndexies();
     FindObjectOfType<HexAutoTiling>().GetComponent<HexAutoTiling>().FindAllTheHexesTransform();
     FindObjectOfType<ReferenceLibrary>().GetComponent<ReferenceLibrary>().FillNullHashsetRefLibary();
     FindObjectOfType<ReferenceLibrary>().GetComponent<ReferenceLibrary>().populateList();
     FindObjectOfType<HexUpdater>().GetComponent<HexUpdater>().updateParticle();
     FindObjectOfType<HexUpdater>().GetComponent<HexUpdater>().updateAudioClips();
     UpdateCollider();
    }
    
    
    public static List<GameObject> SegmentsThatNeedRefresh;
     void UpdateCollider()
    {    
        SegmentsThatNeedRefresh  = new List<GameObject>();
        SegmentsThatNeedRefresh.Clear();
        SegmentsThatNeedRefresh.AddRange(GameObject.FindGameObjectsWithTag("Curve"));
        foreach (GameObject Seg in SegmentsThatNeedRefresh)
        {
            Seg.GetComponent<MeshCollider>().enabled = false;
            Seg.GetComponent<MeshCollider>().enabled = true;
        }
    }
}
#endif
