using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using System;
using System.Threading.Tasks;
using static Avespoir.Core.Modules.Utils.RamdomLong;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("roll", RoleLevel.Public)]
	class Roll : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("テンプレート") {
			{ Database.Enums.Language.en_US, "Template" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			string[] msgs = CommandObject.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				Random random = new Random();

				await CommandObject.Channel.SendMessageAsync(random.NextLong(100).ToString()).ConfigureAwait(false);
			}
			else if (msgs.Length == 1) {
				string Maxstring = msgs[0].Split('.')[0];
				if (!long.TryParse(Maxstring, out long Maxvalue)) {
					await CommandObject.Channel.SendMessageAsync(CommandObject.Language.RollMaxCouldntParse).ConfigureAwait(false);
				}
				else {
					Random random = new Random();

					try {
						if (Maxvalue == 0) {
							await CommandObject.Channel.SendMessageAsync("0").ConfigureAwait(false);
						}
						else {
							long Resultvalue = random.NextLong(Maxvalue);

							await CommandObject.Channel.SendMessageAsync(Resultvalue.ToString()).ConfigureAwait(false);
						}
					}
					catch (ArgumentOutOfRangeException) {
						await CommandObject.Channel.SendMessageAsync(CommandObject.Language.RollMaxCouldntParse).ConfigureAwait(false);
					}
				}
			}
			else if (msgs.Length == 2) {
				string Maxstring = msgs[0].Split('.')[0];
				string Minstring = msgs[1].Split('.')[0];
				if (!long.TryParse(Maxstring, out long Maxvalue)) {
					await CommandObject.Channel.SendMessageAsync(CommandObject.Language.RollMaxCouldntParse).ConfigureAwait(false);
				}
				else {
					if (!long.TryParse(Minstring, out long Minvalue)) {
						await CommandObject.Channel.SendMessageAsync(CommandObject.Language.RollMinCouldntParse).ConfigureAwait(false);
					}
					else {
						try {
							if (Minvalue == Maxvalue) {
								await CommandObject.Channel.SendMessageAsync(Maxvalue.ToString()).ConfigureAwait(false);
							}
							else {
								Random random = new Random();

								await CommandObject.Channel.SendMessageAsync(random.NextLong(Minvalue, Maxvalue).ToString()).ConfigureAwait(false);
							}
						}
						catch (ArgumentOutOfRangeException) {
							await CommandObject.Channel.SendMessageAsync(CommandObject.Language.RollMaxMinCouldntParse).ConfigureAwait(false);
						}
					}
				}
			}
		}
	}
}
