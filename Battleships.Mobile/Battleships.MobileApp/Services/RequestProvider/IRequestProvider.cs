using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.MobileApp.Services.RequestProvider
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "");
        Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "");
        Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "");
        Task DeleteAsync(string uri, string token = "");
    }
}
