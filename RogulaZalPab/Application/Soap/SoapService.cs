namespace Api.Application.Soap
{
    public class SoapService : ISoapService
    {
        public Task<string> GetSoapDataAsync(int value)
        {
            return Task.FromResult($"SOAP data for value: {value}");
        }
    }
}