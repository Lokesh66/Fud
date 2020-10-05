using System;
using System.IO;
using System.Text;


public class FileSystem
{
    private static FileSystem instance = null;

    private FileSystem ()
    {

    }

    public static FileSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FileSystem ();
            }

            return instance;
        }
    }

    public void CreateDirectory (string dir, bool hiddenDirectory = false)
    {
        DirectoryInfo dirInfo = new DirectoryInfo (dir);

        if (!dirInfo.Exists)
        {
            dirInfo.Create ();

            if (hiddenDirectory)
            {
                File.Create (dir + ".nomedia");
            }
        }
    }

    public bool IsDirectoryEmpty (string path)
    {
        int fileCount = Directory.GetFiles (path).Length;

        if (fileCount > 0)
        {
            return false;
        }

        string [] dirs = Directory.GetDirectories (path);

        foreach (string dir in dirs)
        {
            if (!IsDirectoryEmpty (dir))
            {
                return false;
            }
        }

        return true;
    }

    public void CopyDirectory (string sourceDirectory, string destinationDirectory, bool copySubDirectories)
    {
        DirectoryInfo sourceDirectoryInfo = new DirectoryInfo (sourceDirectory);

        if (!sourceDirectoryInfo.Exists)
        {
            throw new DirectoryNotFoundException (string.Format ("Source Directory ({0}) does not exist Or Could not be found", sourceDirectory));
        }

        if (!Directory.Exists (destinationDirectory))
        {
            Directory.CreateDirectory (destinationDirectory);
        }

        FileInfo [] files = sourceDirectoryInfo.GetFiles ();

        foreach (FileInfo file in files)
        {
            string destinationPath = Path.Combine (destinationDirectory, file.Name);

            if (!File.Exists (destinationPath))
            {
                //file.CopyTo (destinationPath, false);

                byte [] bytes = ReadAllBytes (file.FullName);

                WriteAllBytesAtomically (destinationPath, bytes);
            }
        }

        if (copySubDirectories)
        {
            DirectoryInfo [] subDirectoriesInfos = sourceDirectoryInfo.GetDirectories ();

            foreach (DirectoryInfo subDirectoryInfo in subDirectoriesInfos)
            {
                string temppath = Path.Combine (destinationDirectory, subDirectoryInfo.Name);

                CopyDirectory (subDirectoryInfo.FullName, temppath, copySubDirectories);
            }
        }
    }

    public void WriteAllTextAtomically (string destinationPath, string text)
    {
        byte [] bytes = Encoding.UTF8.GetBytes (text);

        WriteAllBytesAtomically (destinationPath, bytes);
    }

    public void WriteAllBytesAtomically (string destinationPath, byte [] bytes)
    {
        string temporaryPath = destinationPath; //Path.GetTempFileName ();

        using (FileStream fileStream = File.Create (temporaryPath, 4096, FileOptions.WriteThrough))
        {
            fileStream.Write (bytes, 0, bytes.Length);

            fileStream.Flush (true);

            fileStream.Dispose ();

            fileStream.Close ();
        }

        /*
        if (File.Exists (destinationPath))
        {
            File.Replace (temporaryPath, destinationPath, null);
        }
        else
        {
            File.Move (temporaryPath, destinationPath);
        }
        */
    }

    public string ReadAllText (string filePath)
    {
        return File.ReadAllText (filePath);
    }

    public byte [] ReadAllBytes (string filePath)
    {
        return File.ReadAllBytes (filePath);
    }
}
