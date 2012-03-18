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

namespace Silverlight.Html.WebGL
{
    /// <summary>
    /// Class to pass contruction parameters to the getContext() method for a canvas.
    /// http://www.khronos.org/registry/webgl/specs/latest/#2.1
    /// e.g. {preserveDrawingBuffer: true}
    /// </summary>
    public class WebGLContextAttributes
    {
#if false
        attribute boolean alpha;
        attribute boolean depth;
        attribute boolean stencil;
        attribute boolean antialias;
        attribute boolean premultipliedAlpha;
#endif
    }
}
