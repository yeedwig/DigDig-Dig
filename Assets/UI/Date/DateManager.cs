using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateManager : MonoBehaviour
{
    public DateTime startDate;
    public DateTime updateDate;
    public double timeFlow;
    public Text dateText;

    public double AddDays;
    // Start is called before the first frame update
    void Awake()
    {
        startDate = DateTime.Now;
        updateDate = startDate;
    }

    // Update is called once per frame
    void Update()
    {
        dateText.text = updateDate.ToString();
        updateDate = updateDate.Add(TimeSpan.FromMinutes(timeFlow));
    }

    public void AddDate()
    {
        updateDate = updateDate.AddDays(AddDays);//(TimeSpan.FromSeconds(180));
    }
}
