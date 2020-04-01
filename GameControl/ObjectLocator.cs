using UnityEngine;

// I don't really like this, but it's better to wrap Unity's stuff with strings with a class method that can be replaced.
// More robust than changing the string with a dictionary, too.
public static class ObjectLocator {

    private static GameObject player;
    private static GameObject camera;

    // May return null right now.
    // TODO: Add a null object for this? idk
	public static GameObject GetPlayer() {
        if (player == null) {
            player = GameObject.FindWithTag("Player");
        }
        return player;
    }

    public static GameObject GetCamera() {
        if (camera == null) {
            camera = GameObject.FindWithTag("MainCamera");
        }
        return camera;
    }
}
