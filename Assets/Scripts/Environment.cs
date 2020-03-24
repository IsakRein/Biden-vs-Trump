using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    private GameManager gameManager;
    private bool gameActive;

    public List<GameObject> originalChildren = new List<GameObject>();

    public List<Transform> children = new List<Transform>();
    public List<Transform> childrenClones = new List<Transform>();
    private List<float> childrenSizes = new List<float>();
    public List<float> childrenSpeeds = new List<float>();


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    public void CustomStart() 
    {          
        while (transform.childCount > 0) 
        {
            Transform child = transform.GetChild(0);
            originalChildren.Add(child.gameObject);
            child.transform.parent = GameObject.Find("Environment Original").transform;
            child.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (gameActive) 
        {
            for (int i = 0; i < children.Count; i++)
            {
                Transform child = children[i];
                Transform childClone = childrenClones[i];
                float currentSizeX = childrenSizes[i];
                
                if (childrenSpeeds.Count > i) 
                { 
                    child.position += new Vector3(-childrenSpeeds[i]*Time.deltaTime, 0f, 0f);
                }
                
                if (childClone.position.x <= 0)
                {
                    GameObject.Destroy(child.gameObject);
                    children[i] = childrenClones[i];
                    originalChildren[i] = childrenClones[i].gameObject;
                    childClone = CreateClone(childrenClones[i]);
                    childrenClones[i] = childClone;
                }

                childClone.transform.position = new Vector2(child.position.x + currentSizeX - 0.1f, child.position.y);
            }
        }
    }

    public void StartGame() 
    {
        gameActive = true;

        children.Clear();
        childrenClones.Clear();
        childrenSizes.Clear();

        foreach (Transform child in transform) 
        { 
            Destroy(child.gameObject);
        }

        foreach (GameObject originalChild in originalChildren) 
        {
            GameObject childObj = Instantiate(originalChild, transform);
            childObj.SetActive(true);
            Transform child = childObj.transform;
            
            child.name = originalChild.name;

            children.Add(child);

            float currentSizeX = child.GetComponent<SpriteRenderer>().bounds.size.x;
            childrenSizes.Add(currentSizeX); 
            child.position = new Vector2((currentSizeX/2)-(gameManager.horizontalSize/2), child.position.y);

        } 

        foreach (Transform child in children) 
        {
            Transform childClone = CreateClone(child);
            childrenClones.Add(childClone);
        }
    }

    public void PauseGame() 
    {
        gameActive = false;
    }

    public void ResumeGame() 
    {
        gameActive = true;
    }

    public void Death()
    {
        gameActive = false;
    }

    Transform CreateClone(Transform child) 
    {
        float currentSizeX = child.GetComponent<SpriteRenderer>().bounds.size.x;
        GameObject childClone = Instantiate(child.gameObject, transform);
        childClone.transform.position = new Vector2(child.position.x + currentSizeX, child.position.y);
        return(childClone.transform); 
    }
}
