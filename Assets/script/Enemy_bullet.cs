﻿using UnityEngine;
using System.Collections;

public class Enemy_bullet : MonoBehaviour {
	private float speed;
	private float angle;
	private int col;
	private int knd;
	// Use this for initialization
	void Start(){}

	public void First(float x,float y,float speed,float angle,int knd,int col) {
		this.transform.position = new Vector3(x,y,0);
		this.speed = speed;
		this.angle = angle;
		this.knd   = knd;
		this.col   = col;
	}
	// Update is called once per frame
	void Update () {
		//移動制御
		Vector3 p = this.transform.position;
		if(speed != 0){
			p.x += speed * Mathf.Cos(angle);
			p.y += speed * Mathf.Sin(angle);
			this.transform.position = p;
		}
	}
}
