using System;
using UnityEngine;

public class Poster : MonoBehaviour
{
    [SerializeField] private int cost = 50;
    public int Cost { get { return cost; } }

    private Vector2 gridCoordinates;

    public event Action<Vector2> onPosterBuildAtlocation;

    private const int NUMBEROFTILESEFFECTED = 3;

    public void CreatePoster(Poster posterPrefab, Vector3 position)
    {
        if (Bank.Instance.CurrentBalance >= cost)
        {
            Instantiate(posterPrefab.gameObject, position, Quaternion.identity);
            Bank.Instance.Withdraw(cost);
            AudioPlayer.Instance.PlayBuildPosterClip();
            gridCoordinates = new Vector2(position.x,position.y);
            ApplySlowEnemiesEffectonTiles();
        }
    }

    private void ApplySlowEnemiesEffectonTiles()
    {
        var startCoordinates = gridCoordinates;

        for(int i = 0; i < NUMBEROFTILESEFFECTED; i++)
        {
            if (onPosterBuildAtlocation != null)
            {
                //Debug.Log($"Send slow to X: {gridCoordinates.x + i - 1}, Y {gridCoordinates.y - 1}");
                onPosterBuildAtlocation(new Vector2(gridCoordinates.x + i - 1, gridCoordinates.y - 1));
            }
        }
    }
}
