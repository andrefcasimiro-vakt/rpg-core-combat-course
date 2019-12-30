using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement { 

    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float fadeInTime = 0.5f;

        const string defaultSaveFile = "save";
        SavingSystem savingSystem;
        Fader fader;

        IEnumerator Start()
        {
            savingSystem = GetComponent<SavingSystem>();
            fader = FindObjectOfType<Fader>();

            // Start any scene with black screen
            fader.FadeOutImmediate();

            yield return savingSystem.LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadMethod();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveMethod();
            }
        }

        public void SaveMethod()
        {
            savingSystem.Save(defaultSaveFile);
        }

        public void LoadMethod()
        {
            savingSystem.Load(defaultSaveFile);
        }
    }

}
