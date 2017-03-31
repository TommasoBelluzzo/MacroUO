# MacroUO

Inspired by UO Loop, MacroUO is a very simple third party macro utility for Ultima Online. I am still maintaining it because a lot of people I know are still playing Ultima Online and, over the past years, it became very popular on some servers.

## Documentation

### Code

The source code is completely undocumented and has no descriptive comments; I know it's a bad practice, but I really don't have time to create a proper documentation. Anyway, everything should be pretty straightforward since it's just a simple form with some buttons.

The following method (contained within the `ApplicationDialog` class):

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
helps MacroUO to identify active Ultima Online clients on which macros can be executed. Making modifications to `windowClass` and `windowText` conditions woul allow MacroUO to work on other games or applications. Removing these conditions would transform it into a universal macro tool.

## Contributions

If you want to start a discussion about the project, just open an issue.
Contributions are more than welcome, fork and create pull requests as needed.
