using System.Diagnostics.CodeAnalysis;

//public class FmFileInfo : IEqualityComparer<FmFileInfo>
//{
//    public string? FileType { get; set; }
//    public string? FileSize { get; set; }

//    public FmFileInfo(string path)
//    {
//    }

//    public bool Equals(FmFileInfo? x, FmFileInfo? y)
//    {
//        if (x == null || y == null)
//        {
//            return false;
//        }

//        return x.FileType == y.FileType && x.FileSize == y.FileSize;
//    }

//    public int GetHashCode([DisallowNull] FmFileInfo obj)
//    {
//        throw new NotImplementedException();
//    }
//}

public class FilePB
{
    public void CompareDirectories(string sourceDirectory, string destDirectory)
    {
        var sourceFiles = Directory.GetFiles(sourceDirectory, "", SearchOption.AllDirectories).ToList();
        var destFiles = Directory.GetFiles(destDirectory, "", SearchOption.AllDirectories).ToList();
        destFiles = destFiles
            .Select(x => x.Replace(destDirectory, ""))
            .ToList();

        var sourceNotInDest = sourceFiles
            .Where(x => !destFiles.Contains(x))
            .ToList();
        var destNotInSource = destFiles
            .Where(x => !sourceFiles.Contains(x))
            .ToList();

        Console.WriteLine("SOURCE FILES NOT IN DESTINATION");
        sourceNotInDest.ForEach(x => Console.WriteLine(x));
        Console.WriteLine("DESTINATION FILES NOT IN SOURCE");
        destNotInSource.ForEach(x => Console.WriteLine(x));
    }

    public List<string> FindDuplicatesByName(string directory)
    {
        List<string> sourceFiles = Directory.GetFiles(directory, "", SearchOption.AllDirectories).ToList();

        var duplicates = new List<string>();
        var alreadyChecked = new List<string>();
        foreach (var fullPath in sourceFiles)
        {
            string fileName = Path.GetFileName(fullPath);
            if (alreadyChecked.Contains(fileName))
            {
                duplicates.AddRange(sourceFiles.Where(x => fileName == Path.GetFileName(x)));
            }
            else
            {
                alreadyChecked.Add(fileName);
            }
        }
        return duplicates.OrderBy(x => Path.GetFileName(x)).Distinct().ToList();
    }

    public void TouchFileDates(string directory)
    {
        var files = Directory.GetFiles(directory, "", SearchOption.AllDirectories).ToList();
        var date = DateTime.Now;
        foreach(var file in files)
        {
            Console.WriteLine(file);
            File.SetLastWriteTime(file, date);
        }
    }

    public string GetConvertSQL(string fileName, string sourceExtension, string destExtension)
    {
        List<string> files = File.ReadAllLines(fileName).ToList();
        var lines = files.Select(f => $"UPDATE Videos SET ArticleImage = '{f.Replace(sourceExtension, destExtension)}' where ArticleImage = '{f}'").ToList();
        return string.Join("\n", lines);
    }

    public string GetRollbackSQL(string fileName, string sourceExtension, string destExtension)
    {
        List<string> files = File.ReadAllLines(fileName).ToList();
        var lines = files.Select(f => $"UPDATE Articles SET SocialImage = '{f}' where SocialImage = '{f.Replace(sourceExtension, destExtension)}'").ToList();
        return string.Join("\n", lines);
    }

    public void ListFiles(string sourceDirectory)
    {
        var sourceFiles = Directory.GetFiles(sourceDirectory, "", SearchOption.AllDirectories).ToList();
        sourceFiles = sourceFiles.Select(x => x.Replace(sourceDirectory, "")).ToList();
        sourceFiles.ForEach(x => Console.WriteLine(x));
    }

    public void ListFilesByExtension(string fileName, string extension)
    {
        List<string> files = File.ReadAllLines(fileName).ToList();
        files = files.Where(x => x.Contains(extension)).ToList();
        files.ForEach(x => Console.WriteLine(x));
    }

    public void CopyFiles(List<string> files, string sourceDir, string destDir)
    {
        foreach (var file in files)
        {
            string sourceFile = Path.Join(sourceDir, file);
            string destFile = Path.Join(destDir, file);
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            try
            {
                if (File.Exists(destFile) == false)
                {
                    Console.WriteLine($"Copying {sourceFile} to {destFile}");

                    string? dir = Path.GetDirectoryName(destFile);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    File.Copy(sourceFile, destFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to move file {sourceFile}: {ex.Message}");
            }
        }
    }

    public void RenameFiles(List<string> files, string sourceDirectory, string destDirectory, string sourceExtension, string destExtension)
    {
        foreach (var file in files)
        {
            string destFile = file.Replace(sourceDirectory, destDirectory);
            string sourceFile = destFile.Replace(destExtension, sourceExtension);
            try
            {
                if (File.Exists(destFile) == false)
                {
                    Console.WriteLine($"Moving {sourceFile} to {destFile}");
                    File.Move(sourceFile, destFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to move file {sourceFile}: {ex.Message}");
            }
        }
    }

    public List<string> GetFiles(string startDirectory, string extension)
    {
        var matchedFiles = new List<string>();
        var directories = Directory.GetDirectories(startDirectory, "", SearchOption.AllDirectories);

        foreach (var directory in directories)
        {
            matchedFiles.AddRange(Directory.GetFiles(directory).Where(x => x.EndsWith(extension)));
        }

        return matchedFiles;
    }
}