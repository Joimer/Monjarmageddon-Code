using UnityEngine;
using System.Collections;

public class InvulnerableAble : MonoBehaviour {

    [HideInInspector]
    public bool invulnerable = false;

    public IEnumerator SetInvulnerable(float seconds) {
        invulnerable = true;
        yield return new WaitForSeconds(seconds);
        SetNotInvulnerable();
    }

    public void SetNotInvulnerable() {
        invulnerable = false;
        GetComponent<Renderer>().enabled = true;
        StopCoroutine(Flash());
    }

    public IEnumerator Flash() {
        GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        yield return new WaitForSeconds(0.3f);
    }
}
