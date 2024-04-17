using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmos_route : MonoBehaviour
{

    private Vector3[] stations_position_t;
    [SerializeField] int total_stations;
    [SerializeField] int number_points;
    [SerializeField] private Transform stations;
    [SerializeField] private Transform routes;
    public int[,] Factorials_coeff = new int[6, 6] { { 0, 0, 0, 0, 0, 0 }, { 1, 1, 0, 0, 0, 0 }, { 1, 2, 1, 0, 0, 0 }, { 1, 3, 3, 1, 0, 0 }, { 1, 4, 6, 4, 1, 0 }, { 1, 5, 10, 10, 5, 1 } };
  public int[,] Route_ends = new int[7, 2] { { 0, 1 }, { 1, 2 }, { 1, 3 }, { 2, 4 }, { 3, 5 }, { 5, 4 } , { 4, 6 } };
    // Start is called before the first frame update
    [System.Obsolete]
    private void OnDrawGizmos()
    {
        GameObject route = GameObject.Find("UIController");
        Route routes_data = route.GetComponent<Route>();
        Factorials_coeff = routes_data.Factorials_coeff;
        Route_ends = routes_data.Route_ends;
        Draw_stations();
    }

    [System.Obsolete]
    void Draw_stations()
    {
        for (int i = 0; i < routes.childCount; i++)
        {
            Form_Route(i);
        }
    }

    [System.Obsolete]
    void Form_Route(int flag)
    {
        int controlpoints_count = routes.GetChild(flag).GetChildCount();
        stations_position_t = new Vector3[controlpoints_count + 2];

        // Debug.Log(stations.GetChild(1).transform.position);
        stations_position_t[0] = stations.GetChild(Route_ends[flag, 0]).position;
        //stations_position_t[0] = stations.GetChild(Route_ends[flag, 0]);

        for (int i = 0; i < controlpoints_count; i++)
            stations_position_t[i + 1] = routes.GetChild(flag).GetChild(i).position;


        stations_position_t[controlpoints_count + 1] = stations.GetChild(Route_ends[flag, 1]).position;
        total_stations = stations_position_t.Length;
        curve();
    }
    void curve()
    {
        Vector3[] temp = new Vector3[number_points + 1];
        Debug.Log(number_points);
        for (int ii = 0; ii <= number_points; ii++)
        {
            float tt = (float)ii / (float)number_points;
            for (int i = 0; i < total_stations; i++)
            {
                temp[ii] = temp[ii] + Factorials_coeff[total_stations - 1, i] * stations_position_t[i] * Mathf.Pow(1 - tt, total_stations - i - 1) * Mathf.Pow(tt, i);
                // Debug.Log(stations_position_t[i].position);
                //  Debug.Log(a.transform.position);
                //Debug.Log(Mathf.Pow(1 - t, total_stations - i) * Mathf.Pow(t, i));
                //Debug.Log(Factorials_coeff[total_stations-1,i]);
                //Debug.Log(Mathf.Pow(1 - t, total_stations - i - 1));
            }
            // Debug.Log("tt is" + tt);
            // Debug.Log(temp[ii]);
            Gizmos.DrawSphere(temp[ii], 0.25f);
        }
        Gizmos.DrawLine(stations_position_t[0], stations_position_t[1]);
        Gizmos.DrawLine(stations_position_t[total_stations-2], stations_position_t[total_stations-1]);
        //Debug.Log(temp);
    }
}
