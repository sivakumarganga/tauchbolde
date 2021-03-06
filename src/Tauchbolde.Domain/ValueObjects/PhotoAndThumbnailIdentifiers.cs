using JetBrains.Annotations;

namespace Tauchbolde.Domain.ValueObjects
{
    public class PhotoAndThumbnailIdentifiers
    {
        public PhotoAndThumbnailIdentifiers(
            [CanBeNull] PhotoIdentifier originalPhotoIdentifier,
            [CanBeNull] PhotoIdentifier thumbnailPhotoIdentifier)
        {
            OriginalPhotoIdentifier = originalPhotoIdentifier;
            ThumbnailPhotoIdentifier = thumbnailPhotoIdentifier;
        }
        
        [CanBeNull] public PhotoIdentifier OriginalPhotoIdentifier { get; }
        [CanBeNull] public PhotoIdentifier ThumbnailPhotoIdentifier { get; }
    }
}