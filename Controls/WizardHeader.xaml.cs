using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ClassicAeroStudio.Controls
{
    public partial class WizardHeader : UserControl
    {
        public WizardHeader()
        {
            InitializeComponent();
            SetStep(1);
        }

        public void SetStep(int step)
        {
            if (step == 1)
            {
                // Step 1 Active
                Step1Path.SetResourceReference(Shape.FillProperty, "ClassicWizardActiveBackground");
                Step1Path.SetResourceReference(Shape.StrokeProperty, "ClassicWizardActiveBorder");
                Step1Badge.SetResourceReference(Border.BackgroundProperty, "ClassicWizardActiveBadge");
                Step1Text.SetResourceReference(TextBlock.ForegroundProperty, "ClassicWizardActiveForeground");
                Step1Text.FontWeight = FontWeights.SemiBold;

                // Step 2 Inactive
                Step2Path.SetResourceReference(Shape.FillProperty, "ClassicWizardInactiveBackground");
                Step2Path.SetResourceReference(Shape.StrokeProperty, "ClassicWizardInactiveBorder");
                Step2Badge.SetResourceReference(Border.BackgroundProperty, "ClassicWizardInactiveBadge");
                Step2Text.SetResourceReference(TextBlock.ForegroundProperty, "ClassicWizardInactiveForeground");
                Step2Text.FontWeight = FontWeights.Normal;
            }
            else if (step == 2)
            {
                // Step 1 Inactive / Completed
                Step1Path.SetResourceReference(Shape.FillProperty, "ClassicWizardInactiveBackground");
                Step1Path.SetResourceReference(Shape.StrokeProperty, "ClassicWizardInactiveBorder");
                Step1Badge.SetResourceReference(Border.BackgroundProperty, "ClassicWizardInactiveBadge");
                Step1Text.SetResourceReference(TextBlock.ForegroundProperty, "ClassicWizardInactiveForeground");
                Step1Text.FontWeight = FontWeights.Normal;

                // Step 2 Active
                Step2Path.SetResourceReference(Shape.FillProperty, "ClassicWizardActiveBackground");
                Step2Path.SetResourceReference(Shape.StrokeProperty, "ClassicWizardActiveBorder");
                Step2Badge.SetResourceReference(Border.BackgroundProperty, "ClassicWizardActiveBadge");
                Step2Text.SetResourceReference(TextBlock.ForegroundProperty, "ClassicWizardActiveForeground");
                Step2Text.FontWeight = FontWeights.SemiBold;
            }
        }
    }
}
