using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyDome
{
    public class CameraManager
    {
        #region Variaveis
        // Armazena a Câmera ativa
        int activeCameraIndex;
        CameraBase activeCamera;

        // Lista contendo todas as cameras
        SortedList<string, CameraBase> cameras;
        #endregion

        #region Properties

        // Indice da camera ativa
        public int ActiveCameraIndex
        {
            get
            {
                return activeCameraIndex;
            }
        }

        public CameraBase ActiveCamera
        {
            get
            {
                return activeCamera;
            }
        }

        public CameraBase this[int index]
        {
            get
            {
                return cameras.Values[index];
            }
        }

        public CameraBase this[string id]
        {
            get
            {
                return cameras[id];
            }
        }

        public int Count
        {
            get
            {
                return cameras.Count;
            }
        }
        #endregion

        #region Construtor

        public CameraManager()
        {
            cameras = new SortedList<string, CameraBase>(4);
            activeCameraIndex = -1;
        }
        #endregion

        #region Metodos
        //Defini qual é a camera ativa
        public void SetActiveCamera(int cameraIndex)
        {
            activeCameraIndex = cameraIndex;
            activeCamera = cameras[cameras.Keys[cameraIndex]];
        }
        //Defini qual é a camera ativa
        public void SetActiveCamera(string id)
        {
            activeCameraIndex = cameras.IndexOfKey(id);
            activeCamera = cameras[id];
        }

        //remove todas as Câmeras e reseta o método
        public void Clear()
        {
            cameras.Clear();
            activeCamera = null;
            activeCameraIndex = -1;
        }

        //Adiciona uma Câmera
        public void Add(string id, CameraBase camera)
        {
            cameras.Add(id, camera);

            if (activeCamera == null)
            {
                activeCamera = camera;
                activeCameraIndex = cameras.IndexOfKey(id);
            }
        }

        //Remove uma Câmera
        public void Remove(string id)
        {
            cameras.Remove(id);
        }
        #endregion
    }
}
