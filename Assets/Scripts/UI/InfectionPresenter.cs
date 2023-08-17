using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectionPresenter : MonoBehaviour
{
    [SerializeField] private Slider infectionRateSlider;
    [SerializeField] private InfectionRate infectionRate;

    private void Start()
    {
        infectionRateSlider.value = infectionRate.CurrentInfectedPatients;
        infectionRate.patientInfected += PatientInfected;
    }

    private void PatientInfected(Vector2 obj)
    {
        infectionRateSlider.value = infectionRate.CurrentInfectedPatients;
    }
}
