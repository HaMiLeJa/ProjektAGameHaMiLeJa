using System.Collections.Generic;
using UnityEngine;
public class UpdateColliderIngame : MonoBehaviour
{
   public static List<GameObject> SegmentsThatNeedRefresh = new List<GameObject>();
    void Awake()
    {
        SegmentsThatNeedRefresh.Clear();
        SegmentsThatNeedRefresh.AddRange(GameObject.FindGameObjectsWithTag("Curve"));
    }
    void Start()
    {
        foreach (GameObject Seg in SegmentsThatNeedRefresh)
        {
            Seg.GetComponent<MeshCollider>().enabled = false;
            Seg.GetComponent<MeshCollider>().enabled = true;
        }
    }
}