using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStat : MonoBehaviour
{
    public float damage;
    public float cooldown;
    public float duration;
    public int maxAnimation;
    public int soundIndex;
    public float skillCooldown;
    public GameObject normalHitEffect;
    public GameObject critHitEffect;
    public GameObject bleedEffect;
    public bool burn;
}
