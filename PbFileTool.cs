﻿namespace PbTools;

public class PbFileTool
{
    public List<string> FindDuplicatesByName(string directory)
    {
        var duplicates = new List<string>();
        var alreadyChecked = new List<string>();

        List<string> sourceFiles = Directory.GetFiles(directory, "", SearchOption.AllDirectories).ToList();
        foreach (var filePath in sourceFiles)
        {
            string fileName = Path.GetFileName(filePath);

            if (alreadyChecked.Contains(fileName))
            {
                duplicates.AddRange(sourceFiles.Where(x => fileName == Path.GetFileName(x)));
            }
            else
            {
                alreadyChecked.Add(fileName);
            }
        }

        return duplicates
            .OrderBy(x => Path.GetFileName(x))
            .Distinct()
            .ToList();
    }

    public List<PbDuplicate> FindDuplicatesByExtensionAndSize(string directory)
    {
        Console.Write("Reading file info...");
        var sourceFilesInfo = new List<PbFileInfo>();
        var files = Directory.GetFiles(directory, "", SearchOption.AllDirectories).ToList();
        long i = 0;
        foreach (var filePath in files)
        {
            sourceFilesInfo.Add(new PbFileInfo(filePath));

            if (i++ == 100)
            {
                Console.Write(".");
                i = 0;
            }
        }
        Console.WriteLine("DONE!");

        var duplicates = new List<PbDuplicate>();
        var alreadyChecked = new List<PbFileInfo>();

        while (sourceFilesInfo.Count > 0)
        {
            var nextFile = sourceFilesInfo.First();
            var matchingFiles = sourceFilesInfo
                .Where(x => x.FileUniqueId.Equals(nextFile.FileUniqueId))
                .ToList();
            if (matchingFiles.Count > 1)
            {
                var duplicate = new PbDuplicate(nextFile.FileUniqueId);
                matchingFiles.ForEach(x => duplicate.FileInfos.Add(x));
                duplicates.Add(duplicate);
            }

            sourceFilesInfo.RemoveAll(x => x.FileUniqueId.Equals(nextFile.FileUniqueId));
        }

        return duplicates;
    }

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

    public void TouchFileDates(string directory)
    {
        var files = Directory.GetFiles(directory, "", SearchOption.AllDirectories).ToList();
        var date = DateTime.Now;
        foreach (var file in files)
        {
            Console.WriteLine(file);
            File.SetLastWriteTime(file, date);
        }
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
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
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
