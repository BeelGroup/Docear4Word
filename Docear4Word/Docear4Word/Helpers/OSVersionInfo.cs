using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Docear4Word
{
	/// <summary>
	/// Provides detailed information about the host operating system.
	/// </summary>
	public static class OSVersionInfo
	{
		static string name;
		static string servicePack;

		/// <summary>
		/// Determines if the current application is 32 or 64-bit.
		/// </summary>
		public static SoftwareArchitecture ApplicationBits
		{
			get
			{
				SoftwareArchitecture result;

				switch (IntPtr.Size * 8)
				{
					case 64:
						result = SoftwareArchitecture.Bit64;
						break;

					case 32:
						result = SoftwareArchitecture.Bit32;
						break;

					default:
						result = SoftwareArchitecture.Unknown;
						break;
				}

				return result;
			}
		}

		public static string ApplicationBitsString
		{
			get
			{
				return GetStringForSoftwareArchitecture(ApplicationBits);
			}
		}

		static string GetStringForSoftwareArchitecture(SoftwareArchitecture softwareArchitecture)
		{
			switch (softwareArchitecture)
			{
				case SoftwareArchitecture.Bit64: return "64-bit";
				case SoftwareArchitecture.Bit32: return "32-bit";
			}

			return "Unknown";
		}

		public static SoftwareArchitecture OSBits
		{
			get
			{
				SoftwareArchitecture result;

				switch (IntPtr.Size * 8)
				{
					case 64:
						result = SoftwareArchitecture.Bit64;
						break;

					case 32:
						result = Is32BitProcessOn64BitProcessor()
						         	? SoftwareArchitecture.Bit64
						         	: SoftwareArchitecture.Bit32;
						break;

					default:
						result = SoftwareArchitecture.Unknown;
						break;
				}

				return result;
			}
		}

		public static string OSBitsString
		{
			get
			{
				return GetStringForSoftwareArchitecture(OSBits);
			}
		}

		/// <summary>
		/// Determines if the current processor is 32 or 64-bit.
		/// </summary>
		public static ProcessorArchitecture ProcessorBits
		{
			get
			{
				ProcessorArchitecture result;

				try
				{
					var systemInfo = new SYSTEM_INFO();
					Win32API.GetNativeSystemInfo(ref systemInfo);

					switch (systemInfo.uProcessorInfo.wProcessorArchitecture)
					{
						case 9:
							result = ProcessorArchitecture.Bit64;
							break;

						case 6:
							result = ProcessorArchitecture.Itanium64;
							break;

						case 0:
							result = ProcessorArchitecture.Bit32;
							break;

						default:
							result = ProcessorArchitecture.Unknown;
							break;
					}
				}
				catch
				{
					result = ProcessorArchitecture.Unknown;
				}

				return result;
			}
		}

		static void EnsureVersionInfo()
		{
			if (name != null) return;

			var osVersion = Environment.OSVersion;
			var osVersionInfo = new OSVERSIONINFOEX
			                    	{
			                    		dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX))
			                    	};
			
			if (Win32API.GetVersionEx(ref osVersionInfo))
			{
				name = GetOSName(osVersion, osVersionInfo) ?? "Unknown";
				servicePack = osVersionInfo.szCSDVersion ?? string.Empty;
			}
			else
			{
				name = "Unknown";
				servicePack = string.Empty;
			}

		}

		static string GetOSName(OperatingSystem osVersion, OSVERSIONINFOEX osVersionInfo)
		{
			var majorVersion = osVersion.Version.Major;
			var minorVersion = osVersion.Version.Minor;

			switch (osVersion.Platform)
			{
				case PlatformID.Win32S: return "Windows 3.1";
				case PlatformID.WinCE: return "Windows CE";
				case PlatformID.Win32Windows:
					{
						if (majorVersion == 4)
						{
							var csdVersion = osVersionInfo.szCSDVersion;

							switch (minorVersion)
							{
								case 0:
									return csdVersion == "B" || csdVersion == "C"
									       	? "Windows 95 OSR2"
									       	: "Windows 95";

								case 10:
									return csdVersion == "A"
									       	? "Windows 98 Second Edition"
									       	: "Windows 98";
								case 90:
									return  "Windows Me";
							}
						}

						break;
					}

				case PlatformID.Win32NT:
					{
						var productType = osVersionInfo.wProductType;

						switch (majorVersion)
						{
							case 3:
								return "Windows NT 3.51";

							case 4:
								switch (productType)
								{
									case 1: return "Windows NT 4.0";
									case 3: return "Windows NT 4.0 Server";
								}

								break;

							case 5:
								switch (minorVersion)
								{
									case 0: return "Windows 2000";
									case 1: return "Windows XP";
									case 2: return "Windows Server 2003";
								}

								break;

							case 6:
								switch (minorVersion)
								{
									case 0:
										switch (productType)
										{
											case 1: return "Windows Vista";
											case 3: return "Windows Server 2008";
										}

										break;

									case 1:
										switch (productType)
										{
											case 1: return "Windows 7";
											case 3: return "Windows Server 2008 R2";
										}

										break;
								}

								break;
						}

						break;
					}
			}

			return null;
		}

		/// <summary>
		/// Gets the name of the operating system running on this computer.
		/// </summary>
		public static string Name
		{
			get
			{
				EnsureVersionInfo();

				return name;
			}
		}

		/// <summary>
		/// Gets the service pack information of the operating system running on this computer.
		/// </summary>
		public static string ServicePack
		{
			get
			{
				EnsureVersionInfo();

				return servicePack;
			}
		}

		/// <summary>
		/// Gets the build version number of the operating system running on this computer.
		/// </summary>
		public static int BuildVersion
		{
			get { return Environment.OSVersion.Version.Build; }
		}

		/// <summary>
		/// Gets the full version string of the operating system running on this computer.
		/// </summary>
		public static string VersionString
		{
			get { return Environment.OSVersion.Version.ToString(); }
		}

		/// <summary>
		/// Gets the full version of the operating system running on this computer.
		/// </summary>
		public static Version Version
		{
			get { return Environment.OSVersion.Version; }
		}

		/// <summary>
		/// Gets the major version number of the operating system running on this computer.
		/// </summary>
		public static int MajorVersion
		{
			get { return Environment.OSVersion.Version.Major; }
		}

		/// <summary>
		/// Gets the minor version number of the operating system running on this computer.
		/// </summary>
		public static int MinorVersion
		{
			get { return Environment.OSVersion.Version.Minor; }
		}

		/// <summary>
		/// Gets the revision version number of the operating system running on this computer.
		/// </summary>
		public static int VersionRevision
		{
			get { return Environment.OSVersion.Version.Revision; }
		}


		static IsWow64ProcessDelegate GetIsWow64ProcessDelegate()
		{
			var handle = Win32API.LoadLibrary("kernel32");
			if (handle == IntPtr.Zero) return null;

			var fnPtr = Win32API.GetProcAddress(handle, "IsWow64Process");

			return fnPtr != IntPtr.Zero
			       	? (IsWow64ProcessDelegate) Marshal.GetDelegateForFunctionPointer(fnPtr, typeof(IsWow64ProcessDelegate))
			       	: null;
		}

		static bool Is32BitProcessOn64BitProcessor()
		{
			var fnDelegate = GetIsWow64ProcessDelegate();
			if (fnDelegate == null) return false;

			bool isWow64;

			return fnDelegate.Invoke(Process.GetCurrentProcess().Handle, out isWow64) && isWow64;
		}

		public static string FullVersionString
		{
			get
			{
				var sb = new StringBuilder();

				sb.Append(Name);
				sb.Append(" ");
				sb.Append(OSBitsString);

				if (ServicePack.Length != 0)
				{
					sb.Append(", ");
					sb.Append(ServicePack);
				}

				if (VersionString.Length != 0)
				{
					sb.Append(", (");
					sb.Append(VersionString);
					sb.Append(")");
				}

				return sb.ToString();
			}
		}

		delegate bool IsWow64ProcessDelegate([In] IntPtr handle, [Out] out bool isWow64Process);

		[ComVisible(false)]
		public enum ProcessorArchitecture
		{
			Unknown = 0,
			Bit32 = 1,
			Bit64 = 2,
			Itanium64 = 3
		}

		[ComVisible(false)]
		public enum SoftwareArchitecture
		{
			Unknown = 0,
			Bit32 = 1,
			Bit64 = 2
		}
	}

	public static class Win32API
	{
		const string KERNEL32 = "kernel32.dll";

		[DllImport(KERNEL32)]
		public static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

		[DllImport(KERNEL32)]
		public static extern void GetNativeSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);

		[DllImport(KERNEL32, SetLastError = true, CallingConvention = CallingConvention.Winapi)]
		public static extern IntPtr LoadLibrary(string libraryName);

		[DllImport(KERNEL32, SetLastError = true, CallingConvention = CallingConvention.Winapi)]
		public static extern IntPtr GetProcAddress(IntPtr hwnd, string procedureName);

	}

	#region SYSTEM_INFO
	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public struct SYSTEM_INFO
	{
		internal _PROCESSOR_INFO_UNION uProcessorInfo;
		public uint dwPageSize;
		public IntPtr lpMinimumApplicationAddress;
		public IntPtr lpMaximumApplicationAddress;
		public IntPtr dwActiveProcessorMask;
		public uint dwNumberOfProcessors;
		public uint dwProcessorType;
		public uint dwAllocationGranularity;
		public ushort dwProcessorLevel;
		public ushort dwProcessorRevision;
	}
	#endregion SYSTEM_INFO

	#region _PROCESSOR_INFO_UNION
	[StructLayout(LayoutKind.Explicit)]
	[ComVisible(false)]
	struct _PROCESSOR_INFO_UNION
	{
		[FieldOffset(0)] internal uint dwOemId;
		[FieldOffset(0)] internal ushort wProcessorArchitecture;
		[FieldOffset(2)] internal ushort wReserved;
	}
	#endregion _PROCESSOR_INFO_UNION

	#region OSVERSIONINFOEX
	[StructLayout(LayoutKind.Sequential)]
	[ComVisible(false)]
	public struct OSVERSIONINFOEX
	{
		public int dwOSVersionInfoSize;
		public int dwMajorVersion;
		public int dwMinorVersion;
		public int dwBuildNumber;
		public int dwPlatformId;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string szCSDVersion;
		public short wServicePackMajor;
		public short wServicePackMinor;
		public short wSuiteMask;
		public byte wProductType;
		public byte wReserved;
	}
	#endregion OSVERSIONINFOEX

}