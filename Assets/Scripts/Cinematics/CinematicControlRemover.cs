using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        public PlayableDirector cutscene;

        GameObject player;

        private void Start()
        {
            if (cutscene == null)
            {
                cutscene = GetComponent<PlayableDirector>();
            }

            cutscene.played += DisableControl;
            cutscene.stopped += EnableControl;

            player = GameObject.FindWithTag("Player");

        }

        public void DisableControl(PlayableDirector playableDirector)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();

            // Disable controller
            player.GetComponent<PlayerController>().enabled = false;
        }

        public void EnableControl(PlayableDirector playableDirector)
        {
            // Enable controller
            player.GetComponent<PlayerController>().enabled = true;
        }
    }

}

