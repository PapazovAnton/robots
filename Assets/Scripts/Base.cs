using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Base : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;

    private float _resourceCount = 0;
    private Queue<Robot> _robots = new Queue<Robot>();

    private void Awake()
    {
        Robot[] robots = FindObjectsOfType<Robot>();

        foreach (Robot robot in robots)
        {
            _robots.Enqueue(robot);
        }
    }

    private void OnEnable()
    {
        Platform.onFreeBarrel += SelectFreeRobot;
        Robot.freed += Shipment;
    }

    private void OnDisable()
    {
        Platform.onFreeBarrel -= SelectFreeRobot;
        Robot.freed -= Shipment;
    }

    private void SelectFreeRobot(Barrel barrel)
    {
        Robot robot = _robots.Dequeue();
        robot.TrySetTarget(barrel);
    }

    private void Shipment(Robot robot)
    {
        _resourceCount++;
        UpdateResourceCounter();
        _robots.Enqueue(robot);
    }

    private void UpdateResourceCounter()
    {
        _textMeshPro.text = _resourceCount.ToString();
    }
}