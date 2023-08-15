using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
[System.Serializable]
public class Level
{

    [AllowNesting]
    public LevelSection[] levelSections;
    public Car carPrefab;
    public Color paintColor;
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

