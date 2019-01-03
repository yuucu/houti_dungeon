using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public GameObject damageEffect;

    protected float str;

    void Start()
    {
        StartCoroutine(DestroyHit());
    }

    protected virtual IEnumerator DestroyHit()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hit");
    }

    public void set(float str)
    {
        this.str = str;
    }
}
