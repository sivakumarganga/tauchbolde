namespace Tauchbolde.Mobile.Application.Services.Communication
{
    internal interface IUrlManager
    {
        string GetUrl(string relativeActionUrl, params object[] queryParams);
    }
}