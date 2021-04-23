using UnityEngine;
using System.Collections.Generic;
public class Build : MonoBehaviour
{
    [HideInInspector] public Figures figures_script;
    [HideInInspector] public UIControl UIControl_script;
    Dictionary<string, int> cost = new Dictionary<string, int>()
    {
        { "Barrack", 80 }, { "LongRanger", 90 }, { "Mage", 120 }, { "WideRanger", 140 },
        { "BarrackUpgrade", 120 }
    };

    void Start()
    {
        GameObject gamestate_obj = GameObject.Find("GameState");
        figures_script = gamestate_obj.GetComponent<Figures>();
        UIControl_script = figures_script.panel.GetComponent<UIControl>();
    }
    public void build()
    {
        //Check and update coins
        if (!figures_script.update_coins(-cost[gameObject.name]))
        {
            UIControl_script.showError();
            return;
        }
        //Build
        Tower tower_script = (Tower)figures_script.capture_script;
        tower_script.build(gameObject.name);
        figures_script.clear_description();
    }
}