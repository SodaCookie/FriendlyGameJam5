﻿using UnityEngine;
using Melodrive;

public class BeatReactor : MelodriveObject
{
    private Vector3 targetScale;
    private Rigidbody body;

    // Use this for initialization
    void Start ()
    {
        targetScale = transform.localScale;
        
        md.Beat += new MelodrivePlugin.BeatHandler(Beat);
        body = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 2 * Time.deltaTime);
        body.AddForce(-transform.position, ForceMode.Impulse);
    }

    private void Beat(float beat, float bar)
    {
        transform.localScale = new Vector3(2, 2, 2);
        body.AddExplosionForce(0.5f, transform.position, 1, 0);
    }
}
