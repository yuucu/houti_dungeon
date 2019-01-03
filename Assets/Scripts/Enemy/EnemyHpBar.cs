using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour {
    
    private Slider hpSlider;
    private Enemy enemy;

    private int maxHp;

	// Use this for initialization
	void Start () {
        enemy = transform.parent.parent.GetComponent<Enemy>();
        hpSlider = GetComponent<Slider>();
        maxHp = enemy.getMaxHp();
    }
	
	// Update is called once per frame
	void Update () {
        if (hpSlider.value > (enemy.getCurrentHp() * hpSlider.maxValue) / maxHp )
            hpSlider.value -= 10;
        if (hpSlider.value < (enemy.getCurrentHp() * hpSlider.maxValue) / maxHp)
            hpSlider.value += 10;

    }
}
