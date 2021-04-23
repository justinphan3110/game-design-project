using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public interface Tower
{
    void build(string type = "");
    void destroy();
}

public interface Character
{
    int get_status();
    bool take_hp(float damage_other, bool is_physical_other);
    float get_armor_rating();
}

public interface Hero
{
}

public interface Enemy
{
    void set_track(string track);
    Vector3 position_predict(float delta_time);
}

public interface EnemyWalker : Enemy
{
    void being_captured(Component hero_script);
}

public interface EnemyFlyer : Enemy
{

}

static class HeroStatus
{
    public const int PREPARING = 1 << 0, MAIN_ATTACKING = 1 << 1, EXTRA_ATTACKING = 1 << 2, DEATH = 1 << 3;
    public const int RANGE_MAIN_ATTACK = 5, RANGE_EXTRA_ATTACK = 8;
}

static class EnemyStatus
{
    public const int RUNNING = 1 << 0, WAITING = 1 << 1, ATTACKING = 1 << 2, DEATH = 1 << 3, PASSING = 1 << 4;
}

public class Figures : MonoBehaviour {
    public const float TIME_DEATH_NEED = 8f;
    public const float PADDING_CHARACTERS = 2.2f;

    [HideInInspector] public List<Component> towers = new List<Component>();
    [HideInInspector] public HashSet<Component> enemy_walkers = new HashSet<Component>();
    [HideInInspector] public HashSet<Component> enemy_flyers = new HashSet<Component>();
    [HideInInspector] public Component capture_script;

    public int coins, lives, wave;
    public Canvas canvas;
    public GameObject panel, stage_info, tower_info, not_enough_coin;
    public GameObject new_wave, defeat, victory, barrack_upgrade, longranger_upgrade, destroy;
    public Text coins_text, lives_text, wave_text, countdown_text;

    // Use this for initialization

    void Start () {        
        update_coins();
        update_lives();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
            clear_description();
        if (Input.GetMouseButtonDown(1))
            clear_description();
    }

    public void clear_description()
    {
        tower_info.SetActive(false);
        barrack_upgrade.SetActive(false);
        longranger_upgrade.SetActive(false);
        destroy.SetActive(false);
    }

    public bool update_coins(int different = 0)
    {
        if (coins + different < 0)
            return false;
        coins += different;
        coins_text.text = coins.ToString();
        return true;
    }

    public void update_lives(int different = 0)
    {
        lives += different;
        lives_text.text = lives.ToString();
        if (lives <= 0)
        {
            defeat.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void update_wave(int value)
    {
        wave = value;

    }
}