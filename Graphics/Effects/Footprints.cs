using Prime31;
using System;
using UnityEngine;

public class Footprints : MonoBehaviour {

    private CharacterController2D controller;

    private float offset = -0.28f;
    private float footprintDistance = 0.45f;
    private Vector2 lastFootPrintPos = Vector2.zero;

    // Kind of footprints.
    private bool bloody = false;
    private bool sand = false;
    private bool snow = false;
    private ObjectPool bloodyFootprints;
    private ObjectPool snowFootprints = null;
    private ObjectPool sandFootprints = null;

    // Only for bloody footprints.
    private float firstBloodyStep;
    private float bloodyStepsDuration = 6f;
    private int steps = 0;

    private void Start() {
        bloodyFootprints = new ObjectPool(Resources.Load<GameObject>("Prefabs/" + StepNames.BLOODY_STEP), 6);
        controller = GetComponent<CharacterController2D>();
    }

    public void SetBloodyFeet() {
        firstBloodyStep = Time.time;
        steps = 6;
        bloody = true;
    }

    public void SteppingSnow() {
        if (snowFootprints == null) {
            snowFootprints = new ObjectPool(Resources.Load<GameObject>("Prefabs/" + StepNames.SNOW_STEP), 12);
        }
        sand = false;
        snow = true;
    }

    public void SteppingSand() {
        if (sandFootprints == null) {
            sandFootprints = new ObjectPool(Resources.Load<GameObject>("Prefabs/" + StepNames.SAND_STEP), 12);
        }
        sand = true;
        snow = false;
    }

    public void Update() {
        // Check for expiration of bloody feet.
        if (bloody && steps <= 0) {
            bloody = false;
        }

        if (bloody && Time.time >= firstBloodyStep + bloodyStepsDuration) {
            bloody = false;
        }

        if ((bloody || sand || snow) && Math.Abs(lastFootPrintPos.x - transform.position.x) >= footprintDistance && controller.isGrounded) {
            AddFootprint();
            if (bloody) {
                steps--;
            }
        }
    }

    private GameObject NextFootstep() {
        ObjectPool pool = sand? sandFootprints : snow? snowFootprints : bloodyFootprints;
        return pool.RetrieveNext();
    }

    public void AddFootprint() {
        lastFootPrintPos.x = transform.position.x;
        var pos = new Vector2(transform.position.x, transform.position.y + offset);
        var footstep = NextFootstep();
        footstep.transform.position = pos;
        footstep.SetActive(true);
        if (transform.parent) {
            footstep.transform.parent = transform.parent;
        } else {
            footstep.transform.parent = null;
        }
    }
}
