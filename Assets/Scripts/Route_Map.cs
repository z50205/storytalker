using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Route_Map : MonoBehaviour
{
    public GameObject St1;
    public GameObject St2;
    public GameObject Player;
    public GameObject Route_;
    [SerializeField] GameObject btn1;
    [SerializeField] GameObject btn2;
    [SerializeField] GameObject btn3;
    Transform pos;

    Transform pospt1;

    Transform pospt2;
    public Vector3 temp_pos;
    private Text tex1;
    private Text tex2;
    private Text tex3;
    public int indicator;
    public float parameter;
    public float dist;
    public float movespeed;
    // Start is called before the first frame update
    void Start()
    {
        indicator = 0;
        parameter = 0;
        dist = Vector3.Distance(St1.transform.position, St2.transform.position);
        pos = Player.transform;
        pospt1 = St1.transform;
        pospt2 = St2.transform;
        tex1 = btn1.GetComponentInChildren<Text>();
        tex2 = btn2.GetComponentInChildren<Text>();
        tex3 = btn3.GetComponentInChildren<Text>();
        temp_pos=new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        move();
        //Debug.Log(Player.transform.position);
        //Debug.Log(Player.transform.position);
    }
    void move()
    {
        // float moveDir=Input.GetAxis("Horizontal");
        // Vector2 playerVel=new Vector2(moveDir*runSpeed,myRigidbody.velocity.y);
        // myRigidbody.velocity=playerVel;

        if (indicator == 1)
        {
            parameter = parameter + movespeed;
        }
        else if (indicator == 2)
        {
            parameter = parameter - movespeed;
        }
        pos.position = (1 - parameter) * pospt1.position + parameter * pospt2.position;
    }
    public void ChangeMode_Stop()
    {
        indicator = 0;
        Mode_Check();
    }
    public void ChangeMode_Go()
    {
        indicator = 1;
        Mode_Check();
    }
    public void ChangeMode_Back()
    {
        indicator = 2;
        Mode_Check();
    }
    public void Mode_Check()
    {
        if (indicator == 0)
        {
            tex1.color = Color.red;
            tex2.color = Color.black;
            tex3.color = Color.black;
        }
        else if (indicator == 1)
        {
            tex1.color = Color.black;
            tex2.color = Color.red;
            tex3.color = Color.black;
        }
        else if (indicator == 2)
        {
            tex1.color = Color.black;
            tex2.color = Color.black;
            tex3.color = Color.red;
        }
    }
}
