﻿using UnityEngine;
using System.Collections;
using System;

public class MeleeMinion : Minion {
    public float timePerAttack;
    private bool ready = true;

    protected override void Attack(GameObject go, Attackable a)
    {

        if (ready) {
            a.Damage(this);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait() {
        ready = false;
        yield return new WaitForSeconds(timePerAttack);
        ready = true;
    }

}
