using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool StepComplete { get; set; }

    // Todo: make this a user input
    public int steps = 10;
    public float speed = 5;

    Vector3 currentdestination;
    
    // Start is called before the first frame update
    void Start()
    {
        StepComplete = true;
        currentdestination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentdestination, speed * Time.deltaTime);
        if(currentdestination == transform.position)
        {
            StepComplete = true;
        }
    }

    public void Move(Vector3 direction)
    {
        // Todo: make this dependable on Tile Map
        currentdestination = transform.position + direction * steps;               
    }
}
