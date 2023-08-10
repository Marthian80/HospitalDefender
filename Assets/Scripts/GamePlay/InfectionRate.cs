using System;
using UnityEngine;

public class InfectionRate : MonoBehaviour
{
    [SerializeField] private int numberofPatients = 5;

    private int currentInfectedPatients = 0;
    public int CurrentInfectedPatients { get { return currentInfectedPatients; } }

    public event Action<Vector2> patientInfected;

    public void InfectPatientAtLocation(Vector2 location)
    {
        currentInfectedPatients++;

        if (patientInfected != null)
        {
            patientInfected(location);
        }
    }
}
