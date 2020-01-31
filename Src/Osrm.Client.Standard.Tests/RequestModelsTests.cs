using Xunit;

namespace Osrm.Client.Standard.Tests
{
    public class RequestModelsTests
    {
        [Fact]
        public void RouteRequest_Defaults()
        {
            var r = new ViarouteRequest();
            Assert.StrictEqual(18, r.Zoom);
            Assert.False(r.Instructions);
            Assert.True(r.Alternative);
            Assert.True(r.Geometry);
            Assert.True(r.Compression);
            Assert.False(r.UTurns);
            Assert.False(r.UTurnAtTheVia);
            Assert.Null(r.Hint);
            Assert.Null(r.Checksum);
        }

        [Fact]
        public void TableRequest_Defaults()
        {
            var r = new TableRequest();
            Assert.Null(r.Locations);
            Assert.Null(r.SourceLocations);
            Assert.Null(r.DestinationLocations);
        }

        [Fact]
        public void MatchRequest_Defaults()
        {
            var r = new MatchRequest();
            Assert.True(r.Geometry);
            Assert.True(r.Geometry);
            Assert.True(r.Compression);
            Assert.False(r.Classify);
            Assert.False(r.Instructions);
            Assert.StrictEqual<float>(5f, r.GpsPrecision);
            Assert.StrictEqual<float>(10f, r.MatchingBeta);
            Assert.Null(r.Hint);
            Assert.Null(r.Checksum);
        }

        [Fact]
        public void NearestRequest_Defaults()
        {
            var r = new NearestRequest();
            Assert.Null(r.Location);
        }

        [Fact]
        public void Location_Equals()
        {
            var a1 = new LocationWithTimestamp(52.542648, 13.393252, 1424684612);
            var a2 = new LocationWithTimestamp(52.542648, 13.393252, 1424684612);
            var a3 = new LocationWithTimestamp(52.542648, 13.393252);
            Assert.True(a1 == a2);
            Assert.False(a1 == a3);
            Assert.True(a1.Equals(a2));
            Assert.False(a1.Equals(a3));

            var b1 = new Location(12.542648, 13.393252);
            var b2 = new Location(12.542648, 13.393252);
            var b3 = new Location(12.542648, 99.393252);
            Assert.True(b1 == b2);
            Assert.False(b1 == b3);
            Assert.True(b1.Equals(b2));
            Assert.False(b1.Equals(b3));
        }
    }
}