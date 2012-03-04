/***********************************************************
 * AGSBlend                                                *
 *                                                         *
 * Author: Steven Poulton                                  *
 *                                                         *
 * Date: 09/01/2011                                        *
 *                                                         *
 * Description: An AGS Plugin to allow true Alpha Blending *
 *                                                         *
 ***********************************************************/

#pragma region Defines_and_Includes

#define MIN_EDITOR_VERSION 1
#define MIN_ENGINE_VERSION 3

#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <stdio.h>
#include <conio.h>
#include <tchar.h>

#define THIS_IS_THE_PLUGIN
#include "agsplugin.h"

#define BUFSIZE 512

#pragma endregion

BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved) {

  switch (ul_reason_for_call)	{
		case DLL_PROCESS_ATTACH:
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
		case DLL_PROCESS_DETACH:
			break;
  }
  return TRUE;
}


//define engine

IAGSEngine *engine;
bool PipeConnected = false;
              
HANDLE hPipe; 
   LPTSTR lpvMessage=TEXT("Default message from client."); 
   TCHAR  chBuf[BUFSIZE]; 
   BOOL   fSuccess = FALSE; 
   DWORD  cbRead, cbToWrite, cbWritten, dwMode; 
   LPTSTR lpszPipename = TEXT("\\\\.\\pipe\\nexusnamedpipe"); 



//==============================================================================

// ***** Design time *****

IAGSEditor *editor; // Editor interface

const char *ourScriptHeader =
  "import void SendNexusMessage(String message);";




//------------------------------------------------------------------------------

LPCSTR AGS_GetPluginName()
{
	return ("AGSBlend");
}

//------------------------------------------------------------------------------

int AGS_EditorStartup(IAGSEditor *lpEditor)
{
	// User has checked the plugin to use it in their game
	
	// If it's an earlier version than what we need, abort.
	if (lpEditor->version < MIN_EDITOR_VERSION)
		return (-1);
	
	editor = lpEditor;
	editor->RegisterScriptHeader(ourScriptHeader);
	
	// Return 0 to indicate success
	return (0);
}

//------------------------------------------------------------------------------

void AGS_EditorShutdown()
{
	// User has un-checked the plugin from their game
	editor->UnregisterScriptHeader(ourScriptHeader);
}

//------------------------------------------------------------------------------

void AGS_EditorProperties(HWND parent)                        //*** optional ***
{
	// User has chosen to view the Properties of the plugin
	// We could load up an options dialog or something here instead
	MessageBox(parent,
	           "AGSBlend v1.0 By Calin Leafshade",
	           "About",
			   MB_OK | MB_ICONINFORMATION);
}

//------------------------------------------------------------------------------

int AGS_EditorSaveGame(char *buffer, int bufsize)             //*** optional ***
{
	// Called by the editor when the current game is saved to disk.
	// Plugin configuration can be stored in [buffer] (max [bufsize] bytes)
	// Return the amount of bytes written in the buffer
	return (0);
}

//------------------------------------------------------------------------------

void AGS_EditorLoadGame(char *buffer, int bufsize)            //*** optional ***
{
	// Called by the editor when a game is loaded from disk
	// Previous written data can be read from [buffer] (size [bufsize]).
	// Make a copy of the data, the buffer is freed after this function call.
}

//==============================================================================

// ***** Run time *****

 // Engine interface

//------------------------------------------------------------------------------


void SendNexusMessage(const char *message)
{

	
	hPipe = CreateFile( 
         lpszPipename,   // pipe name 
         GENERIC_READ |  // read and write access 
         GENERIC_WRITE, 
         0,              // no sharing 
         NULL,           // default security attributes
         OPEN_EXISTING,  // opens existing pipe 
         0,              // default attributes 
         NULL);          // no template file 

	

	if (hPipe == INVALID_HANDLE_VALUE) {
		
		engine->PrintDebugConsole("Invalid Handle");
		return;
	}
		
	
		dwMode = PIPE_READMODE_MESSAGE; 
	   fSuccess = SetNamedPipeHandleState( 
		  hPipe,    // pipe handle 
		  &dwMode,  // new pipe mode 
		  NULL,     // don't set maximum bytes 
		  NULL);    // don't set maximum time 
	
	  if ( ! fSuccess) 
	{
		engine->PrintDebugConsole("SetNamePipeHandleState Error");
		CloseHandle(hPipe);
		return;
	  }

	cbToWrite = (lstrlen(lpvMessage)+1)*sizeof(TCHAR);
		
	
		fSuccess =  WriteFile( 
		  hPipe,                  // pipe handle 
		  lpvMessage,             // message 
		  cbToWrite,              // message length 
		  &cbWritten,             // bytes written 
		  NULL);                  // not overlapped 
	
	 if ( ! fSuccess) 
	{
		engine->PrintDebugConsole("Pipe write failed.");
		CloseHandle(hPipe);
		return;
	  }
	 CloseHandle(hPipe);
}

#define REGISTER(x) engine->RegisterScriptFunction(#x, (void *) (x));
#define STRINGIFY(s) STRINGIFY_X(s)
#define STRINGIFY_X(s) #s




void AGS_EngineStartup(IAGSEngine *lpEngine)
{
	engine = lpEngine;
	
	// Make sure it's got the version with the features we need
	if (engine->version < MIN_ENGINE_VERSION)
		engine->AbortGame("Plugin needs engine version " STRINGIFY(MIN_ENGINE_VERSION) " or newer.");
	
	//register functions

	engine->RegisterScriptFunction ("SendNexusMessage", SendNexusMessage);
	engine->RequestEventHook(AGSE_PRERENDER);
	
	



}




//------------------------------------------------------------------------------

void AGS_EngineShutdown()
{
	// Called by the game engine just before it exits.
	// This gives you a chance to free any memory and do any cleanup
	// that you need to do before the engine shuts down.
}

//------------------------------------------------------------------------------

int AGS_EngineOnEvent(int event, int data)                    //*** optional ***
{
	switch (event)
	{

		
		case AGSE_PRERENDER:
		
			break;

		default:
			break;
	}
	
	// Return 1 to stop event from processing further (when needed)
	return (0);
}

//------------------------------------------------------------------------------
/*
int AGS_EngineDebugHook(const char *scriptName,
                        int lineNum, int reserved)            //*** optional ***
{
	// Can be used to debug scripts, see documentation
}
*/
//------------------------------------------------------------------------------
/*
void AGS_EngineInitGfx(const char *driverID, void *data)      //*** optional ***
{
	// This allows you to make changes to how the graphics driver starts up.
	// See documentation
}
*/
//..............................................................................
