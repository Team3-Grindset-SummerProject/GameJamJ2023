using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaceTrap : MonoBehaviour
{
    [SerializeField] private List<TrapZone> trapLocations;
    public GameObject currentTrap;
    [SerializeField] private bool canPlaceTrap;
    [SerializeField] private TrapZone closestTrap;
    
    private void Start()
    {
        GameObject[] tempZones = GameObject.FindGameObjectsWithTag("TrapZone");

        foreach (var zone in tempZones)
            trapLocations.Add(zone.GetComponent<TrapZone>());
    }

    private void Update()
    {
        TrapZone closestTemp = trapLocations[0];

        foreach (var trapZone in trapLocations)
        {
            if (Vector3.Distance(transform.position, trapZone.transform.position) < 
                Vector3.Distance(transform.position, closestTemp.transform.position))
            {
                closestTemp = trapZone;
            }
        }

        if (Vector3.Distance(transform.position, closestTemp.transform.position) < closestTemp.GetRange() && !closestTemp.trapPlaced)
        {
            canPlaceTrap = true;
            closestTrap = closestTemp;
        }
        else
        {
            canPlaceTrap = false;
        }
    }

    public bool OnPlaceTrap()
    {
        if (closestTrap == null)
         return false;

        if (closestTrap.trapPlaced)
            return false;
        
        if(!canPlaceTrap)
            return false;

        Instantiate(currentTrap, closestTrap.GetTrapPosition(), Quaternion.identity);

        closestTrap.trapPlaced = true;

        return true;
    }
}
