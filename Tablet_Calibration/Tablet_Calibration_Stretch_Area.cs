using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;


namespace Tablet_Calibration
{
    [PluginName("Tablet Calibration Stretch Area")]
    public class Tablet_Calibration_Stretch_Area : Tablet_Calibration_Base
    {
        public Vector2 Quadrant_Stretch(Vector2 input)
        {
            switch (In_Quadrant(input)) {
                case 1: {
                    input.X *= Right_Stretch;
                    input.Y *= Top_Stretch;
                    break;
                }
                case 2: {
                    input.X *= Left_Stretch;
                    input.Y *= Top_Stretch;
                    break;
                }
                case 3: {
                    input.X *= Left_Stretch;
                    input.Y *= Bottom_Stretch;
                    break;
                }
                case 4: {
                    input.X *= Right_Stretch;
                    input.Y *= Bottom_Stretch;
                    break;
                }
                default: {
                    return input;
                }
            }
            return input;
        }

        public override event Action<IDeviceReport> Emit;

        public override void Consume(IDeviceReport value)
        {
            if (value is ITabletReport report)
            {
                report.Position = Filter(report.Position);
                value = report;
            }

            Emit?.Invoke(value);
        }

        public Vector2 Filter(Vector2 input) {
            if (Disable_Clamping) {
                return FromUnitScreen(Quadrant_Stretch(ToUnitScreen(input)));
            }
            return FromUnitScreen(Clamp(Quadrant_Stretch(ToUnitScreen(input))));
        }

        public override PipelinePosition Position => PipelinePosition.PostTransform;

        [Property("Left Stretch Multiplier"), DefaultPropertyValue(1f), ToolTip
            ("Tablet Calibration Stretch Area:\n\n" +
            "The multiplier used to stretch the left side of the tablet area's X axis coordinates.")]
        public static float Left_Stretch { set; get; }

        [Property("Right Stretch Multiplier"), DefaultPropertyValue(1f), ToolTip
            ("Tablet Calibration Stretch Area:\n\n" +
            "The multiplier used to stretch the right side of the tablet area's X axis coordinates.")]
        public static float Right_Stretch { set; get; }

        [Property("Top Stretch Multiplier"), DefaultPropertyValue(1f), ToolTip
            ("Tablet Calibration Stretch Area:\n\n" +
            "The multiplier used to stretch the top of the tablet area's Y axis coordinates.")]
        public static float Top_Stretch { set; get; }

        [Property("Bottom Stretch Multiplier"), DefaultPropertyValue(1f), ToolTip
            ("Tablet Calibration Stretch Area:\n\n" +
            "The multiplier used to stretch the bottom of the tablet area's Y axis coordinates.")]
        public static float Bottom_Stretch { set; get; }

        [BooleanProperty("Disable Clamping", ""), ToolTip
            ("Tablet Calibration Stretch Area:\n\n" +
            "Allows the tablet area coordinates to exceed the tablet area.")]
        public static bool Disable_Clamping { set; get; }
    }
}