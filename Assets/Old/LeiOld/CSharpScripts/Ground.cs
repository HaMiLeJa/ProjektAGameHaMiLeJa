using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ground : MonoBehaviour
{
    public float inputFactor;
    public float borderDegree;

    public Text xDegreeDisplay;
    public Text zDegreeDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float zInput = Input.GetAxis("Horizontal");
        float xInput = Input.GetAxis("Vertical");

        float zDegreeNew = transform.eulerAngles.z - zInput * inputFactor * Time.deltaTime;

        if (zDegreeNew >= 0 && zDegreeNew < 180 && zDegreeNew > borderDegree)
            zDegreeNew = borderDegree;

        if (zDegreeNew >= 180 && zDegreeNew < 360 && zDegreeNew < 360 - borderDegree)
            zDegreeNew = 360 - borderDegree;

        float xDegreeNew = transform.eulerAngles.x + xInput * inputFactor * Time.deltaTime;

        if (xDegreeNew >= 0 && xDegreeNew < 180 && xDegreeNew > borderDegree)
            xDegreeNew = borderDegree;

        if (xDegreeNew >= 180 && xDegreeNew < 360 && xDegreeNew < 360 - borderDegree)
            xDegreeNew = 360 - borderDegree;

        transform.eulerAngles = new Vector3(xDegreeNew, 0, zDegreeNew);

        xDegreeDisplay.text = string.Format("xDegree: {0,6:0.0}", xDegreeNew);
        zDegreeDisplay.text = string.Format("zDegree: {0,6:0.0}", zDegreeNew);
    }
}
