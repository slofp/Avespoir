﻿using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class ModeratorCommands {

		[Command()]
		public async Task Name(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await CommandObject.Message.Channel.SendMessageAsync("何も入力されていません");
					return;
				}
				await Task.Delay(0);
			}
			catch (Exception Error) {
				Log.Error(Error);
			}
		}
	}
}
