using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ChangeLightColor : MonoBehaviour
{
    Light targetLight;
    Color _color;
    Detection scriptDetection;
    private void Awake()
    {
        targetLight = GetComponentInChildren<Light>();
        scriptDetection = GetComponent<Detection>();
    }
    void Update()
    {
       if(scriptDetection.Detected()==true)
        {
            _color = Color.green;
        }
       else if(scriptDetection.Detected()==false)
        {
            _color = Color.red;
        }
        targetLight.color = _color;
    }
}
