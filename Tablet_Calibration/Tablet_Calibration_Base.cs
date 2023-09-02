using OpenTabletDriver;
using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.DependencyInjection;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Attributes;
using System;
using System.Linq;
using System.Numerics;

namespace tablet_calibration;
public abstract class tablet_calibration_base : IPositionedPipelineElement<IDeviceReport>
{
    public Vector2 to_unit_screen(Vector2 input)
    {
        if (output_mode_type == OutputModeType.absolute && absolute_output_mode != null)
        {
            var display = absolute_output_mode.Output;
            var offset = absolute_output_mode.Output.Position;
            var shiftoffX = offset.X - (display.Width / 2);
            var shiftoffY = offset.Y - (display.Height / 2);
            return new Vector2((input.X - shiftoffX) / display.Width * 2 - 1, (input.Y - shiftoffY) / display.Height * 2 - 1);
        }

        try_resolve_output_mode();
        return default;
    }

    public Vector2 from_unit_screen(Vector2 input)
    {
        if (output_mode_type == OutputModeType.absolute && absolute_output_mode != null)
        {
            var display = absolute_output_mode.Output;
            var offset = absolute_output_mode.Output.Position;
            var shiftoffX = offset.X - (display.Width / 2);
            var shiftoffY = offset.Y - (display.Height / 2);
            return new Vector2((input.X + 1) / 2 * display.Width + shiftoffX, (input.Y + 1) / 2 * display.Height + shiftoffY);
        }

        try_resolve_output_mode();
        return default;
    }

    public Vector2 to_unit_tablet(Vector2 input)
    {
        if (output_mode_type == OutputModeType.absolute && absolute_output_mode != null)
        {
            return new Vector2(input.X / absolute_output_mode.Tablet.Properties.Specifications.Digitizer.MaxX * 2 - 1, input.Y / absolute_output_mode.Tablet.Properties.Specifications.Digitizer.MaxY * 2 - 1);
        }

        try_resolve_output_mode();
        return default;
    }

    public Vector2 from_unit_tablet(Vector2 input)
    {
        if (output_mode_type == OutputModeType.absolute && absolute_output_mode != null)
        {
            return new Vector2((input.X + 1) / 2 * absolute_output_mode.Tablet.Properties.Specifications.Digitizer.MaxX, (input.Y + 1) / 2 * absolute_output_mode.Tablet.Properties.Specifications.Digitizer.MaxY);
        }

        try_resolve_output_mode();
        return default;
    }

    public Vector2 clamp(Vector2 input)
    {
        return new Vector2(
        Math.Clamp(input.X, -1, 1),
        Math.Clamp(input.Y, -1, 1)
        );
    }

    public int in_quadrant(Vector2 input)
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

    [Resolved]
    public IDriver driver;
    private OutputModeType output_mode_type;
    private AbsoluteOutputMode absolute_output_mode;
    private RelativeOutputMode relative_output_mode;
    private void try_resolve_output_mode()
    {
        if (driver is Driver drv)
        {
            IOutputMode output = drv.InputDevices
                .Where(dev => dev?.OutputMode?.Elements?.Contains(this) ?? false)
                .Select(dev => dev?.OutputMode).FirstOrDefault();

            if (output is AbsoluteOutputMode abs_output) {
                absolute_output_mode = abs_output;
                output_mode_type = OutputModeType.absolute;
                return;
            }
            if (output is RelativeOutputMode rel_output) {
                relative_output_mode = rel_output;
                output_mode_type = OutputModeType.relative;
                return;
            }
            output_mode_type = OutputModeType.unknown;
        }
    }

    public abstract event Action<IDeviceReport> Emit;
    public abstract void Consume(IDeviceReport value);
    public abstract PipelinePosition Position { get; }
}

enum OutputModeType {
    absolute,
    relative,
    unknown
}