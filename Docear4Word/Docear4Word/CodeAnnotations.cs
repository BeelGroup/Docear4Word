using System;
using System.Runtime.InteropServices;

namespace Docear4Word
{
	namespace Annotations
	{

		/// <summary>
		/// Indicates that the marked symbol is used implicitly (e.g. via reflection, in external library),
		/// so this symbol will not be marked as unused (as well as by other usage inspections)
		/// </summary>
		[ComVisible(false)]
		[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
		public sealed class UsedImplicitlyAttribute: Attribute
		{
			[UsedImplicitly]
			public UsedImplicitlyAttribute()
				: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
			{
			}

			[UsedImplicitly]
			public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
			{
				UseKindFlags = useKindFlags;
				TargetFlags = targetFlags;
			}

			[UsedImplicitly]
			public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
				: this(useKindFlags, ImplicitUseTargetFlags.Default)
			{
			}

			[UsedImplicitly]
			public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
				: this(ImplicitUseKindFlags.Default, targetFlags)
			{
			}

			[UsedImplicitly]
			public ImplicitUseKindFlags UseKindFlags { get; private set; }

			/// <summary>
			/// Gets value indicating what is meant to be used
			/// </summary>
			[UsedImplicitly]
			public ImplicitUseTargetFlags TargetFlags { get; private set; }
		}


		[Flags]
		public enum ImplicitUseKindFlags
		{
			Default = Access | Assign | InstantiatedWithFixedConstructorSignature,

			/// <summary>
			/// Only entity marked with attribute considered used
			/// </summary>
			Access = 1,

			/// <summary>
			/// Indicates implicit assignment to a member
			/// </summary>
			Assign = 2,

			/// <summary>
			/// Indicates implicit instantiation of a type with fixed constructor signature.
			/// That means any unused constructor parameters won't be reported as such.
			/// </summary>
			InstantiatedWithFixedConstructorSignature = 4,

			/// <summary>
			/// Indicates implicit instantiation of a type
			/// </summary>
			InstantiatedNoFixedConstructorSignature = 8,
		}

		/// <summary>
		/// Specify what is considered used implicitly when marked with <see cref="MeansImplicitUseAttribute"/> or <see cref="UsedImplicitlyAttribute"/>
		/// </summary>
		[Flags]
		public enum ImplicitUseTargetFlags
		{
			Default = Itself,

			Itself = 1,

			/// <summary>
			/// Members of entity marked with attribute are considered used
			/// </summary>
			Members = 2,

			/// <summary>
			/// Entity marked with attribute and all its members considered used
			/// </summary>
			WithMembers = Itself | Members
		}
	}
}