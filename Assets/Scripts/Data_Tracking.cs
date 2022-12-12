using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class Data_Tracking : MonoBehaviour
{
    // Pilot (user) data
    private int age;
    private int flying_exp_mins;
    private string gender;
    private string license;

    // Flying data
    private string map_type;
    private int time_overflying_people_ms;
    private int number_overflown_people;
    private double min_dist_to_nearest_structure;
    private double min_dist_to_nearest_person;
    private double avg_distance_to_intruder;
    private double max_dist_to_start;
    private int gated_vul_points;
    private List<DroneVector> vector_list;

    // JSON body
    private string json_string;
    private JObject json;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        map_type = "Intruder";
        vector_list = new List<DroneVector>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(transform.position);
    }

    public void foo() {
        Debug.Log("Foo");
    }

    public void generate_fake_data() {
        age = 69;
        flying_exp_mins = 420;
        gender = "m";
        license = "Everything smaller than an Airbus A380";
        time_overflying_people_ms = 69;
        number_overflown_people = 69;
        min_dist_to_nearest_structure = 666f;
        min_dist_to_nearest_person = 666f;
        avg_distance_to_intruder = 3.14f;
        max_dist_to_start = 123.45;
        gated_vul_points = 9876;
        DroneVector vector1 = new DroneVector(200, 93.4f, 45.0f, 459.5f, 4934.0f, 494.2f, 495.3f);
        DroneVector vector2 = new DroneVector(400, 83.3f, 46.0f, 480.5f, 4334.4f, 454.2f, 245.7f);
        vector_list.Add(vector1);
        vector_list.Add(vector2);
    }

    public void generate_json_object() {
        json_string = "{ \"user_data\": { \"age\": " + age + ", \"flying_exp_mins\": " + flying_exp_mins + ",";
        json_string += "\"gender\": \"" + gender + "\", \"license\": \"" + license + "\" }, ";
        json_string += "\"map\": \"" + map_type + "\", \"summary\": {";
        json_string += "\"time_overflying_people_ms\": " + time_overflying_people_ms + ",";
        json_string += "\"number_overflown_people\": " + number_overflown_people + ",";
        json_string += "\"min_dist_to_nearest_structure\": " + min_dist_to_nearest_structure + ",";
        json_string += "\"min_dist_to_nearest_person\": " + min_dist_to_nearest_person + ",";
        json_string += "\"avg_dist_to_intruder\": " + avg_distance_to_intruder + ",";
        json_string += "\"max_dist_to_start\": " + max_dist_to_start + ",";
        json_string += "\"gated_vul_points\": " + gated_vul_points + "},";
        json_string += "\"vectors\": [";
        foreach (DroneVector dv in vector_list) {
            json_string += "{";
            json_string += "\"time_ms\": " + dv.time_ms + ",";
            json_string += "\"px\": " + dv.px + ",";
            json_string += "\"py\": " + dv.py + ",";
            json_string += "\"pz\": " + dv.pz + ",";
            json_string += "\"vx\": " + dv.vx + ",";
            json_string += "\"vy\": " + dv.vy + ",";
            json_string += "\"vz\": " + dv.vz;
            json_string += "},";
        }
        json_string = json_string.Remove(json_string.Length-1); // Remove last comma
        json_string += "]}";
        Debug.Log(json_string);

        // string test_str = "{ \"context_name\": { \"lower_bound\": \"value\", \"upper_bound\": \"value\", \"values\": [ \"value1\", \"valueN\" ] } }";
        json = JObject.Parse(json_string);
        Debug.Log(json);
    }

    // Sends the locally stored data to the target API
    public WWW send_json_dump() {
         WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        string json_string2 = "{\"user_data\": {\"age\": 0,\"flying_exp_mins\": 0,\"gender\": \"m\",\"license\": \"string\"},\"map\":\"string\",\"summary\": {\"time_overflying_people_ms\": 0,\"number_overflown_people\": 0,\"min_dist_to_nearest_structure\": 0,\"min_dist_to_nearest_person\": 0,\"avg_dist_to_intruder\": 0,\"max_dist_to_start\": 0,\"gated_vul_points\": 0},\"vectors\": [] }";
        var formData = System.Text.Encoding.UTF8.GetBytes(json_string2);

        www = new WWW("http://185.167.96.189:5000/api/dump", formData, postHeader);
        StartCoroutine(WaitForRequest(www));
        return www;
        // StartCoroutine(Post_Request());
    }

    IEnumerator WaitForRequest(WWW data)
{
    yield return data; // Wait until the download is done
    if (data.error != null)
    {
        Debug.Log("There was an error sending request: " + data.error);
    }
    else
    {
        Debug.Log("WWW Request: " + data.text);
    }
}

    IEnumerator Post_Request() {
        // Convert json string into binary data
        var jsonBinaryData = System.Text.Encoding.UTF8.GetBytes(json_string);
        
        // Configure upload and download
        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
        UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinaryData);
        uploadHandlerRaw.contentType = "application/json";
        
        // Generate and sendweb request
        UnityWebRequest www = new UnityWebRequest("http://185.167.96.189:5000/api/dump", "POST", downloadHandlerBuffer, uploadHandlerRaw);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}

class DroneVector {
    public double time_ms;
    public double px;
    public double py;
    public double pz;
    public double vx;
    public double vy;
    public double vz;
    
    public DroneVector(double _time_ms, double _px, double _py, double _pz, double _vx, double _vy, double _vz) {
        this.time_ms = _time_ms;
        this.px = _px;
        this.py = _py;
        this.pz = _pz;
        this.vx = _vx;
        this.vy = _vy;
        this.vz = _vz; 
    }
}
