using UnityEngine;
using System.Collections;

public class Fight : MonoBehaviour {
    GenerateMonsters generate_monsters;
	void Start()
    {
        GameObject gamestate_obj = GameObject.Find("GameState");
        generate_monsters = gamestate_obj.GetComponent<GenerateMonsters>();
    }

	public void fight()
    {
        generate_monsters.fight();
    }
}
