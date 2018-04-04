using System;
using System.IO;
using System.Drawing;

namespace MazeSolver
{
    public static class Program
    {
        public static bool IsValidImageType(string fileName)
        {
            if(!fileName.EndsWith(".png")
                && !fileName.EndsWith(".jpg")
                && !fileName.EndsWith(".bmp"))
            {
                Console.WriteLine("Sorry, {0} isn't an image file" +
                    "that this application supports. [png, jpg, bmp]", fileName);
                return false;
            }
            return true;
        }

        public static FileStream ValidateFile(string fileName)
        {
            FileStream imageStream = null;

            if (!IsValidImageType(fileName)){ throw new Exception(); }

            try
            {
                imageStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("Sorry, '{0}' doesn't seem to exist. Try another.", fileName);
                imageStream.Close();
                throw;
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, '{0}' doesn't seem to work. Try another.", fileName);
                imageStream.Close();
                throw;
            }
            
            return imageStream;
        }

        static string RequestFileName(string fileType)
        {
            string fileName;
            Console.Write("Please enter a {0} file name: ", fileType);
            fileName = Console.ReadLine();
            return fileName;
        }

        static void Main(string[] args)
        {
            int c = args.Length;
            string inputFileName;
            string outputFileName;
            FileStream inputFileStream = null;
            Bitmap mazeImage, solvedMaze;
            bool done;

            if (c == 0)
            {
                inputFileName = RequestFileName("maze input");
                outputFileName = RequestFileName("maze solution");
            }
            else if(c == 1)
            {
                inputFileName = args[0];
                outputFileName = RequestFileName("maze solution");
            }
            else
            {
                inputFileName = args[0];
                outputFileName = args[1];
            }

            while( inputFileStream == null )
            {
                try
                {
                    inputFileStream = ValidateFile(inputFileName);
                }
                catch (Exception)
                {
                    inputFileName = RequestFileName("maze input");
                }
            }
           
            mazeImage = new Bitmap(inputFileStream);
            
            Maze maze = new Maze(mazeImage);
            maze.Build();
            maze.Solve();
            try
            {
                solvedMaze = maze.Print();
            }
            catch(ArgumentNullException)
            {
                return;
            }
            inputFileStream.Close();

            done = false;
            while (!done)
            {
                try
                {
                    solvedMaze.Save(outputFileName);
                    done = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Sorry, '{0}' doesn't seem to work. Try another.", outputFileName);
                    outputFileName = RequestFileName("maze output");
                }
            }
        }
    } 
}
