using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int cost = 75;
    public int Cost { get { return cost; } }

    

    public bool CreateTower(Tower towerPrefab, Vector3 position, AudioPlayer audioPlayer)
    {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null) { return false; }

        if (bank.CurrentBalance >= cost)
        {
            Instantiate(towerPrefab.gameObject, position, Quaternion.identity);
            bank.Withdraw(cost);
            audioPlayer.PlayBuildTowerClip();
            return true;
        }
        return false;            
    }    
}