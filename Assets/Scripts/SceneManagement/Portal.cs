using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {

        [Header("Next Scene Settings")]
        public string destinationPortalName;
        public int sceneToLoad = -1;

        [Header("Current Scene Settings")]
        public Transform spawnPoint;

        [Header("Transition Settings")]
        public float fadeTimeBetweenScenes = 0.25f;

        Fader fader;

        private void Start()
        {
            fader = FindObjectOfType<Fader>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }
        
        private IEnumerator Transition()
        {
            // Preserve this portal for now
            DontDestroyOnLoad(this.gameObject);

            // Fade Out
            yield return fader.FadeOut(fadeTimeBetweenScenes);

            // Load Async
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // Update player position based on future portal spawnpoint
            UpdatePlayer(GetOtherPortal());

            // Fade In
            yield return fader.FadeIn(fadeTimeBetweenScenes);

            // Finally, destroy the old portal
            Destroy(this.gameObject);
        }

        void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");

            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        Portal GetOtherPortal()
        {
            // We modify this portal name before we destroy it so we can use the same portal name across
            // different scenes
            this.gameObject.name = this.gameObject.name + "_";

            return GameObject.Find(destinationPortalName).GetComponent<Portal>();
        }
    }
}
