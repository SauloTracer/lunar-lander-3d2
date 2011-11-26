using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SkyDome
{
    class MouseCamera : CameraBase
    {

        #region Variáveis

        float leftrightRot = MathHelper.PiOver2;
        float updownRot = -MathHelper.Pi / 10.0f;
        const float rotationSpeed = 0.3f;
        const float moveSpeed = 3.0f;
        #endregion

        #region Propriedades

        public float LeftrightRot
        {
            get { return leftrightRot; }
            set { leftrightRot = value; }
        }
        
        public float UpdownRot
        {
            get { return updownRot; }
            set { updownRot = value; }
        }
        
        public float RotationSpeed
        {
            get { return rotationSpeed; }
        }

        public float MoveSpeed
        {
            get { return moveSpeed; }
        } 

        #endregion

        #region Método Construtor da Classe
        public MouseCamera(float aspectRatio)
            : base()
        {
            SetLookAt(new Vector3(8.0f, 0.0f, 0.0f), Vector3.Zero, Vector3.Up);
            SetPerspectiveFov(MathHelper.ToRadians(this.Fovy), aspectRatio, this.NearPlane, this.FarPlane);
        }
        #endregion

        #region Métodos Update e addToCameraPosition
        public override void Update(GameTime gametime)
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = Position + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            SetLookAt(new Vector3(Position.X, Position.Y, Position.Z), cameraFinalTarget, cameraRotatedUpVector);
            base.Update(gametime);
        }

        public override void addToCameraPosition(Vector3 vectorToAdd)
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            Position += moveSpeed * rotatedVector;
        }
        #endregion

    }
}
