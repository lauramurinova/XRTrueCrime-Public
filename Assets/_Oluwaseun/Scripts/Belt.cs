using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Belt : MonoBehaviour
    {




        [SerializeField] private Transform _parentObjectRot;
        [SerializeField] private Vector3 _beltOffset;
        [SerializeField] private Vector3 euler = new Vector3(0, 0, 0);
        [SerializeField] private Quaternion eulerDebug;
        [SerializeField] private float _smoothFactorPos = 2f;
        [SerializeField] private float _smoothFactorRot = 2f;

        private float counter;
        private Transform _parentObjectPos;
        

        // Start is called before the first frame update
        void Start()
        {
            _parentObjectPos = Camera.main.transform;
            //_parentObjectPos = _parentObjectRot;
            transform.rotation = GetTargetRot();

            


        }

        // Update is called once per frame
        void Update()
        {
            
            if (counter < 10)
            {
                counter += Time.deltaTime;
            }
            //_smoothFactorRot = SmoothFactorRot(6f);
            
            UpdateLocation();
            UpdateRotation();

        }

        private void UpdateLocation()
        {
            var newPos = GetTargetPos();
            //transform.position = newPos;
            transform.position = Vector3.Lerp(transform.position, newPos, _smoothFactorPos * Time.deltaTime);
            

        }

        private void UpdateRotation()
        {
            //if (counter > 9) return;
            var newRot = GetTargetRot();
            eulerDebug = GetTargetRot();
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, _smoothFactorRot * Time.deltaTime);
            //prevParentObjectRot = transform.eulerAngles.y;

        }

        Quaternion GetTargetRot()
        {
           return Quaternion.Euler(euler.x, _parentObjectRot.eulerAngles.y, euler.z);
        }

        Vector3 GetTargetPos()
        {
            Vector3 targetPos = _parentObjectPos.TransformPoint(_beltOffset);
            Vector3 forward = Vector3.ProjectOnPlane(_parentObjectPos.forward, Vector3.up);
            targetPos = _parentObjectPos.position + (forward * _beltOffset.z);
            targetPos.x = _parentObjectPos.position.x - _beltOffset.x;
            targetPos.y = _parentObjectPos.position.y - _beltOffset.y;
            return targetPos;
        }

        private float SmoothFactorRot(float timer)
        {
            float number = 0f;
            if (counter <= timer)
            {
                number = 2f;
            }
            else if (counter > timer)
            {
                number = 0.1f;
            }

            return number;
        }
    }




}