using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour {

	void Update()
    {
        if (!this.GetComponent<ParticleSystem>().IsAlive())
            Destroy(this.gameObject);
    }
}
