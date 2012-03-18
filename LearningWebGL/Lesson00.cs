using System;
using OpenTK;
using OpenTK.Graphics.ES20;

using Matrix = OpenTK.Matrix4;
using Vector3 = OpenTK.Vector3;
using Real = System.Single;
using Shader = System.Int32;
using Handle = System.Int32;

#if SILVERLIGHT
using GameWindow = OpenTK.Platform.Silverlight.SilverlightGameWindow;
#endif

namespace LearningWebGL
{
    public class Lesson00 : GameWindow
    {
        /**
         * Lesson_one.js
         */

        // We make use of the WebGL utility library, which was downloaded from here:
        // https://cvs.khronos.org/svn/repos/registry/trunk/public/webgl/sdk/demos/common/webgl-utils.js
        //
        // It defines two functions which we use here:
        //
        // // Creates a WebGL context.
        // WebGLUtils.setupWebGL(canvas);
        //
        // Requests an animation callback. See: https://developer.mozilla.org/en/DOM/window.requestAnimationFrame
        // window.requestAnimFrame(callback, node);
        //
        // We also make use of the glMatrix file which can be downloaded from here:
        // http://code.google.com/p/glmatrix/source/browse/glMatrix.js
        //

        string vertex_shader = @"
uniform mat4 u_MVPMatrix;   // A constant representing the combined model/view/projection matrix.

attribute vec4 a_Position;  // Per-vertex position information we will pass in.
attribute vec4 a_Color;     // Per-vertex color information we will pass in.

varying vec4 v_Color;       // This will be passed into the fragment shader.

void main()                 // The entry point for our vertex shader.
{
    v_Color = a_Color;      // Pass the color through to the fragment shader.
		  					// It will be interpolated across the triangle.

	// gl_Position is a special variable used to store the final position.
	// Multiply the vertex by the matrix to get the final point in normalized screen coordinates.
	gl_Position = u_MVPMatrix * a_Position;
}";

        string fragment_shader = @"
precision mediump float;       // Set the default precision to medium. We don't need as high of a
							   // precision in the fragment shader.
varying vec4 v_Color;          // This is the color from the vertex shader interpolated across the
		  					   // triangle per fragment.
void main()                    // The entry point for our fragment shader.
{
    gl_FragColor = v_Color;    // Pass the color directly through the pipeline.
}";

        /** Hold a reference to the canvas DOM object. */
        OpenTK.INativeWindow canvas;	 

        /**
         * Store the model matrix. This matrix is used to move models from object space (where each model can be thought
         * of being located at the center of the universe) to world space.
         */
        Matrix modelMatrix;

        /**
         * Store the view matrix. This can be thought of as our camera. This matrix transforms world space to eye space;
         * it positions things relative to our eye.
         */
        Matrix viewMatrix;

        /** Store the projection matrix. This is used to project the scene onto a 2D viewport. */
        Matrix projectionMatrix;

        /** Allocate storage for the final combined matrix. This will be passed into the shader program. */
        Matrix mvpMatrix;

        /** Store our model data in a Float32Array buffer. */
        Real[] trianglePositions;
        Real[] triangle1Colors;
        Real[] triangle2Colors;
        Real[] triangle3Colors;

        /** Store references to the vertex buffer objects (VBOs) that will be created. */
        Handle trianglePositionBufferObject;
        Handle triangleColorBufferObject1;
        Handle triangleColorBufferObject2;
        Handle triangleColorBufferObject3;

        /** This will be used to pass in the transformation matrix. */
        Handle mvpMatrixHandle;

        /** This will be used to pass in model position information. */
        Handle positionHandle;

        /** This will be used to pass in model color information. */
        Handle colorHandle;

        /** Size of the position data in elements. */
        int positionDataSize = 3;

        /** Size of the color data in elements. */
        int colorDataSize = 4;	

        //Helper function to load a shader
        Handle loadShader(string shaderSource, ShaderType type)
        {
	        var shaderHandle = GL.CreateShader(type);
            var info = "";

	        if (shaderHandle != 0) 
	        {				
		        // Pass in the shader source.
		        GL.ShaderSource(shaderHandle, shaderSource);		

		        // Compile the shader.
		        GL.CompileShader(shaderHandle);

                // Get the compilation status.		
		        int compiled;
                GL.GetShader(shaderHandle, ShaderParameter.CompileStatus, out compiled);		

		        // If the compilation failed, delete the shader.
		        if (compiled != 1) 
		        {				
			        GL.GetShaderInfoLog(shaderHandle, out info);			
			        GL.DeleteShader(shaderHandle);
			        shaderHandle = 0;
		        }
	        }

	        if (shaderHandle == 0)
	        {
		        throw new Exception("Error creating shader: " + info);
	        }

	        return shaderHandle;
        }

        // Helper function to link a program
        Handle linkProgram(Shader vertexShader, Shader fragmentShader)
        {
	        // Create a program object and store the handle to it.
	        var programHandle = GL.CreateProgram();

	        if (programHandle != 0) 
	        {
		        // Bind the vertex shader to the program.
		        GL.AttachShader(programHandle, vertexShader);			

		        // Bind the fragment shader to the program.
		        GL.AttachShader(programHandle, fragmentShader);

		        // Bind attributes
		        GL.BindAttribLocation(programHandle, 0, "a_Position");
		        GL.BindAttribLocation(programHandle, 1, "a_Color");

		        // Link the two shaders together into a program.
		        GL.LinkProgram(programHandle);
                
		        // Get the link status.
		        Handle linked;
                GL.GetProgram(programHandle, ProgramParameter.LinkStatus, out linked);

		        // If the link failed, delete the program.
		        if (linked == 0) 
		        {				
			        GL.DeleteProgram(programHandle);
			        programHandle = 0;
		        }
	        }

	        if (programHandle == 0)
	        {
		        throw new Exception("Error creating program.");
	        }

	        return programHandle;
        }

        //Called when we have the context
        protected override void OnLoad(EventArgs e)
        {
	        /* Configure viewport */
	        // Set the OpenGL viewport to the same size as the canvas.
	        GL.Viewport(0, 0, canvas.ClientSize.Width, canvas.ClientSize.Height);

	        // Create a new perspective projection matrix. The height will stay the same
	        // while the width will vary as per aspect ratio.
	        var ratio = canvas.ClientSize.Width / canvas.ClientSize.Height;
	        var left = -ratio;
	        var right = ratio;
	        var bottom = -1.0;
	        var top = 1.0;
	        var near = 1.0;
	        var far = 10.0;

	        projectionMatrix = Matrix.CreatePerspectiveOffCenter((Real)left, (Real)right, (Real)bottom, (Real)top, (Real)near, (Real)far);

	        /* Configure general parameters */

	        // Set the background clear color to gray.
	        GL.ClearColor((Real)0.5, (Real)0.5, (Real)0.5, (Real)1.0);		

	        // Set the view matrix. This matrix can be said to represent the camera position.		
	        var eye = new Vector3() { Z = (Real)1.5 };
	        var center = new Vector3() { Z = -(Real)5.0 };
	        var up = new Vector3() { Y= (Real)1.0 };
	        viewMatrix = Matrix.LookAt(eye, center, up);

	        /* Configure shaders */
	        var vertexShaderHandle = loadShader(vertex_shader, ShaderType.VertexShader);
	        var fragmentShaderHandle = loadShader(fragment_shader, ShaderType.FragmentShader);			

	        // Create a program object and store the handle to it.
	        var programHandle = linkProgram(vertexShaderHandle, fragmentShaderHandle);	

            // Set program handles. These will later be used to pass in values to the program.
	        mvpMatrixHandle = GL.GetUniformLocation(programHandle, "u_MVPMatrix");        
            positionHandle = GL.GetAttribLocation(programHandle, "a_Position");
            colorHandle = GL.GetAttribLocation(programHandle, "a_Color");        

            // Tell OpenGL to use this program when rendering.
            GL.UseProgram(programHandle);

            // Create buffers in OpenGL's working memory.
            GL.GenBuffers(1, out trianglePositionBufferObject);
            checkError();
            GL.BindBuffer(BufferTarget.ArrayBuffer, trianglePositionBufferObject);
            checkError();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (trianglePositions.Length*sizeof(Real)), trianglePositions, BufferUsage.StaticDraw);
            checkError();
            
            GL.GenBuffers(1, out triangleColorBufferObject1);
            checkError();
            GL.BindBuffer(BufferTarget.ArrayBuffer, triangleColorBufferObject1);
            checkError();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (triangle1Colors.Length*sizeof(Real)), triangle1Colors, BufferUsage.StaticDraw);
            checkError();

            GL.GenBuffers(1, out triangleColorBufferObject2);
            checkError();
            GL.BindBuffer(BufferTarget.ArrayBuffer, triangleColorBufferObject2);
            checkError();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (triangle2Colors.Length*sizeof(Real)), triangle2Colors, BufferUsage.StaticDraw);
            checkError();

            GL.GenBuffers(1, out triangleColorBufferObject3);
            checkError();
            GL.BindBuffer(BufferTarget.ArrayBuffer, triangleColorBufferObject3);
            checkError();
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr) (triangle3Colors.Length*sizeof(Real)), triangle3Colors, BufferUsage.StaticDraw);
            checkError();   

	        // Tell the browser we want render() to be called whenever it's time to draw another frame.
	        // TODO: window.requestAnimFrame(render, canvas);
            Run();
        }

        // Callback called each time the browser wants us to draw another frame
        protected override void OnRenderFrame(FrameEventArgs e)
        {           	
	        // Clear the canvas
            GL.ClearColor(Color.Blue);
	        GL.Clear(ClearBufferMask.ColorBufferBit);

	        // Do a complete rotation every 10 seconds.
            var time = DateTime.Now.Ticks % 100000000;
            var angleInDegrees = (360.0 / 100000000.0) * time;
            var angleInRadians = angleInDegrees / 57.2957795;

            // Draw the triangle facing straight on.
            modelMatrix = Matrix4.CreateRotationZ((Real)angleInRadians);
            drawTriangle(triangleColorBufferObject1);

            // Draw one translated a bit down and rotated to be flat on the ground.
            modelMatrix = Matrix.CreateTranslation(0, -1, 0);
            modelMatrix = Matrix4.CreateRotationX((Real)(90 / 57.2957795)) * modelMatrix;
            modelMatrix = Matrix4.CreateFromAxisAngle(new Vector3(0,0,1), (Real)angleInRadians)  * modelMatrix;           
            drawTriangle(triangleColorBufferObject2);

            // Draw one translated a bit to the right and rotated to be facing to the left.
            modelMatrix = Matrix.CreateTranslation(1, 0, 0);
            modelMatrix = Matrix4.CreateRotationY((Real)(90 / 57.2957795)) * modelMatrix;
            modelMatrix = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), (Real)angleInRadians) * modelMatrix;           
            drawTriangle(triangleColorBufferObject3);

            // Send the commands to WebGL
	        //GL.Flush();

	        // Request another frame
	        // TODO: window.requestAnimFrame(render, canvas);

            SwapBuffers();
        }

        void checkError()
        {
	        var error = GL.GetError();

	        if (error != ErrorCode.NoError)
	        {
		        throw new Exception("error: " + error);
	        }
        }

        // Draws a triangle from the given vertex data.
        void drawTriangle(Handle triangleColorBufferObject)
        {		
	        // Pass in the position information
        //	console.log("positionHandle=" +  positionHandle);
        //	console.log("colorHandle=" +  colorHandle);
	        GL.EnableVertexAttribArray(positionHandle);
            checkError();

	        GL.BindBuffer(BufferTarget.ArrayBuffer, trianglePositionBufferObject);
            GL.VertexAttribPointer(positionHandle, positionDataSize, VertexAttribPointerType.Float, false,
    		        0, 0);        
            checkError();            

            // Pass in the color information
            GL.EnableVertexAttribArray(colorHandle);
            checkError();

            GL.BindBuffer(BufferTarget.ArrayBuffer, triangleColorBufferObject);
            GL.VertexAttribPointer(colorHandle, colorDataSize, VertexAttribPointerType.Float, false,
    		        0, 0);        
            checkError();

	        // This multiplies the view matrix by the model matrix, and stores the result in the modelview matrix
            // (which currently contains model * view).    
            mvpMatrix = modelMatrix * viewMatrix * projectionMatrix;

            // This multiplies the modelview matrix by the projection matrix, and stores the result in the MVP matrix
            // (which now contains model * view * projection).
            //mvpMatrix = projectionMatrix * mvpMatrix;

            GL.UniformMatrix4(mvpMatrixHandle, false, ref mvpMatrix);
            checkError();
            GL.DrawArrays(BeginMode.Triangles, 0, 3);
            checkError();
        //    console.log("Made it past one frame");
        }

        // Main entry point
        public Lesson00(string canvasId)
            : base(canvasId) //, new OpenTK.Graphics.GraphicsMode(32, 32), "Test OpenGL ES 2.0", GameWindowFlags.Default, DisplayDevice.Default, 2, 0, OpenTK.Graphics.GraphicsContextFlags.Embedded)
        {
            canvas = this;

            // Try to get a WebGL context    
            //canvas = document.getElementById("canvas");    

            // We don't need a depth buffer. See https://www.khronos.org/registry/webgl/specs/1.0/ Section 5.2 for more info.
            //gl = WebGLUtils.setupWebGL(canvas, { depth: false });

            //if (gl != null)
	        //{    	
    	        // Init model data.

    	        // Define points for equilateral triangles.		
		        trianglePositions = new Real[] {
				        // X, Y, Z, 
	                    -0.5f, -0.25f, 0.0f, 	            	            
	                    0.5f, -0.25f, 0.0f,	            	            
	                    0.0f, 0.559016994f, 0.0f};

		        // This triangle is red, green, and blue.
		        triangle1Colors = new Real[] {
  				        // R, G, B, A  	            
  	                    1.0f, 0.0f, 0.0f, 1.0f,  	              	            
  	                    0.0f, 0.0f, 1.0f, 1.0f,  	              	            
  	                    0.0f, 1.0f, 0.0f, 1.0f};				

		        // This triangle is yellow, cyan, and magenta.
		        triangle2Colors = new Real[] {
				        // R, G, B, A	            
	                    1.0f, 1.0f, 0.0f, 1.0f,	            	            
	                    0.0f, 1.0f, 1.0f, 1.0f,	            	            
	                    1.0f, 0.0f, 1.0f, 1.0f};

		        // This triangle is white, gray, and black.
		        triangle3Colors = new Real[] {
				        // R, G, B, A	            
	                    1.0f, 1.0f, 1.0f, 1.0f,	            	            
	                    0.5f, 0.5f, 0.5f, 1.0f,	            	            
	                    0.0f, 0.0f, 0.0f, 1.0f};		
	        //}
        }

        public static void Main(string[] args)
        {
            using (Lesson01 p = new Lesson01(""))
            {
                p.Run(60);
            }
        }

    }
}