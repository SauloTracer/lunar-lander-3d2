using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SkyDome
{
    public abstract class CameraBase
    {
        #region Variáveis

        //Variáveis do Terremoto
        protected static readonly Random random = new Random(); // Criará um número aleatório
        private bool shaking = false;                           //Controle se já esta tremendo
        private float shakeMagnitude;                           //Magnitude do Terremoto
        private float shakeDuration;                            //Duração do Terremoto
        private float shakeTimer;                               //Timer de terremotos
        private Vector3 shakeOffset;                            //Vector de distancia

        //Parametros para a projeção
        float fovy;
        float aspectRatio;
        float nearPlane;
        float farPlane;

        //posição e alvo
        Vector3 position;
        Vector3 target;

        //Vetore de orientação
        Vector3 headingVec;
        Vector3 strafeVec;
        Vector3 upVec;
        #endregion

        #region Propriedades
        public Vector3 Target
        {
            get { return target; }
            set { target = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float FarPlane
        {
            get { return farPlane; }
            set { farPlane = value; }
        }

        public float NearPlane
        {
            get { return nearPlane; }
            set { nearPlane = value; }
        }

        public float AspectRation
        {
            get { return aspectRatio; }
            set { aspectRatio = value; }
        }

        public float Fovy
        {
            get { return fovy; }
            set { fovy = value; }
        }
        #endregion

        #region Matrizes e Controles

        //Controles/Flags
        protected bool needUpdateProjection;
        protected bool needUpdateFrustum;
        protected bool needUpdateView;
        //Matrizes
        protected Matrix viewMatrix;
        protected Matrix projectionMatrix;

        #endregion

        #region Atualizar View e Projection
        //Obtem Matriz de Projeção
        public Matrix Projection
        {
            get
            {
                if (needUpdateProjection) UpdateProjection();
                return projectionMatrix;
            }
        }
        //Obtem a Matriz de Visão da CAmera
        public Matrix View
        {
            get
            {
                if (needUpdateView) UpdateView();
                return viewMatrix;
            }
        }

        //Atualiza a matriz de projeção Perspectiva
        protected virtual void UpdateProjection()
        {
            //Cria a matriz de visão da perspectiva
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                fovy, aspectRatio, nearPlane, farPlane);
            needUpdateProjection = false;
            needUpdateFrustum = true;
        }

        //Atualiza a visão da Camera
        protected virtual void UpdateView()
        {
            if (shaking)
            {
                position += shakeOffset;
                target += shakeOffset;
            }
            viewMatrix = Matrix.CreateLookAt(position, target, upVec);
            needUpdateView = false;
            needUpdateFrustum = true;
        }
        #endregion

        #region Defini nossos View e Projection
        //Atribui Projeção a Camera
        public void SetPerspectiveFov(float fovy, float aspectratio, float nearPlane, float farPlane)
        {
            this.fovy = fovy;
            this.aspectRatio = aspectratio;
            this.nearPlane = nearPlane;
            this.farPlane = farPlane;
            needUpdateProjection = true;
        }

        //Configura a visão da Camera
        public void SetLookAt(Vector3 cameraPos, Vector3 cameraTarget, Vector3 CameraUp)
        {
            this.position = cameraPos;
            this.target = cameraTarget;
            this.upVec = CameraUp;
            //Calcura os eixos da Camera
            headingVec = cameraTarget - cameraPos;
            headingVec.Normalize();
            upVec = CameraUp;
            strafeVec = Vector3.Cross(headingVec, upVec);
            needUpdateView = true;
        }
        #endregion

        #region Update, addToCameraPosition, Shack e nemDouble
        //Atualiza todos os Nossos componentes
        public virtual void Update(GameTime gametime)
        {
            // Se estamos tremendo
            if (shaking)
            {
                // Ajusta nosso timer de acordo com o tempo passado
                shakeTimer += (float)gametime.ElapsedGameTime.TotalSeconds;

                // Se estivermos no limite de tempo desliga o terremoto
                if (shakeTimer >= shakeDuration)
                {
                    shaking = false;
                    shakeTimer = shakeDuration;
                }

                // Compute our progress in a [0, 1] range
                float progress = shakeTimer / shakeDuration;

                // Compute our magnitude based on our maximum value and our progress. This causes
                // the shake to reduce in magnitude as time moves on, giving us a smooth transition
                // back to being stationary. We use progress * progress to have a non-linear fall 
                // off of our magnitude. We could switch that with just progress if we want a linear 
                // fall off.
                float magnitude = shakeMagnitude * (1f - (progress * progress));

                // Generate a new offset vector with three random values and our magnitude
                shakeOffset = new Vector3(NextFloat(), NextFloat(), NextFloat()) * magnitude;
            }

            UpdateView();
        }

        //Atualiza a nossa posição de câmera, assim como target, fovy...
        public virtual void addToCameraPosition(Vector3 vectorToAdd)
        {
            //A implementar - Devemos implementar na hora de criar os diferentes tipos de câmera
        }

        public void Shake(float magnitude, float duration)
        {
            // Estamos no meio de um terremoto
            shaking = true;

            // Guarda nossa magnitude e Duração
            shakeMagnitude = magnitude;
            shakeDuration = duration;

            // Reseta nosso Timer
            shakeTimer = 0f;
        }

        //Gera um número entre -1 e 1;
        private float NextFloat()
        {
            return (float)random.NextDouble() * 2f - 1f;
        }
        #endregion
    }
}
