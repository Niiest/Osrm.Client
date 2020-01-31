using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Osrm.Client.Tests
{
    public class RequestUrlsTests
    {
        private Location[] locations = new Location[] {
                new Location(52.503033, 13.420526),
                new Location(52.516582, 13.42929),
            };

        private Location[] src = new Location[] {
                new Location(52.55407, 13.160621),
            };

        private Location[] dst = new Location[] {
                new Location(52.431272, 13.720654)
            };

        private LocationWithTimestamp[] matchLocations = new LocationWithTimestamp[] {
                new LocationWithTimestamp(52.542648, 13.393252, 1424684612),
                new LocationWithTimestamp(52.543079, 13.394780, 1424684616),
                new LocationWithTimestamp(52.542107, 13.397389, 142468462)
            };

        private string[] ParamValues(List<Tuple<string, string>> urlParams, string paramKey)
        {
            return urlParams.Where(p => p.Item1 == paramKey).Select(p => p.Item2).ToArray();
        }

        [Fact]
        public void RouteRequest_Url()
        {
            var r = new ViarouteRequest();
            r.Locations = locations;
            var locParams = ParamValues(r.UrlParams, "loc");
            Assert.StrictEqual<int>(locations.Length, locParams.Length);
            Assert.True(locParams.Contains("52.503033,13.420526"));
            Assert.True(locParams.Contains("52.516582,13.42929"));
            r.SendLocsAsEncodedPolyline = true;
            Assert.StrictEqual<int>(0, ParamValues(r.UrlParams, "loc").Length);
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "locs").Length);
            r.Zoom = 5;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "z").Length);
            Assert.StrictEqual<string>("5", ParamValues(r.UrlParams, "z")[0]);
            r.Instructions = true;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "instructions").Length);
            Assert.StrictEqual<string>("true", ParamValues(r.UrlParams, "instructions")[0]);
            r.Alternative = false;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "alt").Length);
            Assert.StrictEqual<string>("false", ParamValues(r.UrlParams, "alt")[0]);
            r.Geometry = false;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "geometry").Length);
            Assert.StrictEqual<string>("false", ParamValues(r.UrlParams, "geometry")[0]);
            r.Compression = false;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "compression").Length);
            Assert.StrictEqual<string>("false", ParamValues(r.UrlParams, "compression")[0]);
            r.UTurns = true;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "uturns").Length);
            Assert.StrictEqual<string>("true", ParamValues(r.UrlParams, "uturns")[0]);
            r.UTurnAtTheVia = true;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "u").Length);
            Assert.StrictEqual<string>("true", ParamValues(r.UrlParams, "u")[0]);
            r.Hint = "hinttest";
            Assert.StrictEqual<string>("hinttest", ParamValues(r.UrlParams, "hint")[0]);
            r.Checksum = "checksumTest";
            Assert.StrictEqual<string>("checksumTest", ParamValues(r.UrlParams, "checksum")[0]);
        }

        [Fact]
        public void TableRequest_Url()
        {
            var r = new TableRequest();
            r.Locations = locations;
            var locParams = ParamValues(r.UrlParams, "loc");
            Assert.StrictEqual<int>(locations.Length, locParams.Length);
            Assert.True(locParams.Contains("52.503033,13.420526"));
            Assert.True(locParams.Contains("52.516582,13.42929"));
            r.SourceLocations = src;
            var srcParams = ParamValues(r.UrlParams, "src");
            Assert.StrictEqual<int>(src.Length, srcParams.Length);
            Assert.True(srcParams.Contains("52.55407,13.160621"));
            r.DestinationLocations = dst;
            var dstParams = ParamValues(r.UrlParams, "dst");
            Assert.StrictEqual<int>(dst.Length, dstParams.Length);
            Assert.True(dstParams.Contains("52.431272,13.720654"));
        }

        [Fact]
        public void MatchRequest_Url()
        {
            var r = new MatchRequest();
            r.Locations = matchLocations;
            var locParams = ParamValues(r.UrlParams, "loc");
            Assert.StrictEqual<int>(matchLocations.Length, locParams.Length);
            Assert.True(locParams.Contains("52.542648,13.393252"));
            Assert.True(locParams.Contains("52.543079,13.39478"));
            Assert.True(locParams.Contains("52.542107,13.397389"));
            var tParams = ParamValues(r.UrlParams, "t");
            Assert.StrictEqual<int>(matchLocations.Length, tParams.Length);
            Assert.True(tParams.Contains("1424684612"));
            Assert.True(tParams.Contains("1424684616"));
            Assert.True(tParams.Contains("142468462"));

            r.SendLocsAsEncodedPolyline = true;
            Assert.StrictEqual<int>(0, ParamValues(r.UrlParams, "loc").Length);
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "locs").Length);
            r.Geometry = false;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "geometry").Length);
            Assert.StrictEqual<string>("false", ParamValues(r.UrlParams, "geometry")[0]);
            r.Compression = false;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "compression").Length);
            Assert.StrictEqual<string>("false", ParamValues(r.UrlParams, "compression")[0]);
            r.Classify = true;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "classify").Length);
            Assert.StrictEqual<string>("true", ParamValues(r.UrlParams, "classify")[0]);
            r.Instructions = true;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "instructions").Length);
            Assert.StrictEqual<string>("true", ParamValues(r.UrlParams, "instructions")[0]);
            r.GpsPrecision = 3;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "gps_precision").Length);
            Assert.StrictEqual<string>("3", ParamValues(r.UrlParams, "gps_precision")[0]);
            r.MatchingBeta = 8;
            Assert.StrictEqual<int>(1, ParamValues(r.UrlParams, "matching_beta").Length);
            Assert.StrictEqual<string>("8", ParamValues(r.UrlParams, "matching_beta")[0]);
            r.Hint = "hinttest";
            Assert.StrictEqual<string>("hinttest", ParamValues(r.UrlParams, "hint")[0]);
            r.Checksum = "checksumTest";
            Assert.StrictEqual<string>("checksumTest", ParamValues(r.UrlParams, "checksum")[0]);
        }

        [Fact]
        public void NearestRequest_Url()
        {
            var r = new NearestRequest();
            r.Location = new Location(52.4224, 13.333086);
            var locParams = ParamValues(r.UrlParams, "loc");
            Assert.StrictEqual<int>(1, locParams.Length);
            Assert.True(locParams.Contains("52.4224,13.333086"));
        }
    }
}