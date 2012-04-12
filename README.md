Silverlight.WebGL
=================

Sliverlight.WebGL is a C#/.NET wrapper for in-browser access to [WebGL](http://get.webgl.org/)
functionality using [Silverlight 5](http://www.microsoft.com/silverlight/).

<img src="https://raw.github.com/Arlorean/Silverlight-WebGL/master/Website/Lesson03-Windows-IE9.PNG"></img>

Features
--------

* Create 3D web browser applications using Silverlight plugin on Windows and Mac OSX.
* Easily port existing WebGL JavaScript code to Silverlight.
* Enable C# developers to put cross platform 3D applications in a web browser. 

Restrictions
------------

* Cannot use as a Silverlight Control and mix with other Silverlight controls.
* Cannot hide Silverlight control, it must be visible somewhere even if only 1x1 pixels.
* Cannot be used for out-of-browser 3D applications.
* Cannot show file:// URL based textures in IE9 or Chrome, but Firefox works OK.

How it works
------------

The HTML5 canvas has a WebGL context on recent releases of Firefox, Chrome and Safari, and
it can be faked on Internet Explorer with a plugin.
The [Silverlight HTML Bridge](http://tinyurl.com/7rr4m3o)
allows typesafe wrappers to be created around this JavaScript API so your application/game
loop can be written and compiled in C#/VB.NET with the 3D being fully hardware accelerated  
by the WebGL support in the browser.

Here's an example WebGL called from the C# wrapper:

    public void clearColor(GLclampf red, GLclampf green, GLclampf blue, GLclampf alpha) {
	    Invoke("clearColor", red, green, blue, alpha);
    }

Getting Started
---------------

* Install the Runtime and Developer Requirements
* Download the latest source code from
[Silverlight-WebGL](https://github.com/Arlorean/Silverlight-WebGL) from GitHub. 
On Windows I used [TortoiseGit](http://code.google.com/p/tortoisegit) following 
[these instuctions](http://www.sparkfun.com/tutorials/165).
* Open up the solution file **LearningWebGL/LearningWebGL.sln** in Visual Web Developer 2010 Express (it's free).
* Press **Start Debugging** (F5).
* Edit **App.xaml.cs** and change **Lesson03** in this method to select a different lesson to view:

        private void Application_Startup(object sender, StartupEventArgs e) {
            this.RootVisual = new Canvas();
            new Lesson03() { Id = "Lesson" };
        }

Runtime Requirements 
--------------------

* [Silverlight 5](http://www.microsoft.com/silverlight/)
* [WebGL](http://get.webgl.org/) enabled browser:
  * Internet Explorer via [IEWebGL](http://iewebgl.com/) (tested in IE9 32-bit)
  * Firefox (tested in 10.0.2)
  * Chrome (tested in 17.0.963.79 m)
  * Safari on OSX ([not tested yet](http://www.ikriz.nl/2011/08/23/enable-webgl-in-safari))

Developer Requirements 
----------------------

* [Microsoft Visual Web Developer 2010 Express](http://www.microsoft.com/visualstudio/en-us/products/2010-editions/visual-web-developer-express)
* [Microsoft Silverlight 5 SDK](http://www.microsoft.com/download/en/details.aspx?id=28359)

Licenses
--------

* The source code for Silverlight.WebGL is released under the
[MIT License](http://www.opensource.org/licenses/MIT).
* The source code for the LearningWebGL samples is ported from the original JavaScript WebGL
samples on the [Learning WebGL](http://learningwebgl.com/blog/?page_id=1217) website which is largely
based on the [Nehe OpenGL Tutorials](http://nehe.gamedev.net/) samples.
* The source code for GlMatrix is ported from the original JavaScript
[gl-matrix.js](https://github.com/toji/gl-matrix) library by Brandon Jones.
* The iPhone graphic used in the sample pages is taken from 
[openclipart.org](http://openclipart.org/people/BenBois/BenBois_iPhone_SVG.svg) by Ben Bois.
* The Silverlight.js file is the standard script from Microsoft.
* The WebGLHelper.js file is from [IEWebGL](http://iewebgl.com/) to allow loading of their plugin for internet explorer, or the built-in WebGL if it's available.

TODO:
-----

* Fill in missing implementations of WebGL methods.
* Port the remaining samples from [Learning WebGL](http://learningwebgl.com/blog/?page_id=1217).
* Test on Mac OSX in Safari, Chrome and Firefox and add browser matrix to this README.md file.
