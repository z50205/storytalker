using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.Mathematics;

public class Route : MonoBehaviour
{

    [SerializeField] int flag;
    [SerializeField] int total_stations;
    [SerializeField] int station;
    [SerializeField] int gear;

    [SerializeField] float t;

    [SerializeField] float movespeed;
    [SerializeField] float acceleration;
    [SerializeField] int direction;
    [SerializeField] int route_switcher;
    [SerializeField] private Transform player;
    private Vector3[] stations_position_t;
    [SerializeField] private Transform stations;
    [SerializeField] private Transform routes;
    [SerializeField] private ToggleGroup gearToggleGroup;
    [SerializeField] private Transform gearHandler;
    [SerializeField] private Transform velocity_chart_pointer;
    [SerializeField] private double[] gear_limit = new double[5] { 0.0025, 0.005, 0.01, 0.016, 0.023 };
    public int[,] Factorials_coeff = new int[6, 6] { { 0, 0, 0, 0, 0, 0 }, { 1, 1, 0, 0, 0, 0 }, { 1, 2, 1, 0, 0, 0 }, { 1, 3, 3, 1, 0, 0 }, { 1, 4, 6, 4, 1, 0 }, { 1, 5, 10, 10, 5, 1 } };
    public int[,] Route_ends = new int[7, 2] { { 0, 1 }, { 1, 2 }, { 3, 1 }, { 2, 4 }, { 5, 3 }, { 4, 5 } , { 4, 6 } };
    [SerializeField] public int[,] Station_route_go = new int[,] { };
    [SerializeField] public int[,] Station_route_back = new int[,] { };
    [SerializeField] private Transform render_routes;
    [SerializeField] private LineRenderer render_route;
    [SerializeField] private LineRenderer Lrender_prefab;
    // public int[,] Station_route_go = new int[4, 4] { { 1, 0, 0, 0 }, { 2, 1, 2, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
    // public int[,] Station_route_back = new int[4, 4] { { 0, 0, 0, 0 }, { 1, 0, 0, 0 }, { 1, 1, 0, 0 }, { 1, 2, 0, 0 } };



    [SerializeField] GameObject btn1;
    [SerializeField] GameObject btn2;
    [SerializeField] GameObject btn3;
    [SerializeField] Button btn4;
    private Text tex1;
    private Text tex2;
    private Text tex3;
    private Text tex4;
    // Start is called before the first frame update
    void Start()
    {
        movespeed = 0.00f;
        acceleration = 0.001f;
        station = 0;//是否在站點內、或在哪個站點上
        t = 0;//路徑比例
        flag = -1;//代表行走在哪條路徑上
        direction = -1;
        route_switcher = 1;
        total_stations = 7;
        gear = 1;
        tex1 = btn1.GetComponentInChildren<Text>();
        tex2 = btn2.GetComponentInChildren<Text>();
        tex3 = btn3.GetComponentInChildren<Text>();
        tex4 = btn4.GetComponentInChildren<Text>();
        Form_Station_route();
        player.position = stations.GetChild(0).position;

        // init_Factorials_coeff();
        // init_Route_ends();
        // init_Station_route();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (station != -1)
        {
            Choose_Route();
        }
        if (station == -1)
        {
            Form_Route();
            move();
            curve();
        }
        VelocityChart();
        // Debug.Log(temp.position);
        // Debug.Log(total_stations);
    }
    void move()
    {
        // float moveDir=Input.GetAxis("Horizontal");
        // Vector2 playerVel=new Vector2(moveDir*runSpeed,myRigidbody.velocity.y);
        // myRigidbody.velocity=playerVel;
        if (direction == 0)
        {
            if (-gear_limit[gear - 1] < movespeed)
                movespeed = movespeed - acceleration * Time.deltaTime;
            else if (-gear_limit[gear - 1] > movespeed)
                movespeed = movespeed + acceleration * Time.deltaTime;
        }
        else if (direction == 1)
        {
            if (gear_limit[gear - 1] > movespeed)
                movespeed = movespeed + acceleration * Time.deltaTime;
            else if (gear_limit[gear - 1] < movespeed)
                movespeed = movespeed - acceleration * Time.deltaTime;
        }
        else if (direction == -1)
        {
            if (0 > movespeed)
                movespeed = movespeed + 2 * acceleration * Time.deltaTime;
            else if (0 < movespeed)
                movespeed = movespeed - 2 * acceleration * Time.deltaTime;
        }
        t = t + movespeed;
    }
    [System.Obsolete]
    //選擇路徑的實踐
    void Choose_Route()
    {
        if (movespeed < 0)
        {
            if (Station_route_back[station, 0] == 0)
            {
                direction = -1;
                movespeed = 0;
            }
            else if (Station_route_back[station, 0] >= route_switcher)
                flag = Station_route_back[station, route_switcher];
            else
                ChangeMode_AnotherWay();
        }
        if (movespeed > 0)
        {
            if (Station_route_go[station, 0] == 0)
            {
                direction = -1;
                movespeed = 0;
            }
            else if (Station_route_go[station, 0] >= route_switcher)
                flag = Station_route_go[station, route_switcher];
            else
                ChangeMode_AnotherWay();
        }
        if (movespeed == 0)
        {
            if (Station_route_back[station, 0] >= route_switcher && direction == 0)
                flag = Station_route_back[station, route_switcher];
            else if (Station_route_go[station, 0] >= route_switcher && direction == 1)
                flag = Station_route_go[station, route_switcher];
            else if (direction == 0 || direction == 1)
                ChangeMode_AnotherWay();
        }
        if (flag != -1)
            Form_Route();
        if (flag == -1)
        {
            player.position = stations.GetChild(station).position;
        }
    }
    [System.Obsolete]
    //獲取路徑的資料(地圖)
    void Form_Route()
    {
        int controlpoints_count = routes.GetChild(flag).GetChildCount();
        stations_position_t = new Vector3[controlpoints_count + 2];

        // Debug.Log(Route_ends[flag, 0]);
        // Debug.Log(stations.GetChild(1).transform.position);
        stations_position_t[0] = stations.GetChild(Route_ends[flag, 0]).position;
        //stations_position_t[0] = stations.GetChild(Route_ends[flag, 0]);

        for (int i = 0; i < controlpoints_count; i++)
            stations_position_t[i + 1] = routes.GetChild(flag).GetChild(i).position;


        //Debug.Log(Route_ends[flag, 1]);
        //Debug.Log(stations.GetChild(1).transform.position);
        stations_position_t[controlpoints_count + 1] = stations.GetChild(Route_ends[flag, 1]).position;
        total_stations = stations_position_t.Length;
        if (station != -1)
            Depature();
    }
    //放行，改變各種狀態
    void Depature()
    {
        if (Route_ends[flag, 0] == station)
            t = 0;
        else
            t = 1;
        station = -1;
    }
    //行走的實踐
    void curve()
    {
        Vector3 temp = new Vector3(0, 0, 0);

        for (int i = 0; i < total_stations; i++)
        {
            temp = temp + Factorials_coeff[total_stations - 1, i] * stations_position_t[i] * Mathf.Pow(1 - t, total_stations - i - 1) * Mathf.Pow(t, i);
            // Debug.Log(stations_position_t[i].position);
            //  Debug.Log(a.transform.position);
            //Debug.Log(Mathf.Pow(1 - t, total_stations - i) * Mathf.Pow(t, i));
            //Debug.Log(Factorials_coeff[total_stations-1,i]);
            // Debug.Log(Mathf.Pow(1 - t, total_stations - i - 1));
        }
        //Debug.Log(temp);
        //Debug.Log(Mathf.Pow(0, 0));
        player.position = temp;
        if (t > 1)
        {
            station = Route_ends[flag, 1];
            flag = -1;
        }
        else if (t < 0)
        {
            station = Route_ends[flag, 0];
            flag = -1;
        }
    }

    void init_Factorials_coeff()
    {
        Factorials_coeff = new int[6, 6] { { 0, 0, 0, 0, 0, 0 }, { 1, 1, 0, 0, 0, 0 }, { 1, 2, 1, 0, 0, 0 }, { 1, 3, 3, 1, 0, 0 }, { 1, 4, 6, 4, 1, 0 }, { 1, 5, 10, 10, 5, 1 } };
    }
    void init_Route_ends()
    {
        Route_ends = new int[3, 2] { { 0, 1 }, { 1, 2 }, { 2, 3 } };
    }
    // void init_Station_route()
    // {
    //     Station_route = new int[4, 4] { { -1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 1, 2, 0, 0 }, { 2, -1, 0, 0 } };
    // }
    public void ChangeMode_Go()
    {
        direction = 1;
        Mode_Check();
    }
    public void ChangeMode_AnotherWay()
    {
        if (route_switcher == 1)
        {
            route_switcher = 2;
            tex4.color = Color.red;
        }
        else
        {
            route_switcher = 1;
            tex4.color = Color.black;
        }
    }
    public void ChangeMode_Back()
    {
        direction = 0;
        Mode_Check();
    }
    public void ChangeMode_Stop()
    {
        direction = -1;
        Mode_Check();
    }
    public void ChangeGear()
    {
        Vector3 tempt = new Vector3((float)18.96, (float)23.9, 0);
        Toggle theActiveToggle = gearToggleGroup.ActiveToggles().FirstOrDefault();
        gearHandler.transform.position = theActiveToggle.transform.position + tempt;
        gear = int.Parse(theActiveToggle.name);
    }

    public void VelocityChart()
    {
        velocity_chart_pointer.transform.rotation = Quaternion.Euler(0, 0, -math.abs(movespeed * -9782));
    }
    public void Mode_Check()
    {
        if (direction == -1)
        {
            tex1.color = Color.red;
            tex2.color = Color.black;
            tex3.color = Color.black;
        }
        else if (direction == 1)
        {
            tex1.color = Color.black;
            tex2.color = Color.red;
            tex3.color = Color.black;
        }
        else if (direction == 0)
        {
            tex1.color = Color.black;
            tex2.color = Color.black;
            tex3.color = Color.red;
        }
    }
    public void Form_Station_route()
    {
        Station_route_back = new int[total_stations, 4];
        Station_route_go = new int[total_stations, 4];
        int total_routes = Route_ends.GetLength(0);
        for (int i = 0; i < total_stations; i++)
        {
            Station_route_go[i, 0] = 0;
            Station_route_back[i, 0] = 0;
        }
        for (int i = 0; i < total_routes; i++)
        {
            int temp_route_count = Station_route_back[Route_ends[i, 1], 0];
            Station_route_back[Route_ends[i, 1], temp_route_count + 1] = i;
            Station_route_back[Route_ends[i, 1], 0] = temp_route_count + 1;

        }
        //Show_MATRIX(Station_route_back);
        for (int i = 0; i < total_routes; i++)
        {
            int temp_route_count = Station_route_go[Route_ends[i, 0], 0];
            Station_route_go[Route_ends[i, 0], temp_route_count + 1] = i;
            Station_route_go[Route_ends[i, 0], 0] = temp_route_count + 1;
        }
        Show_MATRIX(Station_route_go);
    }
    void Show_MATRIX(int[,] a)
    {
        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int ii = 0; ii < a.GetLength(1); ii++)
            {
                Debug.Log(a[i, ii]);
            }
        }
    }



}
// void OnDrawGizmos()
// {
//     float scale_x=(point1.transform.position.x-point2.transform.position.x);
//     float scale_y=(point1.transform.position.y-point2.transform.position.y);
//     for (float i=0;i<=1;i+=0.1f)
//     {
//         Vector2 gizmosposition=new Vector2(point1.transform.position.x-i*scale_x,point1.transform.position.y-i*scale_y);
//         //Debug.Log(gizmosposition);
//         Gizmos.DrawSphere(gizmosposition,0.25f);
//     }
// }



