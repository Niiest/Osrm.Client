using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Osrm.Client.Models;

namespace Osrm.Client.Tests
{
    [TestClass]
    public class ResponseTests
    {
        protected OsrmClient osrm = new OsrmClient(new HttpClient()
        {
            BaseAddress = new Uri("http://router.project-osrm.org/")
        });

        private void CheckStatus200(OsrmBaseResponse response)
        {
            Assert.AreEqual(200, response.Status);
        }

        [TestMethod]
        public async Task Route_Response()
        {
            var locations = new Location[] {
                new Location(52.503033, 13.420526),
                new Location(52.516582, 13.429290),
            };

            var result = await osrm.RouteAsync(locations);
            var geometryLenDefaultZoom = result.RouteGeometry.Length;

            CheckStatus200(result);
            Assert.IsTrue(result.RouteName.Length > 0);
            Assert.IsTrue(result.AlternativeGeometries.Length > 0);
            Assert.AreEqual<int>(0, result.RouteInstructions.Length);

            var result2 = await osrm.RouteAsync(new ViarouteRequest()
            {
                Locations = locations,
                SendLocsAsEncodedPolyline = true,
                Alternative = false,
            });

            CheckStatus200(result2);
            Assert.AreEqual<int>(0, result2.AlternativeGeometries.Length);
            Assert.AreEqual<int>(0, result2.RouteInstructions.Length);

            var result3 = await osrm.RouteAsync(new ViarouteRequest()
            {
                Locations = locations,
                Instructions = true,
                Zoom = 5
            });

            CheckStatus200(result3);
            Assert.IsTrue(result3.AlternativeGeometries.Length > 0);
            Assert.IsTrue(result3.RouteInstructions.Length > 0);
            var geometryLenZoom3 = result3.RouteGeometry.Length;
            Assert.IsTrue(geometryLenZoom3 < geometryLenDefaultZoom);
        }

        [TestMethod]
        public async Task Table_Response()
        {
            var locations = new Location[] {
                new Location(52.554070, 13.160621),
                new Location(52.431272, 13.720654),
                new Location(52.554070, 13.720654),
                new Location(52.554070, 13.160621),
            };

            var result = await osrm.TableAsync(locations);
            CheckStatus200(result);
            Assert.AreEqual<int>(4, result.DistanceTable.Length);
            Assert.AreEqual<int>(4, result.DistanceTable[0].Length);
            Assert.AreEqual<int>(4, result.DistanceTable[1].Length);
            Assert.AreEqual<int>(4, result.DistanceTable[2].Length);
            Assert.AreEqual<int>(4, result.DistanceTable[3].Length);

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

            CheckStatus200(result2);
            Assert.AreEqual<int>(1, result2.DistanceTable.Length);
            Assert.AreEqual<int>(3, result2.DistanceTable[0].Length);
        }

        [TestMethod]
        public async Task Match_Response()
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

            CheckStatus200(result);
            Assert.IsTrue(result.Matchings.Length > 0);
            Assert.IsTrue(result.Matchings[0].Instructions.Length > 0);
            Assert.IsNotNull(result.Matchings[0].Confidence);
        }

        [TestMethod]
        public async Task Nearest_Response()
        {
            var result = await osrm.NearestAsync(new Location(52.4224, 13.333086));

            CheckStatus200(result);
            Assert.IsNotNull(result.MappedCoordinate);
        }

        [TestMethod]
        public async Task Trip_Response()
        {
            var locations = new Location[] {
                new Location(52.503033, 13.420526),
                new Location(52.516582, 13.429290),
            };

            var result = await osrm.TripAsync(locations);

            CheckStatus200(result);
            Assert.AreEqual<int>(1, result.Trips.Length);
            Assert.IsTrue(result.Trips[0].Permutation.Length > 0);
            Assert.IsTrue(result.Trips[0].RouteName.Length > 0);
            Assert.IsTrue(result.Trips[0].RouteGeometry.Length > 0);
        }
    }
}