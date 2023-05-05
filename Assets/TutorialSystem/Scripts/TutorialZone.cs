using UnityEngine;

public class TutorialZone : MonoBehaviour
{
    public bool isActive = true;
    [Multiline] public string tutorialMessage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            TutorialController.Instance.DisplayTutorial(tutorialMessage);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            TutorialController.Instance.HideTutorial();
            isActive = false;
        }
    }
}
