using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
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

    // Time tracking
    private Stopwatch timer;
    private int update_count;
    private double last_log_time;

    // Rigidbody reference
    Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        _rigidbody = GetComponent<Rigidbody>();
        map_type = "Intruder";
        vector_list = new List<DroneVector>();
        update_count = 0;
        timer = new Stopwatch();
        timer.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        update_count += 1;
        if (update_count >= 9) { // Perform action on every 10th fixed update
            update_count = 0;
            double millis = timer.ElapsedMilliseconds;
            if (last_log_time != millis) {
                DroneVector droneVector = new DroneVector(
                    millis, 
                    _rigidbody.position.x,
                    _rigidbody.position.y,
                    _rigidbody.position.z,
                    _rigidbody.velocity.x,
                    _rigidbody.velocity.y,
                    _rigidbody.velocity.z
                );
                vector_list.Add(droneVector);
            }
            last_log_time = millis;
        }
    }

    public void foo() {
        UnityEngine.Debug.Log("Foo");
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
        // DroneVector vector1 = new DroneVector(200, 93.4f, 45.0f, 459.5f, 4934.0f, 494.2f, 495.3f);
        // DroneVector vector2 = new DroneVector(400, 83.3f, 46.0f, 480.5f, 4334.4f, 454.2f, 245.7f);
        // vector_list.Add(vector1);
        // vector_list.Add(vector2);
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
        json = JObject.Parse(json_string);
        UnityEngine.Debug.Log(json);
    }

    // Sends the locally stored data to the target API
    public void send_json_dump() {
        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json_string);

        www = new WWW("http://185.167.96.189:5000/api/dump", formData, postHeader);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW data)
    {
        yield return data; // Wait until the download is done
        if (data.error != null)
        {
            UnityEngine.Debug.Log("There was an error sending request: " + data.error);
        }
        else
        {
            UnityEngine.Debug.Log("WWW Request: " + data.text);
        }
        data.Dispose();
    }

    internal void get_request()
    {
        throw new NotImplementedException();
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
