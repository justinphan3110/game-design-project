using UnityEngine;
using System.Collections.Generic;
using System;

public class Blacksmith : MonoBehaviour, Tower
{
    [HideInInspector] public Figures figures_script;
    public int number_hero, respawn;
    public Vector3[] prepare_points = new Vector3[3] { Vector3.zero, Vector3.zero, Vector3.zero };

    LinkedList<float> time_remains = new LinkedList<float>();
    LinkedList<Vector3> next_prepare_points = new LinkedList<Vector3>();


    // Use this for initialization
    void Start()
    {
        GameObject gamestate_obj = GameObject.Find("GameState");
        figures_script = gamestate_obj.GetComponent<Figures>();
    }

    // Update is called once per frame
    void Update()
    {
        respawning();
    }

    void OnMouseDown()
    {
        figures_script.clear_description();
        figures_script.destroy.SetActive(true);
        figures_script.capture_script = this;
    }

    void respawning()
    {
        while (time_remains.Count > 0 && Time.time - time_remains.First.Value >= respawn)
        {
            generate_hero(next_prepare_points.First.Value);
            time_remains.RemoveFirst();
            next_prepare_points.RemoveFirst();
        }
    }

    public void add_hero(Footman hero)
    {
        time_remains.AddLast(Time.time);
        next_prepare_points.AddLast(hero.prepare_point);
    }

    public void on_active()
    {
        for (int i = 0; i < number_hero; ++i)
            generate_hero(prepare_points[i]);
    }

    void generate_hero(Vector3 prepare_point)
    {
        GameObject knight_obj = (GameObject)
            Instantiate(Resources.Load("Footman"),
                transform.position + UnityEngine.Random.insideUnitSphere * 3,
                Quaternion.identity);

        Footman knight_script = knight_obj.GetComponent<Footman>();
        knight_script.blacksmith = GetComponent<Blacksmith>();
        knight_script.prepare_point = prepare_point;
    }

    void Tower.build(string type)
    {

    }

    void Tower.destroy()
    {
        gameObject.SetActive(false);
        transform.parent.Find("RuinHouse").gameObject.SetActive(true);
    }
}