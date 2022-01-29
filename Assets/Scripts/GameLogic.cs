using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    //Between 0 and #
    int sunPosition;
    
    List<Zone> zones;

    // Start is called before the first frame update
    void Start()
    {
        zones = new List<Zone>() { new Zone (), new Zone (), new Zone (), new Zone () };
        sunPosition = 0;
        UpdateSunVisibility();

        InvokeRepeating("UpdateWorldSunnyParts", 0f, 3f);  
        InvokeRepeating("UpdateWorldTemperature", 0f, 3f); 
        InvokeRepeating("UpdateWorldPopulation", 0f, 3f);  
        InvokeRepeating("LogWorldStatus", 0f, 3f);
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
            Debug.Log("Zone " + i.ToString() + ". Population: " + z.population.ToString() + ". Temperature: " + z.temperature.ToString() + ". PopulationType: " + z.populationType.ToString() + "(0 = none, 1 = Human, 2 = Monster).");
            i ++;
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
        foreach (Zone z in zones)
        {
            if (z.temperature == 10)
            {
                if (z.populationType == 0)
                    z.populationType = 1;
                if (z.populationType == 1)
                    z.population ++;
                if (z.populationType == 2 && z.population > 0)
                    z.population --;
            }
            if (z.temperature == -10)
            {
                if (z.populationType == 0)
                    z.populationType = 2;
                if (z.populationType == 2)
                    z.population ++;
                if (z.populationType == 1 && z.population > 0)
                    z.population --;
            }
        }
    }
     
    int[] ZonesAffectedBySun()
    {
        switch(sunPosition)
        {
            case 1:
                return new int[] {0, 1};
            case 2:
                return new int[] {1, 2};
            case 3:
                return new int[] {2, 3};
            case 4:
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
}
