using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetControl : MonoBehaviour
{
    [SerializeField] private List<Patient> patients = new List<Patient>();
    [SerializeField] private float timePatientIsInactive = 3f;

    private BacteriaSpawner bacteriaSpawner;
    private Flash flash;

    private void Awake()
    {
        flash = FindObjectOfType<Flash>();
        bacteriaSpawner = FindObjectOfType<BacteriaSpawner>();
        bacteriaSpawner.enemyInfectedTarget += TargetInfected;
    }

    public List<Patient> GetActivePatientTargets()
    {
        return patients.Where(x => x.isActive).ToList();
    }

    private void TargetInfected(Vector2 targetLocation)
    {
        var patient = FindPatientInList(targetLocation);
        if (patient != null)
        {
            patient.isActive = false;
            var spriteRenderer = patient.patientGameObject.GetComponent<SpriteRenderer>();
            StartCoroutine(flash.SlowFadeOutRoutine(spriteRenderer));
            StartCoroutine(ReactivatePatient(patient, spriteRenderer));
        }
    }

    private Patient FindPatientInList(Vector2 targetLocation)
    {
        return patients.FirstOrDefault(x => x.coordinates.x == (int)targetLocation.x && x.coordinates.y == (int)targetLocation.y);       
    }

    private IEnumerator ReactivatePatient(Patient patient, SpriteRenderer patientSpriteRenderer)
    {
        yield return new WaitForSeconds(timePatientIsInactive);
        patient.isActive = true;
        StartCoroutine(flash.SlowFadeInRoutine(patientSpriteRenderer));
    }
}
