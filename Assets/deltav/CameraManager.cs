using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CanvasManager canvasMan;
    public Transform sunPivot;
    public bool Focused { get; private set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void FocusOn(CelestialBodyClickTarget cbct)
    {
        Camera.main.transform.parent.SetParent(cbct.transform);
        Camera.main.transform.parent.localPosition = Vector3.zero;
        Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, 0, -1);
        Camera.main.transform.localRotation = Quaternion.identity;
        Focused = true;
        canvasMan.FocusPlanet(cbct.GetTiedPlanet());
    }

    internal void ExitPlanetFocus()
    {
        Camera.main.transform.parent.SetParent(sunPivot);
        Camera.main.transform.parent.localPosition = Vector3.zero;
        Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, -40);

        Focused = false;
        canvasMan.UnFocusPlanet();
    }

    public float distance = 1.0f;
    public float xSpeed = 200f;
    public float ySpeed = 200f;
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    //public float distanceMin = 10f;
    //public float distanceMax = 10f;
    //public float smoothTime = 2f;
    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;
    float velocityX = 0.0f;
    float velocityY = 0.0f;
    float prevX, prevY;
    internal void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            float newX = Input.GetAxis("Mouse X");
            float newY = Input.GetAxis("Mouse Y");
            velocityX += xSpeed * (newX - prevX);
            velocityY += ySpeed * (newY - prevY);
            prevX = newX;
            prevY = newY;
            rotationYAxis += velocityX;
            rotationXAxis -= velocityY;
            rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
            Quaternion fromRotation = Quaternion.Euler(Camera.main.transform.parent.rotation.eulerAngles.x, Camera.main.transform.parent.rotation.eulerAngles.y, 0);
            Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
            Quaternion rotation = toRotation;
            Camera.main.transform.parent.rotation = toRotation;
        }
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
