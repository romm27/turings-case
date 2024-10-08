using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Computer File", menuName ="Computer File")]
public class ComputerFile : ScriptableObject
{
    [Header("Data")]
    public string fileTitle;
    public string fileIdentifier;
    [TextArea(10, 10)]public string[] content;
}
