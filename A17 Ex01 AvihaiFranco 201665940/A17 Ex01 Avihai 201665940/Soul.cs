using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameInfrastructure.Managers;
using GameInfrastructure.ObjectModel;
using GameInfrastructure.ServiceInterfaces;

namespace Space_Invaders
{
    public class Soul : Sprite
    {
        private static readonly string sr_LifeTextureString = @"Spaceship/Ship01_32x32";

        public Soul(Game i_Game, Color i_Tint)
            : base(i_Game, sr_LifeTextureString)
        {
            this.m_TintColor = i_Tint;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Scales = Vector2.One / 2;
            this.Opacity = 0.5f;
        }
    }
}
