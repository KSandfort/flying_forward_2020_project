using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CanvasManager : MonoBehaviour
{

    // Script reference
    public Data_Tracking_2 data_tracking_script;
    // Speed
    public GameObject speed_text_object;
    private TextMeshProUGUI speed_text;
    // Current height
    public GameObject height_text_object;
    private TextMeshProUGUI height_text;
    // Overflown people
    public GameObject overflown_people_text_object;
    private TextMeshProUGUI overflown_people_text; 


    // Start is called before the first frame update
    void Start()
    {
        speed_text = speed_text_object.GetComponent<TextMeshProUGUI>();
        height_text = height_text_object.GetComponent<TextMeshProUGUI>();
        overflown_people_text = overflown_people_text_object.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        speed_text.text = ((int) (data_tracking_script.get_speed() * 3.6)) .ToString() + " km/h"; // Displays in km/h
        height_text.text = ((float) (Math.Round(data_tracking_script.get_current_height() * 100f)) /100f).ToString() + " m";
    }

    public void update_overflown_people_counter(int count) {
        overflown_people_text.text = count.ToString();
    }
}
