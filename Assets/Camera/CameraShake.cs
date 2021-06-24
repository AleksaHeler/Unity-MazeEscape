using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	private static CameraShake instance;
	public static CameraShake Instance { get => instance; }

	private CinemachineVirtualCamera virtualCamera;
	private float shakeTimer;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		virtualCamera = GetComponent<CinemachineVirtualCamera>();
	}

	private void Start()
	{
		SetFollowTarget();
	}

	private void Update()
	{
		SetFollowTarget();

		if (shakeTimer > 0)
		{
			shakeTimer -= Time.deltaTime;
			if(shakeTimer <= 0)
			{
				SetCameraShakeInensity(0);
			}
		}
	}

	private void SetFollowTarget()
	{
		if (virtualCamera.Follow == null)
		{
			if (PlayerAbility.Instance != null)
				virtualCamera.Follow = PlayerAbility.Instance.transform;
		}
	}

	public void ShakeCamera(float intensity, float time)
	{
		SetCameraShakeInensity(intensity);
		shakeTimer = time;
	}

	private void SetCameraShakeInensity(float intensity)
	{
		CinemachineBasicMultiChannelPerlin basicPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		basicPerlin.m_AmplitudeGain = intensity;
	}
}
