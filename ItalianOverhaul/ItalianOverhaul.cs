using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ItalianOverhaul
{
    public class ItalianOverhaulModMeta : ModMeta
    {
        public override string Name => "Italian Overhaul";

        public override void ConstructOptionsScreen(RectTransform parent, bool inGame)
        {
            Text text = WindowManager.SpawnLabel();
            text.text = @"
                Italian Overhaul is a mod for Software Inc. which brings the Authentic Italian™ software house experience.
                Currently in development, not much to see here!!
            ";
            WindowManager.AddElementToElement(text.gameObject, parent.gameObject, new Rect(0f, 0f, 400f, 128f),
                new Rect(0f, 0f, 0f, 0f));
        }

        public static bool GiveMeFreedom = true;
    }

    public class IoBehaviour : ModBehaviour
    {
        public override void OnActivate()
        {
            Debug.Log("Italian Overhaul activated!");
        }
        public override void OnDeactivate()
        {
            Debug.Log("Italian Overhaul deactivated!");
        }

        void Start()
        {
            Debug.Log("Italian Overhaul started!");
        }
    }
}
