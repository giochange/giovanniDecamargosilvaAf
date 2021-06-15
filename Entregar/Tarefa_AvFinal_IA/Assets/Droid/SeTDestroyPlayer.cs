using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeTDestroyPlayer : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		Destroy(this.gameObject);//  destroi obj que possuir o script
	}
}
