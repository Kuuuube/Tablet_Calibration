using OpenTabletDriver;
using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.DependencyInjection;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Attributes;
using System;
using System.Linq;
using System.Numerics;

namespace Tablet_Calibration
{
    public abstract class Tablet_Calibration_Base : IPositionedPipelineElement<IDeviceReport>
    {
        protected Vector2 ToUnitScreen(Vector2 input)
        {
            if (outputMode is not null)
            {
                var display = outputMode?.Output;
                var offset = (Vector2)(outputMode?.Output?.Position);
                var shiftoffX = offset.X - (display.Width / 2);
                var shiftoffY = offset.Y - (display.Height / 2);
                return new Vector2(
                    (input.X - shiftoffX) / display.Width * 2 - 1,
                    (input.Y - shiftoffY) / display.Height * 2 - 1
                    );
            }
            else
            {
                tryResolveOutputMode();
                return default;
            }
        }

        protected Vector2 FromUnitScreen(Vector2 input)
        {
            if (outputMode is not null)
            {
                var display = outputMode?.Output;
                var offset = (Vector2)(outputMode?.Output?.Position);
                var shiftoffX = offset.X - (display.Width / 2);
                var shiftoffY = offset.Y - (display.Height / 2);
                return new Vector2(
                    (input.X + 1) / 2 * display.Width + shiftoffX,
                    (input.Y + 1) / 2 * display.Height + shiftoffY
                );
            }
            else
            {
                return default;
            }
        }

        protected Vector2 ToUnitTablet(Vector2 input)
        {
            if (outputMode is not null)
            {
                return new Vector2(
                    input.X / digitizer.MaxX * 2 - 1,
                    input.Y / digitizer.MaxY * 2 - 1
                    );
            }
            else
            {
                tryResolveOutputMode();
                return default;
            }
        }

        protected Vector2 FromUnitTablet(Vector2 input)
        {
            if (outputMode is not null)
            {
                return new Vector2(
                    (input.X + 1) / 2 * digitizer.MaxX,
                    (input.Y + 1) / 2 * digitizer.MaxY
                );
            }
            else
            {
                return default;
            }
        }

        protected static Vector2 Clamp(Vector2 input)
        {
            return new Vector2(
            Math.Clamp(input.X, -1, 1),
            Math.Clamp(input.Y, -1, 1)
            );
        }

        protected static int In_Quadrant(Vector2 input)
        {
            //Due to how OTD sends input, the Y axis ends up flipped. Normally the Y axis for these quadrants would be reversed.
            //for the quadrant checks, true = disabled

            if (input.X > 0 && input.Y < 0)
            {
                return 1;
            }
            if (input.X < 0 && input.Y < 0)
            {
                return 2;
            }
            if (input.X < 0 && input.Y > 0)
            {
                return 3;
            }
            if (input.X > 0 && input.Y > 0)
            {
                return 4;
            }
            return 0;
        }

        private DigitizerSpecifications digitizer;

        [TabletReference]
        public TabletReference TabletReference
        {
            set {
                digitizer = value.Properties.Specifications.Digitizer;
            }
        }

        [Resolved]
        public IDriver driver;
        private AbsoluteOutputMode outputMode;
        private void tryResolveOutputMode()
        {
            if (driver is Driver drv)
            {
                IOutputMode output = drv.InputDevices
                    .Where(dev => dev?.OutputMode?.Elements?.Contains(this) ?? false)
                    .Select(dev => dev?.OutputMode).FirstOrDefault();

                if (output is AbsoluteOutputMode absOutput)
                    outputMode = absOutput;
            }
        }

        public abstract event Action<IDeviceReport> Emit;
        public abstract void Consume(IDeviceReport value);
        public abstract PipelinePosition Position { get; }
    }
}
