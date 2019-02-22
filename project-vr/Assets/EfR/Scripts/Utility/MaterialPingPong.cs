using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPingPong : MonoBehaviour {

	public float min = 0f;
	public float max = 0.5f;
	public float speed = 1f;

	Material targetMaterial;

	// Use this for initialization
	void Start () {
		targetMaterial = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		targetMaterial.SetFloat("_Outline", Mathf.PingPong(Time.time * speed, max - min) + min);
	}
}
