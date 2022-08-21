using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    private RectTransform rectTransform;
    private Vector2 backgroundSize;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        backgroundSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        rectTransform.sizeDelta = backgroundSize;
    }

    // Update is called once per frame
    void Update()
    {
        backgroundSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        rectTransform.sizeDelta = backgroundSize;
    }
}
