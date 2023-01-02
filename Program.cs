global using PbTools;

var fileMaster = new PbFileTool();

List<PbDuplicate> duplicates = fileMaster.FindDuplicatesByExtensionAndSize(@"F:\gdrive\Music\_mine");

foreach(var duplicate in duplicates)
{
    duplicate.FileInfos.ForEach(x => Console.WriteLine($"{x.FileUniqueId.Length} {x.FilePath}"));
}

Console.WriteLine($"DUPLICATE FILES: {duplicates.Count}");
