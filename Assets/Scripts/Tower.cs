using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public bool CreateTower(Tower towerPrefab, Vector3 position)
    {
        Instantiate(towerPrefab.gameObject, position, Quaternion.identity);
        return true;
    }
}
