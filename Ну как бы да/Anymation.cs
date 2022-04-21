using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Ну_как_бы_да
{
    public static class Anymation
    {
        public static void Animation(TextBox obj)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 1;
            animation.Duration = TimeSpan.FromMilliseconds(700);
            obj.BeginAnimation(TextBox.OpacityProperty, animation);
        }
        public static void OpacityAnimationForButton(Button obj)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 1;
            animation.Duration = TimeSpan.FromMilliseconds(700);
            obj.BeginAnimation(Button.OpacityProperty, animation);
        }
    }
}
