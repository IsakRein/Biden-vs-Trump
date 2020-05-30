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
    private List<float> childrenPosY = new List<float>();
    public List<float> childrenSpeeds = new List<float>();
    public List<bool> childrenFollowCamera = new List<bool>();
    public Transform camera;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        childrenSizes.Clear();

        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            originalChildren.Add(child.gameObject);
            childrenSizes.Add(child.GetComponent<SpriteRenderer>().bounds.size.x); 
            child.transform.parent = GameObject.Find("Environment Original").transform;
            child.gameObject.SetActive(false);
        }
    }

    public void CustomStart()
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            GameObject.Destroy(child.gameObject);
        }
    }
    
    public void StartGame() 
    {
        gameActive = true;

        children.Clear();
        childrenClones.Clear();
        childrenPosY.Clear();

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
            childrenPosY.Add(child.position.y);
            child.localPosition = new Vector2(0, child.localPosition.y);
        } 

        foreach (Transform child in children) 
        {
            Transform childClone = CreateClone(child);
            childClone.name = "1";
            childrenClones.Add(childClone);
        }
    }
  
    void Update()
    {
        transform.position = new Vector2(transform.position.x, 0);

        if (gameActive) 
        {
            for (int i = 0; i < children.Count; i++)
            {
                Transform child = children[i];
                Transform childClone = childrenClones[i];
                float currentSizeX = childrenSizes[i];
                
                if (childrenSpeeds.Count > i) 
                { 
                    float x = child.localPosition.x - childrenSpeeds[i]*Time.deltaTime;
                    float y;

                    if (childrenFollowCamera[i]) { y = childrenPosY[i] + Camera.main.transform.position.y; }
                    else { y = child.localPosition.y; }

                    child.localPosition = new Vector3(x, y, 0f);
                }
                
                if (childClone.localPosition.x <= 0)
                {
                    GameObject.Destroy(child.gameObject);
                    children[i] = childrenClones[i];
                    childClone = CreateClone(childrenClones[i]);
                    childClone.name = "1";
                    childrenClones[i] = childClone;
                }

                childClone.transform.localPosition = new Vector2(child.localPosition.x + currentSizeX - 0.1f, child.localPosition.y);
            }
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
