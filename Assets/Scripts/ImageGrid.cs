using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageGrid : MonoBehaviour
{
    public GameObject GridImagePrefab;
    public GameObject GridHolder;
   

    public int GridWidth = 200;
    public int GridHeight = 200;
    public float GridHorizontalSpacing = 10.0f;
    public float GridVerticalSpacing = 10.0f;

    private void Awake()
    {
        if (GridHolder == null)
            throw new System.Exception("A GridImagePrefab must be defined");

        if (GridHolder == null)
            throw new System.Exception("A GridImagePrefab must be defined");

        CreateGrid();
    }

    private void CreateGrid()
    {
        for(int i = 0; i < GridWidth; i++)
        {
            for(int j = 0; j < GridHeight; j++)
            {
                GameObject gridPrefabObj = Instantiate(GridImagePrefab, GridHolder.transform, false);
                gridPrefabObj.transform.position = new Vector3(i * GridHorizontalSpacing, j * GridVerticalSpacing, 0);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
