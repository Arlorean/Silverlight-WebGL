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
using System.Linq;
using System.Windows.Browser;

namespace LearningWebGL
{
    public class LessonBase
    {
        string id;

        public string Id {
            get { return id; }
            set { HtmlPage.RegisterScriptableObject(id = value, this); }
        }

        public void requestAnimFrame(Action callback) {
            window.Invoke("requestAnimFrame", callback);
        }

        public static HtmlDocument document { get { return System.Windows.Browser.HtmlPage.Document; } }
        public static HtmlWindow window { get { return System.Windows.Browser.HtmlPage.Window; } }

        public static void alert(string s) { window.Alert(s); }

        public static float parseFloat(string s) {
            var result = 0f;
            return float.TryParse(s, out result) ? result : float.NaN;
        }
    }

    public static class ArrayExtensionMethod {
        public static T[] concat<T>(this T[] baseArray, params T[][] arrays) {
            var length = baseArray.Length + arrays.Sum(x => x.Length);
            var newArray = new T[length];
            Array.Copy(baseArray, newArray, baseArray.Length);

            var offset = baseArray.Length;
            foreach (var array in arrays) {
                Array.Copy(array, 0, newArray, offset, array.Length);
                offset += array.Length;
            }
            return newArray;
        }
    }
}
