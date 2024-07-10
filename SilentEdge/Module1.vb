Imports System.Runtime.InteropServices

Module Module1

    <StructLayout(LayoutKind.Sequential)>
    Public Structure STARTUPINFO
        Public cb As Integer
        Public lpReserved As String
        Public lpDesktop As String
        Public lpTitle As String
        Public dwX As UInteger
        Public dwY As UInteger
        Public dwXSize As UInteger
        Public dwYSize As UInteger
        Public dwXCountChars As UInteger
        Public dwYCountChars As UInteger
        Public dwFillAttribute As UInteger
        Public dwFlags As UInteger
        Public wShowWindow As Short
        Public cbReserved2 As Short
        Public lpReserved2 As IntPtr
        Public hStdInput As IntPtr
        Public hStdOutput As IntPtr
        Public hStdError As IntPtr
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure PROCESS_INFORMATION
        Public hProcess As IntPtr
        Public hThread As IntPtr
        Public dwProcessId As UInteger
        Public dwThreadId As UInteger
    End Structure

    Public Const DETACHED_PROCESS As UInteger = &H8


    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Function CreateProcess(
        ByVal lpApplicationName As String,
        ByVal lpCommandLine As String,
        ByVal lpProcessAttributes As IntPtr,
        ByVal lpThreadAttributes As IntPtr,
        ByVal bInheritHandles As Boolean,
        ByVal dwCreationFlags As UInteger,
        ByVal lpEnvironment As IntPtr,
        ByVal lpCurrentDirectory As String,
        ByRef lpStartupInfo As STARTUPINFO,
        ByRef lpProcessInformation As PROCESS_INFORMATION
    ) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Function CloseHandle(ByVal hObject As IntPtr) As Boolean
    End Function


    Private Function CreateDetachedHiddenProcess(ByVal processName As String) As IntPtr
        Dim startupInfo As New STARTUPINFO()
        Dim processInfo As New PROCESS_INFORMATION()


        startupInfo.cb = Marshal.SizeOf(startupInfo)
        startupInfo.dwFlags = &H1
        startupInfo.wShowWindow = 0
        Console.WriteLine("------------------------------------")
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine($"[{GetTimestamp()}] Set config for launch.")

        If Not CreateProcess(Nothing, processName, IntPtr.Zero, IntPtr.Zero, False, DETACHED_PROCESS, IntPtr.Zero, Nothing, startupInfo, processInfo) Then
            Throw New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error())
        End If
        Console.WriteLine($"[{GetTimestamp()}] Process created.")

        CloseHandle(processInfo.hThread)
        Console.WriteLine($"[{GetTimestamp()}] Thread closed.")

        Return processInfo.hProcess
    End Function
    Private Function GetTimestamp() As String
        Return DateTime.Now.ToString("HH:mm:ss.fff")
    End Function
    Sub Main()
        Try

            Dim edgeExecutablePath As String = "C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"
            Dim edgeCommandLine As String = "--no-startup-window"


            Dim hEdgeProcess As IntPtr = CreateDetachedHiddenProcess($"{edgeExecutablePath} {edgeCommandLine}")
            Console.WriteLine("")
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine($"[{GetTimestamp()}] Successfully launched Microsoft Edge as a detatched hidden process!")
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("At this point, a socket can be stolen and used for stealth C2 communication!")
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("------------------------------------")


            Console.ReadLine()
        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try
    End Sub
End Module
