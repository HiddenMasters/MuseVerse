using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Reciprocation : MonoBehaviour {
    [SerializeField] [Range(0f,10f)] private float speed = 1f;
    [SerializeField] [Range(0f,10f)]  private float length = 1f;
    public float startTime;
    
    private float _playTime;
    private float _runningTime = 0f;
    private float _yPos = 0f;
 
    // Use this for initialization
    void Start()
    {
        Transform transform = GetComponent<Transform>();
    }

    private void Update()
    {
        _playTime += Time.deltaTime;

        if (startTime < _playTime)
        {
            _runningTime += Time.deltaTime * speed;
            _yPos = Mathf.Sin(_runningTime) * length;
            transform.position += new Vector3(0,_yPos,0);
        }
    }
}