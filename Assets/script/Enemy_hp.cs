﻿using UnityEngine;
using System.Collections;

public class Enemy_hp : MonoBehaviour {
	public int cnt;
	// Use this for initialization
	void Start () {
		this.transform.position = new Vector3(0.95f,0.97f,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(cnt>2) Destroy(this.gameObject);
		cnt++;
	}
}
