using UnityEngine;

public class GreenSpider : MonoBehaviour, Character, EnemyWalker
{
    [HideInInspector] public Figures figures_script;
    [HideInInspector] public Component target_hero;
    [HideInInspector] public string track;

    public float hp, damage, armor_rating, magic_resistence, time_move_need;
    public int lives_taken, bounty;
    float hp_loss;
    int status;

    HealthBar health_bar;
    Animation animation;
    Vector3[] points;
    Vector3 current_position, target_position;
    float length_curve, time_move_pass;

    // Use this for initialization
    void Start()
    {
        GameObject gamestate_obj = GameObject.Find("GameState");
        figures_script = gamestate_obj.GetComponent<Figures>();
        figures_script.enemy_walkers.Add(GetComponent<GreenSpider>());

        gameObject.tag = "Walker";
        status = EnemyStatus.RUNNING;

        health_bar = gameObject.transform.Find("HealthBar").GetComponent<HealthBar>();

        animation = GetComponent<Animation>();
        points = iTweenPath.GetPath(track);
        length_curve = iTween.PathLength(points);
        time_move_pass = 0;
        hp_loss = 0;
    }

    // Update is called once per frame
    void Update()
    {

        health_bar.UpdateBar((float)(hp - hp_loss) / hp);
        switch (status)
        {
            case EnemyStatus.PASSING:
                passing();
                break;
            case EnemyStatus.DEATH:
                animation.Play("Death");
                Destroy(gameObject, GetComponent<Animation>().clip.length + 0.4f);
                break;
            case EnemyStatus.RUNNING:
                running();
                break;
            case EnemyStatus.WAITING:
                waiting();
                break;
            case EnemyStatus.ATTACKING:
                attacking();
                break;
        }
    }

    void passing()
    {
        figures_script.enemy_walkers.Remove(this);
        figures_script.update_lives(-lives_taken);
        Destroy(gameObject);
    }

    void running()
    {
        animation.Play("Run");
        target_position = iTween.PointOnPath(points, (time_move_pass += Time.deltaTime) / time_move_need);
        target_position.y = Terrain.activeTerrain.SampleHeight(target_position);
        transform.position = target_position;

        transform.rotation = Quaternion.LookRotation(target_position - current_position);
        current_position = target_position;

        if (time_move_pass >= time_move_need)
            status = EnemyStatus.PASSING;
    }

    void waiting()
    {
        try
        {
            if (Vector3.Distance(transform.position, target_hero.transform.position) < Figures.PADDING_CHARACTERS)
                status = EnemyStatus.ATTACKING;
            else
                animation.Play("Idle");
        }
        catch
        {
            status = EnemyStatus.RUNNING;
        }
    }

    void attacking()
    {
        try
        {
            animation.Play("Attack");
            if (target_hero == null || !((Character)target_hero).take_hp(Time.deltaTime * damage, true))
                status = EnemyStatus.RUNNING;
        }
        catch
        {
            status = EnemyStatus.RUNNING;
        }
    }

    int Character.get_status()
    {
        return status;
    }

    float Character.get_armor_rating()
    {
        return armor_rating;
    }

    void EnemyWalker.being_captured(Component hero_script)
    {
        if (status == EnemyStatus.RUNNING)
        {
            target_hero = hero_script;
            transform.rotation = Quaternion.LookRotation(target_hero.transform.position - transform.position);
            status = EnemyStatus.WAITING;
        }
    }

    bool Character.take_hp(float damage_other, bool is_physical_other)
    {
        if (hp_loss >= hp)
            return false;
        float different = damage_other;
        different *= 1f - (is_physical_other ? armor_rating : magic_resistence);
        hp_loss += different;

        bool is_living = hp_loss < hp;
        if (!is_living)
        {
            figures_script.update_coins(bounty);
            figures_script.enemy_walkers.Remove(this);
            status = EnemyStatus.DEATH;
        }

        return is_living;
    }

    Vector3 Enemy.position_predict(float delta_time)
    {
        if (status == EnemyStatus.RUNNING)
        {
            target_position = iTween.PointOnPath(points, (time_move_pass + delta_time) / time_move_need);
            target_position.y = Terrain.activeTerrain.SampleHeight(target_position);
            return target_position;
        }

        return transform.position;
    }

    void Enemy.set_track(string track)
    {
        this.track = track;
    }
}
