using Newtonsoft.Json;
using Osrm.Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Osrm.Client
{
    public class OsrmClient
    {
        private readonly HttpClient _client;

        protected readonly string NearestServiceName = "nearest";
        protected readonly string RouteServiceName = "viaroute";
        protected readonly string TableServiceName = "table";
        protected readonly string MatchServiceName = "match";
        protected readonly string TripServiceName = "trip";

        public OsrmClient(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Returns the nearest street segment for a given coordinate
        /// <param name="locs"></param>
        /// <returns></returns>
        public Task<NearestResponse> NearestAsync(Location loc)
        {
            return NearestAsync(new NearestRequest()
            {
                Location = loc
            });
        }

        /// <summary>
        /// Returns the nearest street segment for a given coordinate
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public Task<NearestResponse> NearestAsync(NearestRequest requestParams)
        {
            return SendAsync<NearestResponse>(NearestServiceName, requestParams.UrlParams);
        }

        /// <summary>
        /// This service provides shortest path queries with multiple via locations.
        /// It supports the computation of alternative paths as well as giving turn instructions.
        /// </summary>
        /// <param name="locs"></param>
        /// <returns></returns>
        public Task<ViarouteResponse> RouteAsync(Location[] locs)
        {
            return RouteAsync(new ViarouteRequest()
            {
                Locations = locs
            });
        }

        /// <summary>
        /// This service provides shortest path queries with multiple via locations.
        /// It supports the computation of alternative paths as well as giving turn instructions.
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public Task<ViarouteResponse> RouteAsync(ViarouteRequest requestParams)
        {
            return SendAsync<ViarouteResponse>(RouteServiceName, requestParams.UrlParams);
        }

        /// <summary>
        /// This computes distance tables for the given via points.
        /// Please note that the distance in this case is the travel time which is the default metric used by OSRM.
        /// If only loc parameter is used, all pair-wise distances are computed.
        /// If dst and src parameters are used instead, only pairs between scr/dst are computed.
        /// </summary>
        /// <param name="locs"></param>
        /// <returns></returns>
        public Task<TableResponse> TableAsync(Location[] locs)
        {
            return TableAsync(new TableRequest()
            {
                Locations = locs
            });
        }

        /// <summary>
        /// This computes distance tables for the given via points.
        /// Please note that the distance in this case is the travel time which is the default metric used by OSRM.
        /// If only loc parameter is used, all pair-wise distances are computed.
        /// If dst and src parameters are used instead, only pairs between scr/dst are computed.
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public Task<TableResponse> TableAsync(TableRequest requestParams)
        {
            return SendAsync<TableResponse>(TableServiceName, requestParams.UrlParams);
        }

        /// <summary>
        /// Map matching matches given GPS points to the road network in the most plausible way.
        /// Currently the algorithm works best with trace data that has a sample resolution of 5-10 samples/min.
        /// Please note the request might result multiple sub-traces.
        /// Large jumps in the timestamps (>60s) or improbable transitions lead to trace splits if a complete matching could not be found.
        /// The algorithm might not be able to match all points.
        /// Outliers are removed if they can not be matched successfully.
        /// </summary>
        /// <param name="locs"></param>
        /// <returns></returns>
        public Task<MatchResponse> MatchAsync(LocationWithTimestamp[] locs)
        {
            return MatchAsync(new MatchRequest()
            {
                Locations = locs
            });
        }

        /// <summary>
        /// Map matching matches given GPS points to the road network in the most plausible way.
        /// Currently the algorithm works best with trace data that has a sample resolution of 5-10 samples/min.
        /// Please note the request might result multiple sub-traces.
        /// Large jumps in the timestamps (>60s) or improbable transitions lead to trace splits if a complete matching could not be found.
        /// The algorithm might not be able to match all points.
        /// Outliers are removed if they can not be matched successfully.
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public Task<MatchResponse> MatchAsync(MatchRequest requestParams)
        {
            return SendAsync<MatchResponse>(MatchServiceName, requestParams.UrlParams);
        }

        /// <summary>
        /// The trip plugin solves the famous Traveling Salesman Problem using a greedy heuristic (farest-insertion algorithm).
        /// The returned path does not have to be the shortest path, as TSP is NP-hard it is only an approximation.
        /// Note that if the input coordinates can not be joined by a single trip (e.g. the coordinates are on several disconnecte islands)
        /// multiple trips for each connected component are returned.
        /// Trip does not support computing alternatives
        /// </summary>
        /// <param name="locs">Trip does not support computing alternatives</param>
        /// <returns></returns>
        public Task<TripResponse> TripAsync(Location[] locs)
        {
            return TripAsync(new ViarouteRequest()
            {
                Locations = locs
            });
        }

        /// <summary>
        /// The trip plugin solves the famous Traveling Salesman Problem using a greedy heuristic (farest-insertion algorithm).
        /// The returned path does not have to be the shortest path, as TSP is NP-hard it is only an approximation.
        /// Note that if the input coordinates can not be joined by a single trip (e.g. the coordinates are on several disconnecte islands)
        /// multiple trips for each connected component are returned.
        /// Trip does not support computing alternatives.
        /// <param name="requestParams">Trip does not support computing alternatives</param>
        /// <returns></returns>
        public Task<TripResponse> TripAsync(ViarouteRequest requestParams)
        {
            return SendAsync<TripResponse>(TripServiceName, requestParams.UrlParams);
        }

        protected async Task<T> SendAsync<T>(string service, List<Tuple<string, string>> urlParams)
        {
            var fullUrl = OsrmRequestBuilder.GetUrl(_client.BaseAddress.AbsoluteUri, service, urlParams);
            var json = await _client.GetStringAsync(new Uri(fullUrl))
                .ConfigureAwait(continueOnCapturedContext: false);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}