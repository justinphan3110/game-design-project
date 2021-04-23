using UnityEngine;

public class ToonRTS_demo_Knight : MonoBehaviour, Character, Hero
{
    const double EPSILON = 0.1;

    [HideInInspector]
    public WoodHouse woodhouse;
    [HideInInspector]
    public Vector3 prepare_point;

    public float hp, damage, armor_rating, velocity_run;
    float hp_loss;
    int status;

    HealthBar health_bar;
    Component capture_enemy;
    Animator animator;
    Vector3 direct, current_position, target_position;

    // Use this for initialization
    void Start()
    {
        health_bar = gameObject.transform.Find("HealthBar").GetComponent<HealthBar>();

        status = HeroStatus.PREPARING;
        animator = GetComponent<Animator>();
        current_position = transform.position;
        current_position.y = 0;
        hp_loss = 0;
    }

    // Update is called once per frame
    void Update()
    {
        health_bar.UpdateBar((float)(hp - hp_loss) / hp);
        if (woodhouse.gameObject.activeInHierarchy == false)
            DestroyImmediate(gameObject);
        else
        switch (status)
        {
            case HeroStatus.DEATH:
                animator.Play("Death");
                Destroy(gameObject, 5f);
                break;
            case HeroStatus.PREPARING:
                preparing();
                break;
            case HeroStatus.MAIN_ATTACKING:
                attacking();
                break;
            case HeroStatus.EXTRA_ATTACKING:
                if (finding_enemy(HeroStatus.RANGE_EXTRA_ATTACK, EnemyStatus.RUNNING))
                {
                    EnemyWalker enemy_walker = (EnemyWalker)capture_enemy;
                    enemy_walker.being_captured(GetComponent<ToonRTS_demo_Knight>());
                    transform.rotation = Quaternion.LookRotation(capture_enemy.transform.position - transform.position);
                    status = HeroStatus.MAIN_ATTACKING;
                }
                else
                    attacking();
                break;
        }
    }

    bool finding_enemy(int hero_range, int enemy_status)
    {
        float nearest_distance = Mathf.Infinity;
        float distance;

        Character character;
        Component nearest_enemy = null;
        foreach (Component capture_enemy in woodhouse.figures_script.enemy_walkers)
        {
            character = (Character)capture_enemy;
            if ((character.get_status() & enemy_status) == 0)
                continue;
            distance = Vector3.Distance(transform.position, capture_enemy.transform.position);
            if (distance < nearest_distance)
            {
                nearest_distance = distance;
                nearest_enemy = capture_enemy;
            }
        }

        if (nearest_distance > hero_range)
            return false;

        this.capture_enemy = nearest_enemy;
        return true;
    }

    void preparing()
    {
        if (finding_enemy(HeroStatus.RANGE_MAIN_ATTACK, EnemyStatus.RUNNING))
        {
            EnemyWalker enemy_walker = (EnemyWalker)capture_enemy;
            enemy_walker.being_captured(GetComponent<ToonRTS_demo_Knight>());
            transform.rotation = Quaternion.LookRotation(capture_enemy.transform.position - transform.position);
            status = HeroStatus.MAIN_ATTACKING;
        }
        else
        if (finding_enemy(HeroStatus.RANGE_EXTRA_ATTACK, EnemyStatus.ATTACKING | EnemyStatus.WAITING))
        {
            EnemyWalker enemy_walker = (EnemyWalker)capture_enemy;
            enemy_walker.being_captured(GetComponent<ToonRTS_demo_Knight>());
            transform.rotation = Quaternion.LookRotation(capture_enemy.transform.position - transform.position);
            status = HeroStatus.EXTRA_ATTACKING;
        }
        else
        if (locating_prepare_point())
            ideling();
        else
            reaching_point(prepare_point);
    }

    void attacking()
    {
        try
        {
            if (Vector3.Distance(transform.position, capture_enemy.transform.position) < Figures.PADDING_CHARACTERS)
            {
                animator.Play("WK_heavy_infantry_08_attack_B");
                if (!((Character)capture_enemy).take_hp(Time.deltaTime * damage, true))
                    status = HeroStatus.PREPARING;
            }
            else
                reaching_point(capture_enemy.transform.position);
        }
        catch
        {
            status = HeroStatus.PREPARING;
        }
    }

    void reaching_point(Vector3 reach_point)
    {
        animator.Play("WK_heavy_infantry_04_charge");
        direct = reach_point - current_position;
        target_position = current_position + direct.normalized * Time.deltaTime * velocity_run;
        target_position.y = Terrain.activeTerrain.SampleHeight(target_position);

        transform.rotation = Quaternion.LookRotation(target_position - current_position);
        transform.position = target_position;
        current_position = target_position;
    }

    bool locating_prepare_point()
    {
        Vector3 pos = transform.position;
        pos.y = 0;
        return Vector3.Distance(pos, prepare_point) < EPSILON;
    }

    void ideling()
    {
        animator.Play("WK_heavy_infantry_05_combat_idle");
    }

    int Character.get_status()
    {
        return status;
    }

    float Character.get_armor_rating()
    {
        return armor_rating;
    }

    bool Character.take_hp(float damage_other, bool is_physical_other)
    {
        float different = damage_other;
        if (is_physical_other)
            different *= 1f - armor_rating;
        hp_loss += different;

        bool is_living = hp_loss < hp;

        if (!is_living)
        {
            if (woodhouse.gameObject.activeInHierarchy == true)
                woodhouse.add_hero(this);
            status = HeroStatus.DEATH;
        }

        return is_living;
    }
}