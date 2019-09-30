using Osrm.Client.Models;
using Osrm.Client.v5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Osrm.Client.Demo
{
    internal class Program
    {
        public static string OsrmUrl = "http://router.project-osrm.org/";

        private static void Main(string[] args)
        {
            var osrm5x = new Osrm5x(OsrmUrl);
            Route5x(osrm5x);
            Nearest5x(osrm5x);
            Table5x(osrm5x);
            Match5x(osrm5x);
            Trip5x(osrm5x);

            //  4x
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(OsrmUrl) })
            {
                var osrm4x = new OsrmClient(httpClient);
                Route4x(osrm4x).GetAwaiter().GetResult();
                /*Table4x(osrm4x).GetAwaiter().GetResult();
                Match4x(osrm4x).GetAwaiter().GetResult();
                Nearest4x(osrm4x).GetAwaiter().GetResult();
                Trip4x(osrm4x).GetAwaiter().GetResult();*/
            }
        }

        private static void Route5x(Osrm5x osrm)
        {
            var locations = new Location[] {
                new Location(52.503033, 13.420526),
                new Location(52.516582, 13.429290),
            };

            var result = osrm.Route(locations);

            var result2 = osrm.Route(new RouteRequest()
            {
                Coordinates = locations,
                Steps = true
            });
            var result3 = osrm.Route(new RouteRequest()
            {
                Coordinates = locations,
                SendCoordinatesAsPolyline = true
            });

            var instructions3 = result2.Routes[0].Legs[0].Steps;
        }

        private static void Nearest5x(Osrm5x osrm)
        {
            var result = osrm.Nearest(new Location(52.4224, 13.333086));
        }

        private static void Table5x(Osrm5x osrm)
        {
            var locations = new Location[] {
                new Location(52.517037, 13.388860),
                new Location(52.529407, 13.397634),
                new Location(52.523219, 13.428555)
            };

            //Returns a 3x3 matrix:
            var result = osrm.Table(locations);

            //Returns a 1x3 matrix:
            var result2 = osrm.Table(new Osrm.Client.Models.TableRequest()
            {
                Coordinates = locations,
                Sources = new uint[] { 0 }
                //Sources = src,
                //DestinationLocations = dst
            });

            //Returns a asymmetric 3x2 matrix with from the polyline encoded locations qikdcB}~dpXkkHz:
            var result3 = osrm.Table(new Osrm.Client.Models.TableRequest()
            {
                Coordinates = locations,
                SendCoordinatesAsPolyline = true,
                Sources = new uint[] { 0, 1 },
                Destinations = new uint[] { 1, 2 }
                //Sources = src,
                //DestinationLocations = dst
            });
        }

        private static void Match5x(Osrm5x osrm)
        {
            var locations = new Location[] {
                new Location(52.517037, 13.388860),
                new Location(52.529407, 13.397634),
                new Location(52.523219, 13.428555)
            };

            var request = new Osrm.Client.Models.MatchRequest()
            {
                Coordinates = locations
            };

            var result = osrm.Match(request);
        }

        private static void Trip5x(Osrm5x osrm)
        {
            var locations = new Location[] {
                new Location(55.784824, 37.5953609),
                new Location(47.2357137, 39.701505),
            };

            var result = osrm.Trip(locations);
        }

        #region 4x

        private static async Task Route4x(OsrmClient osrm)
        {
            var locations = new Location[] {
                new Location(55.784824, 37.5953609),
                new Location(47.2357137, 39.701505),
            };

            var result = await osrm.RouteAsync(locations);

            var result2 = await osrm.RouteAsync(new ViarouteRequest()
            {
                Locations = locations,
                SendLocsAsEncodedPolyline = true,
                Alternative = false,
            });

            var result3 = await osrm.RouteAsync(new ViarouteRequest()
            {
                Locations = locations,
                Instructions = true,
                Zoom = 5
            });
            var instructions3 = result3.RouteInstructions;
        }

        private static async Task Table4x(OsrmClient osrm)
        {
            var locations = new Location[] {
                new Location(52.554070, 13.160621),
                new Location(52.431272, 13.720654),
                new Location(52.554070, 13.720654),
                new Location(52.554070, 13.160621),
            };

            var result = await osrm.TableAsync(locations);

            var src = new Location[] {
                new Location(52.554070, 13.160621),
            };

            var dst = new Location[] {
                new Location(52.431272, 13.720654),
                new Location(52.554070, 13.720654),
                new Location(52.554070, 13.160621),
            };

            var result2 = await osrm.TableAsync(new TableRequest()
            {
                SourceLocations = src,
                DestinationLocations = dst
            });
        }

        private static async Task Match4x(OsrmClient osrm)
        {
            var locations = new LocationWithTimestamp[] {
                new LocationWithTimestamp(52.542648, 13.393252, 1424684612),
                new LocationWithTimestamp(52.543079, 13.394780, 1424684616),
                new LocationWithTimestamp(52.542107, 13.397389, 1424684620)
            };

            var request = new MatchRequest()
            {
                Locations = locations,
                Instructions = true,
                Classify = true
            };

            var result = await osrm.MatchAsync(request);
        }

        private static async Task Nearest4x(OsrmClient osrm)
        {
            var result = await osrm.NearestAsync(new Location(52.4224, 13.333086));
        }

        private static async Task Trip4x(OsrmClient osrm)
        {
            var locations = new Location[] {
                new Location(52.503033, 13.420526),
                new Location(52.516582, 13.429290),
            };

            var result = await osrm.TripAsync(locations);
        }

        #endregion 4x
    }
}