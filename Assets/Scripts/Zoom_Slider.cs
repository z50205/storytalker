using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoom_Slider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Camera cam;
    [SerializeField] Slider slider;

    void Start()
    {
        cam.orthographicSize = slider.value;
    }
    public void ChangeZoom()
    {
        cam.orthographicSize = slider.value;
    }
}
