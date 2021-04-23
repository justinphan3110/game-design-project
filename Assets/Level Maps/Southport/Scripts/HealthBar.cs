using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
    public Transform fill;
	// Use this for initialization
    public void UpdateBar(float health)
    {
        if (health < 0)
            health = 0;
        fill.localScale = new Vector3(health, 1f, 1f);
    }

}
