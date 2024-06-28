using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;
using Random = UnityEngine.Random;

using UnityEngine.InputSystem.HID;

namespace Assets
{
    public class PhoneManager : MonoBehaviour
    {
        [Header("Phone Screen")] 
        [SerializeField] private Renderer _phoneScreenRenderer;
        [SerializeField] private List<GameObject> _phoneScreenList;
        [SerializeField] private List<GameObject> _galleryImages;
        [SerializeField] private List<Material> _phoneScreenImages;
        
        
        [Header("Phone Calls")]
        [SerializeField] private AudioClip _callAudio;
        [SerializeField] private float vibrationDuration = 2f; // Duration of vibration in seconds
        [SerializeField] private float vibrationIntensity = 0.5f; // Intensity of the vibration
        
        private AudioSource _audioSource;
        [SerializeField] private int _screenImageNo = 0;
        

        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();

        }
        
        public void ReceiveCall()
        {
            PlayAudio(_callAudio);
            StartCoroutine(VibratePhone());
        }

        private IEnumerator VibratePhone()
        {
            EnableScreen(2);
            float elapsedTime = 0f;
            Vector3 originalPosition = transform.localPosition;
            Quaternion originalRotation = transform.localRotation;

            while (elapsedTime < vibrationDuration)
            {
                float x = Random.Range(-vibrationIntensity, vibrationIntensity) * 0.2f;
                float y = Random.Range(-vibrationIntensity, vibrationIntensity) * 0.2f;
                float z = Random.Range(-vibrationIntensity, vibrationIntensity) * 0.2f;

                transform.localPosition = originalPosition + new Vector3(x, 0, z);
                transform.localRotation = originalRotation * Quaternion.Euler(x, y, z);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Reset position and rotation
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;

            if (!_phoneScreenList[3].activeSelf)
            {
                EnableScreen(6);
            }
            
                
            
            
            
        }

        private void PlayAudio(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void NextPicture()
        {
            _screenImageNo++;
            if (_screenImageNo > _galleryImages.Count -1)
            {
                _screenImageNo = _galleryImages.Count -1;
            }
            HideOtherGalleryImages(_screenImageNo);
        }
        
        public void PrevPicture()
        {
            _screenImageNo--;
            if (_screenImageNo < 0)
            {
                _screenImageNo = 0;
            }
            HideOtherGalleryImages(_screenImageNo);
            
            
        }

        private void SetPhoneScreen(List<Material> matList, int matID)
        {
            _phoneScreenRenderer.material = matList[matID];
        }

        private void HideOtherGalleryImages(int imageNO)
        {
            foreach (var image in _galleryImages)
            {
                image.SetActive(false);
            }
            
            _galleryImages[imageNO].SetActive(true);
        }

        public void EnableScreen(int screenToEnable)
        {
            foreach (var screen in _phoneScreenList)
            {
                screen.SetActive(false);
            }
            
            _phoneScreenList[screenToEnable].SetActive(true);
        }

        
    }
    
    
}
