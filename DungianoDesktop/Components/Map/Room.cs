using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using DungianoDesktop.Components.MenuComponents;
using DungianoDesktop.Components.Characters;
using DungianoDesktop.Components.Objects;

namespace DungianoDesktop.Components.Map
{
    public class Room
    {
        protected Background picture;

        protected List<Rectangle> walls;
        protected List<Body> entities;

        public Room(Background roomPicture, List<Rectangle> walls, List<Body> entities)
        {
            picture = roomPicture;
            this.walls = walls;
            this.entities = entities;
        }

        public Background GetBackground()
        {
            return picture;
        }

        public List<Rectangle> GetWalls()
        {
            return walls;
        }

        public List<DrawableComponent> GetComponents()
        {
            List<DrawableComponent> components = new List<DrawableComponent>(entities);
            components.Insert(0, (DrawableComponent)picture);

            return components;
        }

        public List<Body> GetEntities()
        {
            return new List<Body>(entities);
        }

        public void AddEntity(Body entity)
        {
            entities.Add(entity);
        }

        public void RemoveEntity(Body entity)
        {
            entities.Remove(entity);
        }
    }
}

