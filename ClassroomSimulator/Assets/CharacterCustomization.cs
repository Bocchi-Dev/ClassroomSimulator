using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class CharacterCustomization : MonoBehaviour
{
    [SerializeField] private Renderer rend;

    private Dictionary<string, string> skinColors =
    new Dictionary<string, string>()
    {
            {"Dark", "#8D5524"},
            {"Semi Dark", "#C68642"},
            {"Neutral", "#E0AC69"},
            {"Light", "#F1C27D"},
            {"Very Light", "#FFDBAC"}
    };

    private Dictionary<string, string> colors =
    new Dictionary<string, string>()
    {
            {"Red", "#E74C3C"},
            {"Yellow", "#F0EE8D"},
            {"Green", "#95E163"},
            {"Blue", "#4599C3"},
            {"Purple", "#CD7ADC"},
            {"Orange", "#E78A3C"}
    };

    private int SkinColorID;
    private int ShirtColorID;
    private int ShortsColorID;
    private int ShoesColorID;

    [SerializeField] private TextMeshProUGUI skinText;

    public void SelectSkinColor(bool isForward)
    {
        if (isForward)
        {
            if (SkinColorID == skinColors.Count - 1)
            {
                SkinColorID = 0;
            }
            else
            {
                SkinColorID++;
            }
        }
        else
        {
            if (SkinColorID == 0)
            {
                SkinColorID = skinColors.Count - 1;
            }
            else
            {
                SkinColorID--;
            }
        }
        SetItem("skinColor");
    }

    public void SelectShirtColor(bool isForward)
    {
        if (isForward)
        {
            if (ShirtColorID == colors.Count - 1)
            {
                ShirtColorID = 0;
            }
            else
            {
                ShirtColorID++;
            }
        }
        else
        {
            if (ShirtColorID == 0)
            {
                ShirtColorID = colors.Count - 1;
            }
            else
            {
                ShirtColorID--;
            }
        }
        SetItem("shirtColor");
    }

    public void SelectShortsColor(bool isForward)
    {
        if (isForward)
        {
            if (ShortsColorID == colors.Count - 1)
            {
                ShortsColorID = 0;
            }
            else
            {
                ShortsColorID++;
            }
        }
        else
        {
            if (ShortsColorID == 0)
            {
                ShortsColorID = colors.Count - 1;
            }
            else
            {
                ShortsColorID--;
            }
        }
        SetItem("shortsColor");
    }

    public void SelectShoeColor(bool isForward)
    {
        if (isForward)
        {
            if (ShoesColorID == colors.Count - 1)
            {
                ShoesColorID = 0;
            }
            else
            {
                ShoesColorID++;
            }
        }
        else
        {
            if (ShoesColorID == 0)
            {
                ShoesColorID = colors.Count - 1;
            }
            else
            {
                ShoesColorID--;
            }
        }
        SetItem("shoesColor");
    }

    private void SetItem(string type)
    {
        switch (type)
        {
            case "skinColor":
                string skinColorName = skinColors.Keys.ElementAt(SkinColorID);
                if (ColorUtility.TryParseHtmlString(skinColors.Values.ElementAt(SkinColorID), out Color skinColor))
                {
                    rend.materials[1].SetColor("_Color", skinColor);
                }
                break;

            case "shirtColor":
                string shirtColorName = colors.Keys.ElementAt(ShirtColorID);
                if (ColorUtility.TryParseHtmlString(colors.Values.ElementAt(ShirtColorID), out Color shirtColor))
                {
                    rend.materials[0].SetColor("_Color", shirtColor);
                }
                break;

            case "shortsColor":
                string shortsColorName = colors.Keys.ElementAt(ShortsColorID);
                if (ColorUtility.TryParseHtmlString(colors.Values.ElementAt(ShortsColorID), out Color shortsColor))
                {
                    rend.materials[3].SetColor("_Color", shortsColor);
                }
                break;

            case "shoesColor":
                string shoesColorName = colors.Keys.ElementAt(ShoesColorID);
                if (ColorUtility.TryParseHtmlString(colors.Values.ElementAt(ShoesColorID), out Color shoesColor))
                {
                    rend.materials[2].SetColor("_Color", shoesColor);
                }
                break;
        }
    }

    void Start()
    {
        
    }
}
