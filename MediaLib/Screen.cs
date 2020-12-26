using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaLib
{
	public class Screen : IDisposable
	{
		List<Element> elements = new List<Element>();
		internal Window? Window;

		public void AddElement(Element element)
		{
			elements.Add(element);
			element.Screen = this;
			OrderElements();
		}

		internal void OrderElements() => elements = elements.OrderBy(element => element.Depth).ToList();

		internal void HandleUpdate(TimeSpan delta)
		{
			foreach (var element in elements)
				element.HandleUpdate(delta);
		}

		internal void HandleDraw()
		{
			foreach (var element in elements)
				element.HandleDraw();
		}

		public virtual void Dispose() { GC.SuppressFinalize(this); }
	}
}
