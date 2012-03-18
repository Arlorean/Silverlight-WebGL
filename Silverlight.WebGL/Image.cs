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
using System.Windows.Browser;

namespace Silverlight.Html
{
    public class Image
    {
        public ScriptObject Object { get; set; }

        public Image() { Object = System.Windows.Browser.HtmlPage.Window.CreateInstance("Image", null); }
        public Image(ScriptObject obj) { Object = obj; }

        public string src {
            get { return Object.GetProperty("src") as string; }
            set { Object.SetProperty("src", value); }
        }

        public event Action onload {
            add { Object.SetProperty("onload", value); }
            remove { Object.SetProperty("onload", null); }
        }
    }
}
