﻿using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
  public class ExperienceDisplay : MonoBehaviour
  {
    private Experience experience;

    private void Awake()
    {
      experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
    }

    private void Update()
    {
      GetComponent<Text>().text = string.Format("{0:0.0}", experience.GetExperience());
    }
  }

}
