using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFollow))]
public class CinemachineManager : MonoBehaviour
{
    static public CinemachineManager Instance { get; private set; }

    private CinemachineFollow cinemachineFollow;
    private CinemachineCamera CinemachineCamera;
    private void Awake()
    {
        Instance = this;

        cinemachineFollow = GetComponent<CinemachineFollow>();
        CinemachineCamera = GetComponent<CinemachineCamera>();
    }
    
    public void ShakeCamera(
        float duration, 
        float strength = 1f, 
        int vibrato = 10, 
        float randomness = 90, 
        bool snapping = false, 
        bool fadeOut = true
        )
    {

        Vector3 presentValue = cinemachineFollow.TrackerSettings.PositionDamping;
        cinemachineFollow.TrackerSettings.PositionDamping = Vector3.zero;
        CinemachineCamera.Follow.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut).SetUpdate(true);
        StartCoroutine(ResetPositionDamping(presentValue, duration));
    }

    private IEnumerator ResetPositionDamping(Vector3 presentValue, float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        cinemachineFollow.TrackerSettings.PositionDamping = presentValue;
    }
}
