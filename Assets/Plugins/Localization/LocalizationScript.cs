using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LocalizationScript : MonoBehaviour
{
    public LocalizationData data;
    
    public enum Language
    {
        EN,
        RU
    }
    public void SetLanguageEN()
    {
        SetLanguage(Language.EN);
    }
    public void SetLanguageRU()
    {
        SetLanguage(Language.RU);
    }
    public static void SetLanguage(Language lang)
    {
        PlayerPrefs.SetInt("Language", (int) lang);
        foreach (var ls in FindObjectsOfType<LocalizationScript>())
        {
            ls.Localize(lang);
        }
    } 
    public static Language GetLanguage()
    {
        if (!PlayerPrefs.HasKey("Language")) SetLanguage(Language.EN);
        return (Language) PlayerPrefs.GetInt("Language");
    }

    Text txt;
    TMP_Text tmpTxt;
    private void Awake()
    {
        txt = GetComponent<Text>();
        tmpTxt = GetComponent<TMP_Text>();
    }
    void SetText(string text)
    {
        if (txt != null) txt.text = text;
        if (tmpTxt != null) tmpTxt.text = text;
    }

    private void OnEnable()
    {
        Localize(GetLanguage());
    }

    public void Localize(Language lang)
    {
        if (data == null) return;
        switch (lang)
        {
            case Language.EN:
                SetText(data.EN);
                break;
            case Language.RU:
                SetText(data.RU);
                break;
        }
    }
    
}
