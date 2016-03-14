using System;
using System.Diagnostics.Tracing;

namespace HelpPen.Client.Common.Tracing
{
	[EventSource(Name = "HelpPen-Services-Auth")]
	public sealed class AuthEventSource : EventSource
	{
		#region Constructors and Destructors

		static AuthEventSource()
		{
			Instance = new AuthEventSource();
		}

		#endregion

		#region Properties

		public static AuthEventSource Instance { get; private set; }

		#endregion

		#region Public Methods and Operators

		[Event(
			1,
			Message = @"Пользователь {0} осуществил успешный вход в систему.",
			Task = Tasks.Login,
			Opcode = EventOpcode.Info,
			Keywords = Keywords.Main,
			Channel = EventChannel.Operational,
			Level = EventLevel.Informational)]
		public void Login(string userName)
		{
			WriteEvent(1, userName);
		}

		[Event(
			2,
			Message = @"Для пользователя {0} вход занял {1} мс.",
			Task = Tasks.Login,
			Opcode = EventOpcode.Extension,
			Keywords = Keywords.Performance,
			Channel = EventChannel.Analytic,
			Level = EventLevel.Informational)]
		public void LoginTime(string userName, uint loginMilliseconds)
		{
			WriteEvent(2, userName, loginMilliseconds);
		}

		#endregion

		#region Nested Types

		public class Keywords
		{
			#region Static Fields

			public const EventKeywords Main = (EventKeywords)1;

			public const EventKeywords Diagnostic = (EventKeywords)4;

			public const EventKeywords Performance = (EventKeywords)8;

			#endregion
		}

		public class Tasks
		{
			#region Static Fields

			public const EventTask Login = (EventTask)1;

			public const EventTask Logout = (EventTask)2;

			#endregion
		}

		#endregion
	}
}