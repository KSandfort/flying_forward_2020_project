using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Data_Tracking_2 : MonoBehaviour
{
    // --- Canvas Script reference ---
    public CanvasManager canvasManager_script;

    // --- Pilot (user) data ---
    public static int age = 0;
    public static int flying_exp_hours = 0;
    public static string license = "None";

    // --- Tracked variables ---
    private bool mission_success = true;
    private float timer = 0.0f;
    private float flown_distance = 0;
    private float max_speed = 0;
    private float avg_speed = 0;
    private float max_height = 0;
    private float avg_height = 0;
    private int num_overflown_people = 0;
    private float closest_distance_to_person = float.MaxValue;
    private float closest_distance_to_building = float.MaxValue;
    private float average_closest_distance_to_building = 0;

    // --- Helper variables ---
    private float current_speed = 0;
    private float current_height = 0;
    private long update_count = 0;
    private Vector3 last_pos;
    // ------------------------

    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onPersonOverflown += OnPersonOverflownIncreaseCounter;
        GameEvents.current.onTargetAreaReached += OnHasReachedTarget;
        _rigidbody = GetComponent<Rigidbody>();
        last_pos = _rigidbody.position;
    }

    private void OnHasReachedTarget() {
        post_request();
        // End simulation afterwards
    }

    private void OnPersonOverflownIncreaseCounter() {
        num_overflown_people += 1;
        canvasManager_script.update_overflown_people_counter(num_overflown_people);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        update_count += 1; // Keep track of the number of updates

        // --- Mission success/failure ---
        // TODO

        // --- Timer ---
        timer += Time.deltaTime;

        // --- Flown distance ---
        Vector3 new_pos = _rigidbody.position;
        flown_distance += Vector3.Distance(last_pos, new_pos);
        last_pos = new_pos;

        // --- Speed calculation ---
        current_speed = Mathf.Sqrt(Mathf.Pow(_rigidbody.velocity.x,2) + Mathf.Pow(_rigidbody.velocity.y,2) + Mathf.Pow(_rigidbody.velocity.z,2));

        // Avg speed
        avg_speed += (current_speed - avg_speed) / update_count; // Incremental average algorithm

        // Max speed
        if (current_speed > max_speed) {
            max_speed = current_speed;
        }

        // --- Height calculation ---
        current_height = _rigidbody.position.y - 20; // Base height is at 20 meters.
        if (max_height < current_height) {
            max_height = current_height;
        }

        // Avg height
        avg_height += (current_height - avg_height) / update_count;
    }

    public void get_request() {
        StartCoroutine(GetRequest("https://drone-flying-online.space/"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    // --- POST request (Data dump to API) ---

    public void post_request() {
        string api_url = "https://drone-flying-online.space/api/data";

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("accept", "application/json");
        postHeader.Add("Content-Type", "application/json");

        //string json_str = "{\"pilot\": {\"age\": 10,\"licenses\": \"string\",\"flight_hrs\": 0},\"mission\": {\"success\": true,\"duration_secs\": 0,\"distance_m\": 0,\"max_speed_mps\": 0,\"avg_speed_mps\": 0,\"max_height_m\": 0,\"avg_height_m\": 0,\"overflown_people\": 0}}";
        string json_str = generate_json_str();
        
        // convert json string to byte
        var formData = System.Text.Encoding.UTF8.GetBytes(json_str);
        JObject json = JObject.Parse(json_str);
        UnityEngine.Debug.Log(json);

        www = new WWW(api_url, formData, postHeader);
        StartCoroutine(PostRequest(www));
    }

    IEnumerator PostRequest(WWW data)
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

    private string generate_json_str() {
        string str = "";
        str += "{\"pilot\": {\"age\":";
        str += age;
        str += ",\"licenses\": \"";
        str += license;
        str += "\",\"flight_hrs\":";
        str += flying_exp_hours;
        str += "},\"mission\": {\"success\":";
        if (mission_success) {
            str += "true";
        } 
        else {
            str += "false";
        }
        str += ",\"duration_secs\":";
        str += (int) timer;
        str += ",\"distance_m\":";
        str += flown_distance;
        str += ",\"max_speed_mps\":";
        str += max_speed;
        str += ",\"avg_speed_mps\":";
        str += avg_speed;
        str += ",\"max_height_m\": ";
        str += max_height;
        str += ",\"avg_height_m\": ";
        str += avg_height;
        str += ",\"overflown_people\":";
        str += num_overflown_people;
        str += "}}";

        return str;
    } 

    // --- Access methods ---
    
    public int get_timer() {
        return (int) timer;
    }

    public float get_speed() {
        return this.current_speed;
    }

    public float get_avg_speed() {
        return this.avg_speed;
    }

    public float get_current_height() {
        return current_height;
    }

    public float get_max_height() {
        return max_height;
    }

    public float get_avg_height() {
        return avg_height;
    }

}
