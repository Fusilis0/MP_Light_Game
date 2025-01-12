using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColorManager : MonoBehaviour
{
    public bool rValue;
    public bool gValue;
    public bool bValue;

    public RGB_Button R_Button;
    public RGB_Button G_Button;
    public RGB_Button B_Button;

    public Material outColor;
    public List<Material> colors;
    public int colorNumber;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rValue = R_Button.isOn;
        gValue = G_Button.isOn;
        bValue = B_Button.isOn;

        if (rValue && gValue && bValue)
        {
            colorNumber = 6; //white
        }
        else if (rValue && gValue && !bValue)
        {
            colorNumber = 3; // yellow
        }
        else if (rValue && !gValue && !bValue)
        {
            colorNumber = 0; //red
        }
        else if (!rValue && gValue && bValue)
        {
            colorNumber = 4; //cyan
        }
        else if (!rValue && !gValue && bValue)
        {
            colorNumber = 2; //blue
        }
        else if (rValue && !gValue && bValue)
        {
            colorNumber = 5; //magenta
        }
        else if (!rValue && gValue && !bValue)
        {
            colorNumber = 1; // green
        }
        else
        {
            colorNumber = 7; //transparent
        }

        outColor = colors[colorNumber];
    }

}
