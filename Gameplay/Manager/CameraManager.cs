using UnityEngine;

public class CameraManager : MonoBehaviour {

    private GameObject target;
    private Camera cam;
    private FocusArea focusArea;
    private InputManager im;

    // Look ahead and camera smoothing.
    private float currentLookAheadX;
    private float targetLookAheadX;
    private float lookAheadDirX;
    private float smoothLookVelocityX;
    private float smoothVelocityY;
    private bool lookAheadStopped;
    private float lookAheadDstX = 0.5f;
    private float lookSmoothTimeX = 0.4f;
    private float downButtonHeldTime = 0f;
    private float upButtonHeldTime = 0f;
    private float holdTimeToChangeFocus = 3f;
    private float yFocusDifferential = 0f;

    private void Awake() {
        var player = ObjectLocator.GetPlayer();
        im = InputManager.GetInstance();
        // Player can be null in places with this camera but no gameplay, like menus.
        if (player != null) {
            target = player;
            focusArea = GetComponent<FocusArea>();
        }
    }

    private void Start()  {
        // Adapt the camera to 4:3 to fit more the retro style.
        cam = GetComponent<Camera>();
        var targetaspect = 4.0f / 3.0f;
        var windowaspect = (float) Screen.width / (float) Screen.height;
        var scaleheight = windowaspect / targetaspect;
        var rect = cam.rect;
        if (scaleheight < 1.0f) {
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
        } else {
            var scalewidth = 1.0f / scaleheight;
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
        }
        cam.rect = rect;
    }

    // To control the holding 
    private void Update() {
        if (!GameState.GetInstance().isCameraLocked) {
            if (im.IsOnlyActionPressed(GameCommand.DOWN)) {
                downButtonHeldTime += Time.deltaTime;
            } else {
                downButtonHeldTime = 0f;
            }
            if (im.IsOnlyActionPressed(GameCommand.UP)) {
                upButtonHeldTime += Time.deltaTime;
            } else {
                upButtonHeldTime = 0f;
            }
        }
    }

    private void LateUpdate() {
        if (target && !GameState.GetInstance().isCameraLocked) {
            Vector2 focusPosition = focusArea.centre;

            // If the focus area is moving.
            var velocity = target.GetComponent<PlatformerMovement2D>().GetVelocity();
            if (focusArea.velocity.x != 0) {
                lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
                if (Mathf.Sign(velocity.x) == Mathf.Sign(focusArea.velocity.x) && velocity.x != 0) {
                    lookAheadStopped = false;
                    targetLookAheadX = lookAheadDirX * lookAheadDstX;
                } else {
                    if (!lookAheadStopped) {
                        lookAheadStopped = true;
                        targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 8f;
                    }
                }
            }

            currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
            focusPosition += Vector2.right * currentLookAheadX;
            // Holding down moves the focus down, releasing slowing puts it up on its place.
            if (downButtonHeldTime >= holdTimeToChangeFocus && yFocusDifferential > -2f) {
                yFocusDifferential -= Time.deltaTime;
                if (yFocusDifferential < -2f) {
                    yFocusDifferential = -2f;
                }
            }
            if (!im.IsOnlyActionPressed(GameCommand.DOWN) && yFocusDifferential < 0f) {
                yFocusDifferential += Time.deltaTime;
                if (yFocusDifferential > 0f) {
                    yFocusDifferential = 0f;
                }
            }
            if (upButtonHeldTime >= holdTimeToChangeFocus && yFocusDifferential < 1.3f) {
                yFocusDifferential += Time.deltaTime;
                if (yFocusDifferential > 1.3f) {
                    yFocusDifferential = 1.3f;
                }
            }
            if (!im.IsOnlyActionPressed(GameCommand.UP) && yFocusDifferential > 0f) {
                yFocusDifferential -= Time.deltaTime;
                if (yFocusDifferential < 0f) {
                    yFocusDifferential = 0f;
                }
            }
            focusPosition += new Vector2(0f, yFocusDifferential);
            transform.position = ClampPosition((Vector3)focusPosition + Vector3.forward * -10);
        }
    }

    // Limit the camera in places it should not show.
    private Vector3 ClampPosition(Vector3 pos) {
        string currentScene = GameState.GetInstance().GetCurrentScene();
        float x = pos.x;
        float y = pos.y;

        // Tutorial
        if (currentScene == Scenes.TUTORIAL) {
            x = Mathf.Clamp(pos.x, 3.2f, 10.66f);
            y = Mathf.Clamp(pos.y, -1.52f, 0f);
        }

        // Limits for Monastery act 1
        if (currentScene == Scenes.MONASTERY_ACT_1) {
            x = Mathf.Clamp(pos.x, 3.2f, 70.08f);
            var minY = pos.x > 42.5f ? -3.16f : -1.56f;
            y = Mathf.Clamp(pos.y, minY, 40f);
        }

        // Limits for Monastery act 2
        if (currentScene == Scenes.MONASTERY_ACT_2) {
            x = Mathf.Clamp(pos.x, 3.2f, 112.62f);
            // Approaching the boss makes the camera go up.
            // The treshold indicates from which X axis point it happens.
            // The absolute min Y indicates the minimum value of Y always at this point, then clamped up the farther we are from the treshold.
            var treshold = 105.4f;
            var absoluteMinY = -12.2f;
            var minY = pos.x >= treshold ? Mathf.Clamp(absoluteMinY + (pos.x - treshold) / 2.1f, absoluteMinY, -8.966419f) : absoluteMinY;
            y = Mathf.Clamp(pos.y, minY, -2.39f);
        }

        // Limits for Hospital Act 1
        if (currentScene == Scenes.HOSPITAL_ACT_1) {
            x = Mathf.Clamp(pos.x, 13.49f, 125.8f);
            var minY = pos.x > 116f ? 19.5f : pos.x > 60f ? 3.36f : -2.62f;
            y = Mathf.Clamp(pos.y, minY, 40f);
        }

        // Limits for Hospital Act 2
        if (currentScene == Scenes.HOSPITAL_ACT_2) {
            x = Mathf.Clamp(pos.x, 3.14f, 94.6f);
            var treshold = 89f;
            var absoluteMinY = -7.3f;
            var minY = pos.x >= treshold ? Mathf.Clamp(absoluteMinY + (pos.x - treshold) / 2.1f, absoluteMinY, -6.88f) : -25.42f;
            y = Mathf.Clamp(pos.y, minY, 2f);
        }

        // Limits for Night Bar Act 1
        if (currentScene == Scenes.NIGHT_BAR_ACT_1) {
            x = Mathf.Clamp(pos.x, 3.2f, 115.182f);
            y = Mathf.Clamp(pos.y, -2.61f, 25f);
        }

        // Limits for Night Bar Act 2
        if (currentScene == Scenes.NIGHT_BAR_ACT_2) {
            var minX = 3.2f;
            var maxX = 203.4f;
            if (GameState.activatingBoss || GameState.bossActive) {
                minX = 198.9177f;
                maxX = 202.6855f;
            }
            x = Mathf.Clamp(pos.x, minX, maxX);
            y = Mathf.Clamp(pos.y, -23.62f, 25.6f);
        }

        // Limits for Desert Act 1
        if (currentScene == Scenes.DESERT_ACT_1) {
            x = Mathf.Clamp(pos.x, 3.2f, 124.56f);
            y = Mathf.Clamp(pos.y, -27.9f, 5.11f);
        }

        // Limits for Desert Act 2
        if (currentScene == Scenes.DESERT_ACT_2) {
            var minY = pos.x >= 191f? -1.24f : -28.4f;
            x = Mathf.Clamp(pos.x, 3.2f, 195.19f);
            y = Mathf.Clamp(pos.y, minY, -1.21f);
        }

        // Limits for Lab Act 1
        if (currentScene == Scenes.LAB_ACT_1) {
            var minY = pos.x < 30 ? -14.95f : -17.22f;
            var maxY = 5.63f;
            if (pos.x >= 9.26f && pos.x <= 21.42f) {
                maxY = 3.29f;
            } else if (pos.x >= 27.35f && pos.x <= 39.41f) {
                maxY = 4.93f;
            } else if (pos.x >= 60.72f && pos.x <= 72.92f) {
                maxY = 1.69f;
            } else if (pos.x >= 103.26f && pos.x <= 115.31f) {
                maxY = 5.59f;
            }
            x = Mathf.Clamp(pos.x, 3.2f, 124.75f);
            y = Mathf.Clamp(pos.y, minY, maxY);
        }

        // Limits for Lab Act 2
        if (currentScene == Scenes.LAB_ACT_2) {
            var maxY = 4.26f;
            if (pos.x >= 7.8f && pos.x <= 20f) {
                maxY = 4.2f;
            } else if (pos.x >= 53.2f && pos.x <= 65.4f) {
                maxY = 3.4f;
            } else if (pos.x >= 73.1f && pos.x <= 85.2f) {
                maxY = 2.7f;
            }
            var minY = pos.x < 39.57f? -25.13f : -28.04f;

            x = Mathf.Clamp(pos.x, 3.2f, 121.58f);
            y = Mathf.Clamp(pos.y, minY, maxY);
        }

        // Limits for Commie HQ 1
        if (currentScene == Scenes.COMMIE_HQ_ACT_1) {
            var minY = pos.x < 9.03f ? - 7.78f : -10.93f;

            x = Mathf.Clamp(pos.x, 3.2f, 184.29f);
            y = Mathf.Clamp(pos.y, minY, 16.16f);
        }

        // Limits for Commie HQ 2
        if (currentScene == Scenes.COMMIE_HQ_ACT_2) {
            x = Mathf.Clamp(pos.x, 3.2f, 166.8f);
            y = Mathf.Clamp(pos.y, -16.37f, 10.79f);
        }

        // Limits for Final Zone
        if (currentScene == Scenes.FINAL_ZONE) {
            x = Mathf.Clamp(x, 3.17f, 6.33f);
            y = Mathf.Clamp(pos.y, -4.28f, -4.28f);
        }

        return new Vector3(x, y, pos.z);
    }
}
