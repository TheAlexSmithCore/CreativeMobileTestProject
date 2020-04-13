using System;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace GameManagers
{
    public class MainGameManager : MonoBehaviour
    {
        public delegate void ProjectVersionChanged();
        public static event ProjectVersionChanged OnProjectVersionChanged;

        private void Start() {
            OnProjectVersionChanged();
        }
    }
}
