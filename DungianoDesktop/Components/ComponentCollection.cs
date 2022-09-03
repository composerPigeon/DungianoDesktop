using System.Collections.Generic;
using System.Collections;

using Microsoft.Xna.Framework;

namespace DungianoDesktop.Components
{
    public class ComponentDatabase : IEnumerable<Component>
    {
        protected List<Component> components;

        public IEnumerator<Component> GetEnumerator()
        {
            return components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ComponentDatabase()
        {
            components = new List<Component>();
        }

        public void Add(Component component)
        {
            component.LoadContent();
            components.Add(component);
        }

        public void Remove(Component component)
        {
            components.Remove(component);
        }

        public void Clear()
        {
            components.Clear();
        }

        public List<Component> GetData()
        {
            return components;
        }
    }

    public class DrawableComponentDatabase : IEnumerable<DrawableComponent>
    {
        protected List<DrawableComponent> components;

        public IEnumerator<DrawableComponent> GetEnumerator()
        {
            return components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public DrawableComponentDatabase()
        {
            components = new List<DrawableComponent>();
        }

        public void Add(DrawableComponent component)
        {
            component.LoadContent();
            components.Add(component);
        }

        public void Remove(DrawableComponent component)
        {
            components.Remove(component);
        }

        public void Clear()
        {
            components.Clear();
        }

        public List<DrawableComponent> GetData()
        {
            return components;
        }
    }
}

