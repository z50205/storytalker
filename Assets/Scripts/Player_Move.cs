using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public float runSpeed;
    private Rigidbody2D myRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
    void move()
     {
        // float moveDir=Input.GetAxis("Horizontal");
        // Vector2 playerVel=new Vector2(moveDir*runSpeed,myRigidbody.velocity.y);
        // myRigidbody.velocity=playerVel;
        float moveDir=Input.GetAxis("Horizontal");
        Vector2 playerVel=new Vector2(moveDir*runSpeed,myRigidbody.velocity.y);
        myRigidbody.velocity=playerVel;
      }
}
