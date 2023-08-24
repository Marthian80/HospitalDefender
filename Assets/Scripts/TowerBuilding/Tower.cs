using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int cost = 75;
    public int Cost { get { return cost; } }
        
    public bool CreateTower(Tower towerPrefab, Vector3 position)
    {
        if (Bank.Instance.CurrentBalance >= cost)
        {
            Instantiate(towerPrefab.gameObject, position, Quaternion.identity);
            Bank.Instance.Withdraw(cost);
            AudioPlayer.Instance.PlayBuildTowerClip();
            return true;
        }
        return false;            
    }    
}