using System;
using UnityEngine;

public class InfectionRate : MonoBehaviour
{
    [SerializeField] private int allowedNumberofInfectedPatients = 5;

    private static InfectionRate instance;
    private LevelManager levelManager;
    private AudioPlayer audioPlayer;
    private int currentInfectedPatients = 0;
    public int CurrentInfectedPatients { get { return instance.currentInfectedPatients; } }

    public event Action<Vector2> patientInfected;

    private void Awake()
    {
        ManageSingleton();
        levelManager = FindObjectOfType<LevelManager>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    private void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void InfectPatientAtLocation(Vector2 location)
    {
        currentInfectedPatients++;
        audioPlayer.PlayPatientInfectedClip();

        if (patientInfected != null)
        {
            patientInfected(location);
        }

        if (currentInfectedPatients >= allowedNumberofInfectedPatients)
        {
            levelManager.LoadGameOver();
        }
    }
}
