using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MapaDeLuzes
{
    public class World
    {
        Vector3 translate;
        Vector3 scale;
        Vector3 rotate;

        bool needUpDate;
        Matrix matrix;

        public World()
            : this(Vector3.Zero, Vector3.Zero, Vector3.One)
        {
            matrix = Matrix.Identity;
            needUpDate = false;
        }
        public World(Vector3 translate, Vector3 rotate, Vector3 scale)
        {
            this.translate = translate;
            this.rotate = rotate;
            this.scale = scale;
            needUpDate = true;
            matrix = Matrix.Identity;
        }

        public Vector3 Translate
        {
            get { return translate; }
            set { translate = value; needUpDate = true; }
        }
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; needUpDate = true; }
        }
        public Vector3 Rotate
        {
            get { return rotate; }
            set { rotate = value; needUpDate = true; }
        }

        public Matrix Matrix
        {
            get
            {
                if (needUpDate)
                {
                    matrix =
                    Matrix.CreateScale(scale) *
                    Matrix.CreateRotationY(MathHelper.ToRadians(rotate.Y)) *
                    Matrix.CreateRotationX(MathHelper.ToRadians(rotate.X)) *
                    Matrix.CreateRotationZ(MathHelper.ToRadians(rotate.Z)) *
                    Matrix.CreateTranslation(translate);
                    needUpDate = false;
                }
                return matrix;
            }
        }

    }
}
