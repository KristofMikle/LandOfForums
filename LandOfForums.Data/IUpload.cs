using Microsoft.WindowsAzure.Storage.Blob;

namespace LandOfForums.Service
{
    public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString);
    }
}
