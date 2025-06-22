using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFollow))]
public class CinemachineManager : MonoBehaviour
{
    static public CinemachineManager Instance { get; private set; }

    public Vector3 PositionDamping
    {
        get => cinemachineFollow.TrackerSettings.PositionDamping;
        set => cinemachineFollow.TrackerSettings.PositionDamping = value;
    }

    private CinemachineFollow cinemachineFollow;
    private CinemachineCamera cinemachineCamera;

    private Vector3 startValue;

    private void Awake()
    {
        Instance = this;

        cinemachineFollow = GetComponent<CinemachineFollow>();
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        cinemachineCamera.Follow = GameManager.PlayerController.TrackingSubject;
        startValue = cinemachineCamera.Follow.transform.localPosition;
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
        cinemachineCamera.Follow.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut).SetUpdate(true).OnComplete(() => { cinemachineCamera.Follow.transform.localPosition = startValue; });
        StartCoroutine(ResetPositionDamping(presentValue, duration));
    }

    private IEnumerator ResetPositionDamping(Vector3 presentValue, float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        cinemachineFollow.TrackerSettings.PositionDamping = presentValue;
    }
}
