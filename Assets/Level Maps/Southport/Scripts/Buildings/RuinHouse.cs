using System;
using UnityEngine;

public class RuinHouse : MonoBehaviour, Tower
{
    // Use this for initialization
    Figures figures_script;
    GameObject flag_obj;
    void Start()
    {
        GameObject gamestate_obj = GameObject.Find("GameState");
        figures_script = gamestate_obj.GetComponent<Figures>();
        flag_obj = transform.Find("flag_med").gameObject;
        figures_script.towers.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
            }
        }
        */
        
    }

    void OnMouseDown()
    {
        figures_script.clear_description();
        figures_script.tower_info.SetActive(true);
        figures_script.capture_script = this;
    }

    void OnMouseEnter()
    {
        flag_obj.transform.localScale *= 1.5f;
    }

    void OnMouseExit()
    {
        flag_obj.transform.localScale /= 1.5f;
    }

    void Tower.build(string type)
    {
        GameObject new_tower = null;
        switch (type)
        {
            case "Barrack":
                new_tower = transform.parent.Find("WoodHouse").gameObject;
                new_tower.GetComponent<WoodHouse>().on_active();
                break;
            case "LongRanger":
                new_tower = transform.parent.Find("ArcherTower").gameObject;
                break;
            case "Mage":
                new_tower = transform.parent.Find("Tower Mage").gameObject;
                break;
            case "WideRanger":
                new_tower = transform.parent.Find("Cannon").gameObject;
                break;
        }
        gameObject.SetActive(false);
        new_tower.SetActive(true);
    }

    void Tower.destroy()
    {
    }
}
