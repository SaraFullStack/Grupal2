using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq; 

public class HUDController : MonoBehaviour
{
    private const string gear = "Gear";
    private const string life = "LifeUnit";
    

    private VisualElement _gear;
    private List<VisualElement> _lifeUnits;
    
    private float actualAngle = 0f;
    private const float degreesPerTime = 180f;
    
    // Singleton
    private static HUDController _instance;
    public static HUDController Instance { get { return _instance; } }
    
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        _lifeUnits = new List<VisualElement>();
        
        var root = GetComponent<UIDocument>().rootVisualElement;
        _gear = root.Q<VisualElement>(gear);
        
        foreach (int i in Enumerable.Range(1, 10))
        {
            string elementName = "LifeUnit" + i;
            _lifeUnits.Add(root.Q<VisualElement>(elementName));
        }
        
        root.style.display = DisplayStyle.Flex; // Show HUD
        
    }
    
    #region Static Methods
    public static void GainLife(int newLife)
    {
        _instance.UpdateLife(newLife);
        _instance.AddLife();

    }
    
    public static void LoseLife(int newLife)
    {
        _instance.UpdateLife(newLife);
        _instance.SubstractLife();

    }
    #endregion

    private void AddLife()
    {
        actualAngle += degreesPerTime;
        _gear.style.rotate = new StyleRotate(new Rotate(Angle.Degrees(actualAngle)));

    }
    
    private void SubstractLife()
    {
        actualAngle -= degreesPerTime;
        _gear.style.rotate = new StyleRotate(new Rotate(Angle.Degrees(actualAngle)));
    }

    private void UpdateLife(int newLife)
    {
        foreach (int i in Enumerable.Range(1, 10))
        {
            if (i <= newLife)
            {
                _lifeUnits[i-1].style.visibility = Visibility.Visible;
            }
            else
            {
                _lifeUnits[i-1].style.visibility = Visibility.Hidden;
            }
            
        }
    }
}
