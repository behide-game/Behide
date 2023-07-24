// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.Auth
{
	/// <summary>
	/// All possible types of login methods, availability depends on permissions granted to the client.
	/// <seealso cref="AuthInterface.Login" />
	/// <seealso cref="Credentials" />
	/// </summary>
	public enum LoginCredentialType : int
	{
		/// <summary>
		/// Login using account email address and password.
		///
		/// @note Use of this login method is restricted and cannot be used in general.
		/// </summary>
		Password = 0,
		/// <summary>
		/// A short-lived one-time use exchange code to login the local user.
		///
		/// @details Typically retrieved via command-line parameters provided by a launcher that generated the exchange code for this application.
		/// When started, the application is expected to consume the exchange code by using the <see cref="AuthInterface.Login" /> API as soon as possible.
		/// This is needed in order to authenticate the local user before the exchange code would expire.
		/// Attempting to consume an already expired exchange code will return <see cref="Result" />::AuthExchangeCodeNotFound error by the <see cref="AuthInterface.Login" /> API.
		/// </summary>
		ExchangeCode = 1,
		/// <summary>
		/// Desktop and Mobile only; deprecated on Console platforms in favor of <see cref="ExternalAuth" /> login method.
		///
		/// Long-lived access token that is stored on the local device to allow persisting a user login session over multiple runs of the application.
		/// When using this login type, if an existing access token is not found or it is invalid or otherwise expired, the error result <see cref="Result" />::<see cref="Result.InvalidAuth" /> is returned.
		///
		/// @note On Desktop and Mobile platforms, the persistent access token is automatically managed by the SDK that stores it in the keychain of the currently logged in user of the local device.
		/// On Console platforms, after a successful login using the <see cref="DeviceCode" /> login type,
		/// the persistent access token is retrieved using the <see cref="AuthInterface.CopyUserAuthToken" /> API and
		/// stored by the application for the currently logged in user of the local device.
		/// <seealso cref="ExternalAuth" />
		/// </summary>
		PersistentAuth = 2,
		/// <summary>
		/// Deprecated and no longer used. Superseded by the <see cref="ExternalAuth" /> login method.
		///
		/// Initiates a PIN grant login flow that is used to login a local user to their Epic Account for the first time,
		/// and also whenever their locally persisted login credentials would have expired.
		///
		/// @details The flow is as following:
		/// 1. Game initiates the user login flow by calling <see cref="AuthInterface.Login" /> API with the <see cref="DeviceCode" /> login type.
		/// 2. The SDK internally requests the authentication backend service to begin the login flow, and returns the game
		/// a new randomly generated device code along with authorization URL information needed to complete the flow.
		/// This information is returned via the <see cref="AuthInterface.Login" /> API callback. The <see cref="LoginCallbackInfo" />::ResultCode
		/// will be set to <see cref="Result.AuthPinGrantCode" /> and the <see cref="PinGrantInfo" /> struct will contain the needed information.
		/// 3. Game presents the device code and the authorization URL information on screen to the end-user.
		/// 4. The user will login to their Epic Account using an external device, e.g. a mobile device or a desktop PC,
		/// by browsing to the presented authentication URL and entering the device code presented by the game on the console.
		/// 5. Once the user has successfully logged in on their external device, the SDK will call the <see cref="AuthInterface.Login" /> callback
		/// once more with the operation result code. If the user failed to login within the allowed time before the device code
		/// would expire, the result code returned by the callback will contain the appropriate error result.
		///
		/// @details After logging in a local user for the first time, the game can remember the user login to allow automatically logging
		/// in the same user the next time they start the game. This avoids prompting the same user to go through the login flow
		/// across multiple game sessions over long periods of time.
		/// To do this, after a successful login using the <see cref="DeviceCode" /> login type, the game can call the <see cref="AuthInterface.CopyUserAuthToken" /> API
		/// to retrieve a long-lived refresh token that is specifically created for this purpose on Console. The game can store
		/// the long-lived refresh token locally on the device, for the currently logged in local user of the device.
		/// Then, on subsequent game starts the game can call the <see cref="AuthInterface.Login" /> API with the previously stored refresh token and
		/// using the <see cref="PersistentAuth" /> login type to automatically login the current local user of the device.
		/// <seealso cref="ExternalAuth" />
		/// </summary>
		DeviceCode = 3,
		/// <summary>
		/// Login with named credentials hosted by the EOS SDK Developer Authentication Tool.
		///
		/// @note Used for development purposes only.
		/// </summary>
		Developer = 4,
		/// <summary>
		/// Refresh token that was retrieved from a previous call to <see cref="AuthInterface.Login" /> API in another local process context.
		/// Mainly used in conjunction with custom launcher applications.
		///
		/// @details Can be used for example when launching the game from Epic Games Launcher and having an intermediate process
		/// in-between that requires authenticating the user before eventually starting the actual game client application.
		/// In such scenario, an intermediate launcher will log in the user by consuming the exchange code it received from the
		/// Epic Games Launcher. To allow the game client to also authenticate the user, it can copy the refresh token using the
		/// <see cref="AuthInterface.CopyUserAuthToken" /> API and pass it via launch parameters to the started game client. The game client can then
		/// use the refresh token to log in the user.
		/// </summary>
		RefreshToken = 5,
		/// <summary>
		/// Desktop and Mobile only.
		///
		/// Initiate a login through the Epic account portal.
		///
		/// @details Can be used in scenarios where seamless user login via other means is not available,
		/// for example when starting the application through a proprietary ecosystem launcher or otherwise.
		/// </summary>
		AccountPortal = 6,
		/// <summary>
		/// Login using external account provider credentials, such as Steam, PlayStation(TM)Network, Xbox Live, or Nintendo.
		///
		/// This is the intended login method on Console. On Desktop and Mobile, used when launched through any of the commonly supported platform clients.
		///
		/// @details The user is seamlessly logged in to their Epic account using an external account access token.
		/// If the local platform account is already linked with the user's Epic account, the login will succeed and <see cref="Result" />::<see cref="Result.Success" /> is returned.
		/// When the local platform account has not been linked with an Epic account yet,
		/// <see cref="Result" />::<see cref="Result.InvalidUser" /> is returned and the <see cref="ContinuanceToken" /> will be set in the <see cref="LoginCallbackInfo" /> data.
		/// If <see cref="Result" />::<see cref="Result.InvalidUser" /> is returned,
		/// the application should proceed to call the <see cref="AuthInterface.LinkAccount" /> API with the <see cref="ContinuanceToken" /> to continue with the external account login
		/// and to link the external account at the end of the login flow.
		///
		/// @details On Console, login flow when the platform user account has not been linked with an Epic account yet:
		/// 1. Game calls <see cref="AuthInterface.Login" /> with the <see cref="ExternalAuth" /> credential type.
		/// 2. <see cref="AuthInterface.Login" /> returns <see cref="Result" />::<see cref="Result.InvalidUser" /> with a non-null <see cref="ContinuanceToken" /> in the <see cref="LoginCallbackInfo" /> data.
		/// 3. Game calls <see cref="AuthInterface.LinkAccount" /> with the <see cref="ContinuanceToken" /> to initiate the login flow for linking the platform account with the user's Epic account.
		/// - During the login process, the user will be able to login to their existing Epic account or create a new account if needed.
		/// 4. <see cref="AuthInterface.LinkAccount" /> will make an intermediate callback to provide the caller with <see cref="PinGrantInfo" /> struct set in the <see cref="LoginCallbackInfo" /> data.
		/// 5. Game examines the retrieved <see cref="PinGrantInfo" /> struct for a website URI and user code that the user needs to access off-device via a PC or mobile device.
		/// - Game visualizes the URI and user code so that the user can proceed with the login flow outside the console.
		/// - In the meantime, EOS SDK will internally keep polling the backend for a completion status of the login flow.
		/// 6. Once user completes the login, cancels it or if the login flow times out, <see cref="AuthInterface.LinkAccount" /> makes the second and final callback to the caller with the operation result status.
		/// - If the user was logged in successfully, <see cref="Result" />::<see cref="Result.Success" /> is returned in the <see cref="LoginCallbackInfo" />. Otherwise, an error result code is returned accordingly.
		/// </summary>
		ExternalAuth = 7
	}
}