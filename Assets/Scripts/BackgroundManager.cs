using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public float groundSpeed;
    public float mainCloudSpeed;
    public float smallCloudSpeed;

    public GameObject groundPrefab;
    public List<GameObject> groundObjects = new List<GameObject>();
    public float groundWidth;

    public GameObject mainCloudPrefab;
    public List<GameObject> mainCloudObjects = new List<GameObject>();
    public float mainCloudWidth;

    public Transform clouds;
    public List<Sprite> cloudSprites = new List<Sprite>();
    public List<GameObject> cloudObjects = new List<GameObject>();

    public List<float> cloudSpeeds = new List<float>();
    public List<float> cloudHeights = new List<float>();

    private int lastChosenSpriteIndex = 0;

    private void Start()
    {
        groundWidth = 4f * 16f; //scale of object * relation to height (1 unit height no scaling)
        mainCloudWidth = 8f * 10f; //scale of object * relation to height (1 unit height no scaling)
    }

    void Update()
    {
        MoveGround();
        MoveClouds();
        MoveSmallClouds();
    }

    void MoveGround()
    {
        foreach (GameObject ground in groundObjects)
        {
            ground.transform.position = new Vector2(ground.transform.position.x - Time.deltaTime * groundSpeed, -8f);

            if (ground.transform.position.x < -groundWidth)
            {
                groundObjects.Remove(ground);
                GameObject.Destroy(ground);
            }
        }

        while (groundObjects.Count < 3)
        {
            int index = Random.Range(0, groundObjects.Count);

            GameObject segmentPrefab = GameObject.Instantiate(groundObjects[index], transform);
            float previousX = groundObjects[groundObjects.Count - 1].transform.position.x;
            segmentPrefab.transform.position = new Vector2(previousX + (groundWidth), -8f);
            groundObjects.Add(segmentPrefab);
        }
    }

    void MoveClouds()
    {
        foreach (GameObject mainCloud in mainCloudObjects)
        {
            mainCloud.transform.position = new Vector2(mainCloud.transform.position.x - Time.deltaTime * mainCloudSpeed, -3f);

            if (mainCloud.transform.position.x < -mainCloudWidth)
            {
                mainCloudObjects.Remove(mainCloud);
                GameObject.Destroy(mainCloud);
            }
        }

        while (mainCloudObjects.Count < 3)
        {
            int index = Random.Range(0, mainCloudObjects.Count);

            GameObject segmentPrefab = GameObject.Instantiate(mainCloudObjects[index], transform);
            float previousX = mainCloudObjects[mainCloudObjects.Count - 1].transform.position.x;
            segmentPrefab.transform.position = new Vector2(previousX + (mainCloudWidth), -3f);
            mainCloudObjects.Add(segmentPrefab);
        }
    }


    void MoveSmallClouds()
    {
        for (int i = 0; i < cloudObjects.Count; i++)
        {
            GameObject cloud = cloudObjects[i];
            cloud.transform.position = new Vector2(cloud.transform.position.x - Time.deltaTime * cloudSpeeds[i], cloudHeights[i]);

            if (cloud.transform.position.x < -15f)
            {
                cloudObjects.RemoveAt(i);
                GameObject.Destroy(cloud);

                cloudHeights.RemoveAt(i);
                cloudSpeeds.RemoveAt(i);
            }
        } 

        while (cloudObjects.Count < 8)
        {
            int index = Random.Range(0, cloudSprites.Count);
            while (index == lastChosenSpriteIndex)
            {
                index = Random.Range(0, cloudSprites.Count);
            }
            lastChosenSpriteIndex = index;


            GameObject emptyGameObject = new GameObject();

            GameObject segmentPrefab = GameObject.Instantiate(emptyGameObject, transform);
            segmentPrefab.AddComponent<SpriteRenderer>();
            segmentPrefab.GetComponent<SpriteRenderer>().sprite = cloudSprites[index];

            float previousX;
            if (cloudObjects.Count > 0)
            {
                previousX = cloudObjects[cloudObjects.Count - 1].transform.position.x;
            }
            else
            {
                previousX = 0f;
            }

            float width = 5f + 5f * Random.Range(-0.2f, 0.2f);
            float height = 6.5f + 5f * Random.Range(-0.5f, 0.5f);
            float speed = smallCloudSpeed + smallCloudSpeed * Random.Range(-0.5f, 0.5f);
            cloudHeights.Add(height);
            cloudSpeeds.Add(speed);

            segmentPrefab.transform.position = new Vector2(previousX + width, height);
            cloudObjects.Add(segmentPrefab);
        }
    }
}
