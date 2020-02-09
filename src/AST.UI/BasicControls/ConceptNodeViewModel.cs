using OneCSharp.AST.Model;

namespace OneCSharp.AST.UI
{
    public sealed class ConceptNodeViewModel : SyntaxNodeViewModel
    {
        public ConceptNodeViewModel(ISyntaxNodeViewModel owner, ISyntaxNode model) : base(owner, model) { }
        public void ShowOptions()
        {
            IsMouseOver = true;
            ShowHideOptions(this);
        }
        public void HideOptions()
        {
            IsMouseOver = false;
            ShowHideOptions(this);
        }
        private void ShowHideOptions(ISyntaxNodeViewModel parent)
        {
            foreach (var line in parent.Lines)
            {
                foreach (var node in line.Nodes)
                {
                    node.IsTemporallyVisible = IsMouseOver;
                    if (node is RepeatableViewModel)
                    {
                        // TODO: if repeatable has no items - nothing will be shown !
                        //ShowHideOptions(node);
                    }
                }
            }
        }
        //private void FadeOut(UIElement uiElement, double fromOpacity,
        //double toOpacity, int durationInMilliseconds, bool loopAnimation,
        //bool showOnStart, bool collapseOnFinish)
        //{
        //    var timeSpan = TimeSpan.FromMilliseconds(durationInMilliseconds);
        //    var doubleAnimation =
        //          new DoubleAnimation(fromOpacity, toOpacity,
        //                              new Duration(timeSpan));
        //    if (loopAnimation)
        //        doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
        //    uiElement.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);
        //    if (showOnStart)
        //    {
        //        uiElement.ApplyAnimationClock(UIElement.VisibilityProperty, null);
        //        uiElement.Visibility = Visibility.Visible;
        //    }
        //    if (collapseOnFinish)
        //    {
        //        var keyAnimation = new ObjectAnimationUsingKeyFrames { Duration = new Duration(timeSpan) };
        //        keyAnimation.KeyFrames.Add(new DiscreteObjectKeyFrame(Visibility.Collapsed, KeyTime.FromTimeSpan(timeSpan)));
        //        uiElement.BeginAnimation(UIElement.VisibilityProperty, keyAnimation);
        //    }
        //    return uiElement;
        //}

        //public static T FadeIn(this UIElement uiElement, int durationInMilliseconds)
        //{
        //    return uiElement.FadeFromTo(0, 1, durationInMilliseconds, false, true, false);
        //}

        //public static T FadeOut(this UIElement uiElement, int durationInMilliseconds)
        //{
        //    return uiElement.FadeFromTo(1, 0, durationInMilliseconds, false, false, true);
        //}
    }
}