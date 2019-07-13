namespace WPFUI.Services
{
    ///References
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using WPFUI.Models;

    /// <summary>
    /// Network Service
    /// </summary>
    public class NetworkService
    {
        /// <summary>
        /// Check connection from client
        /// </summary>
        /// <returns>object Response</returns>
        public async Task<Response> CheckConnection()
        {
            var client = new WebClient();
            try
            {
                //ping client
                using (var response = await client.OpenReadTaskAsync("http://clients3.google.com/generate_204"))
                {
                    return new Response
                    {
                        IsSucess = true,
                        Message = "You have internet connection"
                    };
                }
            }
            catch(Exception e)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = e.Message
                };
            }
        }
    }
}
