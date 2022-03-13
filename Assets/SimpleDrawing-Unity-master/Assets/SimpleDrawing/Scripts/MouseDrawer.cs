using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleDrawing
{
    public class MouseDrawer : MonoBehaviour
    {
        // Drawing Group UI Top
        public Slider redSlider;
        public Slider greenSlider;
        public Slider blueSlider;
        public Slider sizeSlider;
        public Image colorImage;

        // Drawing Group UI Bottom
        public Toggle eraserToggle;
        
        static int red = 0, green = 0, blue = 0;
        Color penColor = new Color(red, green, blue);

        private bool reset = false;

        [SerializeField]
        int penWidth = 1;

        [SerializeField]
        bool erase = false;
 
        Vector2 defaultTexCoord = Vector2.zero;
        Vector2 previousTexCoord;
        
        void Update()
        {
            SizeSlider();
            RedSlider();
            BlueSlider(); 
            GreenSlider();
            ChangeColor(red,green,blue);
            
            bool mouseDown = Input.GetMouseButton(0);
            if (mouseDown)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
				if(Physics.Raycast(ray, out hitInfo))
                {
                    if(hitInfo.collider != null && hitInfo.collider is MeshCollider)
                    {
                        var drawObject = hitInfo.transform.GetComponent<DrawableCanvas>();
                        if (drawObject != null)
                        {
                            if (reset)
                            {
                                drawObject.ResetCanvas();
                            }
                            
                            Vector2 currentTexCoord = hitInfo.textureCoord;
                            if (erase)
                            {
                                drawObject.Erase(currentTexCoord, previousTexCoord, penWidth);
                            }
                            else
                            {
                                drawObject.Draw(currentTexCoord, previousTexCoord, penWidth, penColor);
                            }
                            previousTexCoord = currentTexCoord;
                            reset = false;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("If you want to draw using a RaycastHit, need set MeshCollider for object.");
                    }
                }
                else
                {
                    previousTexCoord = defaultTexCoord;
                }
            }
            else if (!mouseDown) // Mouse is released
            {
                previousTexCoord = defaultTexCoord;
            }
        }

        public void RedSlider()
        {
            red = (int)redSlider.value;
        }
        
        public void GreenSlider()
        {
            green = (int)greenSlider.value;
        }
        public void BlueSlider()
        {
            blue = (int)blueSlider.value;
        }

        private void ChangeColor(int red, int green, int blue)
        {
            penColor = new Color(red / 255f, green / 255f, blue / 255f);
            colorImage.color = penColor;
        }
        public void SizeSlider()
        {
            penWidth = (int) sizeSlider.value;
        }

        public void ResetButton()
        {
            reset = true;
        }
        public void EraserToggle()
        {
            erase = eraserToggle.isOn;
        }
    }
}