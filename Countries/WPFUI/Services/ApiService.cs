namespace WPFUI.Services
{
    ///References
    using Newtonsoft.Json;
    using Svg;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using WPFUI.Models;

    /// <summary>
    /// Api connection
    /// </summary>
    public class ApiService
    {
        #region Atributes

        /// <summary>
        /// List of countries
        /// </summary>
        private List<Country> countries;

        /// <summary>
        /// List of rates
        /// </summary>
        private List<Rate> rates;

        #endregion

        /// <summary>
        /// Api Service init
        /// </summary>
        public ApiService()
        {
            countries   = new List<Country>();
            rates       = new List<Rate>();
        }

        #region Methods

        /// <summary>
        /// Async task response to get countries
        /// </summary>
        /// <param name="url">system.string</param>
        /// <param name="controller">system.string</param>
        /// <returns>collection of countries</returns>
        public async Task<Response> GetCountries(string url, string controller)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(url);

                var response = await client.GetAsync(controller);

                var result = await response.Content.ReadAsStringAsync();
                

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSucess = false,
                        Message = result
                    };
                }
                

                countries = JsonConvert.DeserializeObject<List<Country>>(result);

                

                return new Response
                {
                    IsSucess = true,
                    Result = countries
                };
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

        /// <summary>
        /// Async task response to get rates
        /// </summary>
        /// <param name="url">system.string</param>
        /// <param name="controller">system.string</param>
        /// <returns>collection of rates</returns>
        public async Task<Response> GetRates(string urlBase, string controller)
        {
            try
            {
                var client = new HttpClient();
                
                client.BaseAddress = new Uri(urlBase);

                var response = await client.GetAsync(controller);

                var result = await response.Content.ReadAsStringAsync();
                
                
                if (!response.IsSuccessStatusCode)
                {
                    new Response
                    {
                        IsSucess = false,
                        Message = result,
                    };
                }
                

                rates = JsonConvert.DeserializeObject<List<Rate>>(result);
                

                return new Response
                {
                    IsSucess = true,
                    Result = rates,
                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = e.Message,
                };
            }
        }

        /// <summary>
        /// Async task response to get flags
        /// </summary>
        /// <param name="flag">string</param>
        /// <param name="name">string</param>
        /// <returns>Object Flag</returns>
        public async Task<Flags> GetFlags(string flag, string name)
        {
            Flags F = new Flags();

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    await webClient.DownloadFileTaskAsync(flag, $@"./Flags/{name}.svg");

                    var svgDocument = SvgDocument.Open($@"./Flags/{name}.svg");
                    svgDocument.ShapeRendering = SvgShapeRendering.Auto;

                    return DataAccess.ConvertSVG(svgDocument, name);

                }
                catch (Exception)
                {
                    await webClient.DownloadFileTaskAsync("https://svgshare.com/getbyhash/sha1-5jcSNQkMekIRGnRqiNuKIEWG1w0=", $@"./Flags/{name}.svg");

                    var svgDocument = SvgDocument.Open($@"./Flags/{name}.svg");
                    svgDocument.ShapeRendering = SvgShapeRendering.Auto;

                    return DataAccess.ConvertSVG(svgDocument, name);
                }
            }

        }
        #endregion
    }
}
