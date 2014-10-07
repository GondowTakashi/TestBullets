﻿using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {
	private int cnt;
	// Use this for initialization
	void Start () {
		cnt = 0 ;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			if(this.guiText.HitTest(Input.mousePosition)){
				cnt = 1;
			}
		}
		if(cnt>0){
			this.guiText.color = new Color(1,1-cnt/30.0f,1-cnt/30.0f,1);
			cnt++;
		}
		if(cnt == 30){
			Application.LoadLevel("MainMenu");
		}
	}
}
