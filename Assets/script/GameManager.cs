using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject jiki_Prefab;
	public bool jiki_alive;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(jiki_alive == false){
			jiki_alive = true ;
			var go = Instantiate( jiki_Prefab ) as GameObject;
			Jiki jiki = go.GetComponent<Jiki>();
		}
		if(GameObject.Find("jiki")==null&GameObject.Find("jiki(Clone)")==null) 	jiki_alive = false;
	}
}
