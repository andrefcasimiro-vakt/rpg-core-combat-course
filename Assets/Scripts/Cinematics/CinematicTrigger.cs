using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool alreadyTriggered = false;
        public PlayableDirector cutscene;

        private void Start()
        {
            if (cutscene == null)
            {
                // Attempt to find PlayableDirector component
                cutscene = GetComponent<PlayableDirector>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && cutscene != null && !alreadyTriggered) { 
                cutscene.Play();
                alreadyTriggered = true;
            }
        }
    }

}

