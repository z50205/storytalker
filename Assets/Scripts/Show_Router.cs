using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show_Router : MonoBehaviour
{

    private Vector3[] stations_position_t;
    [SerializeField] int total_stations;
    [SerializeField] int number_points;
    [SerializeField] private Transform stations;
    [SerializeField] private Transform routes;
    [SerializeField] private Transform render_routes;
    [SerializeField] private LineRenderer render_route;
    [SerializeField] private LineRenderer Lrender_prefab;
    private int[,] Factorials_coeff;
    private int[,] Route_ends;


    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        GameObject route = GameObject.Find("UIController");
        Route routes_data = route.GetComponent<Route>();
        Factorials_coeff = routes_data.Factorials_coeff;
        Route_ends = routes_data.Route_ends;
        Instantiate_Route();
        Draw_stations();
        number_points = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Draw_stations();

    }

    [System.Obsolete]

    void Instantiate_Route()
    {
        for (int i = 0; i < routes.childCount; i++)
        {
            Instantiate(Lrender_prefab, transform.position, transform.rotation, render_routes);
        }
        Draw_stations();
    }


    [System.Obsolete]
    void Draw_stations()
    {
        for (int i = 0; i < routes.childCount; i++)
        {
            render_route = render_routes.GetChild(i).GetComponent<LineRenderer>();
            Form_Route(i);
        }
    }

    [System.Obsolete]
    void Form_Route(int flag)
    {
        Debug.Log(flag);
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
        //Debug.Log(number_points);
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
        }
        render_route.widthMultiplier = 0.1f;
        render_route.positionCount = number_points + 1;
        render_route.SetPositions(temp);
        //Debug.Log(temp);
    }
}
