#region License
//
// Silverlight.WebGL License (MIT License) 
//
// Copyright (c) 2012 Adam Davidson
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the 
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
// sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//
// see http://www.opensource.org/licenses/MIT for more details
//
#endregion

using System;
using System.Windows;
using System.Windows.Browser;

namespace Silverlight.Html
{
    public static class HtmlElementExtensions
    {
        public static Rect Bounds(this HtmlElement element)
        {
            var bounds = new Rect(element.OffsetLeft(), element.OffsetTop(), element.OffsetWidth(), element.OffsetHeight());
            for (var target = element; target != null; target = target.OffsetParent()) {
                bounds.X += target.OffsetLeft();
                bounds.Y += target.OffsetTop();
            }
            return bounds;
        }

        public static HtmlDocument ContentDocument(this HtmlElement element) {
            return element.GetProperty("contentDocument") as HtmlDocument;
        }
        public static HtmlDocument Document(this HtmlElement element) {
            return element.GetProperty("document") as HtmlDocument;
        }
        public static ScriptObject Parent(this HtmlElement element) {
            return element.GetProperty("parent") as ScriptObject;
        }

        public static bool Checked(this HtmlElement element) {
            return (bool) element.GetProperty("checked");
        }
        public static void Checked(this HtmlElement element, bool @checked) {
            element.SetProperty("checked", @checked);
        }
        public static string Value(this HtmlElement element) {
            return (string) element.GetProperty("value");
        }
        public static void Value(this HtmlElement element, string value) {
            element.SetProperty("value", value);
        }

        public static HtmlElement OffsetParent(this HtmlElement element) {
            return element.GetProperty("offsetParent") as HtmlElement;
        }
        public static void OffsetParent(this HtmlElement element, HtmlElement offsetParent) {
            element.SetProperty("offsetParent", offsetParent);
        }

        public static double OffsetTop(this HtmlElement element) {
            return System.Convert.ToDouble(element.GetProperty("offsetTop"));
        }
        public static void OffsetTop(this HtmlElement element, double offsetTop) {
            element.SetProperty("offsetTop", offsetTop);
        }

        public static double OffsetBottom(this HtmlElement element) {
            return System.Convert.ToDouble(element.GetProperty("offsetBottom"));
        }
        public static void OffsetBottom(this HtmlElement element, double offsetBottom) {
            element.SetProperty("offsetBottom", offsetBottom);
        }

        public static double OffsetLeft(this HtmlElement element) {
            return System.Convert.ToDouble(element.GetProperty("offsetLeft"));
        }
        public static void OffsetLeft(this HtmlElement element, double offsetLeft) {
            element.SetProperty("offsetLeft", offsetLeft);
        }

        public static double OffsetRight(this HtmlElement element) {
            return System.Convert.ToDouble(element.GetProperty("offsetRight"));
        }
        public static void OffsetRight(this HtmlElement element, double offsetRight) {
            element.SetProperty("offsetRight", offsetRight);
        }

        public static double OffsetHeight(this HtmlElement element) {
            return System.Convert.ToDouble(element.GetProperty("offsetHeight"));
        }
        public static void OffsetHeight(this HtmlElement element, double offsetHeight) {
            element.SetProperty("offsetHeight", offsetHeight);
        }

        public static double OffsetWidth(this HtmlElement element) {
            return System.Convert.ToDouble(element.GetProperty("offsetWidth"));
        }
        public static void OffsetWidth(this HtmlElement element, double offsetWidth) {
            element.SetProperty("offsetWidth", offsetWidth);
        }

        public static double Width(this HtmlElement element) {
            return System.Convert.ToDouble(element.GetProperty("width"));
        }
        public static void Width(this HtmlElement element, double width) {
            element.SetProperty("width", width);
        }

        public static double Height(this HtmlElement element) {
            return System.Convert.ToDouble(element.GetProperty("height"));
        }
        public static void Height(this HtmlElement element, double height) {
            element.SetProperty("height", height);
        }
    }
}
