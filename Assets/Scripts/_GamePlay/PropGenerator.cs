
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropGenerator : MonoBehaviour
{
    GameObject[] props;
    [SerializeField] float startDistance;


    public void Generate(LevelSection level, Vector3 startPos)
    {
        var inkBottle = Resources.Load<GameObject>("Bottles/InkBottle");
        props = Resources.LoadAll<GameObject>("Props");
        
        var bottleTrigger=  Resources.Load<GameObject>("BottleTrigger");
        int i = 0;

        GameObject lastInkBottle = null;
        for (float currentDist = startDistance; currentDist < level.length - level.propSpan -5; currentDist += level.propSpan)
        {
            var propToInstantiate = i % 2 == 0 ? props[Random.Range(0, props.Length)] : inkBottle;

            var prop = Instantiate(propToInstantiate, startPos + new Vector3(Random.Range(-1, 2) * 2, 0, currentDist), propToInstantiate.transform.rotation);
            if (i % 2 != 0)
            {
                lastInkBottle = prop;
            }

            if (Random.Range(0, 1f) > 0.5f)
            {
                var sideMovement = prop.AddComponent<SideMovement>();
                sideMovement.cycleOffset = Random.Range(0, 1f);
                sideMovement.span = level.width * 0.5f;
                sideMovement.speed = GameManager.Instance.gameData.obstacleSpeed;
            }
            i++;
        }

        Instantiate(bottleTrigger, lastInkBottle.transform.position, Quaternion.identity);
    }
}
