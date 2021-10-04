using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Create Recipe", fileName = "New Recipe")]
public class Recipe : ScriptableObject
{
    public List<Step> steps;
    public int reward = 250;
}
