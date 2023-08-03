using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
[System.Serializable]
public class Level
{
    public int level;
    [AllowNesting]
    public LevelSection[] levelSections;

}
[System.Serializable]
public class LevelSection
{
    public enum SectionType
    {
        Road,
        Paint,
        Finish
    }

    public SectionType type;
    public GameObject prefab;
    public float length;

    [ShowIf("type", "SectionType.Road")]
    public float width;

    [ShowIf("type", "SectionType.Road")]
    public float propSpan;
}

