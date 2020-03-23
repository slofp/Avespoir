using System;
using System.Runtime.Serialization;

namespace Avespoir.Core.Exceptions {

	[Serializable()]
	class DirectoryCouldNotCreatedException : Exception {

		public DirectoryCouldNotCreatedException() : base() { }

		public DirectoryCouldNotCreatedException(string Message) : base(Message) { }

		public DirectoryCouldNotCreatedException(string Message, Exception Inner) : base(Message, Inner) { }

		protected DirectoryCouldNotCreatedException(SerializationInfo Info, StreamingContext Context) : base(Info, Context) { }
	}
}
