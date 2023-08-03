using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Level level;
    [SerializeField] PropGenerator propGenerator;
    [SerializeField] RoadGenerator roadGenerator;

    List<PaintSection> paintSections = new();
    void Start()
    {
        Vector3 startPos = transform.position;
        foreach (var levelSection in level.levelSections)
        {
            if (levelSection.type == LevelSection.SectionType.Road)
            {
                roadGenerator.Generate(levelSection, startPos);
                propGenerator.Generate(levelSection, startPos);
                startPos += Vector3.forward * levelSection.length;
            }
            if (levelSection.type == LevelSection.SectionType.Paint)
            {
                var paintSection = Instantiate(levelSection.prefab, startPos, Quaternion.identity).GetComponent<PaintSection>();
                startPos += (paintSection.endPos - paintSection.startPos);
                paintSections.Add(paintSection);
            }
            if (levelSection.type == LevelSection.SectionType.Finish)
            {
                Instantiate(levelSection.prefab, startPos, Quaternion.identity);
                // startPos += (paintSection.endPos - paintSection.startPos);
            }
        }
        var car = Instantiate(Resources.Load<GameObject>("Cars/car1"));
        var car2 = Instantiate(Resources.Load<GameObject>("Cars/car2"));
        paintSections[0].AddCar(car);
        paintSections[0].onTankEmpty += () =>
        {
            car.transform.DOScale(0, 0.3f);
        };
        paintSections[1].AddCar(car2);
        paintSections[1].onTankEmpty += () =>
        {
            car2.transform.DOScale(0, 0.3f);
        };
        // paintSections[1].onTankEmpty += () => car.SetActive(false);
    }
}
