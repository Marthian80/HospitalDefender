using UnityEngine;
using UnityEngine.UI;

public class InfectionPresenter : MonoBehaviour
{
    [SerializeField] private Slider infectionRateSlider;

    private void Start()
    {
        infectionRateSlider.maxValue = InfectionRate.Instance.AllowedNumberOfInfectedPatients;
        infectionRateSlider.value = InfectionRate.Instance.CurrentInfectedPatients;
        InfectionRate.Instance.patientInfected += PatientInfected;
    }

    private void PatientInfected()
    {
        infectionRateSlider.value = InfectionRate.Instance.CurrentInfectedPatients;
    }
}
