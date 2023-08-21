using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class questManager : MonoBehaviour
{
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TextMeshProUGUI questText;

    private Image panelImage;
    void Start()
    {
        panelImage = questPanel.GetComponent<Image>();
    }

    public void updateQuest(string text)
    {
        questText.text = text;
        StartCoroutine(flashQuest());
    }

    public IEnumerator flashQuest() //flash the quest container to alert player that the quest has been updated
    {
        panelImage.color = new Color(0.7653933f, 0.4098532f, 1.0f, 1.0f); //change colour to bright purple

        yield return new WaitForSeconds(1.0f);

        panelImage.color = new Color(0.0f, 0.0f, 0.0f, 0.7529412f); //return colour to black
    }

}
