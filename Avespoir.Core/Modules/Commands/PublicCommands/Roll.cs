using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Assets;
using Avespoir.Core.Modules.Utils;
using Avespoir.Core.Modules.Visualize;
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
			VisualGenerator Visual = new VisualGenerator();
			if (msgs.Length == 0) {
				Random random = new Random();
				Visual.AddEmbed(random.NextLong(100).ToString());

				await Command_Object.Channel.SendMessageAsync(Visual.Generate()).ConfigureAwait(false);
			}
			else if (msgs.Length == 1) {
				string Maxstring = msgs[0].Split('.')[0];
				if (!long.TryParse(Maxstring, out long Maxvalue)) {
					Visual.AddEmbed(Command_Object.Language.RollMaxCouldntParse);
					await Command_Object.Channel.SendMessageAsync(Visual.Generate()).ConfigureAwait(false);
				}
				else {
					Random random = new Random();

					try {
						if (Maxvalue == 0) {
							Visual.AddEmbed("0");
							await Command_Object.Channel.SendMessageAsync(Visual.Generate()).ConfigureAwait(false);
						}
						else {
							long Resultvalue = random.NextLong(Maxvalue);

							Visual.AddEmbed(Resultvalue.ToString());
							await Command_Object.Channel.SendMessageAsync(Visual.Generate()).ConfigureAwait(false);
						}
					}
					catch (ArgumentOutOfRangeException) {
						Visual.AddEmbed(Command_Object.Language.RollMaxCouldntParse);
						await Command_Object.Channel.SendMessageAsync(Visual.Generate()).ConfigureAwait(false);
					}
				}
			}
			else if (msgs.Length == 2) {
				string Maxstring = msgs[0].Split('.')[0];
				string Minstring = msgs[1].Split('.')[0];
				if (!long.TryParse(Maxstring, out long Maxvalue)) {
					Visual.AddEmbed(Command_Object.Language.RollMaxCouldntParse);
					await Command_Object.Channel.SendMessageAsync(Visual.Generate()).ConfigureAwait(false);
				}
				else {
					if (!long.TryParse(Minstring, out long Minvalue)) {
						Visual.AddEmbed(Command_Object.Language.RollMinCouldntParse);
						await Command_Object.Channel.SendMessageAsync(Visual.Generate()).ConfigureAwait(false);
					}
					else {
						try {
							if (Minvalue == Maxvalue) {
								Visual.AddEmbed(Maxvalue.ToString());
								await Command_Object.Channel.SendMessageAsync(Visual.Generate()).ConfigureAwait(false);
							}
							else {
								Random random = new Random();

								Visual.AddEmbed(random.NextLong(Minvalue, Maxvalue).ToString());
								await Command_Object.Channel.SendMessageAsync(Visual.Generate()).ConfigureAwait(false);
							}
						}
						catch (ArgumentOutOfRangeException) {
							Visual.AddEmbed(Command_Object.Language.RollMaxMinCouldntParse);
							await Command_Object.Channel.SendMessageAsync(Visual.Generate()).ConfigureAwait(false);
						}
					}
				}
			}
		}
	}
}
