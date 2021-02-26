using LangAppApi.Domain.Auth;
using LangAppApi.Domain.Common;
using System.Threading.Tasks;

namespace LangAppApi.Service.Contract
{
    public interface IAccountService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        Task<Response<string>> RegisterAsync(RegisterRequest request);
    }
}