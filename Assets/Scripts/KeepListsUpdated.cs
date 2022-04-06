
using UnityEngine;
public class KeepListsUpdated : MonoBehaviour
{
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
         FindObjectOfType<CurveManager>().GetComponent<CurveManager>().UpdateCollider();
        }
    #endif
}

