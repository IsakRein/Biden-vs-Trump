using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	public float speed;

	void OnTriggerStay2D() 
	{
		transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
	}

	void OnTriggerExit2D() 
	{
		Destroy(gameObject);
	}
}
