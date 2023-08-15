using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Level[] levels;
    Level level;
    [SerializeField] PropGenerator propGenerator;
    [SerializeField] RoadGenerator roadGenerator;

    public Level CurrentLevel => levels[GameManager.Instance.gameData.currentLevel % levels.Length];
    List<PaintSection> paintSections = new();
    public void Generate()
    {
        level = CurrentLevel;

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
        var car = Instantiate(level.carPrefab).gameObject;
        paintSections[0].AddCar(car);
        paintSections[0].OnTankEmpty += () =>
        {

            paintSections[0].AddCar(Instantiate(car));
            paintSections[1].AddCar(car, true);
        };
        car.GetComponent<Car>().onPainted += () =>
        {
            foreach (var paintSection in paintSections)
            {
                paintSection.DisableTrigger();
            }
        };

        CanvasManager.Instance.SetPaintProgressTrackingObject(car);
    }
}
