using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPanel : MonoBehaviour {

	public GameObject storyPanel;
	public GameObject artPanel;

	public void ShowStoryPanel()
	{
		storyPanel.SetActive(true);
		HideArtPanel();
	}

	public void ShowArtPanel()
	{
		artPanel.SetActive(true);
		HideStoryPanel();
	}

	public void HideStoryPanel()
	{
		storyPanel.SetActive(false);
	}

	public void HideArtPanel()
	{
		artPanel.SetActive(false);
	}

}
