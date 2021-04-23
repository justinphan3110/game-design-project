using System;
using UnityEngine;

public class TowerMage : MonoBehaviour, Tower
{
    public const int PREPARING = 0, STARING = 1, SHOOTING = 2;

    [HideInInspector] public Figures figures_script;

    public float damage, fire_range, shoot_time;
    float time_shoot_pass;
    int status;

    Component capture_enemy;
    GameObject arrow_obj;
    Vector3 floor_tower, peek_tower, direct_arrow, target_position, current_position;
    void Start()
    {
        GameObject gamestate_obj = GameObject.Find("GameState");
        figures_script = gamestate_obj.GetComponent<Figures>();

        status = PREPARING;
        peek_tower = transform.TransformPoint(new Vector3(0f, 1.2f, 0));
        floor_tower = peek_tower;
        floor_tower.y = 0f;

        arrow_obj = (GameObject)Instantiate(Resources.Load("MageBullet"));
        arrow_obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (status)
        {
            case PREPARING:
                preparing();
                break;
            case STARING:
                staring();
                break;
            case SHOOTING:
                shooting();
                break;
        }
    }

    void OnMouseDown()
    {
        figures_script.clear_description();
        figures_script.destroy.SetActive(true);
        figures_script.capture_script = this;
    }
    bool finding_enemy(int enemy_status)
    {
        Character character;
        foreach (Component capture_enemy in figures_script.enemy_walkers)
        {
            character = (Character)capture_enemy;
            if ((character.get_status() & enemy_status) == 0)
                continue;
            if (Vector3.Distance(floor_tower, capture_enemy.transform.position) < fire_range)
            {
                this.capture_enemy = capture_enemy;
                return true;
            }
        }

        return false;
    }

    void preparing()
    {
        if (finding_enemy(EnemyStatus.RUNNING | EnemyStatus.WAITING | EnemyStatus.ATTACKING))
        {
            arrow_obj.SetActive(true);
            status = STARING;
        }
    }

    void staring()
    {
        try
        {
            if (((Character)capture_enemy).get_status() == EnemyStatus.DEATH)
            {
                arrow_obj.SetActive(false);
                status = PREPARING;
                return;
            }

            time_shoot_pass = 0;
            arrow_obj.transform.position = current_position = peek_tower;
            Vector3 enemy_center = ((Enemy)capture_enemy).position_predict(shoot_time);
            direct_arrow = enemy_center - peek_tower;
            status = SHOOTING;
        }
        catch
        {
            arrow_obj.SetActive(false);
            status = PREPARING;
        }
    }

    void shooting()
    {
        try
        {
            ((Character)capture_enemy).take_hp(Time.deltaTime * damage / shoot_time, false);
        }
        catch
        { }

        if ((time_shoot_pass += Time.deltaTime) < shoot_time)
        {
            target_position = current_position + direct_arrow * Time.deltaTime / shoot_time;

            arrow_obj.transform.rotation = Quaternion.LookRotation(target_position - current_position);
            arrow_obj.transform.Rotate(Vector3.right * 90);
            arrow_obj.transform.position = target_position;

            current_position = target_position;
        }
        else
            status = STARING;
    }

    void Tower.build(string type)
    {
        return;
    }

    void Tower.destroy()
    {
        gameObject.SetActive(false);
        transform.parent.Find("RuinHouse").gameObject.SetActive(true);
    }
}
