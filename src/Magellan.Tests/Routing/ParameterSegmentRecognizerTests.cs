using Magellan.Routing;
using NUnit.Framework;

namespace Magellan.Tests.Routing
{
    [TestFixture]
    public class ParameterSegmentRecognizerTests
    {
        [Test]
        public void PositiveRecognizeTest()
        {
            ShouldRecognize("{foo}");
            ShouldRecognize("{FOO}");
            ShouldRecognize("{foo123}");
        }

        [Test]
        public void NegativeRecognizeTest()
        {
            ShouldNotRecognize("");
            ShouldNotRecognize("  ");
            ShouldNotRecognize("{}");
            ShouldNotRecognize("{foo} ");
            ShouldNotRecognize("{foo}}");
            ShouldNotRecognize("{  foo123  }");
            ShouldNotRecognize("{foo#@PV}");
            ShouldNotRecognize("{foo-bar}");
            ShouldNotRecognize("{foo$}");
            ShouldNotRecognize("{foo%}");
            ShouldNotRecognize("{foo^}");
            ShouldNotRecognize("{foo*}");
        }
        
        private static void ShouldRecognize(string segment)
        {
            var recognizer = new ParameterSegmentRecognizer();
            var recognized = recognizer.Recognise(segment, new RouteValueDictionary(), new RouteValueDictionary());
            Assert.IsInstanceOf<ParameterSegment>(recognized);
        }

        private static void ShouldNotRecognize(string segment)
        {
            var recognizer = new ParameterSegmentRecognizer();
            var recognized = recognizer.Recognise(segment, new RouteValueDictionary(), new RouteValueDictionary());
            Assert.IsNull(recognized);
        }
    }
}
