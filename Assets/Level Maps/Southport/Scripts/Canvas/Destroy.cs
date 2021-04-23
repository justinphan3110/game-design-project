using UnityEngine;
using System.Collections.Generic;

public class Destroy : MonoBehaviour {
    [HideInInspector] public Figures figures_script;

    Dictionary<string, int> cost = new Dictionary<string, int>()
    {
        { "WoodHouse", 80 }, { "ArcherTower", 90 }, { "Tower Mage", 120 }, { "Cannon", 140 },
        { "Blacksmith", 120 }
    };

    void Start()
    {
        GameObject gamestate_obj = GameObject.Find("GameState");
        figures_script = gamestate_obj.GetComponent<Figures>();
    }

    public void destroy()
    {
        Tower tower_script = (Tower)figures_script.capture_script;
        figures_script.update_coins(cost[figures_script.capture_script.name] / 2);
        tower_script.destroy();
        figures_script.clear_description();
    }
}
