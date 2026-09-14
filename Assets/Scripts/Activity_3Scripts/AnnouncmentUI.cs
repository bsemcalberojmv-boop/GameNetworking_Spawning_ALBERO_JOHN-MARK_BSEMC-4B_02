using UnityEngine;
using TMPro;
using System.Collections;

public class AnnouncementUI : MonoBehaviour
{
    public static AnnouncementUI Instance;
    [SerializeField] private TextMeshProUGUI announcementText;
    [SerializeField] private float displayDuration = 3f;

    private void Awake()
    {
        Instance = this;
        announcementText.text = "";
    }

    public void ShowAnnouncement(string message)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayRoutine(message));
    }

    private IEnumerator DisplayRoutine(string message)
    {
        announcementText.text = message;
        yield return new WaitForSeconds(displayDuration);
        announcementText.text = "";
    }
}