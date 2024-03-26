
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float TimeStart = 45;
    public TMP_Text textbox; // this should enable me to add a specified text box 
    void Start()
    {
        textbox.text = TimeStart.ToString();

    }


    void Update()
    {
        TimeStart -= Time.deltaTime;
        textbox.text = Mathf.Round(TimeStart).ToString();
    }
}
