using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Transitionals.Controls;
using Transitionals.Transitions;

namespace Magellan.Transitionals.Transitions
{
    /// <summary>
    /// A transition that places the old and new content side by side and slides them to the left or right.
    /// </summary>
    public class SlideTransition : StoryboardTransition
    {
        private readonly SlideDirection direction;

        /// <summary>
        /// Initializes the <see cref="SlideTransition"/> class.
        /// </summary>
        static SlideTransition()
        {
            ClipToBoundsProperty.OverrideMetadata(typeof(SlideTransition), new FrameworkPropertyMetadata(true));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SlideTransition"/> class.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public SlideTransition(SlideDirection direction)
        {
            this.direction = direction;
            Duration = new Duration(TimeSpan.FromSeconds(0.5));
        }

        /// <summary>
        /// Begins the transition.
        /// </summary>
        /// <param name="transitionElement">The transition element.</param>
        /// <param name="oldContent">The old content.</param>
        /// <param name="newContent">The new content.</param>
        protected override void BeginTransition(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            if (direction == SlideDirection.Back)
            {
                Slide(transitionElement, oldContent, 0, 1.0, null);
                Slide(transitionElement, newContent, -1.0, 0.0, () => EndTransition(transitionElement, oldContent, newContent));
            }
            else
            {
                Slide(transitionElement, oldContent, 0.0, -1.0, null);
                Slide(transitionElement, newContent, 1.0, 0.0, () => EndTransition(transitionElement, oldContent, newContent));
            }
        }

        private void Slide(FrameworkElement transitionElement, UIElement content, double startX, double endX, Action completeCallback)
        {
            content.RenderTransform = new TranslateTransform(startX * transitionElement.ActualWidth, 0);
            
            var animation = new DoubleAnimation(endX * transitionElement.ActualWidth, Duration);
            animation.AccelerationRatio = 0.5;
            animation.DecelerationRatio = 0.5;
            Storyboard.SetTarget(animation, content.RenderTransform);
            Storyboard.SetTargetProperty(animation, new PropertyPath(TranslateTransform.XProperty));
            if (completeCallback != null)
            {
                animation.Completed += (x,y) => completeCallback();
            }

            content.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        /// <summary>
        /// Called when the transition ends.
        /// </summary>
        /// <param name="transitionElement">The transition element.</param>
        /// <param name="oldContent">The old content.</param>
        /// <param name="newContent">The new content.</param>
        protected override void OnTransitionEnded(TransitionElement transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            newContent.ClearValue(UIElement.RenderTransformProperty);
            oldContent.ClearValue(UIElement.RenderTransformProperty);
        }
    }
}