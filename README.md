# LibSharpQ
A client library for the Das Keyboard 5Q (and other cloud-enabled Das keyboards).

# Installation
Install the NuGet package `LibSharpQ`, grab the `.nupkg` from the Releases page, or download and build yourself from this repository. Requires the newest version of Visual Studio and .NET Core 2.1 tooling.

# How to Use
This library includes two main "client" classes - `CloudDasClient` and `LocalDasClient`. The former is designed for use with the "cloud" Das Keyboard API, and the latter is for the local Das Keyboard API available via the Das Keyboard Q desktop software.

Both provide async-only APIs, implement `IDisposable`, and provide the option to inject your own `HttpClient` - for demos / basic applications, I recommend switching your C# language version to "latest minor" and using an `async Task Main()` - see the Examples project for, well, examples.

If you want information on the API itself, you can find the official documentation here: https://www.daskeyboard.io/q-api-doc/

Below is a list of all available methods.

---

# Cloud & Local APIs

## GetSignals(bool retrieveAll = true)
This method return an `IReadOnlyList<Signal>` of all the previously sent signals. Note that signals are not synced, so signals sent to the Cloud API will not be returned if this method is called on a `LocalDasClient` instance, or vice-versa.

The `retrieveAll` parameter, if set to false, will make this method only return the first page of results from the API. This parameter does nothing on a `LocalDasClient`, where there is no pagination on this endpoint.

## SendSignal(Signal signal)
This method will send a signal to the API. There are convenient constructor overloads for `Signal` that will ensure the required parameters are present - but you can override anything you'd like via the properties, like `Effect`, which is `"SET_COLOR"` by default, or Pid, which is `"DK5QPID"` by default.

For indexed location zones, there is an overload that allows you to pass a `ValueTuple(int x, int y)` for location, which will be formatted appropriately.

This method will return a new instance of `Signal` as per the API response, which will include the signal ID you need to delete it later. The `ZoneId` of this response should be in the same format it was sent. If you sent it with an indexed location, it will be available as a `(int x, int y)` via the `ZonePosition` property.

## DeleteSignal(int signalId)
## DeleteSignals(IEnumerable\<int\> signalIds)
## DeleteAllSignals()
These methods will delete the signal with the given id, the signal(s) with the given id(s), or all the signals, respectively.

The DeleteAllSignals() method follows the same rules as `GetSignals()` - if called on a `LocalDasClient`, it will *not* delete signals on the Cloud, and vice-versa. This method also has a different implementation for the `LocalDasClient` - there, it will delete signals one at a time, whereas it will try to delete them all at once for the `CloudDasClient`. This is due to a hard-crash bug with concurrent requests in the current version of the Das Keyboard software, and will be changed once that bug is fixed.

---

# Cloud API

Certain features are only available via the Cloud API - primarily the endpoints that return device information.

## Login(OAuthTokenRequest credentials)
This is required to be called before calling any other methods on the `CloudDasClient` instance. Pass your OAuth credentials as provided by Das - available on this page after logging in: https://q.daskeyboard.com/account

You will need to populate both the Client ID and Client Secret. I highly recommend storing these in a config file, and loading them at runtime for security purposes - especially if you are writing an application that will be shared with others, or made open-source. 

## GetDevices()
This method returns an `IReadOnlyList<Device>` of devices registered & active on the logged-in account. 

## GetDeviceDefinitions()
This method returns an `IReadOnlyList<DeviceDefinition>` of definitions for devices that the API supports. This includes the Das Keyboard 5Q, other cloud-enabled Das keyboards, and international layouts of those keyboards. Useful to retrieve layout information.

## GetPredefinedColors()
This method returns an `IReadOnlyList<Color>` of predefined colors available via the API. The resulting `Color` instances will have their `Name` properties populated, which can be used instead of hex codes when sending a signal - and if a `Color` instance is passed to the `Signal` constructor, `Name` will be used instead of `HexCode` if not null.

## GetZones(string pid = "DK5QPID")
This method will return an `IReadOnlyList<Zone>` all the zones defined for the provided product ID - default is the Das Keyboard 5Q's PID. The resulting `Zone` instances will have a linearly numbered Id that can be used as a `ZoneId` for sending Signals, the `Code` which can also be used, and a `Description` of the zone. 

## GetEffects(string pid = "DK5QPID")
This method will return an `IReadOnlyList<Effect>` of available signal effects for the provided product ID - again defaulting to the Das Keyboard 5Q. The resulting `Effect` instances will have the `Code` that must be assigned to the `Signal.Effect` property when creating/sending Signals, and a user-friendly `Name`. 