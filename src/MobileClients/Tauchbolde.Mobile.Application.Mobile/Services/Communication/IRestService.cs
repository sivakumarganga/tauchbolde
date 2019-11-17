using System.Threading.Tasks;

namespace Tauchbolde.Mobile.Application.Services.Communication
{
    public interface IRestService
    {
        Task<T> GetAsync<T>(string url);
    }
}