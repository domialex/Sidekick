# Input Simulator Standard
This library is a fork of Michael Noonan's *Windows Input Simulator* and Theodoros Chatzigiannakis's *Input Simulator Plus* (a C# wrapper around the `SendInput` functionality of Windows). It can be used as a replacement of the original library with some small source code changes.

This fork supports scan codes, making it compatible with many applications that the original library does not support.

The target framework has been changed to .Net Standard to support the usage with .Net Core and .Net Framework projects on Windows.

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/GregsStack.InputSimulatorStandard)](https://www.nuget.org/packages/GregsStack.InputSimulatorStandard)
[![Build Status](https://dev.azure.com/GregsStack/GitHub/_apis/build/status/GregsStack.InputSimulatorStandard?branchName=master)](https://dev.azure.com/GregsStack/GitHub/_build/latest?definitionId=2?branchName=master)
[![codecov](https://codecov.io/gh/GregsStack/InputSimulatorStandard/branch/master/graph/badge.svg)](https://codecov.io/gh/GregsStack/InputSimulatorStandard)

# Examples

## Example: Single key press
```csharp
public void PressTheSpacebar()
{
    var simulator = new InputSimulator();
    simulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
}
```

## Example: Key-down and Key-up
```csharp
public void ShoutHello()
{
    var simulator = new InputSimulator();

    // Simulate each key stroke
    simulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
    simulator.Keyboard.KeyPress(VirtualKeyCode.VK_H);
    simulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
    simulator.Keyboard.KeyPress(VirtualKeyCode.VK_L);
    simulator.Keyboard.KeyPress(VirtualKeyCode.VK_L);
    simulator.Keyboard.KeyPress(VirtualKeyCode.VK_O);
    simulator.Keyboard.KeyPress(VirtualKeyCode.VK_1);
    simulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);

    // Alternatively you can simulate text entry to acheive the same end result
    simulator.Keyboard.TextEntry("HELLO!");
}
```

## Example: Modified keystrokes such as CTRL-C
```csharp
public void SimulateSomeModifiedKeystrokes()
{
    var simulator = new InputSimulator();

    // CTRL-C (effectively a copy command in many situations)
    simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);

    // You can simulate chords with multiple modifiers
    // For example CTRL-K-C whic is simulated as
    // CTRL-down, K, C, CTRL-up
    simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, new [] {VirtualKeyCode.VK_K, VirtualKeyCode.VK_C});

    // You can simulate complex chords with multiple modifiers and key presses
    // For example CTRL-ALT-SHIFT-ESC-K which is simulated as
    // CTRL-down, ALT-down, SHIFT-down, press ESC, press K, SHIFT-up, ALT-up, CTRL-up
    simulator.Keyboard.ModifiedKeyStroke(
        new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.MENU, VirtualKeyCode.SHIFT },
        new[] { VirtualKeyCode.ESCAPE, VirtualKeyCode.VK_K });
}
```

## Example: Simulate text entry
```csharp
public void SayHello()
{
    var simulator = new InputSimulator();
    simulator.Keyboard.TextEntry("Say hello!");
}
```

## Example: Determine the state of different types of keys
```csharp
public void GetKeyStatus()
{
    var simulator = new InputSimulator();

    // Determines if the shift key is currently down
    var isShiftKeyDown = simulator.InputDeviceState.IsKeyDown(VirtualKeyCode.SHIFT);

    // Determines if the caps lock key is currently in effect (toggled on)
    var isCapsLockOn = simulator.InputDeviceState.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL);
}
```

## Example: Get current mouse position
```csharp
public void GetMousePosition()
{
    var simulator = new InputSimulator();

    // Gets the mouse position as System.Drawing.Point
    var position = simulator.Mouse.Position;
}
```
