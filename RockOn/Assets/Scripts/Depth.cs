﻿using UnityEngine;
using System.Collections;

/* 
 * Simple script for adding "depth" to our 2D world.
 * Just add this script to the GameObject and voila.
 * 
 * The script takes the GameObject's position and
 * changes it so that the position on Z axis is
 * the same as Y axis.
 */
public class Depth : MonoBehaviour
{

    // this GameObject's Transform component
    private Transform _transform;

    void Start()
    {
        // initializing this GameObject's Transform
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        // update the Z position to be equal the Y position
        _transform.position = new Vector3(_transform.position.x, _transform.position.y, _transform.position.y);
    }
}
