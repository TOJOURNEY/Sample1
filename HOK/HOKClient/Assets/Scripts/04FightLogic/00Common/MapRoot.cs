using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoot : MonoBehaviour
{
    public Transform transCameraRoot;
    public Transform transEnvCollider;

    public Transform blueTower;
    public Transform redTower;
    public Transform blueCrystal;
    public Transform redCrystal;
    public Transform desBlueTower;
    public Transform desRedTower;
    public Transform desBlueCrystal;
    public Transform desRedCrystal;


    public void DestroyBlueTower()
    {
        blueTower.gameObject.SetActive(false);
        desBlueTower.gameObject.SetActive(true);
    }

    public void DestroyRedTower()
    {
        redTower.gameObject.SetActive(false);
        desRedTower.gameObject.SetActive(true);
    }
    public void DestroyBlueCrystal()
    {
        //blueCrystal.gameObject.SetActive(false);
        desBlueCrystal.gameObject.SetActive(true);
    }
    public void DestroyRedCrystal()
    {
        //redCrystal.gameObject.SetActive(false);
        desRedCrystal.gameObject.SetActive(true);
    }
}

