# MacroUO

Inspired by UO Loop, MacroUO is a simple third party macro utility for Ultima Online designed for Windows operating systems. I am still maintaining it because a lot of people I know are still playing Ultima Online and, over the past years, it became very popular on some servers.

## Documentation

### Usage

![Interface](https://i.imgur.com/oifLOCC.png)

#### Options

Two different interface options can be toggled on and off through the menu bar. The `TopMost` option forces the MacroUO window to overlap all the other windows, even when it's not active. The `Transparency` option makes the MacroUO window slightly transparent.

#### Clients Panel

The `Scan` button analyses running processes in order to identify active Ultima Online clients. Only the clients on which an account has been successfully authenticated will show up.

The purpose of the dropdown menu is to select the target client on which the selected macro will be executed.

#### Macro Panel

This panel holds the macro settings. The `Key` dropdown sets the keyboard key to send to the target client. The `Delay` control sets the delay (in milliseconds) between each macro execution. The key modifiers can be turned on and off using their respective checkboxes. A limit to the number of executions can be set through the `Runs` control: MacroUO will automatically stop once the specified limit has been reached.

On the bottom right of the panel there are two counters: the first one records how many times the current macro has been executed, the second one keeps track of the elapsed time since the start of the process. Pushing the `Reset` button will immediately reset both counters.

#### Presets Panel

The purpose of this panel is to manage the preset macros stored in the `Presets.xml` file. The dropdown menu loads the selected preset into the `Macro` panel. The `Add` button, enabled only if no preset is selected, creates a new preset (with a default name) from the current macro settings. The `Remove` button deletes the currently selected preset. The `Reload` button forces MacroUO to reload the preset macros from the `Presets.xml` file.

While a preset is selected in the dropdown menu:
* changing the `Macro` panel settings  modifies the active preset;
* pushing `F2` opens up a dialog that allows to rename the active preset (the name must be unique and composed of alphanumeric characters separated by single white-space characters).

All the changes to the preset macros are automatically saved when MacroUO is closed.

#### Buttons

On the bottom of the window there are three buttons. The `Start` button activates the process that executes the specified macro on the target client, while the `Stop` button interrupts it. The `Collapse` button reduces the MacroUO window size, while the `Expand` button restores it.

### Code

The source code is completely undocumented and has no descriptive comments; I know it's a bad practice, but I really don't have time to create a proper documentation. Nevertheless, everything should be pretty straightforward since it's just a simple form with a few buttons.

The following method (which can be found within the `ApplicationDialog` class):

```csharp
private Boolean EnumerateWindow(IntPtr windowHandle, IntPtr lParameter)
{
	String windowClass = NativeMethods.GetWindowClass(windowHandle);

	if (!windowClass.Contains("Ultima Online"))
		return true;

	String windowText = NativeMethods.GetWindowText(windowHandle);

	if (!windowText.Contains("Ultima Online - "))
		return true;

	UInt32 windowThreadId = NativeMethods.GetWindowThreadId(windowHandle);

	m_Clients.Add(new Client(windowText, windowHandle, windowThreadId));

	return true;
}
```
helps MacroUO to identify active Ultima Online clients on which macros can be executed. Making modifications to `windowClass` and `windowText` conditions would allow MacroUO to work on other games or applications. Removing the aforementioned conditions would transform it into a universal macro tool.
