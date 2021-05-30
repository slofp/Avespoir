using Avespoir.Core.Language;
using Avespoir.Core.Modules.Commands;
using System.Threading.Tasks;

namespace Avespoir.Core.Abstructs {

	abstract class CommandAbstruct {

		/// <summary>
		/// Description of the command for each language
		/// </summary>
		internal abstract LanguageDictionary Description { get; }

		/// <summary>
		/// How to use the commands of each language.
		/// </summary>
		/// <remarks>Value: {0} is prefix+command string</remarks>
		internal abstract LanguageDictionary Usage { get; }

		/// <summary>
		/// Process executed by <see cref="CommandRegister"/>
		/// </summary>
		internal virtual async Task Execute(CommandObjects CommandObject) {
			await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.CommandNotImpl).ConfigureAwait(false);
		}
	}
}
