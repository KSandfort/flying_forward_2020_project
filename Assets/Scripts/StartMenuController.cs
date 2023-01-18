using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{

    public GameObject dropdown_age_object;
    private TMP_Dropdown dropdown_age;

    public GameObject dropdown_license_object;
    private TMP_Dropdown dropdown_license;

    public GameObject dropdwon_experience_object;
    private TMP_Dropdown dropdown_experience;

    public void Start() {
        dropdown_age = dropdown_age_object.GetComponent<TMP_Dropdown>();
        dropdown_license = dropdown_license_object.GetComponent<TMP_Dropdown>();
        dropdown_experience = dropdwon_experience_object.GetComponent<TMP_Dropdown>();
        if (dropdown_age == null) {
            Debug.Log("It's true");
        }
    }

    public void StartMainScene() {
        

        // Set age
        int dropdown_age_index = dropdown_age.value;
        switch(dropdown_age_index) {
        case 0:
            Data_Tracking_2.age = 0;
            break;
        case 1:
            Data_Tracking_2.age = 20;
            break;
        case 2:
            Data_Tracking_2.age = 30;
            break;
        case 3:
            Data_Tracking_2.age = 40;
            break;
        case 4:
            Data_Tracking_2.age = 50;
            break;
        case 5:
            Data_Tracking_2.age = 60;
            break;
        case 6:
            Data_Tracking_2.age = 70;
            break;
        case 7:
            Data_Tracking_2.age = 80;
            break;
        case 8:
            Data_Tracking_2.age = 90;
            break;
        default:
            Data_Tracking_2.age = 90;
            break;
        }

        // Set License
        int dropdown_license_index = dropdown_license.value;
        switch(dropdown_license_index) {
        case 0:
            Data_Tracking_2.license = "None";
            break;
        case 1:
            Data_Tracking_2.license = "A1 & A2";
            break;
        case 2:
            Data_Tracking_2.license = "A1 & A2 & A3";
            break;
        default:
            Data_Tracking_2.license = "None";
            break;
        }

        // Set Experience
        int dropdown_experience_index = dropdown_experience.value;
        switch(dropdown_experience_index) {
        case 0:
            Data_Tracking_2.flying_exp_hours = 0;
            break;
        case 1:
            Data_Tracking_2.flying_exp_hours = 4;
            break;
        case 2:
            Data_Tracking_2.flying_exp_hours = 9;
            break;
        case 3:
            Data_Tracking_2.flying_exp_hours = 19;
            break;
        case 4:
            Data_Tracking_2.flying_exp_hours = 49;
            break;
        case 5:
            Data_Tracking_2.flying_exp_hours = 199;
            break;
        case 6:
            Data_Tracking_2.flying_exp_hours = 499;
            break;
        case 7:
            Data_Tracking_2.flying_exp_hours = 500;
            break;
        default:
            Data_Tracking_2.flying_exp_hours = 0;
            break;
        }

        SceneManager.LoadScene("Health-Scenario");
    }
}
