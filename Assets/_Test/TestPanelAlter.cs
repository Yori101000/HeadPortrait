using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Slap.Test
{

    public class TestPanelAlter : MonoBehaviour
    {
        public Button btn_Create;
        public Button btn_Input;
        public GameObject panel_Create;
        public GameObject panel_Input;

        void Start()
        {
            btn_Create.onClick.AddListener(() =>
            {
                panel_Create.SetActive(true);
                panel_Input.SetActive(false);
            });

            btn_Input.onClick.AddListener(() =>
            {
                panel_Input.SetActive(true);
                panel_Create.SetActive(false);
            });
        }

    }
}