using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] GameObject roadPrefab;
    [SerializeField] float tileLength = 5;
    public void Generate(LevelSection levelSection, Vector3 startPos)
    {
        var road = Instantiate(roadPrefab, transform);
        road.transform.position = startPos;
        road.transform.localScale = new Vector3(levelSection.width, 1, levelSection.length);
        road.GetComponentInChildren<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, levelSection.length / 10 / tileLength));
    }
}
