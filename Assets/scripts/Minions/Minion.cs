﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Minion : RadialMovement, Attackable
{
    public static void Spawn(Minion prefab, bool toLeft, float pos)
    {
        Minion m = Instantiate(prefab);
        m.center = GameObject.FindGameObjectWithTag("Center").transform;
        m.DirectionIsToLeft = toLeft;
        m.Position = pos;
    }

    public int health;
    public Animator animator;
    public float timeToDie;

    public int Health
    {
        get { return health; }
        set
        {
            health = value;

            if (health <= 0 && !dead)
            {
                //DIEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
                StartCoroutine(Die());
            }
        }
    }
    protected virtual IEnumerator Die()
    {

        dead = true;
        Destroy(GetComponent<Collider2D>());
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }
    public float sight;
    public LayerMask enemyMask;
    private bool directionIsToLeft;
    public bool DirectionIsToLeft
    {
        get { return directionIsToLeft; }
        set
        {
            if (directionIsToLeft != value)
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            directionIsToLeft = value;
        }
    }
    private Vector2 V3toV2(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }
    protected bool CanSeeEnemy(out Attackable a, out GameObject go, float overrideSight = -1)
    {
        a = null;
        go = null;
        RaycastHit2D h = Physics2D.Raycast(transform.position, (DirectionIsToLeft) ? transform.up : -transform.up, (overrideSight<0)? sight: overrideSight, enemyMask);

        if (h.collider == null) return false;

        a = h.collider.GetComponent<Attackable>();
        go = h.collider.gameObject;
        return a != null;
    }

    protected abstract void Attack(GameObject go, Attackable a);

    public bool dead = false;

    protected override void Update()
    {
        base.Update();
        if (!dead)
        {
            Attackable a;
            GameObject go;
            if (CanSeeEnemy(out a, out go))
            {
                Attack(go, a);
            }
            else
                Move(DirectionIsToLeft);
        }
    }
    public Renderer[] visuals;
    public Material flash;
    public virtual void Damage(MonoBehaviour damager)
    {
        Health--;
        if (damager is Builder) {
            ((Builder)damager).Loot();
        }
        if (!dead)
        {
            StartCoroutine(DmgFlash());
        }
    }

    List<Material[]> mats = new List<Material[]>();
    public IEnumerator DmgFlash() {
        for (int i = 0; i < visuals.Length; i++)
        {
            Material[] m = visuals[i].materials;

            for (int j = 0; j < m.Length; j++)
            {
                m[j] = flash;
            }
            visuals[i].materials = m;
        }
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < visuals.Length; i++)
        {

            visuals[i].materials = mats[i];
        }
    }
    void Start() {

        for (int i = 0; i < visuals.Length; i++)
        {
            mats.Add(visuals[i].materials);
        }
    }

    public enum Type { Melee, Ranged, Shield, None }
}
