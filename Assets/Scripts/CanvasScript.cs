using UnityEngine;
using UnityEngine.UI;
public class CanvasScript : MonoBehaviour 
{
 public Text canvasText1;
   void Start () 
   {
      canvasText1.enabled = false; 
   }
   void DisableText()
   { 
      canvasText1.enabled = false; 
   }    
   public void goodjob()
   {
    canvasText1.enabled = true; 
      Invoke("DisableText", 2f);//invoke after 5 seconds
   }
}