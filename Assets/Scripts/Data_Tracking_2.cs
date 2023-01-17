using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Data_Tracking_2 : MonoBehaviour
{
    // --- Tracked variables ---
    private bool mission_success = true;
    private float timer = 0.0f;
    private float max_speed = 0;
    private float avg_speed = 0;
    private float current_height = 0;
    private float max_height = 0;
    private float avg_height = 0;
    private int num_overflown_people = 0;
    private float closest_distance_to_person = float.MaxValue;
    private float closest_distance_to_building = float.MaxValue;
    private float average_closest_distance_to_building = 0;
    // --------------------------

    // --- Helper variables ---
    private float current_speed = 0;
    private long update_count = 0;
    // ------------------------

    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onPersonOverflown += OnPersonOverflownIncreaseCounter;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        update_count += 1; // Keep track of the number of updates

        // --- Mission success/failure ---
        // TODO

        // --- Timer ---
        timer += Time.deltaTime;

        // --- Speed calculation ---
        current_speed = Mathf.Sqrt(Mathf.Pow(_rigidbody.velocity.x,2) + Mathf.Pow(_rigidbody.velocity.y,2) + Mathf.Pow(_rigidbody.velocity.z,2));

        // Avg speed
        avg_speed += (current_speed - avg_speed) / update_count; // Incremental average algorithm

        // --- Height calculation ---
        current_height = _rigidbody.position.y;
        if (max_height < current_height) {
            max_height = current_height;
        }

        // Avg height
        avg_height += (current_height - avg_height) / update_count;
    }

    private void OnPersonOverflownIncreaseCounter() {
        num_overflown_people += 1;
        Debug.Log("Overflown people: " + num_overflown_people);
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
