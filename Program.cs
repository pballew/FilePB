global using PbTools;

//string _sourceDirectory = @"C:\temp";
//string _destDirectory = @"C:\temp\resizing\resized";
//string _fileName = @"c:\temp\resizing\social-image-files.txt";

var fileMaster = new PbFileTool();

var duplicates = fileMaster.FindDuplicatesByExtensionAndSize(@"C:\temp\test");
//var duplicates = fileMaster.FindDuplicatesBySize(@"F:\gdrive\Music\_mine\_classical");
duplicates.ForEach(x => Console.WriteLine($"{x.Length} {x.FilePath}"));
Console.WriteLine($"DUPLICATE FILES: {duplicates.Count}");

//fileMaster.TouchFileDates(@"C:\temp\resizing\resized\article-collection\all");

//var files = GetFiles(sourceDirectory, ".jpeg");
//files.Sort();
//files.ForEach(x => Console.WriteLine(x));
//fileMaster.MoveFiles(files, _sourceDirectory, _destDirectory, ".jpg", ".jpeg");

//fileMaster.CompareDirectories(_sourceDirectory, _destDirectory);

// Convert to SQL
//Console.Write(fileMaster.GetConvertSQL(@"c:\temp\resizing\social-image-files-png.txt", ".png", ".jpg"));
//Console.Write(fileMaster.GetConvertSQL(@"c:\temp\resizing\social-image-files-jpeg.txt", ".jpeg", ".jpg"));

// Rollback SQL
//Console.Write(fileMaster.GetRollbackSQL(@"c:\temp\resizing\social-image-files-png.txt", ".png", ".jpg"));
//Console.Write(fileMaster.GetRollbackSQL(@"c:\temp\resizing\social-image-files-jpeg.txt", ".jpeg", ".jpg"));

//List<string> files = File.ReadAllLines(@"c:\temp\resizing\social-image-files.txt").ToList();
//fileMaster.CopyFiles(files, @"c:\temp\resizing\all", @"c:\temp\resizing\resized\social\all");

//fileMaster.ListFilesByExtension(_fileName, ".jpeg");
//fileMaster.ListFilesByExtension(_fileName, ".png");