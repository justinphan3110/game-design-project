using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateMonsters : MonoBehaviour {
    [System.Serializable]
    public class EnemySpawn
    {
        public string name, track;
        public float time;
    }

    public class Strategy
    {
        public string level;
        public List<EnemySpawn> enemys;
    }
    Strategy strategy;

    [HideInInspector] public Figures figures_script;
    Dictionary<string, string> map_name = new Dictionary<string, string>()
    {
        { "goblin", "GOBLIN" }, { "orc", "OrcWarrior" },
        { "skeleton", "Skeleton" }, { "greenspider", "GreenSpider" },
        { "troll", "Troll" }, { "albinodragon", "AlbinoDragon" }
    };
    int cnt_waves, current_wave, index;
    float time_generate_pass, time_clock;

    const int GENERATING = 0, CLOCKING = 1;
    const float WAVE_TIME = 15f;
    int status;

    void Start () {
        GameObject gamestate_obj = GameObject.Find("GameState");
        figures_script = gamestate_obj.GetComponent<Figures>();

        TextAsset bindata = Resources.Load("GenerateMonstersConfig") as TextAsset;
        strategy = JsonUtility.FromJson<Strategy>(bindata.text);
        current_wave = cnt_waves = 1;
        foreach (EnemySpawn enemy in strategy.enemys)
        if (enemy.time == 0)
            ++cnt_waves;

        figures_script.wave_text.text = string.Format("1/{0}", cnt_waves);

        index = 0;
        time_generate_pass = 0;
        status = GENERATING;
    }
	
	// Update is called once per frame
	void Update () {
        if (index == strategy.enemys.Count && figures_script.enemy_walkers.Count == 0 && figures_script.enemy_flyers.Count == 0)
            figures_script.victory.SetActive(true);
        switch (status)
        {
            case GENERATING:
                generating();
                break;
            case CLOCKING:
                clocking();
                break;
        }
	}

    void generating()
    {
        if (index < strategy.enemys.Count)
        if (strategy.enemys[index].time == 0)
        {
            strategy.enemys[index].time = 0.1f;
            time_clock = WAVE_TIME;
            figures_script.new_wave.SetActive(true);
            status = CLOCKING;
        }
        else
        if ((time_generate_pass += Time.deltaTime) >= strategy.enemys[index].time)
        {
            GameObject enemy_obj = (GameObject)Instantiate(Resources.Load(map_name[strategy.enemys[index].name]));
            enemy_obj.GetComponent<Enemy>().set_track(strategy.enemys[index].track);
            ++index;
        }
    }

    void clocking()
    {
        if ((time_clock -= Time.deltaTime) < 0)
        {
            time_generate_pass = 0;
            figures_script.new_wave.SetActive(false);
            figures_script.wave_text.text = string.Format("{0}/{1}", ++current_wave, cnt_waves);
            status = GENERATING;
            return;
        }

        figures_script.countdown_text.text = (Mathf.Ceil(time_clock)).ToString();
    }

    public void fight()
    {
        time_generate_pass = 0;
        figures_script.new_wave.SetActive(false);
        figures_script.wave_text.text = string.Format("{0}/{1}", ++current_wave, cnt_waves);
        status = GENERATING;
    }
}
