using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarxFlame : MonoBehaviour {

    // Colliders for each frame.
    public PolygonCollider2D frame0;
    public PolygonCollider2D frame1;
    public PolygonCollider2D frame2;
    public PolygonCollider2D frame3;
    public PolygonCollider2D frame4;
    public PolygonCollider2D frame5;
    private PolygonCollider2D[] hitboxes;
    private Animator anim;

    public enum HitBoxes : int {
        box0 = 0,
        box1 = 1,
        box2 = 2,
        box3 = 3,
        box4 = 4,
        box5 = 5
    }

    public void Awake() {
        hitboxes = new PolygonCollider2D[] {
            frame0,
            frame1,
            frame2,
            frame3,
            frame4,
            frame5
        };
        anim = GetComponent<Animator>();
    }

    public void SetHitbox(HitBoxes value) {
        for (var i = 0; i < 6; i++) {
            if (i == (int) value) {
                hitboxes[i].enabled = true;
            } else {
                hitboxes[i].enabled = false;
            }
        }
    }

    public void ResetFlame() {
        SetHitbox(HitBoxes.box0);
        anim.Play(Animations.MARX_FLAME_START, -1, 0f);
    }
}
