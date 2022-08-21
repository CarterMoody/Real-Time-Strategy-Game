/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationParticleSystemHandler : MonoBehaviour {

    public static DestinationParticleSystemHandler Instance { get; private set; }

    private MeshParticleSystem meshParticleSystem;
    private List<Single> singleList;

    private void Awake() {
        Instance = this;
        meshParticleSystem = GetComponent<MeshParticleSystem>();
        singleList = new List<Single>();
    }

    private void Update() {
        for (int i=0; i<singleList.Count; i++) {
            Single single = singleList[i];
            single.Update();
            if (single.IsRotationComplete()) {
                single.DestroySelf();
                singleList.RemoveAt(i);
                i--;
            }
        }
    }

    public void SpawnDestinationParticle(Vector3 position) {
        singleList.Add(new Single(position, meshParticleSystem));
    }


    /*
     * Represents a single Destination Icon effect
     * */
    private class Single {

        private MeshParticleSystem meshParticleSystem;
        private Vector3 position;
        private int quadIndex;
        private Vector3 quadSize;
        private Vector3 maxQuadSize;
        private float shrinkSpeed;
        private float growthSpeed;
        private float rotation;
        private float rotationSpeed;
        private const float DISAPPEAR_TIMER_MAX = 2f;
        private float disappearTimer;

        public Single(Vector3 position, MeshParticleSystem meshParticleSystem) {
            this.position = position;
            this.meshParticleSystem = meshParticleSystem;

            quadSize = new Vector3(0.1f, 0.1f);
            maxQuadSize = new Vector3(1f, 1f);
            rotation = Random.Range(0, 360f);
            rotationSpeed = Random.Range(10.0f, 15.0f);
            shrinkSpeed = 0.99f;
            growthSpeed = 1.01f;
            quadIndex = meshParticleSystem.AddQuad(position, rotation, quadSize, false, 0);
            disappearTimer = DISAPPEAR_TIMER_MAX;
        }

        public void Update() {
            rotation += 360f * (rotationSpeed / 10f) * Time.deltaTime;
            
            if (disappearTimer > DISAPPEAR_TIMER_MAX * .25f) {
                // First half of the popup lifetime
                quadSize = quadSize * growthSpeed;
                if (quadSize.x > maxQuadSize.x){
                    quadSize = maxQuadSize;
                }
            }
            else{
                // Second half of the popup lifetime
                quadSize = quadSize * shrinkSpeed;
            }
            disappearTimer -= Time.deltaTime;

            meshParticleSystem.UpdateQuad(quadIndex, position, rotation, quadSize, true, 0);

            float slowDownFactor = 0.5f;
            rotationSpeed -= rotationSpeed * slowDownFactor * Time.deltaTime;
    
        }

        public bool IsRotationComplete() {
            return rotationSpeed < .1f;
        }

        public void DestroySelf(){
            meshParticleSystem.DestroyQuad(quadIndex);
        }

    }

}
