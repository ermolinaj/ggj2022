using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotableMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            RotateRight();
        
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            RotateLeft();
    }

    void RotateRight()
    {
        transform.Rotate(new Vector3(0, 0, 90f), Space.Self );
    }

    void RotateLeft()
    {
        transform.Rotate(new Vector3(0, 0, -90f), Space.Self );
    }
}
