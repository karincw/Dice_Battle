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
        team, //�Ʊ�
        Opponenet //����
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
        Bondage,
        Crumbs,
        Gingerbread,
        Music,
    }

    public enum BuffUseCondition
    {
        OnStart,
        Update,
        OnEnd,
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
        SelfStack, // Ư�� ����
        SelfHp // ü��
    }

    public enum StatEnum
    {
        MaxHp,
        Hp,
        Str,
        Def
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
        Opposite,//������
        Random, //������
        All, //��ü
        LessHp,
        MoreHp,
        Already,
        Fast,
        Slow,
    }

    public enum SkillMotion
    {
        Near = 0,
        Long,
        All,
        Self,
        PetSelf,
        PetAndShort,
        PetAndLong,
        SummonAndShort,
        SummonAndLong,
        DropAttack,
    }
}
