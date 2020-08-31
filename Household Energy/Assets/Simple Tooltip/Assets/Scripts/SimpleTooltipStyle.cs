using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[Serializable]
[CreateAssetMenu]
public class SimpleTooltipStyle : ScriptableObject
{
    [System.Serializable]
    public struct Style
    {
        public string tag;
        public Color color;
        public bool bold;
        public bool italic;
        public bool underline;
        public bool strikethrough;
    }

    [Header("Tooltip Panel")]
    public Sprite slicedSprite;
    public Color color = Color.gray;

    [Header("Display Text Side")]
    public Alignment alignment = Alignment.Left; 

    [Header("Left Font")]
    public TMP_FontAsset leftFontAsset;
    public float leftFontSize = 0;
    public Color leftDefaultColor = Color.white;

    [Header("Right Font")]
    public TMP_FontAsset rightFontAsset;
    public float rightFontSize = 0;
    public Color rightDefaultColor = Color.white;

    [Header("Formatting")]
    public Style[] fontStyles;
}

[Serializable]
public enum Alignment
{
    Left, 
    Right, 
    Both
}