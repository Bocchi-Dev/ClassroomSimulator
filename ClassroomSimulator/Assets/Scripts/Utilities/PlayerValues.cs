using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public string playerName;

    public Color skinColor;
    public Color shirtColor;
    public Color shortsColor;
    public Color shoesColor;

    public int hairStyleNum;

    public void setValues()
    {
        skinColor = FindObjectOfType<CharacterCustomization>().skinHex;
        shirtColor = FindObjectOfType<CharacterCustomization>().shirtHex;
        shortsColor = FindObjectOfType<CharacterCustomization>().shortsHex;
        shoesColor = FindObjectOfType<CharacterCustomization>().shoesHex;
    }

    public void setHairValue(int num)
    {
        hairStyleNum = num;
    }
}
