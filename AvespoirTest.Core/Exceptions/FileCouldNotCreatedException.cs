using System;
using System.Runtime.Serialization;

namespace AvespoirTest.Core.Exceptions {

	[Serializable()]
	class FileCouldNotCreatedException : Exception {

		public FileCouldNotCreatedException() : base() { }

		public FileCouldNotCreatedException(string Message) : base(Message) { }

		public FileCouldNotCreatedException(string Message, Exception Inner) : base(Message, Inner) { }

		protected FileCouldNotCreatedException(SerializationInfo Info, StreamingContext Context) : base(Info, Context) { }
	}
}
