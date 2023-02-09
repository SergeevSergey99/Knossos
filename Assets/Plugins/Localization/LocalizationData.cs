using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Localization", menuName = "Localization", order = 10)]
public class LocalizationData : ScriptableObject
{
    [TextArea(3, 10)]
    public string EN;
    [TextArea(3, 10)]
    public string RU;
}
