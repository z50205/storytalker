using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Finger_Touch : MonoBehaviour, IPointerDownHandler// required interface when using the OnPointerDown method.
{
    public GameObject player;
    
    public Rigidbody2D myRigidbody;
    public float runSpeed;
    //Do this when the mouse is clicked over the selectable object this script is attached to.
    void Start()
    {
        myRigidbody = player.GetComponent<Rigidbody2D>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name + " Was Clicked.");
        move();
    }

    // Update is called once per frame
    void move()
    {
        Vector2 playerVel = new Vector2( runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVel;
        Debug.Log(playerVel);
        Debug.Log(myRigidbody.velocity);
    }
}