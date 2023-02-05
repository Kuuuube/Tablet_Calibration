# Display Tablet Calibration Guide

When calibrating the tablet make sure to sit in a your normal posture and distance from the tablet. You may end up seeing larger parallax inaccuracies if you calibrate at an odd viewing angle.

Make sure to drag your pen on the tablet surface while calibrating. If this causes issues with the image in the centering step, temporarily clear the Tip Binding in the Pen Settings tab in OTD.

Both `Tablet Calibration Stretch Tablet` and `Tablet Calibration Stretch Area` can achieve the same results. When using `Tablet Calibration Stretch Area`, right click your tablet area in the main OTD window and disable `Clamp input outside area` and `Ignore input outside area`.

## Centering

- Find the [calibration image](./calibration_images/calibration_images.md) matching your tablet's resolution and fullscreen it on your tablet's display. 

    Make sure to fullscreen the image without borders. There should be no window borders, taskbars, menubars, or toolbars around the image (overlays will not cause issues as long as they do not move the image). One method to achieve this is to open the image in a new tab of your browser and press `F11`.

- Put your pen's tip in the center of the red cross. Check if the tip of your cursor lines up with the tip of the pen. If not, change the applicable offset value.

    If the cursor is too far to the left, add positive `X Offset`.

    If the cursor is too far to the right, add negative `X Offset`.

    If the cursor is too far to the top, add positive `Y Offset`.

    If the cursor is too far to the bottom, add negative `Y Offset`.

- Repeat the previous step until the offset appears correct. Make sure to apply settings inbetween tests. Don't be worried if your tablet is perfect at zero offset, not all tablets require this step.

## Stretching

- Pick any side of the tablet and move your pen tip roughly 75% between the center and a corner on the X or Y axis.

- If your pen tip doesn't match the cursor on the axis you moved to, change the applicable stretch multiplier. Only edit the multiplier of the side your pen is closest to while testing each side. For example, while your pen is on the X axis near the right side of the tablet and you're testing the right side, only edit `Right Stretch Multiplier`.

    If the cursor is too close to the center of the tablet, increase the multiplier.

    If the cursor is too close the the side of the tablet, decrease the multiplier.

- Repeat the previous step until every side is lined up. Make sure to apply settings inbetween tests. Sometimes it can be hard to perfectly align one side when another side is misaligned so you may need to test each side again after initially lining everything up.

## Notes

- Make sure to save your settings after completing calibration.

- It is impossible to perfectly calibrate the entire surface of most tablets. The far edges tend to have more inaccuracy.

- Looking at the tablet from different viewing angles may make it seem like your tablet isn't calibrated correctly due to parallax. This cannot be fixed by using calibration and is caused by the gap between the pen tip and the screen's display.

- It is impossible to perfectly calibrate both drag and hover. Do not change your calibration values due to hover appearing incorrect.