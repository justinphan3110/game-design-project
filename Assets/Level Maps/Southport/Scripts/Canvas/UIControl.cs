using UnityEngine;
using System.Collections;
public class UIControl : MonoBehaviour
{
    [HideInInspector] public Figures figures_script;
    // Use this for initialization
    float time_showing = 3f;
    void Start()
    {
        GameObject gamestate_obj = GameObject.Find("GameState");
        figures_script = gamestate_obj.GetComponent<Figures>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject error_msg = figures_script.not_enough_coin;
        if (time_showing >= 3f)
            error_msg.SetActive(false);
        else
        {
            time_showing += Time.deltaTime;
            error_msg.SetActive(true);
        }
    }
    public void showError()
    {
        time_showing = 0f;
    }
}