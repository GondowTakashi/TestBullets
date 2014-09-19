﻿using UnityEngine;
using System.Collections;

public class EasyStartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			if(this.guiText.HitTest(Input.mousePosition)){
				var go = GameObject.Find("back") as GameObject;
				GameManager game = go.GetComponent(typeof(GameManager)) as GameManager;
				game.SendMessage("SetLevel",1);
				Application.LoadLevel("TestBullet");
			}
		}
	}
}