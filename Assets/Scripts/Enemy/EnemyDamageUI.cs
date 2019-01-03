using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageUI : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GetComponent<MeshRenderer>().sortingLayerName = "Effect";
        GetComponent<Rigidbody2D>().AddForce(new Vector3(30, 150, 0));
        Destroy(this.gameObject, 0.65f);
    }
}
