using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_GenerateIsland : MonoBehaviour
{
    private int randomIsland;
    public Collider islandSpawnArea;
    public GameObject[] islands;
    public int islandTotal;
    public float islandCurrent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (islandCurrent <= islandTotal) { SpawnArea(); }
        }
        if(islandCurrent <= 0 ) { islandCurrent = 0; } // prevents islandCurrent from going into negatives

        if (islandCurrent <= islandTotal) { SpawnArea(); }
    }


    // set barrier
    //check distance
    //spawn island

    private void SpawnArea()
    {
        islandCurrent++;
        //takes the min and max values from the collider and randomizes a value between them
        float islandX = Random.Range(-islandSpawnArea.bounds.extents.x, islandSpawnArea.bounds.extents.x);
        float islandZ = Random.Range(-islandSpawnArea.bounds.extents.z, islandSpawnArea.bounds.extents.z);
        Vector3 newSpawnArea = new Vector3(islandX, -1, islandZ);
        //spawns island from the random coords 
        randomIsland = Random.Range(0, islands.Length);
        
        Quaternion islandRotation = Quaternion.Euler(0, Random.Range(1, 360), 0);
        Instantiate(islands[randomIsland], newSpawnArea, islandRotation);
    }


}
