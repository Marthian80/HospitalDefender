using System;
using UnityEngine;

public class TankActivator : Singleton<TankActivator>
{
    [SerializeField] private Tank tank;
        
    [SerializeField][Range(0, 50)] private int poolSize = 4;

    private GameObject[] tankPool;

    public event Action<Vector2Int> onTankBuildAtlocation;

    protected override void Awake()
    {
        base.Awake();
        PopulatePool();        
    }

    private void PopulatePool()
    {
        tankPool = new GameObject[poolSize];

        for (int i = 0; i < tankPool.Length; i++)
        {
            tankPool[i] = Instantiate(tank.gameObject, transform.position, Quaternion.identity);
            tankPool[i].SetActive(false);
        }
    }

    public void ActivateTank(Vector3 position)
    {
        if (Bank.Instance.CurrentBalance >= tank.Cost)
        {
            EnableObjectInPool();
            if (onTankBuildAtlocation != null)
            {
                onTankBuildAtlocation(new Vector2Int((int)position.x, (int)position.y));
                Bank.Instance.Withdraw(tank.Cost);
                AudioPlayer.Instance.PlayBuildTankClip();
            }
        }
    }

    private void EnableObjectInPool()
    {
        foreach (var gameObject in tankPool)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                return;
            }
        }
    }
}