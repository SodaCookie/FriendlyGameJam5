﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    public float Speed = 1;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, Speed * Time.deltaTime);
	}
}