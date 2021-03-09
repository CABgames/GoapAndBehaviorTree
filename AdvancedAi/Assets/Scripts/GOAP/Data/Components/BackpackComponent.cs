////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: BackpackComponent.cs 
///Created by: Charlie Bullock based on GOAP example given in CT6024
///Description: This component is on each spy and contains the values for variables they may contain or gain from other components
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

public class BackpackComponent : MonoBehaviour
{
    //Variables
    public int numWeapons;
    public int numDocuments;
    public int numComponents;
    public int numIntel;
    public bool hasRadioDevice  ;
    public bool hasReportingDevice;
}
