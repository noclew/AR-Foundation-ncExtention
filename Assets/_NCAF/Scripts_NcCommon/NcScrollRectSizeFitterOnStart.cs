using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NcScrollRectSizeFitterOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.offsetMin = new Vector2(rt.offsetMin.x, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
