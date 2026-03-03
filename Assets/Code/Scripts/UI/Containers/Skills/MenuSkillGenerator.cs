using UnityEngine;

public class MenuSkillGenerator: UIObjectGeneraterFromPool<MenuSkill, Skill>
{
    [SerializeField]
    private MenuSkill m_prefab;

    protected override void GeneratePoolableFromData(Skill skill)
    {
        MenuSkill newSkill = m_assetPool.PullFrom(m_prefab.GetType(), m_holder) as MenuSkill;
        newSkill.InitializeFromData(skill);
        m_content.Add(newSkill);
    }
}
