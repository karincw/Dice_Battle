namespace Shy
{
    public enum Team
    {
        None,
        Player,
        Enemy
    }

    public enum TargetWay
    {
        team, //아군
        Opponenet //적군
    }

    public enum EventType
    {
        AttackEvent,
        ShieldEvent,
        HealEvent,
        BuffEvent
    }

    public enum BuffType
    {
        Brave = 0,
        Bleeding,
        Gingerbread,
        Crumbs,
        Bondage,
        Burn,
        Music,
        Confusion,
        Shield,
    }

    public enum BuffUseCondition
    {
        OnStart,
        Update,
        OnEnd,
        OnAttack,
        OnHit,
        None
    }

    public enum BuffRemoveCondition
    {
        Count,
        Use,
        Never,
    }

    public enum UpgradeCondition
    {
        SelfStack, // 특정 스택
        SelfHp // 체력
    }

    public enum StatEnum
    {
        MaxHp,
        Hp,
        Str,
        Def,
        AdditionalDmg,
        ReductionDmg
    }

    public enum PoolingType
    {
       Buff,
       DmgText,
       end
    }

    public enum ActionWay
    {
        None,
        Self,
        Opposite,//맞은편
        Random, //무작위
        All, //전체
        LessHp,
        MoreHp,
        Already,
        Fast,
        Slow,
    }

    public enum SkillMotion
    {
        AttackNear = 0,
        AttackLong,
        PetAttackNear,
        PetAttackLong,
        TeamBySelf,
        TeamByPet,
        EveryOne,
    }

    public enum PetSpawnWay
    {
        Normal,
        Create,
        Drop
    }

    public enum InfoType
    {
        Minion,
        Enemy,
        Buff,
        Dice
    }

    namespace Anime
    {
        public enum AnimeType
        {
            UserVisual,
            UserAnime,
            TeamEffect,
            OpponentEffect,
            SummonVisual,
            SummonAnime
        }
    }
}