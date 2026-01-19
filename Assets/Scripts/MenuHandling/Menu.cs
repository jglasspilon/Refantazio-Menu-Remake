using System.Collections;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public IEnumerator OpenPage(MenuPages page)
    {
        yield break;
    }

    public IEnumerator Teardown()
    {
        yield break;
    }
}

public enum MenuPages
{
    Main,
    Skill,
    Item,
    Equipment,
    Party,
    Follower,
    Quest,
    Calendar,
    Memorandum,
    System
}
