using UnityEngine;
using UnityEngine.UI;

namespace SimpleDrawing
{
    public class MouseDrawer : MonoBehaviour
    {
        public static Slider RedSlider;
        public static Slider GreenSlider;
        public static Slider BlueSlider;
        public static Slider SizeSlider;
        public void setRed(int red)
        {
            RedSlider.value = red;
        }
        public void setGreen(int green)
        {
            GreenSlider.value = green;
        }
        public void setBlue(int blue)
        {
            BlueSlider.value = blue;
        }
        public void setSize(float size)
        {
            SizeSlider.value = size;
        }

        private Color penColor = Color.black;
        float penWidth = 3f;

        [SerializeField]
        bool erase = false;
 
        Vector2 defaultTexCoord = Vector2.zero;
        Vector2 previousTexCoord;

        void Update()
        {
            penColor = new Color(RedSlider.value/255f, GreenSlider.value/255f,BlueSlider.value/255f);
            penWidth = SizeSlider.value;
            
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
    }
}