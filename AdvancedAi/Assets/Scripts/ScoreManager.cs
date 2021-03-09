/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: ScoreManager.cs 
///Created by: Charlie Bullock
///Description: This class is responsible for managing the score and resetting the ai when criteria are emt
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    //Variables
    #region Variables
    [SerializeField]
    private TextMeshProUGUI spyText;
    private float spyScore = -1;
    [SerializeField]
    private TextMeshProUGUI guardText;
    private float guardScore = -1;
    private AudioManager aM;
    private GameObject[] guards;
    private GameObject[] spies;
    #endregion Variables

    private void Start()
    {
        guards = GameObject.FindGameObjectsWithTag("Guard");
        spies = GameObject.FindGameObjectsWithTag("Spy");
        aM = FindObjectOfType<AudioManager>();
        IncrementGuardScore();
        IncrementSpyScore();
    }

    public void IncrementSpyScore()
    {
        spyScore++;
        spyText.text = "Spy Score: " + spyScore;
        aM.ScoreSound(false);
        if (LevelFinished())
        {
            Debug.Log("Spies Won!");
            spyScore += 11;
            Reset();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void IncrementGuardScore()
    {
        guardScore++;
        guardText.text = "Guard Score: " + guardScore;
        aM.ScoreSound(true);
        if (LevelFinished())
        {
            Debug.Log("Guards Won!");
            guardScore += 11;
            Reset();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    private void Reset()
    {
        //Reset scores
        guardText.text = "Guard Score: " + guardScore;
        spyText.text = "Spy Score: " + spyScore;
        //Guards
        foreach (GameObject guard in guards)
        {
            guard.SetActive(true);
        }
        //Spys
        foreach (GameObject spy in spies)
        {
            spy.SetActive(true);
        }
        //Weapons
        WeaponComponent[] weapons = (WeaponComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(WeaponComponent));
        foreach (WeaponComponent weapon in weapons)
        {
            weapon.numWeapons = 5;
        }

        //Documents
        DocumentComponent[] documents = (DocumentComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(DocumentComponent));
        foreach (DocumentComponent document in documents)
        {
            document.numDocuments = 5;
        }
        //Intel
        IntelComponent[] intels = (IntelComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(IntelComponent));
        foreach (IntelComponent intel in intels)
        {
            intel.numIntel = 5;
        }
        //Components
        WorkshopComponent[] components = (WorkshopComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(WorkshopComponent));
        foreach (WorkshopComponent component in components)
        {
            component.numComponents = 5;
        }
        //Radios
        RadioComponent[] radios = (RadioComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(RadioComponent));
        foreach (RadioComponent radio in radios)
        {
            radio.tamperedWith = false;
        }
        //Reporting points
        ReportingComponent[] reportings = (ReportingComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(ReportingComponent));
        foreach (ReportingComponent reporting in reportings)
        {
            reporting.tamperedWith = false;
        }
    }

    private bool LevelFinished()
    {
        //Guards
        GameObject[] guardsCurrently = GameObject.FindGameObjectsWithTag("Guard");
        GameObject activeGuard  = null;

        foreach (GameObject guard in guardsCurrently)
        {
            if (activeGuard == null)
            {
                // first one, so choose it for now
                activeGuard = guard;
                break;
            }
        }
        if (activeGuard == null)
        {
            return true;
        }
        //Spys
        GameObject[] spiesCurrently = GameObject.FindGameObjectsWithTag("Spy");
        GameObject activeSpy = null;

        foreach (GameObject spy in spiesCurrently)
        {
            if (activeSpy == null)
            {
                // first one, so choose it for now
                activeSpy = spy;
                break;
            }
        }
        if (activeSpy == null)
        {
            return true;
        }
        //Weapons
        WeaponComponent[] weapons = (WeaponComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(WeaponComponent));
        foreach (WeaponComponent weapon in weapons)
        {
            if ( weapon.numWeapons > 0)
            {
                return false;
            }
        }

        //Documents
        DocumentComponent[] documents = (DocumentComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(DocumentComponent));
        foreach (DocumentComponent document in documents)
        {
            if (document.numDocuments > 0)
            {
                return false;
            }
        }
        //Intel
        IntelComponent[] intels = (IntelComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(IntelComponent));
        foreach (IntelComponent intel in intels)
        {
            if (intel.numIntel > 0)
            {
                return false;
            }
        }
        //Components
        WorkshopComponent[] components = (WorkshopComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(WorkshopComponent));
        foreach (WorkshopComponent component in components)
        {
            if (component.numComponents > 0)
            {
                return false;
            }
        }
        //Radios
        RadioComponent[] radios = (RadioComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(RadioComponent));
        foreach (RadioComponent radio in radios)
        {
            if (radio.tamperedWith == false)
            {
                return false;
            }
        }
        //Reporting points
        ReportingComponent[] reportings = (ReportingComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(ReportingComponent));
        foreach (ReportingComponent reporting in reportings)
        {
            if (reporting.tamperedWith == false)
            {
                return false;
            }
        }
        return true;
    }


}
