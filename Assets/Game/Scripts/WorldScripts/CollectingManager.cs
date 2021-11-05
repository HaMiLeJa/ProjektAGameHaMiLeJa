using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CollectingManager : MonoBehaviour
{
    #region Inspector
    
    [HideInInspector]
    public static int counterCaught = 0;
    public TextMeshProUGUI collectedTextMesh;
    
    #endregion

    void Update()
    {
        collectedTextMesh.text = counterCaught.ToString();
    }
}
