using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBooth : MonoBehaviour {

    public GameObject[] pages;
    public GameObject continueButton;
    public GameObject welcomeScreen;
    public GameObject mainCamera;

    protected int currentPage;
    protected SimplePlayback videoPlayer;
    protected bool videoPlayerInitialized = false;

	// Use this for initialization
	void Start () {
        currentPage = 0;
        refreshPage();

        mainCamera = GameObject.Find("Main Camera");

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i].tag == "VideoPlayer")
            {
                videoPlayer = GameObject.Find(pages[i].name).GetComponent<SimplePlayback>();
                // videoPlayer.Start();
                pages[i].GetComponent<Renderer>().enabled = false;
            }
        }

        updateButton();
    }

    // Update is called once per frame
    void Update () {
    }

    public void refreshPage()
    {
        if (pages.Length == 0) return;

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i].tag != "VideoPlayer") pages[i].active = false;
        }

        pages[currentPage].active = true;
    }

    public GameObject getNextPage()
    {
        int nextPage = currentPage + 1;
        if (nextPage >= pages.Length) nextPage = 0;
        return pages[nextPage];
    }

    public void updateButton()
    {
        if (pages.Length == 0) return;

        GameObject nextPage = getNextPage();
        if (nextPage.tag == "VideoPlayer")
        {
            GameObject.Find(continueButton.name).GetComponentInChildren<Text>().text = "Click to watch a video";
        } else
        {
            GameObject.Find(continueButton.name).GetComponentInChildren<Text>().text = "Click to continue reading";
        }
    }

    public void OnClickContinue()
    {
        GvrAudioSource clickSound = GameObject.Find(continueButton.name).GetComponent<GvrAudioSource>();
        if (clickSound != null) clickSound.Play();

        GameObject[] videoPlayerObjects = GameObject.FindGameObjectsWithTag("VideoPlayer");
        foreach (GameObject videoPlayerObject in videoPlayerObjects)
        {
            SimplePlayback player = GameObject.Find(videoPlayerObject.name).GetComponent<SimplePlayback>();
            if (player != null) player.PlayerPause();
        }

        if (pages[currentPage].tag == "VideoPlayer")
        {
            // videoPlayer.PlayerPause();
            pages[currentPage].GetComponent<Renderer>().enabled = false;
            mainCamera.GetComponent<AudioSource>().volume = 0.7f;
        }

        currentPage++;
        if (currentPage >= pages.Length) currentPage = 0;
        refreshPage();
        if (pages[currentPage].tag == "VideoPlayer")
        {
            pages[currentPage].GetComponent<Renderer>().enabled = true;

            mainCamera.GetComponent<AudioSource>().volume = 0.2f;

            if (videoPlayerInitialized) videoPlayer.Play_Pause();
            else {
                videoPlayer.PlayYoutubeVideo(videoPlayer.videoId);
                videoPlayerInitialized = true;
            }
        }

        updateButton();
    }

    public void OnClickPrevious()
    {
        currentPage--;
        if (currentPage < 0) currentPage = 0;
        refreshPage();
    }

    public void OnClickStart()
    {
        GvrAudioSource clickSound = GameObject.Find(continueButton.name).GetComponent<GvrAudioSource>();
        if (clickSound != null) clickSound.Play();

        Invoke("DisableWelcomeScreen", 1);
    }

    public void DisableWelcomeScreen()
    {
        welcomeScreen.active = false;
    }
}
