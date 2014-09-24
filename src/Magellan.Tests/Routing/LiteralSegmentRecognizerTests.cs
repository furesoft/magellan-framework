using Magellan.Routing;
using NUnit.Framework;

namespace Magellan.Tests.Routing
{
    [TestFixture]
    public class LiteralSegmentRecognizerTests
    {
        [Test]
        public void PositiveRecognizeTest()
        {
            ShouldRecognize("foo");
            ShouldRecognize("FOO");
            ShouldRecognize("foo123");
            ShouldRecognize("foo-bar");
        }

        [Test]
        public void NegativeRecognizeTest()
        {
            ShouldNotRecognize("");
            ShouldNotRecognize("  ");
            ShouldNotRecognize("  foo123  ");
            ShouldNotRecognize("foo#@PV");
            ShouldNotRecognize("foo$");
            ShouldNotRecognize("foo%");
            ShouldNotRecognize("foo^");
            ShouldNotRecognize("foo*");
        }
        
        private static void ShouldRecognize(string segment)
        {
            var recognizer = new LiteralSegmentRecognizer();
            var recognized = recognizer.Recognise(segment, new RouteValueDictionary(), new RouteValueDictionary());
            Assert.IsInstanceOf<LiteralSegment>(recognized);
        }

        private static void ShouldNotRecognize(string segment)
        {
            var recognizer = new LiteralSegmentRecognizer();
            var recognized = recognizer.Recognise(segment, new RouteValueDictionary(), new RouteValueDictionary());
            Assert.IsNull(recognized);
        }
    }
}
