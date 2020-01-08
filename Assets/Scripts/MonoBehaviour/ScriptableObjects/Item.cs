using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Item : ScriptableObject
{
    public string Name = string.Empty;
    public bool DoesUseAttributes = true;
    [ShowIf("DoesUseAttributes"),InlineProperty,HideLabel]
    public RPGAttributes Attributes;
    [BoxGroup("Information")]
    public bool DoesItHeal = false;
    [BoxGroup("Information")]
    public bool DoesItDamages = false;
    [BoxGroup("Information")]
    public bool IsEquipable = false;
}
