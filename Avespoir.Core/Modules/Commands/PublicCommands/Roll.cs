using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using System;
using System.Threading.Tasks;
using static Avespoir.Core.Modules.Utils.RamdomLong;

namespace Avespoir.Core.Modules.Commands.PublicCommands {

	[Command("roll", RoleLevel.Public)]
	class Roll : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("ランダムの数値を生成します") {
			{ Database.Enums.Language.en_US, "Generate random numbers" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}roll (最大値) (最小値)") {
			{ Database.Enums.Language.en_US, "{0}roll (Maximum value) (Minimum value)" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			string[] msgs = Command_Object.CommandArgs.Remove(0);
			if (msgs.Length == 0) {
				Random random = new Random();

				await Command_Object.Channel.SendMessageAsync(random.NextLong(100).ToString()).ConfigureAwait(false);
			}
			else if (msgs.Length == 1) {
				string Maxstring = msgs[0].Split('.')[0];
				if (!long.TryParse(Maxstring, out long Maxvalue)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RollMaxCouldntParse).ConfigureAwait(false);
				}
				else {
					Random random = new Random();

					try {
						if (Maxvalue == 0) {
							await Command_Object.Channel.SendMessageAsync("0").ConfigureAwait(false);
						}
						else {
							long Resultvalue = random.NextLong(Maxvalue);

							await Command_Object.Channel.SendMessageAsync(Resultvalue.ToString()).ConfigureAwait(false);
						}
					}
					catch (ArgumentOutOfRangeException) {
						await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RollMaxCouldntParse).ConfigureAwait(false);
					}
				}
			}
			else if (msgs.Length == 2) {
				string Maxstring = msgs[0].Split('.')[0];
				string Minstring = msgs[1].Split('.')[0];
				if (!long.TryParse(Maxstring, out long Maxvalue)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RollMaxCouldntParse).ConfigureAwait(false);
				}
				else {
					if (!long.TryParse(Minstring, out long Minvalue)) {
						await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RollMinCouldntParse).ConfigureAwait(false);
					}
					else {
						try {
							if (Minvalue == Maxvalue) {
								await Command_Object.Channel.SendMessageAsync(Maxvalue.ToString()).ConfigureAwait(false);
							}
							else {
								Random random = new Random();

								await Command_Object.Channel.SendMessageAsync(random.NextLong(Minvalue, Maxvalue).ToString()).ConfigureAwait(false);
							}
						}
						catch (ArgumentOutOfRangeException) {
							await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RollMaxMinCouldntParse).ConfigureAwait(false);
						}
					}
				}
			}
		}
	}
}
