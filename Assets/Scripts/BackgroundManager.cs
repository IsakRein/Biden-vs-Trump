using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public float groundSpeed;
    public float mainCloudSpeed;
    public float smallCloudSpeed;

    public GameObject groundPrefab;
    public GameObject mainCloudPrefab;
    public GameObject combinedCloudPrefab;
    public Transform clouds;
    public List<Sprite> cloudSprites = new List<Sprite>();

    public List<GameObject> groundObjects = new List<GameObject>();
    public List<GameObject> mainCloudObjects = new List<GameObject>();
    public List<GameObject> combinedCloudObjects = new List<GameObject>();
    public List<GameObject> cloudObjects = new List<GameObject>();

    private float mainCloudWidth;
    private float combinedCloudWidth;
    private float groundWidth;

    private List<float> cloudSpeeds = new List<float>();
    private List<float> cloudHeights = new List<float>();

    private int lastChosenSpriteIndex = 0;

    private void Start()
    {
        groundWidth = 5f * 16f; //scale of object * relation to height (1 unit height no scaling)
        mainCloudWidth = 8f * 10f; //scale of object * relation to height (1 unit height no scaling)
        combinedCloudWidth = 5.625457f * 512/36; //scale of object * relation to height (1 unit height no scaling)
    }

    void Update()
    {
        MoveGround();
        MoveMainCloud();
        MoveCombinedCloud();
    }

    void MoveGround()
    {
        foreach (GameObject ground in groundObjects)
        {
            ground.transform.position = new Vector2(ground.transform.position.x - Time.deltaTime * groundSpeed, -7.5f);

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
            segmentPrefab.transform.position = new Vector2(previousX + (groundWidth), -7.5f);
            groundObjects.Add(segmentPrefab);
        }
    }

    void MoveMainCloud()
    {
        foreach (GameObject clouds in mainCloudObjects)
        {
            clouds.transform.position = new Vector2(clouds.transform.position.x - Time.deltaTime * mainCloudSpeed, -3f);

            if (clouds.transform.position.x < -mainCloudWidth)
            {
                mainCloudObjects.Remove(clouds);
                GameObject.Destroy(clouds);
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

    void MoveCombinedCloud()
    {
        foreach (GameObject combinedClouds in combinedCloudObjects)
        {
            combinedClouds.transform.position = new Vector2(combinedClouds.transform.position.x - Time.deltaTime * smallCloudSpeed, 5.16f);

            if (combinedClouds.transform.position.x < - combinedCloudWidth)
            {
                mainCloudObjects.Remove(combinedClouds);
                GameObject.Destroy(combinedClouds);
            }
        }
         
        while (combinedCloudObjects.Count < 3)
        {
            GameObject segmentPrefab = GameObject.Instantiate(combinedCloudPrefab, transform);
            float previousX = mainCloudObjects[combinedCloudObjects.Count - 1].transform.position.x;
            segmentPrefab.transform.position = new Vector2(previousX + (mainCloudWidth), 5.16f);
            combinedCloudObjects.Add(segmentPrefab);
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


            GameObject segmentPrefab = new GameObject((cloudObjects.Count + 1).ToString());
            segmentPrefab.transform.parent = transform;
            segmentPrefab.AddComponent<SpriteRenderer>();
            segmentPrefab.GetComponent<SpriteRenderer>().sprite = cloudSprites[index];
            segmentPrefab.GetComponent<SpriteRenderer>().sortingOrder = 2;

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
