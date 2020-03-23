using System;
using System.Runtime.Serialization;

namespace Avespoir.Core.Exceptions {

	[Serializable()]
	class UrlNotFoundException : Exception {

		public UrlNotFoundException() : base() { }

		public UrlNotFoundException(string Message) : base(Message) { }

		public UrlNotFoundException(string Message, Exception InnerException) : base(Message, InnerException) { }

		protected UrlNotFoundException(SerializationInfo Info, StreamingContext Context) : base(Info, Context) { }
	}
}
