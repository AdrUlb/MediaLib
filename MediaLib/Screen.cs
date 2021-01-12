using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaLib
{
	public class Screen : IDisposable
	{
		List<Element> elements = new List<Element>();
		internal Window? Window;

		public event ShowEventHandler? Show;
		public event HideEventHandler? Hide;
		public event UpdateEventHandler? Update;
		public event DrawEventHandler? Draw;

		protected virtual void OnShow() { }
		protected virtual void OnHide() { }
		protected virtual void OnUpdate(UpdateEventArgs e) { }
		protected virtual void OnDraw() { }

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
			Show?.Invoke();
			OnShow();

			foreach (var element in elements)
				element.HandleShow();
		}

		internal void HandleHide()
		{
			Hide?.Invoke();
			OnHide();

			foreach (var element in elements)
				element.HandleHide();
		}

		internal void HandleUpdate(TimeSpan delta)
		{
			var e = new UpdateEventArgs(delta);

			Update?.Invoke(e);
			OnUpdate(e);

			foreach (var element in elements)
				element.HandleUpdate(delta, e);
		}

		internal void HandleDraw()
		{
			Draw?.Invoke();
			OnDraw();

			foreach (var element in elements)
				element.HandleDraw();
		}

		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
