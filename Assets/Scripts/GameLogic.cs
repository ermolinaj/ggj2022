using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    //Between 0 and #
    int sunPosition;
    int[ , , ] gridWorld = new int[4, 10, 10];
    float coordinationSpace = 0.25f;
    int secondsAlive = 0;
    bool areYaWinningSon = true;

    List<Zone> zones;

    // Start is called before the first frame update
    void Start()
    {
        zones = new List<Zone>() { new Zone (), new Zone (), new Zone (), new Zone () };
        sunPosition = 0;
        UpdateSunVisibility();

        InvokeRepeating("UpdateWorldSunnyParts", 0f, 0.1f);  
        InvokeRepeating("UpdateWorldTemperature", 0f, 0.1f); 
        InvokeRepeating("UpdateWorldPopulation", 0f, 0.1f);
        InvokeRepeating("UpdateWorldConflict", 0f, 0.1f);  
        InvokeRepeating("LogWorldStatus", 0f, 0.1f);
        InvokeRepeating("UpdateSeconds", 0f, 1f);
        InvokeRepeating("CheckWinConditionAndUpdateIt", 0f, 1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeSunPositionRight();
            UpdateSunVisibility();
        }
        
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeSunPositionLeft();
            UpdateSunVisibility();
        }
            
    }

    void LogWorldStatus()
    {
        Debug.Log("Sun Position: " + sunPosition.ToString() + ". Zones Affected by sun: " + ZonesAffectedBySun()[0].ToString()   + " " +  ZonesAffectedBySun()[1].ToString());
        int i = 0;
        foreach(Zone z in zones)
        {
            Debug.Log("Zone " + i.ToString() + ". Human Population: " + z.humanPopulation.ToString() + ". Monster Population: " + z.monsterPopulation.ToString() +". Temperature: " + z.temperature.ToString() + ".");
            i ++;
        }
    }

    void UpdateSeconds()
    {
        if (areYaWinningSon)
            secondsAlive++;
    }

    void CheckWinConditionAndUpdateIt()
    {
        int doomedZones = 0;
        foreach(Zone z in zones)
        {
            if (z.humanPopulation + z.monsterPopulation == 100)
                doomedZones ++;                
        }

        if (doomedZones == 4)
        {
            areYaWinningSon = false;
            GameObject.Find("endscreen").GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = secondsAlive.ToString();
            GameObject.Find("endscreen").GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void UpdateWorldSunnyParts()
    {
        int[] sunAffectedZones;

        foreach (Zone z in zones)
            z.isDaytime = false;
        
        sunAffectedZones = ZonesAffectedBySun();

        foreach(int z in sunAffectedZones)
        {
            zones[z].isDaytime = true;
        }

    }

    void UpdateWorldTemperature()
    {
        foreach (Zone z in zones)
        {
            if (z.isDaytime)
                z.changeTemperature(1);
            else
                z.changeTemperature(-1);
        }
    }

    void UpdateWorldPopulation()
    {
        int zoneNumber = 0;

        foreach (Zone z in zones)
        {
            if (z.IsSummer() && z.humanPopulation <= 100 && z.humanPopulation + z.monsterPopulation < 100)
            {
                z.humanPopulation ++;
                DrawHuman(zoneNumber);     
            }
            if (z.IsWinter() && z.monsterPopulation < 100 && z.monsterPopulation + z.humanPopulation < 100)
            {
                z.monsterPopulation ++;
                DrawMonster(zoneNumber);
            }
            zoneNumber ++;
        }
    }

    void UpdateWorldConflict()
    {
        int zoneNumber = 0;
        foreach (Zone z in zones)
        {
            if (z.humanPopulation + z.monsterPopulation == 100 && (z.humanPopulation != 0 && z.monsterPopulation != 0))
            {
                if (z.humanPopulation > z.monsterPopulation)
                {
                    z.monsterPopulation = 0;
                    if (z.humanPopulation + 5 < 100)
                        z.humanPopulation += 5;
                    else
                        z.humanPopulation = 100;

                    
                    for(int h = 0; h < 10; h++)
                    {
                        for (int w = 0; w < 10; w++)
                        {
                            gridWorld[zoneNumber, h, w] = 0;
                        }
                    }

                    DeleteUnits(zoneNumber);

                    for (int i = 1; i <= z.humanPopulation; i ++)
                        DrawHuman(zoneNumber);
                }
                if (z.monsterPopulation > z.humanPopulation)
                {
                    z.humanPopulation = 0;
                    if (z.monsterPopulation + 5 < 100)
                        z.monsterPopulation += 5;
                    else
                        z.monsterPopulation = 100;

                    for(int h = 0; h < 10; h++)
                    {
                        for (int w = 0; w < 10; w++)
                        {
                            gridWorld[zoneNumber, h, w] = 0;
                        }
                    }

                    DeleteUnits(zoneNumber);
                    
                    for (int i = 1; i <= z.monsterPopulation; i ++)
                        DrawMonster(zoneNumber);
                }
            }
            zoneNumber ++;
        }
    }
     
    int[] ZonesAffectedBySun()
    {
        switch(sunPosition)
        {
            case 0:
                return new int[] {0, 1};
            case 1:
                return new int[] {1, 2};
            case 2:
                return new int[] {2, 3};
            case 3:
                return new int[] {3, 1};
            default:
                return new int[] {0, 1};
        }
    }

    void UpdateSunVisibility()
    {
        for (int i = 0; i < 4; i ++)
             GameObject.Find("Sun" + i.ToString()).GetComponent<SpriteRenderer>().enabled = false;
        
        GameObject.Find("Sun" + sunPosition.ToString()).GetComponent<SpriteRenderer>().enabled = true;
    }

    void ChangeSunPositionLeft()
    {
        if (sunPosition > 0 && sunPosition <= 3)
            sunPosition--;
        else
            sunPosition = 3;
    }
    
    void ChangeSunPositionRight()
    {
        if (sunPosition >= 0 && sunPosition < 3)
            sunPosition++;
        else
            sunPosition = 0;
    }

    void DrawHuman(int zoneNumber)
    {
        int[] positionInGrid = GetEmptyGridSpace(zoneNumber);
        Vector3 positionInScene = CalculatePositionInScene(zoneNumber, positionInGrid);
        
        // GameObject newUnit = (GameObject)Instantiate(Resources.Load("HumanUnit"), positionInScene, new Quaternion ());
        Instantiate(Resources.Load("HumanUnit"), positionInScene, new Quaternion ());
        gridWorld[ positionInGrid[0], positionInGrid[1], positionInGrid[2] ] = 1;
    }

    void DrawMonster(int zoneNumber)
    {
        int[] positionInGrid = GetEmptyGridSpace(zoneNumber);
        Vector3 positionInScene = CalculatePositionInScene(zoneNumber, positionInGrid);

        Instantiate(Resources.Load("MonsterUnit"), positionInScene, new Quaternion ());
        
        gridWorld[ positionInGrid[0], positionInGrid[1], positionInGrid[2] ] = 2;
    }

    void DeleteUnits(int zoneNumber)
    {
        GameObject[] objectsInScene = SceneManager.GetActiveScene().GetRootGameObjects();

        if (zoneNumber == 0)
        {
            foreach (GameObject o in objectsInScene)
            {
                if (o.transform.position.x < 0 && o.transform.position.y > 0 && o.transform.position.z == -0.5f)
                    Destroy(o);
            }
        }
        if (zoneNumber == 1)
        {
            foreach (GameObject o in objectsInScene)
            {
                if (o.transform.position.x > 0 && o.transform.position.y > 0 && o.transform.position.z == -0.5f)
                    Destroy(o);
            }
        }
        if (zoneNumber == 2)
        {
            foreach (GameObject o in objectsInScene)
            {
                if (o.transform.position.x > 0 && o.transform.position.y < 0 && o.transform.position.z == -0.5f)
                    Destroy(o);
            }
        }
        if (zoneNumber == 3)
        {
            foreach (GameObject o in objectsInScene)
            {
                if (o.transform.position.x < 0 && o.transform.position.y < 0 && o.transform.position.z == -0.5f)
                    Destroy(o);
            }
        }
    }

    int [] GetEmptyGridSpace(int zoneNumber)
    {
        for (int h = 0; h < 10; h ++)
        {
            for (int w = 0; w < 10; w ++)
            {
                if (gridWorld[zoneNumber, h, w ] == 0)
                    return new int[] { zoneNumber, h, w };
            }
        }

        return new int[] {};
    }

    Vector3 CalculatePositionInScene(int zoneNumber, int[] gridPosition)
    {
        switch(zoneNumber)
        {
            case 0:
                return new Vector3(-0.25f * (gridPosition[2] + 1), 0.25f * (gridPosition[1] + 1), -0.5f);
            case 1:
                return new Vector3(0.25f * (gridPosition[2] + 1), 0.25f * (gridPosition[1] + 1), -0.5f);
            case 2:
                return new Vector3(0.25f * (gridPosition[2] + 1), -0.25f * (gridPosition[1] + 1), -0.5f);
            case 3:
                return  new Vector3(-0.25f * (gridPosition[2] + 1), -0.25f * (gridPosition[1] + 1), -0.5f);
            default:
                return new Vector3();
        }

    }
}
