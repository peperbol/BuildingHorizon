﻿using UnityEngine;
using System.Collections;
using System;

public abstract class Minion : RadialMovement, Attackable
{
    public static void Spawn(Minion prefab, bool toLeft)
    {
        Minion m = Instantiate(prefab);
        m.DirectionIsToLeft = toLeft;
    }

    public float health;

    public float Health {
        get { return health; }
        set
        { health = value;

            if (health <= 0) {
                //DIEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
                GameObject.Destroy(gameObject);
            }
       }
    }

    public float sight;
    public LayerMask enemyMask;
    private bool directionIsToLeft;
    public bool DirectionIsToLeft {
        get { return directionIsToLeft; }
        set { 
            directionIsToLeft = value;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y,transform.localScale.z);
        }
    }
    public float walkSpeed = 4;
    private Vector2 V3toV2(Vector3 v) {
        return new Vector2(v.x, v.y);
    }
    protected bool CanSeeEnemy(out Attackable a)
    {
        a = null;
        RaycastHit2D h = Physics2D.Raycast(transform.position, (DirectionIsToLeft) ? transform.right : -transform.right, sight, enemyMask);

        if (h.collider == null) return false;

        a = h.collider.GetComponent<Attackable>();
        
        return a != null;
    }

    protected abstract void Attack(Attackable a);

    protected override void Update() {
        Attackable a;
        if (CanSeeEnemy(out a)) {
            Attack(a);
        } else
        Move(DirectionIsToLeft);
    }

    public void Damage() {
        Health--;
    }
}
