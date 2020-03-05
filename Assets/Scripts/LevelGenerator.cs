using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public float levelScrollingSpeed = 10f;
    public List<GameObject> segmentPrefabs = new List<GameObject>();
    public List<GameObject> currentSegments = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {
        foreach (GameObject segment in currentSegments)
        {
            segment.transform.position = new Vector2(segment.transform.position.x - Time.deltaTime * levelScrollingSpeed, 0);
            
            if (segment.transform.position.x < -2*segment.transform.lossyScale.x)
            {
                currentSegments.Remove(segment);
                GameObject.Destroy(segment);
            }
        }

        while (currentSegments.Count < 5)
        {
            int index = Random.Range(0, segmentPrefabs.Count);

            GameObject segmentPrefab = GameObject.Instantiate(segmentPrefabs[index], transform);
            float previousX = currentSegments[currentSegments.Count - 1].transform.position.x;
            segmentPrefab.transform.position = new Vector2(previousX + 10f, 0);
            currentSegments.Add(segmentPrefab);
        }

    }
}
