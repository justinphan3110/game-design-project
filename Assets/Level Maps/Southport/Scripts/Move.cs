using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    Animation animation;
    Animator animator;
    Vector3[] points;
    Vector3 current_position, target_position;
    float length, padding_bottom, time_pass, time_total;

	// Use this for initialization
	void Start () {
        return;
        //animation = GetComponent<Animation>();
        animator = GetComponent<Animator>();
        points = iTweenPath.GetPath("curve2");
        length = iTween.PathLength(points);
        time_pass = 0;
        time_total = 50;
        padding_bottom = 0.2f;

        Debug.Log("Count " + points.Length);
        Debug.Log("Len " + length);
    }
	
	// Update is called once per frame
	void Update () {
        return;
        /*
        if (Input.GetKey(KeyCode.G))
        {
            GameObject building = GameObject.Find("Building");
            building.transform.Find("WoodHouse").gameObject.SetActive(true);
            building.transform.Find("ruinhouse").gameObject.SetActive(false);
        }
        */
        if (Input.GetKey(KeyCode.F))
        {
            //animation.Play("run");
            animator.Play("WK_heavy_infantry_04_charge");
            target_position = iTween.PointOnPath(points, (time_pass += Time.deltaTime) / time_total);
            target_position.y = Terrain.activeTerrain.SampleHeight(target_position);
            transform.position = target_position;

            transform.rotation = Quaternion.LookRotation(target_position - current_position);
            current_position = target_position;
        }
        else
            animator.Play("WK_heavy_infantry_08_attack_B");
        //animation.Play("attack1");
    }
}
