using JetBrains.Annotations;

namespace Tauchbolde.Common.DomainServices.PhotoStorage.Stores.FileSystemStore
{
    public interface IFilePathCalculator
    {
        string CalculateUniqueFilePath(
            string rootPath, 
            PhotoCategory category, 
            [CanBeNull] string baseFileName, 
            [CanBeNull] string contentType,
            ThumbnailType thumbnailType = ThumbnailType.None);
        }
}