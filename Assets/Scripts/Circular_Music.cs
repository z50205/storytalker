using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circular_Music : MonoBehaviour
{
    [SerializeField] float broadcast_angle;
    [SerializeField] Vector3 broadcast_angle_left;
    [SerializeField] Vector3 broadcast_angle_right;
    [SerializeField] float angle_speed;
    [SerializeField] float objects_angle;
    [SerializeField] GameObject Audio_listener;
    [SerializeField] GameObject Audio_source;
    [SerializeField] LineRenderer Ray;
    [SerializeField] LineRenderer Lrender_audio_prefab;

    private float portion;
    AudioSource vol;
    LineRenderer lin;
    void Start()
    {
        Application.targetFrameRate = 20;
        vol = Audio_source.GetComponent<AudioSource>();
        Ray = Instantiate(Lrender_audio_prefab, transform.position, transform.rotation, Audio_source.transform);
        lin = Ray.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (broadcast_angle >= 360)
        {
            broadcast_angle = 0;
        }
        else if (broadcast_angle < 0)
        {
            broadcast_angle = 360;
        }
        broadcast_angle = broadcast_angle + Time.deltaTime * angle_speed;
        objects_angle = detect_angle(Audio_source, Audio_listener);
        vol.volume = (float)(0.2 + (angle_difference(broadcast_angle, objects_angle)) * 0.8);
        //Debug.Log(detect_angle(Audio_source, Audio_listener));
        SetRay();
    }
    // void OnDrawGizmos()
    // {
    //     broadcast_angle_left.x=Mathf.Cos(broadcast_angle * Mathf.Deg2Rad) * Audio_source.GetComponent<AudioSource>().maxDistance;
    //     broadcast_angle_left.y=Mathf.Sin(broadcast_angle * Mathf.Deg2Rad) * Audio_source.GetComponent<AudioSource>().maxDistance;
    //     broadcast_angle_left.z=0f;
    //     Gizmos.DrawRay(Audio_source.transform.position, broadcast_angle_left);
    // }

    void SetRay()
    {
        lin.SetPosition(0, Audio_source.transform.position);
        broadcast_angle_left.x = Audio_source.transform.position.x + Mathf.Cos(broadcast_angle * Mathf.Deg2Rad) * vol.maxDistance;
        broadcast_angle_left.y = Audio_source.transform.position.y + Mathf.Sin(broadcast_angle * Mathf.Deg2Rad) * vol.maxDistance;
        broadcast_angle_left.z = 0f;
        lin.SetPosition(1, broadcast_angle_left);
    }
    float detect_angle(GameObject Audio_source, GameObject Audio_listener)
    {
        float angle = Mathf.Atan2((Audio_source.transform.position.y - Audio_listener.transform.position.y), (Audio_source.transform.position.x - Audio_listener.transform.position.x)) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle = angle + 360;
        }
        return angle;
    }
    float angle_difference(float source, float listener)
    {
        float portion;
        portion = Mathf.Abs(source - listener) / 180;
        if (portion >= 1 && source < listener)
            portion = Mathf.Abs(360 + source - listener) / 180;
        if (portion >= 1 && source > listener)
            portion = Mathf.Abs(source - listener - 360) / 180;
        return portion;
    }
}
