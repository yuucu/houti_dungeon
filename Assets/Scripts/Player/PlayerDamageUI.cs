using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().sortingLayerName = "Effect";
        GetComponent<Rigidbody2D>().AddForce(new Vector3(-30, 150, 0));
        Destroy(this.gameObject, 0.7f);
    }
    
}
