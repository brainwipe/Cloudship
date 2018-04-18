using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IAmBuilding, IHaveGridSpace {

    public Grid GridSpaceLocation { get; set; }

}