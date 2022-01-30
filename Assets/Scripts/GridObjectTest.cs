using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Grid grid = new Grid(2, 2, 10f, new Vector3(-5, -4, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
