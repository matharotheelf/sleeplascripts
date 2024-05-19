using Meta.WitAi;
using Meta.WitAi.Json;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace Meta.Voice.Samples.Shapes
{
    public class BreathController : MonoBehaviour
    {
        [SerializeField] Animator breathingAnimator;

        [SerializeField] float breathingAnimationLength;
        [SerializeField] float minBreathVolume;
        [SerializeField] float outBreathNormalisedDuration;
        [SerializeField] float smoothCoef;
        [SerializeField] Color defaultHoopColor;

        [SerializeField] Material defaultHoopMaterial;
        [SerializeField] Material failHoopMaterial;

        [SerializeField] GameObject hoop;
        [SerializeField] Vector3 hoopScale;
        [SerializeField] GameObject[] timerNumbers;
        [SerializeField] ParticleSystem breathParticleSystem;
        [SerializeField] ParticleSystem leftHandParticleSystem;
        [SerializeField] ParticleSystem rightHandParticleSystem;

        [SerializeField] float breathParticleMaxCount;
        [SerializeField] float handParticleMaxCount;

        private float smoothedVolumeLevel = 0.0001f;

        void Update()
        {
            // This grows or shrinks the oscillating hoop
            if(breathingAnimator.GetBool("isBreathing"))
            {
                hoop.transform.localScale = hoopScale;
            }
        }

        // run when a breath is complete
        public void EndBreath()
        {
            // sets the volume level of the mic to the default
            smoothedVolumeLevel = 0.0002f;

            // resets all of the variables to their default value
            breathingAnimator.SetBool("isBreathing", false);
            breathingAnimator.SetBool("failedBreath", false);
            hoop.GetComponent<Renderer>().material = defaultHoopMaterial;
            hoop.SetActive(false);
        }

        // run when a failed breath is complete to restart a new one
        public void EndFailure()
        {
            hoop.GetComponent<Renderer>().material = defaultHoopMaterial;
            breathingAnimator.SetBool("failedBreath", false);
        }

        // run when the breath falls below the critical volume to fail the breath
        public void voiceBelowThreshold(float volumeLevel)
        {
            // apply smoothing function to steady volume
            smoothedVolumeLevel = smoothedVolumeLevel + smoothCoef * (volumeLevel - smoothedVolumeLevel);

            if (2*smoothedVolumeLevel <= minBreathVolume && isBreathFailable())
            {
                failBreath();
            }
        }

        // ensures breath is in a state where it can fail
        private bool isBreathFailable()
        {
            // is in first half of a breath out
            return breathingAnimator.GetBool("isBreathingOut") && !breathingAnimator.GetBool("failedBreath") && breathingAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5;
        }

        // trigger the start of a failed breath
        public void failBreath()
        {
            breathingAnimator.Play("FailedBreathAnimation");
            breathingAnimator.SetBool("failedBreath", true);
            breathingAnimator.SetBool("isBreathingOut", false);
            hoop.GetComponent<Renderer>().material = failHoopMaterial;
        }

        // if voice is above threshold trigger particle emissions
        public void voiceAboveThreshold(float volumeLevel)
        {
            if (smoothedVolumeLevel >= minBreathVolume && breathingAnimator.GetBool("isBreathingOut") && !breathingAnimator.GetBool("failedBreath"))
            {
                var emissionBreath = breathParticleSystem.emission;
                emissionBreath.rateOverTime = breathParticleMaxCount * (smoothedVolumeLevel/ minBreathVolume);

                var emissionLeft = leftHandParticleSystem.emission;
                emissionLeft.rateOverTime = handParticleMaxCount * (smoothedVolumeLevel / minBreathVolume);

                var emissionRight = rightHandParticleSystem.emission;
                emissionRight.rateOverTime = handParticleMaxCount * (smoothedVolumeLevel / minBreathVolume);
            }
        }

        // trigger the begginning of a new breath
        public void voiceAboveThresholdTrigger(float volumeLevel)
        {
            if (volumeLevel >= 10*minBreathVolume && breathingAnimator.GetCurrentAnimatorStateInfo(0).IsName("StartBreatheOut"))
            {
                breathingAnimator.SetBool("isBreathingOut", true);
                breathingAnimator.Play("BreathOut");
                smoothedVolumeLevel = 0.0001f;
            }
        }

        // start the breathing excercise
        public void startBreathingExcercise()
        {
            hoop.SetActive(true);
            breathingAnimator.SetBool("isBreathing", true);
        }

        // complete a full iteration of a breathing cycle
        public void completeBreath()
        {
            breathingAnimator.SetInteger("BreathNumber", breathingAnimator.GetInteger("BreathNumber") + 1);
            breathingAnimator.SetBool("isBreathingOut", false);

            var emissionBreath = breathParticleSystem.emission;
            emissionBreath.rateOverTime = 0;

            var emissionLeft = leftHandParticleSystem.emission;
            emissionLeft.rateOverTime = 0;

            var emissionRight = rightHandParticleSystem.emission;
            emissionRight.rateOverTime = 0;

            if (breathingAnimator.GetInteger("BreathNumber") == 4)
            {
                hoop.SetActive(false);
            }
        }

        // a coroutine to change the number timers
        IEnumerator StartTimer(int duration)
        {
            foreach (int index in Enumerable.Range(0, duration + 1))
            {
                int time = duration - index;

                // disable last number
                if(time != 0)
                {
                    timerNumbers[time - 1].SetActive(true);
                }

                // enable new number
                if(time != duration)
                {
                    timerNumbers[time].SetActive(false);
                }

                // wait a second until next number change
                yield return new WaitForSeconds(1f);
            }
        }

        // start breath out timer
        public void breathOutTimer()
        {
            StartCoroutine(StartTimer(8));
        }

        // start breath in timer
        public void breathInTimer()
        {
            StartCoroutine(StartTimer(4));
        }

        // hold breath timer
        public void holdBreathTimer()
        {
            StartCoroutine(StartTimer(7));
        }
    }
}
