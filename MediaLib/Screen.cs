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
			if (element.Screen != null)
				throw new InvalidOperationException("Can't add element to multiple screens at once");

			elements.Add(element);
			element.Screen = this;
			OrderElements();

			if (Window != null)
				element.HandleShow();
		}

		public void RemoveElement(Element element)
		{
			if (!elements.Contains(element))
				return;

			if (Window != null)
				element.HandleHide();

			element.Screen = null;
			elements.Remove(element);
		}

		internal void OrderElements() => elements = elements.OrderBy(element => element.Depth).ToList();

		internal void HandleShow()
		{
			foreach (var element in elements)
				element.HandleShow();
		}

		internal void HandleHide()
		{
			foreach (var element in elements)
				element.HandleHide();
		}

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

		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
