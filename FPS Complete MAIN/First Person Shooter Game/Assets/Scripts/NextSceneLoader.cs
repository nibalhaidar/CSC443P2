using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NextSceneLoader : MonoBehaviour
{
    public float rotationDuration = 1f;
    public Animator animator;

   public void LoadNextScene()
{
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    StartCoroutine(RotateThenLoad());
}

    private IEnumerator RotateThenLoad()
    {
        yield return new WaitForSeconds(1f);

        Transform cam = Camera.main.transform;
        Quaternion startRotation = Quaternion.Euler(-44f, cam.eulerAngles.y, cam.eulerAngles.z);
        Quaternion endRotation   = Quaternion.Euler(-116f, cam.eulerAngles.y, cam.eulerAngles.z);

        float elapsed = 0f;
        bool triggerFired = false;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / rotationDuration);
            cam.rotation = Quaternion.Lerp(startRotation, endRotation, t);

            // -110 wraps to 250 in Unity's 0-360 euler representation
            if (!triggerFired && cam.eulerAngles.x >= 230)
            {
                animator.SetTrigger("spidey");
                triggerFired = true;
            }

            yield return null;
        }

        cam.rotation = endRotation;

        // make sure trigger fired even if angle was missed
        if (!triggerFired)
        {
            animator.SetTrigger("spidey");
            triggerFired = true;
        }

        yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("spidey") &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            Debug.LogWarning("No next scene — already on the last scene.");
    }
}