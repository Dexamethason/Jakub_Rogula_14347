using System.ServiceModel;

namespace Api.Application.Soap
{
    [ServiceContract]
    public interface ISoapService
    {
        [OperationContract]
        Task<string> GetSoapDataAsync(int value);
    }
}