using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 1f;

    [Header("Audio")]
    AudioSource audioSource;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip failSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem sideLeftEngineParticle;
    [SerializeField] ParticleSystem sideRightEngineParticle;
    [SerializeField] ParticleSystem mainEngineParticle;

    bool isAlive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        processRotation();
    }

    private void processRotation()
    { 
        if (Input.GetKey(KeyCode.A))
        {
            RightRotate();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            LeftRotate();
        }
        else 
        {
            if(sideRightEngineParticle.isPlaying)
            { 
                sideRightEngineParticle.Stop();
            }
            if(sideLeftEngineParticle.isPlaying)
            {
                sideLeftEngineParticle.Stop();
            }
        }

    }

    void RightRotate()
    {
        if (!sideRightEngineParticle.isPlaying)
        {
            sideRightEngineParticle.Play();
        }
        rotateObj(rotationThrust);
    }

    void LeftRotate()
    {
        if (!sideLeftEngineParticle.isPlaying)
        {
            sideLeftEngineParticle.Play();
        }
        rotateObj(-rotationThrust);
    }

    public void playSuccessSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
    }

    public void playFailSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(failSound);
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrust();   
        }
        else
        {
            StopThrust();
        }
    }

    void StartThrust()
    {
        //상대적인 힘을 방향에 맞게 0,1,0 사물의 y축방향으로 증가
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainEngineParticle.isPlaying)
        {
            mainEngineParticle.Play();
        }
    }

    void StopThrust()
    {
        audioSource.Stop();
        if (mainEngineParticle.isPlaying)
        {
            mainEngineParticle.Stop();
        }

    }

    void rotateObj(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * Time.deltaTime *  rotationThisFrame);
        rb.freezeRotation = false; // unfreezing rotation system
    }
}
